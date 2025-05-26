using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerCharacter : MonoBehaviour
{
    public bool Pogo = false;
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

    public float hitboxDuration = 0.2f;

    public float raycastDistance = 0.1f;
    public LayerMask groundLayer;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool isAttacking = false;
    private float attackTimer = 0f;
    private BoxCollider2D activeHitbox = null;

    public float footstompImpulse = 10f;

    private bool enPortal = false;

    void Start()
    {
        if (playerRB != null)
        {
            playerRB = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }

        if (enPortal)
        {
            xDirection = 0;
            return; // Bloquea movimiento y ataques
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
        if (collision.gameObject.CompareTag("PINCHOS"))
        {
            ReiniciarNivel();
            return;
        }

        if (collision.gameObject.CompareTag("ENEMIC") || collision.gameObject.CompareTag("ENEMIC_AMARILLO"))
        {
            bool colisionDesdeArriba = false;

            foreach (ContactPoint2D contacto in collision.contacts)
            {
                if (contacto.normal.y > 0.5f)
                {
                    colisionDesdeArriba = true;
                    break;
                }
            }

            if (colisionDesdeArriba)
            {
                Rigidbody2D rbJugador = GetComponent<Rigidbody2D>();
                rbJugador.velocity = Vector2.zero;
                rbJugador.AddForce(Vector2.up * footstompImpulse, ForceMode2D.Impulse);

                Rigidbody2D rbEnemigo = collision.rigidbody;
                if (rbEnemigo != null)
                {
                    float fuerzaHaciaAbajo = -13f;
                    rbEnemigo.velocity = Vector2.zero;
                    rbEnemigo.AddForce(Vector2.up * fuerzaHaciaAbajo, ForceMode2D.Impulse);
                    Enemy enemic = collision.gameObject.GetComponent<Enemy>();
                    if (enemic != null)
                    {
                        enemic.RebreDany();
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Portal"))
        {
            enPortal = true;
            playerRB.velocity = Vector2.zero;
            xDirection = 0;
            


        }

        if ((other.gameObject.CompareTag("ENEMIC") || other.gameObject.CompareTag("ENEMIC_AMARILLO")) && HitboxDown.gameObject.activeSelf)
        {
            Enemy enemic = other.GetComponent<Enemy>();
            if (enemic != null)
            {
                enemic.RebreDany();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Portal"))
        {
            enPortal = false;
        }
    }
}
