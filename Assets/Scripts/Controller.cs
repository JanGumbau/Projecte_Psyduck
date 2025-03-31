using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCharacter : MonoBehaviour
{
    public bool canJump = false;
    public float Impuls = 5f;
    public float velocity = 5f;
    private int xDirection = 0;
    [SerializeField]
    private Rigidbody2D playerRB;
    private Animator animator;
    private bool isAttacking = false;

    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 10;
    public float attackCooldown = 0.3f;
    public LayerMask enemyLayers;

    public BoxCollider2D HitboxRight;
    public BoxCollider2D HitboxLeft;

    public float hitboxDuration = 0.2f; // Tiempo de duración de la hitbox

    void Start()
    {
        if (playerRB != null)
        {
            playerRB = GetComponent<Rigidbody2D>();
        }

        animator = GetComponent<Animator>();
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("isRunning", true);
            xDirection = 1;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            xDirection = -1;
            animator.SetBool("isRunning", true);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            xDirection = 0;
            animator.SetBool("isRunning", false);
        }

        // Salto
        if (Input.GetKeyDown(KeyCode.W) && canJump)
        {
            animator.SetBool("isJumping", true);
            playerRB.AddForce(Vector3.up * Impuls);
            canJump = false;
        }

        else
        {
            animator.SetBool("isJumping", false);
        }

     
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                StartCoroutine(PerformAttack());
                animator.SetBool("isAttacking", true);
                HitboxRight.gameObject.SetActive(true);
                StartCoroutine(DeactivateHitbox(HitboxRight));
                animator.SetBool("isAttacking", false);
            }

            // Activación de la hitbox izquierda
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                StartCoroutine(PerformAttack());
                animator.SetBool("isAttacking", true);
                HitboxLeft.gameObject.SetActive(true);
                StartCoroutine(DeactivateHitbox(HitboxLeft));

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


    IEnumerator PerformAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        // Determinar dirección del ataque
        Vector2 attackDirection = Vector2.right * transform.localScale.x;
        if (Input.GetAxisRaw("Vertical") > 0) attackDirection = Vector2.up;
        if (Input.GetAxisRaw("Vertical") < 0) attackDirection = Vector2.down;

        // Mover el attackPoint en la dirección correcta
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
}