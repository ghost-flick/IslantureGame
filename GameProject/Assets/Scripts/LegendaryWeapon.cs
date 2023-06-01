using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class LegendaryWeapon : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerController player;
    public void InvokeInteraction()
    {
        player.TakeLegendaryWeapon();
        Destroy(gameObject);
    }
}
