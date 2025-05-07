using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJump : MonoBehaviour
{
    [SerializeField] private float Impuls = 0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float fallMultiplier = 0f;
    [SerializeField] private float raycastDistance = 0f;

    public Rigidbody2D rb;

    public bool isJumping = false;
    public bool isGrounded = false;
    public bool canJump = false;
    public bool jumpPressed = false;
    public bool jumpReleased = false; // Nova variable per detectar si el botó s'ha deixat anar
    private Collider2D col; // Per obtenir les dimensions del personatge

    private Collider2D col; // Per obtenir les dimensions del personatge


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>(); // Obtenir el Collider2D del personatge
    }

    void Update()
    {
        // Calcular les posicions dels extrems esquerre i dret del personatge
        Vector2 leftRaycastOrigin = new Vector2(transform.position.x - col.bounds.extents.x, transform.position.y);
        Vector2 rightRaycastOrigin = new Vector2(transform.position.x + col.bounds.extents.x, transform.position.y);

        // Llançar els Raycasts des dels extrems
        RaycastHit2D leftHit = Physics2D.Raycast(leftRaycastOrigin, Vector2.down, raycastDistance, groundLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(rightRaycastOrigin, Vector2.down, raycastDistance, groundLayer);

        // Actualitzar l'estat de isGrounded basant-nos en els Raycasts
        isGrounded = leftHit.collider != null || rightHit.collider != null;

        if (isGrounded)
        {
            canJump = true;
            isJumping = false;
        }
        else
        {
            canJump = false;
        }

        // Capturar l'entrada de salt
        if (Input.GetKeyDown(KeyCode.W) && canJump)
        {
            jumpPressed = true;
            jumpReleased = false; // Restablir l'estat de jumpReleased
        }

        // Detectar si el botó W es deixa anar
        if (Input.GetKeyUp(KeyCode.W) && isJumping)
        {
            jumpReleased = true;
        }
    }

    void FixedUpdate()
    {
        if (jumpPressed)
        {
            rb.velocity = new Vector2(rb.velocity.x, Impuls);
            isJumping = true;
            isGrounded = false;
            jumpPressed = false; // Restablir l'estat del botó de salt
        }

        // Reduir la força del salt si el botó es deixa anar ràpidament
        if (jumpReleased)
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); // Reduir la velocitat vertical
            }
            jumpReleased = false; // Restablir l'estat de jumpReleased
        }

        // Aplicar multiplicador de caiguda
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    void OnDrawGizmos()
    {
        if (col != null)
        {
            // Calcular les posicions dels extrems esquerre i dret del personatge
            Vector2 leftRaycastOrigin = new Vector2(transform.position.x - col.bounds.extents.x, transform.position.y);
            Vector2 rightRaycastOrigin = new Vector2(transform.position.x + col.bounds.extents.x, transform.position.y);

            // Dibuixar els Raycasts al Scene View per depuració
            Gizmos.color = Color.red;
            Gizmos.DrawLine(leftRaycastOrigin, leftRaycastOrigin + Vector2.down * raycastDistance);
            Gizmos.DrawLine(rightRaycastOrigin, rightRaycastOrigin + Vector2.down * raycastDistance);
        }
    }
}