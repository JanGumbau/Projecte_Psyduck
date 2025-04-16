using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJump : MonoBehaviour
{
    [SerializeField] public float Impuls = 7f;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public float fallMultiplier = 5.8f;
    [SerializeField] public float holdForce = 0.86f;
    [SerializeField] public float maxHoldTime = 0.5f;
private float holdTimer = 0f;
private float currentJumpTime = 0f;
    public Rigidbody2D rb;
    public bool canJump = true;
    private bool isHoldingJump = false;
    public float jumpTime = 0.2f;
    public float jumpMultiplier;
public bool isGrounded = false;

public RaycastHit2D groundHit;

private bool isJumping = false;




    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
    }


     void Update()
    {


    if (Input.GetKey(KeyCode.W) && canJump && isGrounded){
            rb.velocity = new Vector2(rb.velocity.x, Impuls);
            isJumping = true;
            holdTimer = 0f;
        }

    if (Input.GetKey(KeyCode.W) && isJumping)
        {
            holdTimer += Time.deltaTime;
    if (holdTimer <= maxHoldTime){
                
                float forceToAdd = (holdTimer / maxHoldTime) * holdForce;
                rb.AddForce(Vector2.up * forceToAdd);
            }
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
       
   
    }

private void OnCollisionEnter2D(Collision2D collision)
{
    if (((1 << collision.gameObject.layer) & groundLayer) != 0)
    {
        isGrounded = true;
        isJumping = false;
        isHoldingJump = false;
        canJump = true;
    }
    currentJumpTime = 0f;
}

private void OnCollisionExit2D(Collision2D collision)
{
    if (((1 << collision.gameObject.layer) & groundLayer) != 0)
    {
        isGrounded = false;
        isHoldingJump = false;
        currentJumpTime = 0f;
}
}
}

