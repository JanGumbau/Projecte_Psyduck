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

    void Start()
    {
        if (playerRB != null)
        {
            playerRB = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
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

       

        //Atac
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

        // Activación de la hitbox de arriba
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            HitboxUp.gameObject.SetActive(true);
            StartCoroutine(DeactivateHitbox(HitboxUp));
        }

        // Activación de la hitbox de abajo
        if (Input.GetKeyDown(KeyCode.DownArrow) )
        {
            
           
            HitboxDown.gameObject.SetActive(true);
            StartCoroutine(DeactivateHitbox(HitboxDown));

            
            
        }
        if (Pogo)
        {
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

//void OnCollisionStay2D(Collision2D collision)
//{
//    if (collision.gameObject.CompareTag("GROUND"))
//    {
//        canJump = true;
//    }
//}

//void OnCollisionExit2D(Collision2D collision)
//{
//    if (collision.gameObject.CompareTag("GROUND"))
//    {
//        canJump = false;
//    }
//}
    void OnCollisionEnter2D(Collision2D collision)
    {
            if (collision.gameObject.CompareTag("PINCHOS") || collision.gameObject.CompareTag("ENEMIC"))
            ReiniciarNivel();
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
        if (other.CompareTag("RedSlime"))
        {
            other.GetComponent<RedSlime>().TakeDamage(999, gameObject);
        }
    }
}
