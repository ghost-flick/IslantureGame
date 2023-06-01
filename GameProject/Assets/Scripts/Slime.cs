using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = Unity.Mathematics.Random;

public class Slime : Enemy, IBehaviour
{
    protected Vector3 direction;
    public bool chasingState = false;
    public bool attackingState = false;
    private Unity.Mathematics.Random random;
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Move = Animator.StringToHash("Move");
    public bool processingChase;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        SetupDamageableObject();
        SetHealth(100);
        damage = 5;
        knockBackForce = 10f;
        moveSpeed = 500f;
    }


    public void ChangeState(int animationID, bool value)
    {
        if (animationID == Attack)
        {
            attackingState = value;
        }
        else if (animationID == Move)
        {
            chasingState = value;
            if (value && !processingChase)
                StartCoroutine(ChasePlayer());
        }

        ;
    }

    private void CalculateDirection()
    {
        direction = (player.position - transform.position).normalized;
        spriteRenderer.flipX = direction.x < 0;
    }

    public IEnumerator ChasePlayer()
    {
        processingChase = true;
        while (chasingState)
        {
            if (attackingState)
            {
                animator.SetTrigger(Attack);
                yield return new WaitForSeconds(2);
                continue;
            }

            animator.SetTrigger(Move);
            yield return new WaitForSeconds(1f);
        }

        processingChase = false;
    }

    public void AttackPlayer()
    {
        CalculateDirection();
        rb.AddForce(direction * (moveSpeed / 2));
    }

    public void MoveToPlayer()
    {
        CalculateDirection();
        rb.AddForce(direction * (moveSpeed / 3));
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        var playerObj = col.collider.GetComponentInParent<PlayerObj>();
        if (playerObj != null)
        {
            var slimePosition = transform.position;
            var dir = (Vector2)(playerObj.transform.position - slimePosition).normalized;
            playerObj.ReceiveHit(damage, dir * knockBackForce);
        }
    }
}