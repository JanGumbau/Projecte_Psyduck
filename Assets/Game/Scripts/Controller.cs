using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerCharacter : MonoBehaviour
{
    public bool Pogo = false;
    private float Impuls = 5f;
    public float PogoImpuls = 5f;
    public float velocity = 5f;
    private int xDirection = 0;
    [SerializeField]
    private Rigidbody2D playerRB;

    public float attackCooldown = 0.5f;

    public BoxCollider2D HitboxRight;
    public BoxCollider2D HitboxLeft;
    public BoxCollider2D HitboxUp;
    public BoxCollider2D HitboxDown;

    public float hitboxDuration = 0.2f; // Tiempo de duración de la hitbox

    public float raycastDistance = 0.1f;
    public LayerMask groundLayer;

    private SpriteRenderer spriteRenderer;
    private Animator animator; // Referencia al Animator

    private bool isAttacking = false; // Control del estado de ataque
    private float attackTimer = 0f;
    private float hitboxTimer = 0f;
    private BoxCollider2D activeHitbox = null;

    public float footstompImpulse = 10f;

    void Start()
    {
        if (playerRB != null)
        {
            playerRB = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        animator = GetComponent<Animator>(); // Inicializar el Animator
    }

    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }

        if (Input.GetKey(KeyCode.D))
        {
            xDirection = 1;
            spriteRenderer.flipX = false;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            xDirection = -1;
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKey(KeyCode.R))
        {
           ReiniciarNivel();
        }
       

        else
        {
            xDirection = 0;
        }

        // Ataque
        if (!isAttacking && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            HandleAttack();
        }
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                isAttacking = false;
                if (activeHitbox != null)
                {
                    activeHitbox.gameObject.SetActive(false);
                    activeHitbox = null;
                }
            }
        }
        //if (Pogo)
        //{
        //    playerRB.velocity = new Vector2(playerRB.velocity.x, 0); // Resetear la velocidad en Y
        //    playerRB.AddForce(Vector3.up * Impuls);
        //    Pogo = false;
        //}

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, groundLayer);
    }

    void FixedUpdate()
    {
        playerRB.velocity = new Vector2(xDirection * velocity, playerRB.velocity.y);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastDistance);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Si colisiona con pinchos, siempre reinicia el nivel
        if (collision.gameObject.CompareTag("PINCHOS"))
        {
            ReiniciarNivel();
            return;
        }

        // Si colisiona con un enemigo
        if (collision.gameObject.CompareTag("ENEMIC"))
        {
            bool colisionDesdeArriba = false;

            // Verificar todos los puntos de contacto
            foreach (ContactPoint2D contacto in collision.contacts)
            {
                // La normal apunta hacia arriba (desde la perspectiva del enemigo)
                if (contacto.normal.y > 0.5f)
                {
                    colisionDesdeArriba = true;
                    break;
                }
            }

            if (colisionDesdeArriba)
            {
                // ✅ Rebote del jugador hacia arriba de forma consistente
                Rigidbody2D rbJugador = GetComponent<Rigidbody2D>();
                rbJugador.velocity = Vector2.zero; // Reset completo
                rbJugador.AddForce(Vector2.up * footstompImpulse, ForceMode2D.Impulse);

                // ✅ Impulsar al enemigo solo en el eje Y (negativo)
                Rigidbody2D rbEnemigo = collision.rigidbody;
                if (rbEnemigo != null)
                {
                    float fuerzaHaciaAbajo = -13f; // Ajusta este valor según lo que quieras
                    rbEnemigo.velocity = Vector2.zero; // Reset completo
                    rbEnemigo.AddForce(Vector2.up * fuerzaHaciaAbajo, ForceMode2D.Impulse); // Solo eje Y negativo
                    Enemy enemic = collision.gameObject.GetComponent<Enemy>();
                    if (enemic != null)
                    {
                        enemic.RebreDany(); // Fa que canvii de color
                    }
                }
            }
            else
            {
                ReiniciarNivel();
            }
        }
    }

    void HandleAttack()
    {
        isAttacking = true;
        attackTimer = attackCooldown;

        animator.SetTrigger("isAttacking");

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ActivateHitbox(HitboxRight);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ActivateHitbox(HitboxLeft);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ActivateHitbox(HitboxUp);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ActivateHitbox(HitboxDown);
        }
    }

    void ActivateHitbox(BoxCollider2D hitbox)
    {
        hitbox.gameObject.SetActive(true);
        activeHitbox = hitbox;
        
       
    }
    void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia el nivel
    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    // Comprova si col·lisiona amb un enemic quan ataca avall
    //    if (other.gameObject.CompareTag("ENEMIC") && HitboxDown.gameObject.activeSelf)
    //    {
    //        // Realitza el Pogo: afegeix l'impuls cap amunt
    //        playerRB.velocity = new Vector2(playerRB.velocity.x, 0); // Reseteja la velocitat vertical
    //        playerRB.AddForce(Vector2.up * PogoImpuls);
    //    }
    //}
}