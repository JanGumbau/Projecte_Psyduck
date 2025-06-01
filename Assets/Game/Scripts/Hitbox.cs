using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private Collider2D colliderToReactivate;
    private float reActivationTime;
    public Vector2 hitboxforce;

    private CharacterJump characterJump; // Referencia al script del personaje

    private void Start()
    {
        // Buscar el componente CharacterJump en el padre
        characterJump = GetComponentInParent<CharacterJump>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entro");
        if (collision.CompareTag("ENEMIC") || collision.CompareTag("ENEMIC_AMARILLO"))
        {
            // Si es ENEMIC_AMARILLO, permitir salto extra
            if (collision.CompareTag("ENEMIC_AMARILLO") && characterJump != null)
            {
                characterJump.allowExtraJump = true;
            }

            // Desactivar temporalmente el collider del enemigo
            Collider2D enemyCollider = collision.GetComponent<Collider2D>();
            if (enemyCollider != null)
            {
                // Aplicar fuerza al enemigo (si deseas que se mueva)
                Rigidbody2D enemyRB = collision.GetComponent<Rigidbody2D>();
                if (enemyRB != null)
                {
                    Debug.Log("Hit by: " + gameObject.name + " ForceX: " + hitboxforce.x + " ForceY: " + hitboxforce.y);
                    enemyRB.AddForce(hitboxforce, ForceMode2D.Impulse);
                    Enemy enemic = collision.GetComponent<Enemy>();
                    if (enemic != null)
                    {
                        enemic.RebreDany(); // Truca el mètode que fa el canvi de color
                    }
                }

                // Reactivar el collider del enemigo después de un tiempo
                colliderToReactivate = enemyCollider;
                reActivationTime = Time.time + 0.5f;
            }
        }
    }

    private void Update()
    {
        if (colliderToReactivate != null && Time.time > reActivationTime)
        {
            colliderToReactivate.enabled = true;
            colliderToReactivate = null;
        }
    }
}