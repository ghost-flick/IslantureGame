using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BehaviorCollider : MonoBehaviour
{
    public bool playerInPosition = false;
    public PlayerObj player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        var enterPlayer = other.GetComponent<PlayerObj>();
        if (enterPlayer != null)
        {
            playerInPosition = true;
            player = enterPlayer;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        playerInPosition = false;
    }
}