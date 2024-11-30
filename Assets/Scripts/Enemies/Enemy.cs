using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 2f;
    public float stopDistance = 1f;
    public float damage = 25f;

    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (GameManager.Instance.player == null) return;
        Vector3 playerPosition = GameManager.Instance.player.position;

        RotateTowardsPlayer(playerPosition);

        float distanceToPlayer = Vector2.Distance(transform.position, GameManager.Instance.player.position);


        if (distanceToPlayer > stopDistance)
        {
            Vector2 direction = (GameManager.Instance.player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void RotateTowardsPlayer(Vector3 playerPosition)
    {
        // Calcula a direção do player
        Vector2 direction = (playerPosition - transform.position).normalized;

        // Calcula o ângulo em graus
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Define a rotação do inimigo no eixo Z
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // -90 para alinhar a frente do inimigo
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }
}
