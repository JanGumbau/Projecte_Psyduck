using System.Collections;
using System.Collections.Generic;
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

    public float attackCooldown = 0.3f;

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
        else
        {
            xDirection = 0;
        }

        // Ataque
        if (!isAttacking && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            StartCoroutine(HandleAttack());
        }

        if (Pogo)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, 0); // Resetear la velocidad en Y
            playerRB.AddForce(Vector3.up * Impuls);
            Pogo = false;
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
        if (collision.gameObject.CompareTag("PINCHOS") || collision.gameObject.CompareTag("ENEMIC"))
            ReiniciarNivel();
    }

    IEnumerator HandleAttack()
    {
        isAttacking = true;

        // Activar la animación de ataque
        animator.SetTrigger("isAttacking");

        // Activar la hitbox correspondiente
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HitboxRight.gameObject.SetActive(true);
            StartCoroutine(DeactivateHitbox(HitboxRight));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HitboxLeft.gameObject.SetActive(true);
            StartCoroutine(DeactivateHitbox(HitboxLeft));
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            HitboxUp.gameObject.SetActive(true);
            StartCoroutine(DeactivateHitbox(HitboxUp));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            HitboxDown.gameObject.SetActive(true);
            StartCoroutine(DeactivateHitbox(HitboxDown));
        }

        // Esperar el tiempo de cooldown
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    IEnumerator DeactivateHitbox(BoxCollider2D hitbox)
    {
        yield return new WaitForSeconds(hitboxDuration);
        hitbox.gameObject.SetActive(false);
    }

    void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia el nivel
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Comprova si col·lisiona amb un enemic quan ataca avall
        if (other.gameObject.CompareTag("ENEMIC") && HitboxDown.gameObject.activeSelf)
        {
            // Realitza el Pogo: afegeix l'impuls cap amunt
            playerRB.velocity = new Vector2(playerRB.velocity.x, 0); // Reseteja la velocitat vertical
            playerRB.AddForce(Vector2.up * PogoImpuls);
        }
    }
}