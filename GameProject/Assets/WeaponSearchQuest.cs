using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class WeaponSearchQuest : Quest
{
    public void Start()
    {
        Title = "Легендарное орудие";
        Description = "Найти орудие предков на горе к востоку от деревни";
        Progress = "";
        progressCheckable = false;
        SetProgressValues(0, 1);
    }

    public override void Setup()
    {
        return;
    }
}