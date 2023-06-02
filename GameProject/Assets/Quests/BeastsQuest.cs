using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public class BeastsQuest : Quest
{
    [SerializeField] private List<Beast> beasts;
    public void Start()
    {
        foreach (var beast in beasts)
        {
            beast.eventOnDeath.AddListener(UpdateProgress);
        }

        var curProgress = beasts.Count(obj => obj.IsDestroyed());
        SetProgressValues(curProgress, beasts.Count);
        Title = "Недодемоны";
        Description = "Избавьте летописца от недодемонов";
        Progress = $"{curProgress}/{beasts.Count}";
        progressCheckable = true;
    }

    public override void Setup()
    {
        foreach (var beast in beasts)
            beast.gameObject.SetActive(true);
    }
    
    public void UpdateProgress()
    {
        CurrentProgress += 1;
        questSystem.UpdateProgressText();
    }
}