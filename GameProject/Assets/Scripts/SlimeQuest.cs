using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SlimeQuest : Quest
{
    [SerializeField] private List<Slime> slimes;
    public void Start()
    {
        foreach (var slime in slimes)
        {
            slime.eventOnDeath.AddListener(UpdateProgress);
        }

        var curProgress = slimes.Count(obj => obj.IsDestroyed());
        SetProgressValues(curProgress, slimes.Count);
        Title = "Слаймы?!";
        Description = "Помоги дедушке избавиться от слаймов";
        Progress = $"{curProgress}/{slimes.Count}";
        progressCheckable = true;
    }

    public override void Begin()
    {
        foreach (var slime in slimes)
            slime.gameObject.SetActive(true);
    }
    
    public void UpdateProgress()
    {
        CurrentProgress += 1;
        questSystem.UpdateProgressText();
    }
}