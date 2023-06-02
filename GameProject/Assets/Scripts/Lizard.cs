using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;
using Random = UnityEngine.Random;

public class Lizard : Enemy
{
    private Vector3 playerOffset;
    private Collider2D collider1;
    private Collider2D collider2;
    private Vector2 direction;
    private Vector3 leftDirection;
    private Vector3 rightDirection;
    private Vector3 playerPosition;
    private Vector3 directionToPlayer;
    private Vector3 thisPosition;
    private static readonly int Attack1 = Animator.StringToHash("Attack1");
    private static readonly int Attack2 = Animator.StringToHash("Attack2");
    private int attackNumber;
    private bool attackState;
    private Collider2D[] overlapColliders;
    private static readonly int Move = Animator.StringToHash("Move");
    private ContactFilter2D contactFilter;
    [SerializeField] private LayerMask playerLayerMask;
    
    private Vector2 collider1Offset;
    private Vector2 collider2Offset;
    private Collider2D currentCollider;

    void Start()
    {
        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(playerLayerMask);
        overlapColliders = new Collider2D[1];
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerOffset = new Vector3(0.3f, 0, 0);
        collider1 = transform.Find("SwordCollider1").GetComponent<Collider2D>();
        collider2 = transform.Find("SwordCollider2").GetComponent<Collider2D>();
        collider1Offset = collider1.offset;
        collider2Offset = collider2.offset;
        
        SetupDamageableObject();
        SetHealth(150);
        damage = 15;
        knockBackForce = 30f;
        moveSpeed = 500f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (attackState || defeated)
            return;
        playerPosition = player.position;
        thisPosition = transform.position;
        leftDirection = playerPosition - playerOffset - thisPosition;
        rightDirection = playerPosition + playerOffset - thisPosition;
        direction = leftDirection.magnitude > rightDirection.magnitude ? rightDirection : leftDirection;
        if (direction.magnitude < 0.1)
            AttackPlayer();
        else
        {
            ChasePlayer();
        }
    }
    
    public void ChasePlayer()
    {
        direction.Normalize();
        animator.SetBool(Move, true);
        spriteRenderer.flipX = direction.x < 0;
        rb.AddForce(direction * (moveSpeed * Time.deltaTime));
    }

    public void AttackPlayer()
    {
        EnterAttackState();
        directionToPlayer = playerPosition - thisPosition;
        
        attackNumber = Random.Range(1, 3);
        Vector2 currentOffset;
        if (attackNumber == 1)
        {
            animator.SetTrigger(Attack1);
            currentOffset = collider1Offset;
            currentCollider = collider1;
        }
        else
        {
            animator.SetTrigger(Attack2);
            currentOffset = collider2Offset;
            currentCollider = collider2;
        }
        
        if (directionToPlayer.x < 0)
        {
            spriteRenderer.flipX = true;
            currentCollider.offset = new Vector2(-Math.Abs(currentOffset.x), currentOffset.y);
        }
        else
        {
            spriteRenderer.flipX = false;
            currentCollider.offset = new Vector2(Math.Abs(currentOffset.x), currentOffset.y);
        }
    }

    public void EnterAttackState()
    {
        attackState = true;
    }

    public void LeaveAttackState()
    {
        attackState = false;
    }

    public void Attack()
    {
        currentCollider = attackNumber == 1 ? collider1 : collider2;
        Array.Clear(overlapColliders, 0, overlapColliders.Length);
        currentCollider.OverlapCollider(contactFilter, overlapColliders);
        foreach (var obj in overlapColliders.Where(item => item is not null))
        {
            var playerObj = obj.GetComponentInParent<PlayerController>();
            if (playerObj is not null)
            {
                playerObj.ReceiveHit(damage, directionToPlayer.normalized * knockBackForce);
            }
        }
    }
}
