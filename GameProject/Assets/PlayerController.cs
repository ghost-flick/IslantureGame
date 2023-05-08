using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : PlayerObj
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;

    private Vector2 movementInput;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D playerRB;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerRB = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        if (!canMove)
            return;
        // <param name="contactFilter">.</param>
        // <param name="results">Array to receive results.</param>
        // <param name="distance">Maximum distance over which to cast the Collider(s).</param>
        // If movement input is not 0, try to move
        if (movementInput != Vector2.zero)
        {
            var success = TryMove(movementInput);

            if (!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));
            }

            if (!success)
            {
                success = TryMove((new Vector2(0, movementInput.y)));
            }

            animator.SetBool("isMoving", success);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        // set direction of sprite to movement direction
        if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return false;
        var count = rb.Cast(
            direction, // Vector representing the direction to cast each Collider2D shape
            movementFilter, // Filter results defined by the contact filter
            castCollisions, // List of collisions to store the found collisions into 
            moveSpeed * Time.fixedDeltaTime + collisionOffset); // the amount to cast
        rb.MovePosition(rb.position + direction * (moveSpeed * Time.fixedDeltaTime));

        return count == 0;
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }

    void SwordAttack()
    {
        LockMovement();
        if (spriteRenderer.flipX == true)
        {
            swordAttack.isAttackLeft = false;
        }
        else
        {
            swordAttack.isAttackLeft = true;
        }

        swordAttack.Attack();
    }

    void EndSwordAttack()
    {
        UnlockMovement();
    }
}