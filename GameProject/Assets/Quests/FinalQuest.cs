using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public class FinalQuest : Quest
{
    [SerializeField] private List<Portal> portals;
    [SerializeField] private GameObject image;
    private bool active;
    public void Start()
    {
        SetProgressValues(0, 100);
        Title = "Судный день";
        Description = "Возвращайся в деревню и покажи им, где раки зимуют!";
        Progress = $"{0}/{100}";
        progressCheckable = true;
    }

    public override void Setup()
    {
        active = true;
        eventOnCompletion.AddListener(ShowImage);
    }

    public void ShowImage()
    {
        image.SetActive(true);
    }
    public void UpdateProgress()
    {
        CurrentProgress += 1;
        questSystem.UpdateProgressText();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (!active)
            return;
        if (col.transform.GetComponentInParent<PlayerController>() is not null)
        {
            foreach (var portal in portals)
            {
                portal.gameObject.SetActive(true);
            }
        }
    }
}