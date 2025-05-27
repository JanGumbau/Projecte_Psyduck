using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyManager enemyManager;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public static int enemiesDestroyed;
    private Animator animator;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip deathSound;

    private bool hasDied = false; 

    void Awake()
    {
        EnemyManager.Instance.enemiesCount++;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        animator = GetComponent<Animator>();
    }

    public void RebreDany()
    {
        if (!hasDied)
        {
            StartCoroutine(CanviColorTemporal(Color.red, 0.2f));
        }
    }

    private IEnumerator CanviColorTemporal(Color nouColor, float duracio)
    {
        spriteRenderer.color = nouColor;
        yield return new WaitForSeconds(duracio);
        spriteRenderer.color = originalColor;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasDied) return;

        if (collision.collider.CompareTag("PINCHOS") || collision.collider.CompareTag("ENEMIC") || collision.collider.CompareTag("ENEMIC_AMARILLO"))
        {
            hasDied = true;
            animator.SetTrigger("isDead");

            if (audioSource != null && deathSound != null)
            {
                audioSource.PlayOneShot(deathSound);
            }
        }
    }

    
    public void DestroyEnemy()
    {
        EnemyManager.Instance.enemiesDestroyed++;
        Destroy(gameObject);
    }
}
