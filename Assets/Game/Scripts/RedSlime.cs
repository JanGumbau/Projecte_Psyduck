using UnityEngine;

public class EnemyPush : MonoBehaviour
{
    public float pushForce = 15f;     // Force applied to push the player

    private Transform player;         // Reference to the player

    private void Start()
    {
        // Find player by tag (make sure it's tagged "Player")
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Die()
    {
        // Calculate push direction (from enemy to player)
        Vector3 pushDirection = (player.position - transform.position).normalized;

        // Apply force to player in opposite direction
        if (player.TryGetComponent<Rigidbody>(out var playerRb))
        {
            playerRb.AddForce(-pushDirection * pushForce, ForceMode.Impulse);
        }

        // Destroy the enemy
        Destroy(gameObject);
    }
}
public class RedSlime : EnemyPush
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar si el objeto que colisiona es la hitbox del jugador
        if (collision.CompareTag("PlayerAttack"))
        {
            // Llamar al método Die() para que el enemigo muera
            Die();
        }
    }
}