using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyManager enemyManager;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public static int enemiesDestroyed; // Track enemies destroyed
    private Animator animator;


    void Awake()
    {
        EnemyManager.Instance.enemiesCount++; // Increment enemies count when an enemy is created
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        animator = GetComponent<Animator>();
    }
    public void RebreDany()
    {
        StartCoroutine(CanviColorTemporal(Color.red, 0.2f)); // Canvi a vermell durant 1 segon
    }
    private System.Collections.IEnumerator CanviColorTemporal(Color nouColor, float duracio)
    {
        spriteRenderer.color = nouColor;
        yield return new WaitForSeconds(duracio);
        spriteRenderer.color = originalColor;
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("PINCHOS") || collision.collider.CompareTag("ENEMIC") || collision.collider.CompareTag("ENEMIC_AMARILLO"))
        {
            animator.SetTrigger("isDead");
            // NO destruyas aquí, espera al evento de la animación
        }
    }

    // Este método lo llamará el Animation Event al final de la animación de muerte
    public void DestroyEnemy()
    {
        EnemyManager.Instance.enemiesDestroyed++;
        Destroy(gameObject);
    }
}
