using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = Unity.Mathematics.Random;

public class Beast : Enemy
{
    protected BehaviorCollider noticeCollider;
    protected BehaviorCollider attackCollider;
    protected Vector3 direction;
    
    public float moveSpeed;
    private bool chasingState = false;
    public bool attackingState = false;
    private Unity.Mathematics.Random random;

    private void Start()
    {
        SetupDamageableObject();
        noticeCollider = gameObject.transform.Find("NoticeCollider").GetComponent<BehaviorCollider>();
        attackCollider = gameObject.transform.Find("AttackCollider").GetComponent<BehaviorCollider>();
        damage = 5;
        knockBackForce = 10f;
        moveSpeed = 500f;
    }

    private void FixedUpdate()
    {
        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        
        if (attackCollider.playerInPosition)
        {
            animator.SetBool("Attack", true);
        }
        else
        {
            animator.SetBool("Attack", false);
        }

        if (noticeCollider.playerInPosition)
        {
            animator.SetBool("MakeMove", true);
        }
        else
        {
            animator.SetBool("MakeMove", false);
        }

        if (attackingState)
            return;
        if (chasingState)
            ChasePlayer();
    }

    public void StartChasingPlayer()
    {
        chasingState = true;
    }

    public void StopChasingPlayer()
    {
        chasingState = false;
    }

    public void StartAttackingPlayer()
    {
        attackingState = true;
        AttackPlayer();
    }

    public void StopAttackingPlayer()
    {
        attackingState = false;
    }

    public void RandomizeNextMove()
    {
        animator.SetBool("MoveType", UnityEngine.Random.value > 0.5);
    }

    public void AttackPlayer()
    {
        if (!attackingState)
            return;
        direction = (attackCollider.player.transform.position - transform.position).normalized;
        rb.AddForce(direction * (moveSpeed) / 2);
    }

    public void ChasePlayer()
    {
        if (!chasingState)
            return;
        direction = (noticeCollider.player.transform.position - transform.position).normalized;
        rb.AddForce(direction * (moveSpeed * Time.deltaTime));
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        var colObject = col.collider.GetComponent<PlayerObj>();
        if (colObject != null)
        {
            var player = col.collider.GetComponent<PlayerObj>();
            if (player != null)
            {
                var slimePosition = transform.position;
                var dir = (Vector2)(player.transform.position - slimePosition).normalized;
                player.ReceiveHit(damage, dir * knockBackForce);
            }
        }
    }
}