using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ControllerCharacter))]
[RequireComponent(typeof(CharacterJump))]

public class Animations : MonoBehaviour
{
    private Animator animator;
    private ControllerCharacter characterMovement;
    private CharacterJump characterJump;
    private Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterMovement = GetComponent<ControllerCharacter>();
        characterJump = GetComponent<CharacterJump>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Movimiento horizontal (Idle y Run)
        bool isRunning = Mathf.Abs(rb.velocity.x) > 0.1f;
        animator.SetBool("isRunning", isRunning);

        // Salto (Idle y Jump)
        animator.SetBool("isJumping", characterJump.isJumping);

        // Ataque espameable
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            animator.SetTrigger("isAttacking");
        }
    }
}