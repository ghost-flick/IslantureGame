using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = Unity.Mathematics.Random;

public class Dragon : Enemy, IBehaviour
{
    public Vector2 direction;
    public bool attackingState = false;
    public bool chasingState = false;
    private static readonly int Xdir = Animator.StringToHash("Xdir");
    private static readonly int Ydir = Animator.StringToHash("Ydir");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Move = Animator.StringToHash("Move");
    private bool processingAttack;
    private bool processingChase;
    private GameObject fire;
    [SerializeField] private GameObject bigFireBall;
    [SerializeField] private GameObject littleFireBall;
    private Quaternion baseRotation;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        SetupDamageableObject();
        SetHealth(200);
        damage = 10;
        knockBackForce = 10f;
        moveSpeed = 500f;
    }

    public void ChangeState(int animationID, bool value)
    {
        // animator.SetBool(animationID, value);
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
            animator.SetBool(animationID, value);
            chasingState = value;
            if (value && !processingChase)
            {
                chasingState = true;
                StartCoroutine(ChasePlayer());
            }
        }
    }

    public void startAttackCoroutine()
    {
        StartCoroutine(AttackPlayer());
    }

    public IEnumerator AttackPlayer()
    {
        while (attackingState)
        {
            processingAttack = true;
            animator.SetTrigger(Attack);
            yield return new WaitForSeconds(0.5f);
            LaunchFireBalls();
            yield return new WaitForSeconds(3);
            processingAttack = false;
        }
    }


    public void LaunchFireBalls()
    {
        CalculateDirection();
        SetAnimatorXY();
        baseRotation = Quaternion.Euler(0, 0, Vector2.Angle(direction, new Vector2(1, 0))
                                              * (direction.y >= 0 ? 1 : -1));
        LaunchFireBall(littleFireBall, Quaternion.Euler(0, 0, 30));
        LaunchFireBall(bigFireBall, Quaternion.Euler(0, 0, 0));
        LaunchFireBall(littleFireBall, Quaternion.Euler(0, 0, -30));
    }


    public void LaunchFireBall(Object fireBallObject, Quaternion relativeRotation)
    {
        GameObject newFireBall =
            Instantiate(fireBallObject, transform.position, relativeRotation * baseRotation) as GameObject;
        if (newFireBall is null)
            throw new Exception("Can't initialize fireball");
        FireBall fireBall = newFireBall.GetComponent<FireBall>();
        fireBall.Setup(relativeRotation * direction);
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
            CalculateDirection();
            SetAnimatorXY();
            if (attackingState || !canMove)
            {
                yield return new WaitForSeconds(1);
                continue;
            }

            rb.AddForce(direction * (moveSpeed * Time.deltaTime));
            yield return null;
        }

        processingChase = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        var playerObj = col.collider.GetComponent<PlayerObj>();
        if (playerObj != null)
        {
            var slimePosition = transform.position;
            var dir = (Vector2)(playerObj.transform.position - slimePosition).normalized;
            playerObj.ReceiveHit(damage, dir * knockBackForce);
        }
    }


    private void ProcessDefeat()
    {
        StopAllCoroutines();
    }
}