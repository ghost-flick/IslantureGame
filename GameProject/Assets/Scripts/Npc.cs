using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Windows;
using System.IO;
using Directory = System.IO.Directory;
using File = System.IO.File;

public class Npc : MonoBehaviour, IInteractable
{
    [SerializeField] private Dialog currentDialog;
    [SerializeField] private Quest currentQuest;
    [SerializeField] private QuestSystem questSystem;
    protected List<Dialog> commonDialogs;
    protected List<Dialog> defaultDialogs;
    protected List<Dialog> afterDialogs;
    static System.Random rnd;
    private int dialogIndex;
    private bool awaitingQuest;

    public void Start()
    {
        rnd = new System.Random();
        InitializeDialogs();
        DialogManager.Instance.orderedActions.AddListener(StartNewQuest);
    }

    private void InitializeDialogs()
    {
        afterDialogs = Directory.GetFiles("Assets/Dialogs/OldMan/AfterDialogs", "*.txt")
            .Select(File.ReadAllLines)
            .Select(fileLines => new Dialog(fileLines)).ToList();
        commonDialogs = Directory.GetFiles("Assets/Dialogs/OldMan/CommonDialogs", "*.txt")
            .Select(File.ReadAllLines)
            .Select(fileLines => new Dialog(fileLines)).ToList();
        defaultDialogs = Directory.GetFiles("Assets/Dialogs/OldMan/DefaultDialogs", "*.txt")
            .Select(File.ReadAllLines)
            .Select(fileLines => new Dialog(fileLines)).ToList();
    }

    public void InvokeInteraction()
    {
        if (awaitingQuest)
        {
            if (currentQuest.isCompleted)
            {
                awaitingQuest = false;
                questSystem.HideQuest();
            }
            else
            {
                DialogManager.Instance.ShowDialog(afterDialogs[dialogIndex-1]);
                return;
            }
        }

        if (dialogIndex >= commonDialogs.Count)
        {
            var dialogNum = rnd.Next(defaultDialogs.Count);
            currentDialog = defaultDialogs[dialogNum];
        }
        else
        {
            currentDialog = commonDialogs[dialogIndex];
            dialogIndex++;
        }
        
        currentQuest = transform.Find(currentDialog.questToStartAfter)?.gameObject.GetComponent<Quest>();
        DialogManager.Instance.ShowDialog(currentDialog);
    }
    
    public void StartNewQuest()
    {
        if (currentQuest is null) return;
        awaitingQuest = true;
        questSystem.Quest = currentQuest; // here happens setting magic
    }
}
