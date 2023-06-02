using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class FirstLizardQuest : Quest
{
    [SerializeField] private GameObject lizard;
    public void Start()
    {
        Title = "";
        Description = "";
        Progress = $"";
        SetProgressValues(0, 1);
    }

    public override void Setup()
    {
        lizard.SetActive(true);
    }
}