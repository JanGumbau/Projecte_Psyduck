using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJump : MonoBehaviour
{
    [SerializeField] private float Impuls = 0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float fallMultiplier = 0f;
    [SerializeField] private float raycastDistance = 0f;
    public AudioClip soundEffect; // Arrastra tu archivo de sonido aquí en el Inspector
    private AudioSource audioSource;

    public Rigidbody2D rb;

    public bool isJumping = false;
    public bool isGrounded = false;
    public bool canJump = false;
    public bool jumpPressed = false;
    public bool jumpReleased = false;
    private Collider2D col;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
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
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            jumpPressed = true;
            jumpReleased = false;
        }

        // Detectar si el botó W es deixa anar
        if (Input.GetKeyUp(KeyCode.Space) && isJumping)
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
            jumpPressed = false;

           
            if (soundEffect != null)
            {
                audioSource.PlayOneShot(soundEffect);
            }
            else
            {
                Debug.LogWarning("No hay un AudioClip asignado para el efecto de sonido.");
            }
        }

        // Reduir la força del salt si el botó es deixa anar ràpidament
        if (jumpReleased)
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
            jumpReleased = false;
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
            Vector2 leftRaycastOrigin = new Vector2(transform.position.x - col.bounds.extents.x, transform.position.y);
            Vector2 rightRaycastOrigin = new Vector2(transform.position.x + col.bounds.extents.x, transform.position.y);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(leftRaycastOrigin, leftRaycastOrigin + Vector2.down * raycastDistance);
            Gizmos.DrawLine(rightRaycastOrigin, rightRaycastOrigin + Vector2.down * raycastDistance);
        }
    }
}