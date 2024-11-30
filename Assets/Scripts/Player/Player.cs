using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float pickupRange = 2f;
    private float currentHealth = 100f;
    public LayerMask itemLayer;
    public Transform handPosition;

    [SerializeField] private GameObject currentItem = null;

    private void Update()
    {
        OnInput();
    }

    private void OnInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickupItem();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (currentItem != null)
            {
                Boomerang boomerang = currentItem.GetComponent<Boomerang>();
                if (boomerang != null && !boomerang.IsFlying)
                {
                    ThrowBoomerang(boomerang);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Boomerang boomerang = currentItem.GetComponent<Boomerang>();
            if (boomerang != null && boomerang.IsFlying)
            {
                if (Vector3.Distance(transform.position, boomerang.transform.position) > 0.2f && Vector3.Distance(transform.position, boomerang.transform.position) <= 0.3f)
                {
                    print("Perfeito!");
                }
                else if (Vector3.Distance(transform.position, boomerang.transform.position) > 0.2f && Vector3.Distance(transform.position, boomerang.transform.position) <= 0.5f)
                {
                    print("OK");
                }
                else
                {
                    print("Falhou!");
                }
            }
        }
    }

    void TryPickupItem()
    {
        Collider2D item = Physics2D.OverlapCircle(transform.position, pickupRange, itemLayer);

        if (item != null)
        {
            PickupItem(item.gameObject);
        }
    }

    void PickupItem(GameObject item)
    {
        if (currentItem != null)
        {
            if (currentItem.GetComponent<Boomerang>().IsFlying)
            {
                return;
            }
            else
            {
                DropItem();
            }

        }
        currentItem = item;

        item.SetActive(true);

        item.transform.SetParent(handPosition);
        item.transform.localPosition = Vector3.zero;

        Collider2D itemCollider = item.GetComponent<Collider2D>();
        if (itemCollider != null)
        {
            itemCollider.isTrigger = true;
        }

        Rigidbody2D currentItemRigidbody = item.GetComponent<Rigidbody2D>();
        if (currentItemRigidbody != null)
        {
            currentItemRigidbody.bodyType = RigidbodyType2D.Kinematic;
        }

        Debug.Log("Item Pegado!");
    }

    void DropItem()
    {
        currentItem.SetActive(true);

        Collider2D itemCollider = currentItem.GetComponent<Collider2D>();
        if (itemCollider != null)
        {
            itemCollider.isTrigger = false;
        }

        Rigidbody2D currentItemRigidbody = currentItem.GetComponent<Rigidbody2D>();
        if (currentItemRigidbody != null)
        {
            currentItemRigidbody.bodyType = RigidbodyType2D.Dynamic;
        }

        currentItem.transform.SetParent(null);
        currentItem.transform.position = transform.position + new Vector3(1f, 0f, 0f);

        currentItem = null;
        Debug.Log("Item Solto!");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }

    void ThrowBoomerang(Boomerang boomerang)
    {
        Vector3 throwDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        throwDirection.z = 0;
        boomerang.Throw(throwDirection);

        currentItem.transform.SetParent(null);
        // currentItem = null;
    }

    public void SetCurrentItem(GameObject item)
    {
        currentItem = item;
    }

    public void TakeDamage(float damage)
    {
        print("-"+damage);
        print(currentHealth);
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
