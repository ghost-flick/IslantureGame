using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UI;
using UnityEngine;

public class LegendaryWeapon : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerController player;
    [SerializeField] private List<Dragon> dragons;
    [SerializeField] private GameManager gameManager;
    private int progress;

    public void Start()
    {
        foreach (var dragon in dragons)
        {
            dragon.eventOnDeath.AddListener(UpdateProgress);
        }
    }

    public void InvokeInteraction()
    {
        if (progress < dragons.Count)
            return;
        player.TakeLegendaryWeapon();
        StartCoroutine(gameManager.ShowNextPlayerDialog());
        Destroy(gameObject);
    }

    public void UpdateProgress()
    {
        progress++;
    }
}
