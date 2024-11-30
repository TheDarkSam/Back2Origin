using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public float throwSpeed = 10f;
    public float maxDistance = 5f;
    public float damage = 25f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Transform player;
    private bool isReturning = false;
    public bool IsFlying { get; private set; } = false;

    private void Update()
    {
        if (!IsFlying) return;

        if (!isReturning)
        {
            // Movendo-se em direção ao alvo
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, throwSpeed * Time.deltaTime);

            if (Vector3.Distance(initialPosition, transform.position) >= maxDistance - 0.1f)
            {
                isReturning = true;
            }
        }
        else
        {
            // Voltando
            transform.position = Vector3.MoveTowards(transform.position, player.position, throwSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, player.position) < 0.2f)
            {

                ResetBoomerang();
            }
        }
    }

    public void Throw(Vector3 direction)
    {
        initialPosition = transform.position;
        targetPosition = transform.position + direction.normalized * maxDistance;
        IsFlying = true;
        isReturning = false;

        player = FindFirstObjectByType<Player>().transform;
    }

    private void ResetBoomerang()
    {
        IsFlying = false;
        isReturning = false;

        transform.SetParent(player.GetComponent<Player>().handPosition);
        transform.localPosition = Vector3.zero;
        player.GetComponent<Player>().SetCurrentItem(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<EnemyFollow>().TakeDamage(damage);
        }
    }
}
