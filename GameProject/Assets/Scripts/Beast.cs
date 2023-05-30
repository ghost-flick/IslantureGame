using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = Unity.Mathematics.Random;

public class Beast : Enemy, IBehaviour
{
    protected Vector3 direction;
    public float moveSpeed;
    public bool attackingState = false;
    public bool chasingState = false;
    private Unity.Mathematics.Random random;
    private float timePassed = 0;
    [SerializeField] public PlayerController player;
    private static readonly int Xdir = Animator.StringToHash("Xdir");
    private static readonly int Ydir = Animator.StringToHash("Ydir");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Move = Animator.StringToHash("Move");
    private bool processingAttack;
    private bool processingChase;

    private void Start()
    {
        SetupDamageableObject();
        SetHealth(50);
        damage = 5;
        knockBackForce = 10f;
        moveSpeed = 500f;
        
    }

    public void ChangeState(int animationID, bool value)
    {
        animator.SetBool(animationID, value);
        if (animationID == Attack)
        {
            attackingState = value;
            if (value && !processingAttack)
            {
                StartCoroutine(AttackPlayer());
            }
        }
        else if (animationID == Move)
        {
            chasingState = value;
            if (value && !processingChase)
            {
                chasingState = true;
                StartCoroutine(ChasePlayer());
            }
        }
    }
    
    public IEnumerator AttackPlayer()
    {
        processingAttack = true;
        while (attackingState)
        {
            CalculateDirection();
            SetAnimatorXY();
            while (timePassed <= 0.5)
            {
                timePassed += Time.deltaTime;
                rb.AddForce(direction * (moveSpeed * Time.deltaTime*5));
                yield return new WaitForSeconds(0.001f);
            }
            timePassed = 0;
            canMove = false;
            yield return new WaitForSeconds(1);
            canMove = true;
        }

        processingAttack = false;
    }

    private void SetAnimatorXY()
    {
        animator.SetFloat(Xdir, direction.x);
        animator.SetFloat(Ydir, direction.y);
    }

    private void CalculateDirection()
    {
        direction = (player.transform.position - transform.position).normalized;
    }

    public IEnumerator ChasePlayer()
    {
        processingChase = true;  
        
        while (chasingState)
        {
            if (attackingState || !canMove)
            {
                yield return new WaitForSeconds(1);
                continue;
            }
            CalculateDirection();
            SetAnimatorXY();
            rb.AddForce(direction * (moveSpeed * Time.deltaTime));
            yield return null;
        }

        processingChase = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        var colObject = col.collider.GetComponent<PlayerObj>();
        if (colObject != null)
        {
            var playerObj = col.collider.GetComponent<PlayerObj>();
            if (playerObj != null)
            {
                var slimePosition = transform.position;
                var dir = (Vector2)(playerObj.transform.position - slimePosition).normalized;
                playerObj.ReceiveHit(damage, dir * knockBackForce);
            }
        }
    }


    private void ProcessDefeat()
    {
        StopAllCoroutines();
    }
}