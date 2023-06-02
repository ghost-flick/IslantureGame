using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class RoadToMasterQuest : Quest
{
    public void Start()
    {
        Title = "В поиске надежды";
        Description = "Найти летописца и поговорить с ним";
        Progress = "";
        progressCheckable = false;
        SetProgressValues(0, 1);
    }

    public override void Setup()
    {
        return;
    }
}