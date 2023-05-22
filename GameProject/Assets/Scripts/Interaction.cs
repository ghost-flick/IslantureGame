using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

public class Interaction : MonoBehaviour
{
    
    private Vector2 rightInteractOffset = new Vector2(0.1f, 0);
    private Vector2 leftInteractOffset = new Vector2(-0.1f, 0);
    private Vector2 downInteractOffset = new Vector2(0, -0.1f);
    private Vector2 topInteractOffset = new Vector2(0, 0.1f);
    public LayerMask interactiveLayer;

    public void Start()
    {
        interactiveLayer = LayerMask.GetMask("Interactive");
        Physics2D.autoSyncTransforms = true;
    }

    public void Interact(float dirX, float dirY)
    {
        
        transform.localPosition = dirY switch
        {
            >= 0.8f => topInteractOffset,
            <= -0.8f => downInteractOffset,
            _ => dirX >= 0 ? rightInteractOffset : leftInteractOffset
        };
        var interactObject = Physics2D.OverlapCircle(transform.position, 0.1f, interactiveLayer);
        if (interactObject is not null)
        {
            interactObject.GetComponent<IInteractable>()?.InvokeInteraction();
        }
    }
    
    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}
