using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[Serializable]
public abstract class Quest : MonoBehaviour
{
    public QuestSystem questSystem;
    [NonSerialized] public string Title;
    [NonSerialized] public string Description;
    [NonSerialized] public string Progress;
    public bool isCompleted;
    private int target;
    private int currentProgress;
    public bool progressCheckable;
    public UnityEvent eventOnCompletion;
    
    public int CurrentProgress
    {
        set
        {
            currentProgress = value;
            if (currentProgress == target)
            {
                isCompleted = true;
                eventOnCompletion.Invoke();
                QuestSystem.completedQuests.Add(this);
            }
        }
        get => currentProgress;
    }

    public int Target
    {
        get => target;
        set => target = value;
    }

    public void SetProgressValues(int cur, int tar)
    {
        target = tar;
        currentProgress = cur;
    }

    public abstract void Setup();
}