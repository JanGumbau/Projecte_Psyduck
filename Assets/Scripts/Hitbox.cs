using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Vector2 hitboxforce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entro");
        if (collision.CompareTag("ENEMIC"))
        {            
            // Desactivar temporalmente el collider del enemigo
            Collider2D enemyCollider = collision.GetComponent<Collider2D>();
            if (enemyCollider != null)
            {
                //enemyCollider.enabled = false;

                // Aplicar fuerza al enemigo (si deseas que se mueva)
                Rigidbody2D enemyRB = collision.GetComponent<Rigidbody2D>();
                if (enemyRB != null)
                {
                    Debug.Log("Hit by: " + gameObject.name + "ForceX: " + hitboxforce.x + "ForceY: " + hitboxforce.y);
                    enemyRB.AddForce(hitboxforce, ForceMode2D.Impulse);
                }

                // Reactivar el collider del enemigo después de un tiempo
                StartCoroutine(ReactivateEnemyCollider(enemyCollider));
            }
        }
    }

    private IEnumerator ReactivateEnemyCollider(Collider2D enemyCollider)
    {
        yield return new WaitForSeconds(0.5f); // Espera para reactivar el collider
        enemyCollider.enabled = true;
    }
}