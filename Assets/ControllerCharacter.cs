
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public bool canJump = false;
    public float Impuls = 5f;
    public float velocity = 5f;
    private int xDirection = 0;
    public Rigidbody2D playerRB;

    void Start()
    {
        if (playerRB == null)
        {
            playerRB = GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            xDirection = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            xDirection = -1;
        }
        else
        {
            xDirection = 0;
        }

        if (Input.GetKeyDown(KeyCode.W) && canJump)
        {
            playerRB.AddForce(Vector2.up * Impuls, ForceMode2D.Impulse);
            canJump = false;
        }
    }

    void FixedUpdate()
    {
        playerRB.velocity = new Vector2(xDirection * velocity, playerRB.velocity.y);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GROUND"))
        {
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GROUND"))
        {
            canJump = false;
        }
    }
}
