using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyManager enemyManager;
    public static int enemiesDestroyed; // Track enemies destroyed


    void Awake()
    {
        EnemyManager.Instance.enemiesCount++; // Increment enemies count when an enemy is created
    }



    public void OnCollisionEnter2D(Collision2D collision)
    {
        
        // Comprova si el collider té el tag "Enemic" o "Spikes"
        if ( collision.collider.CompareTag("PINCHOS") || collision.collider.CompareTag("ENEMIC"))
        {
            
            Destroy(gameObject);
            EnemyManager.Instance.enemiesDestroyed++;
        }
    }
}
