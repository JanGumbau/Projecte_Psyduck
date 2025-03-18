using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCharacter : MonoBehaviour
{
    public bool canJump = false;
    public float Impuls = 5f;
    public float velocity = 5f;
    private int xDirection = 0;
    public Rigidbody2D playerRB;

    public BoxCollider2D HitboxRight;
    public BoxCollider2D HitboxLeft;

    public float hitboxDuration = 0.2f; // Tiempo de duración de la hitbox

    void Start()
    {
        if (playerRB == null)
        {
            playerRB = GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        // Movimiento horizontal
        if (Input.GetKey(KeyCode.D))
        {
            xDirection = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            xDirection = -1;
        }
        else
        {
            xDirection = 0;
        }

        // Salto
        if (Input.GetKeyDown(KeyCode.W) && canJump)
        {
            playerRB.AddForce(Vector2.up * Impuls, ForceMode2D.Impulse);
            canJump = false;
        }

        // Activación de la hitbox derecha
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HitboxRight.gameObject.SetActive(true);
            StartCoroutine(DeactivateHitbox(HitboxRight));
        }

        // Activación de la hitbox izquierda
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HitboxLeft.gameObject.SetActive(true);
            StartCoroutine(DeactivateHitbox(HitboxLeft));
        }
    }

    void FixedUpdate()
    {
        playerRB.velocity = new Vector2(xDirection * velocity, playerRB.velocity.y);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GROUND"))
        {
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GROUND"))
        {
            canJump = false;
        }
    }

    private IEnumerator DeactivateHitbox(BoxCollider2D hitbox)
    {
        yield return new WaitForSeconds(hitboxDuration);
        hitbox.gameObject.SetActive(false);
    }
}