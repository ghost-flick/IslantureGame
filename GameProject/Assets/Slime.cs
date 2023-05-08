using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = Unity.Mathematics.Random;

public class Slime : Enemy
{
    public SlimeBehavior slimeBehavior;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        damage = 1f;
        knockbackForce = 10f;
    }

    private void FixedUpdate()
    {
        if (slimeBehavior.direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (slimeBehavior.direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void StartChasingPlayer()
    {
        slimeBehavior.chasingState = true;
    }

    public void StopChasingPlayer()
    {
        slimeBehavior.chasingState = false;
    }

    public void StartAttackingPlayer()
    {
        slimeBehavior.attackingState = true;
        slimeBehavior.AttackPlayer();
    }

    public void StopAttackingPlayer()
    {
        slimeBehavior.attackingState = false;
    }

    public void RandomizeNextMove()
    {
        slimeBehavior.slimeAnimator.SetBool("MoveType", UnityEngine.Random.value > 0.5);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        print("bumped into slime!");
        var colObject = col.collider.GetComponent<PlayerObj>();
        if (colObject != null)
        {
            var player = col.collider.GetComponent<PlayerController>();
            if (player != null)
            {
                var slimePosition = transform.position;
                var direction = (Vector2)(player.transform.position - slimePosition).normalized;
                player.ReceiveHit(damage, direction * knockbackForce);
            }
        }
    }
}