using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BehaviorCollider : MonoBehaviour
{
    public bool playerInPosition = false;
    private IBehaviour parent;
    [SerializeField] private string animationBoolName;
    private int animationID;

    public void Start()
    {
        animationID = Animator.StringToHash(animationBoolName);
        parent = transform.parent.GetComponent<IBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        parent.ChangeState(animationID, true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        parent.ChangeState(animationID, false);
    }
}