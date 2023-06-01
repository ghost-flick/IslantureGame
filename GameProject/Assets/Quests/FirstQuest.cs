using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class FirstQuest : Quest
{
    public void Start()
    {
        Title = "Еще один прекрасный день";
        Description = "Навестить деда";
        Progress = "";
        progressCheckable = false;
        SetProgressValues(0, 1);
    }

    public override void Setup()
    {
        return;
    }
}