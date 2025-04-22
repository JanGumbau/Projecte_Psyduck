using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprova si el collider té el tag "Ground" o "Spikes"
        if (collision.collider.CompareTag("GROUND") || collision.collider.CompareTag("PINCHOS") || collision.collider.CompareTag("ENEMIC"))
        {
            Destroy(gameObject);
        }
    }
}
