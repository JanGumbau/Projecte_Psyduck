using UnityEngine;

public class RedSlime : MonoBehaviour
{
    public float explosionForce = 500f;

    void Die(GameObject killer)
    {
        // Aplicar fuerza al jugador si tiene Rigidbody2D
        if (killer.TryGetComponent<Rigidbody2D>(out var rb))
        {
            Vector2 direction = (rb.position - (Vector2)transform.position).normalized;
            rb.AddForce(direction * explosionForce);
        }

        // Destruir el slime
        Destroy(gameObject);
    }

    public void TakeDamage(int amount, GameObject killer)
    {
        // Lógica de vida, si quieres, pero aquí directamente muere
        Die(killer);
    }
}