using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerCharacter : MonoBehaviour
{
    public bool canJump = false;
    public float Impuls = 5f;
    public float velocity = 5f;
    private int xDirection = 0;
    [SerializeField]
    private Rigidbody2D playerRB;

    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public float attackCooldown = 0.3f;

    public BoxCollider2D HitboxRight;
    public BoxCollider2D HitboxLeft;
    public BoxCollider2D HitboxUp;
    public BoxCollider2D HitboxDown;

    public float hitboxDuration = 0.2f; // Tiempo de duraci�n de la hitbox

    void Start()
    {
        if (playerRB != null)
        {
            playerRB = GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.D))
        {
            xDirection = 1;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            xDirection = -1;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            xDirection = 0;
        }

        // Salto
        if (Input.GetKeyDown(KeyCode.W) && canJump)
        {
            playerRB.AddForce(Vector3.up * Impuls);
            canJump = false;
        }

        //Atac
        // Activaci�n de la hitbox derecha
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HitboxRight.gameObject.SetActive(true);
            StartCoroutine(DeactivateHitbox(HitboxRight));
        }

        // Activaci�n de la hitbox izquierda
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HitboxLeft.gameObject.SetActive(true);
            StartCoroutine(DeactivateHitbox(HitboxLeft));

        }

        // Activaci�n de la hitbox de arriba
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            HitboxUp.gameObject.SetActive(true);
            StartCoroutine(DeactivateHitbox(HitboxUp));
        }

        // Activaci�n de la hitbox de abajo
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            HitboxDown.gameObject.SetActive(true);
            StartCoroutine(DeactivateHitbox(HitboxDown));
        }
    }

    void FixedUpdate()
    {
        playerRB.velocity = new Vector2(xDirection * velocity, playerRB.velocity.y);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GROUND"))
        {
            canJump = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GROUND"))
        {
            canJump = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PINCHOS") || collision.gameObject.CompareTag("ENEMIC"))
            ReiniciarNivel();
    }

    IEnumerator PerformAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        // Determinar direcci�n del ataque
        Vector2 attackDirection = Vector2.right * transform.localScale.x;
        if (Input.GetAxisRaw("Vertical") > 0) attackDirection = Vector2.up;
        if (Input.GetAxisRaw("Vertical") < 0) attackDirection = Vector2.down;

        // Mover el attackPoint en la direcci�n correcta
        attackPoint.localPosition = attackDirection * 0.5f;

        // Detectar enemigos en la hitbox
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        //foreach (Collider2D enemy in hitEnemies)
        //{
        //    enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        //}

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);


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


}