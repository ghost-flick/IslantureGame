using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class BadNewsQuest : Quest
{
    public void Start()
    {
        Title = "Плохие новости";
        Description = "Спросить совета у деда";
        Progress = "";
        progressCheckable = true;
    }

    public override void Setup()
    {
        return;
    }
    
    public void UpdateProgress()
    {
        CurrentProgress += 1;
        questSystem.UpdateProgressText();
    }
}