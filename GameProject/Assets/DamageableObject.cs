using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableObject : MonoBehaviour
{
    public float health = 1f;
    public bool targetable = false;
    public Animator animator;
    public Rigidbody2D rb;
    public bool invulnerability = false;
    protected bool canMove = true;

    private static readonly int Defeated1 = Animator.StringToHash("Defeated");
    private static readonly int Hit1 = Animator.StringToHash("Hit");

    public float Health
    {
        set
        {
            health = value;
            if (Health <= 0)
            {
                Defeated();
            }
        }
        get => health;
    }

    public void ReceiveHit(float damage, Vector2 knockbackForce)
    {
        print($"knockback_damage_received_by_{transform.tag}");
        if (!invulnerability)
            Health -= damage;
        rb.AddForce(knockbackForce);
        animator.SetTrigger(Hit1);
        canMove = true;
    }

    public void ReceiveHit(float damage)
    {
        canMove = false;
        Health -= damage;
        animator.SetTrigger(Hit1);
        canMove = true;
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    private void Defeated()
    {
        animator.SetTrigger(Defeated1);
    }

    public void RemoveObject()
    {
        Destroy(gameObject);
    }
}