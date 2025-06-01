using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CharacterJump : MonoBehaviour
{
    [SerializeField] private float Impuls = 0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float fallMultiplier = 0f;
    [SerializeField] private float raycastDistance = 0.5f;
    [SerializeField] private float coyoteTime = 0.25f;
    [SerializeField] private float jumpBufferTime = 0.15f;

    public AudioClip soundEffect;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioMixerGroup jumpSoundMixerGroup;

    public Rigidbody2D rb;

    public bool isJumping = false;
    public bool isGrounded = false;
    public bool canJump = false;
    public bool jumpPressed = false;
    public bool jumpReleased = false;
    private Collider2D col;

    public bool allowExtraJump = false;

    private float coyoteTimeCounter = 0f;
    private float jumpBufferCounter = 0f;

    private bool canDoubleJump = false;
    private bool hasDoubleJumped = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (jumpSoundMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = jumpSoundMixerGroup;
        }
    }

    void Update()
    {
        Vector2 leftRaycastOrigin = new Vector2(transform.position.x - col.bounds.extents.x, transform.position.y);
        Vector2 rightRaycastOrigin = new Vector2(transform.position.x + col.bounds.extents.x, transform.position.y);

        RaycastHit2D leftHit = Physics2D.Raycast(leftRaycastOrigin, Vector2.down, raycastDistance, groundLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(rightRaycastOrigin, Vector2.down, raycastDistance, groundLayer);

        isGrounded = leftHit.collider != null || rightHit.collider != null;

        if (isGrounded && rb.velocity.y <= 0.01f)
        {
            isJumping = false;
            hasDoubleJumped = false;
            allowExtraJump = false; // ✅ Resetear al tocar suelo
        }

        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        if (
            (jumpBufferCounter > 0 && coyoteTimeCounter > 0) ||
            (canJump && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))) ||
            (!isGrounded && allowExtraJump && !hasDoubleJumped && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)))
        )
        {
            jumpPressed = true;
            jumpBufferCounter = 0f;

            if (!isGrounded && coyoteTimeCounter <= 0 && allowExtraJump && !hasDoubleJumped)
            {
                hasDoubleJumped = true;
                allowExtraJump = false; // ✅ Se consume al usar el doble salto
            }
        }

        if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W)) && isJumping)
        {
            jumpReleased = true;
        }
    }

    void FixedUpdate()
    {
        if (jumpPressed)
        {
            float finalImpuls = Impuls;

            if (hasDoubleJumped && !isGrounded)
            {
                finalImpuls *= 1.2f;
            }

            rb.velocity = new Vector2(rb.velocity.x, finalImpuls);
            isJumping = true;
            isGrounded = false;
            jumpPressed = false;
            coyoteTimeCounter = 0f;

            PlayJumpSound();
        }

        if (jumpReleased)
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
            jumpReleased = false;
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ✅ Solo da doble salto si estás en el aire
        if (collision.gameObject.CompareTag("ENEMIC_AMARILLO") && !isGrounded)
        {
            allowExtraJump = true;
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

    private void PlayJumpSound()
    {
        if (soundEffect != null)
        {
            audioSource.PlayOneShot(soundEffect);
        }
        else
        {
            Debug.LogWarning("No hay un AudioClip asignado para el efecto de sonido.");
        }
    }
}
