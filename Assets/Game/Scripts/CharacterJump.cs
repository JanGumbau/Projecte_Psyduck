    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Audio;  // Para AudioMixerGroup

    public class CharacterJump : MonoBehaviour
    {
        [SerializeField] private float Impuls = 0f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float fallMultiplier = 0f;
        [SerializeField] private float raycastDistance = 0.5f;
        [SerializeField] private float coyoteTime = 0.25f;      // Tiempo de gracia después de salir del suelo
        [SerializeField] private float jumpBufferTime = 0.15f;  // Tiempo para guardar el input de salto

        public AudioClip soundEffect;
        [SerializeField] private AudioSource audioSource;

        [SerializeField] private AudioMixerGroup jumpSoundMixerGroup; // Asignar en inspector

        public Rigidbody2D rb;

        public bool isJumping = false;
        public bool isGrounded = false;
        public bool canJump = false;
        public bool jumpPressed = false;
        public bool jumpReleased = false;
        private Collider2D col;

        public bool allowExtraJump = false;

        // Coyote time & jump buffer
        private float coyoteTimeCounter = 0f;
        private float jumpBufferCounter = 0f;

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
                isJumping = false;

            // --- COYOTE TIME ---
            if (isGrounded)
                coyoteTimeCounter = coyoteTime;
            else
                coyoteTimeCounter -= Time.deltaTime;

            // --- JUMP BUFFER ---
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
                jumpBufferCounter = jumpBufferTime;
            else
                jumpBufferCounter -= Time.deltaTime;

            // --- JUMP REQUEST ---
            if ((jumpBufferCounter > 0 && coyoteTimeCounter > 0) || (canJump && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))))
            {
                jumpPressed = true;
                jumpBufferCounter = 0f;
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
                rb.velocity = new Vector2(rb.velocity.x, Impuls);
                isJumping = true;
                isGrounded = false;
                jumpPressed = false;
                coyoteTimeCounter = 0f; // Consume el coyote time

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
            // Si toca un enemigo en el aire, gana un salto extra
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
