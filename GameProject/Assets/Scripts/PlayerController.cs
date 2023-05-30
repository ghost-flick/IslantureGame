using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : PlayerObj
{
    public UnityEvent actions;
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    protected SwordAttack swordAttack;
    protected Interaction interaction;
    private float lastXInput;
    private float lastYInput;
    private Vector2 movementInput;
    private readonly List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    public static PlayerController Instance;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        swordAttack = GetComponentInChildren<SwordAttack>();
        interaction = GetComponentInChildren<Interaction>();
        SetupDamageableObject();
        SetHealth(50);
        
        targetable = true;
        invulnerable = false;
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
            lastXInput = movementInput.x;
            lastYInput = movementInput.y;
            var success = TryMove(movementInput);

            if (!success)
            {
                success = TryMove(new Vector2(lastXInput, 0));
            }

            if (!success)
            {
                success = TryMove((new Vector2(0, lastYInput)));
            }

            animator.SetBool("isMoving", success);
            animator.SetFloat("XInput", lastXInput);
            animator.SetFloat("YInput", lastYInput);
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
        if (!GameStateController.NormalMode)
            return;
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire()
    {
        if (!GameStateController.NormalMode)
            return;
        animator.SetTrigger("swordAttack");
    }

    void OnInteract()
    {
        
        if (GameStateController.NormalMode)
            interaction.Interact(lastXInput, lastYInput);
        else 
            actions.Invoke();
    }

    public void EnterDialog()
    {
        animator.SetBool("isMoving", false);
    }

    void SwordAttack()
    {
        LockMovement();
        swordAttack.Attack(lastXInput, lastYInput);
    }

    void EndSwordAttack()
    {
        UnlockMovement();
    }
}