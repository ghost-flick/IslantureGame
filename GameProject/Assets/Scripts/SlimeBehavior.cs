using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SlimeBehavior : MonoBehaviour
{
    public BehaviorCollider noticeCollider;
    public BehaviorCollider attackCollider;

    private Unity.Mathematics.Random random;

    public Rigidbody2D slimeRb;
    public float moveSpeed = 0.001f;
    public Animator slimeAnimator;

    public bool chasingState = false;

    public bool attackingState = false;

    public Vector3 direction;

    // Start is called before the first frame update
    // Update is called once per frame
    void FixedUpdate()
    {
        if (attackCollider.playerInPosition)
        {
            slimeAnimator.SetBool("Attack", true);
        }
        else
        {
            slimeAnimator.SetBool("Attack", false);
        }

        if (noticeCollider.playerInPosition)
        {
            slimeAnimator.SetBool("MakeMove", true);
        }
        else
        {
            slimeAnimator.SetBool("MakeMove", false);
        }

        if (attackingState)
            return;
        if (chasingState)
            ChasePlayer();
    }

    public void AttackPlayer()
    {
        if (!attackingState)
            return;
        direction = (attackCollider.player.transform.position - transform.position).normalized;
        slimeRb.AddForce(direction * (moveSpeed) / 2);
    }

    public void ChasePlayer()
    {
        if (!chasingState)
            return;
        direction = (noticeCollider.player.transform.position - transform.position).normalized;
        slimeRb.AddForce(direction * (moveSpeed * Time.deltaTime));
    }

    public void StopChasingPlayer()
    {
        chasingState = false;
    }
}