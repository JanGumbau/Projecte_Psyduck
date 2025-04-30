using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJump : MonoBehaviour
{
    [SerializeField] private float Impuls = 0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float fallMultiplier = 0f;
    [SerializeField] private float holdForce = 0f;
    [SerializeField] private float maxHoldTime = 0f;
    [SerializeField] private float raycastDistance = 0f; // Dist�ncia del raycast per detectar el terra

    private float holdTimer = 0f;
    
    public Rigidbody2D rb;
  
    private bool isJumping = false;
    private bool isGrounded = false;
    private bool canJump = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Detectar si el jugador est� tocant el terra amb un Raycast
        Vector2 raycastOrigin = new Vector2(transform.position.x, transform.position.y - 0.5f); // Llan�ar el Raycast des d'una posici� lleugerament inferior
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, raycastDistance, groundLayer);

        // Actualitzar l'estat de isGrounded basant-nos en el Raycast
        isGrounded = hit.collider != null;



        if (isGrounded)
        {
            canJump = true;
            isJumping = false;
            
        }
        else
        {
            canJump = false; // Assegurar que no es pot saltar si no est� tocat el terra
        }

        if (Input.GetKey(KeyCode.W) && canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, Impuls);
            isJumping = true;
            holdTimer = 0f;
            isGrounded = false;
        }

        if (Input.GetKey(KeyCode.W) && isJumping)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer <= maxHoldTime)
            {
                float forceToAdd = (holdTimer / maxHoldTime) * holdForce;
                rb.AddForce(Vector2.up * forceToAdd);
            }
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }

    void OnDrawGizmos()
    {
        // Dibuixar el Raycast al Scene View per depuraci�
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastDistance);

    }
}