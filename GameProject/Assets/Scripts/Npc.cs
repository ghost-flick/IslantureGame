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
    private Quest currentQuest;
    protected List<Dialog> commonDialogs;
    protected List<Dialog> defaultDialogs;
    protected List<Dialog> afterDialogs;
    static System.Random rnd;
    private int dialogIndex;
    private bool awaitingQuest;
    [SerializeField] public string npcName;
    [SerializeField] public int threshold;

    public void Start()
    {
        rnd = new System.Random();
        InitializeDialogs();
    }

    private void InitializeDialogs()
    {
        afterDialogs = Directory.GetFiles($"Assets/Dialogs/{npcName}/AfterDialogs", "*.txt")
            .Select(File.ReadAllLines)
            .Select(fileLines => new Dialog(fileLines)).ToList();
        commonDialogs = Directory.GetFiles($"Assets/Dialogs/{npcName}/CommonDialogs", "*.txt")
            .Select(File.ReadAllLines)
            .Select(fileLines => new Dialog(fileLines)).ToList();
        defaultDialogs = Directory.GetFiles($"Assets/Dialogs/{npcName}/DefaultDialogs", "*.txt")
            .Select(File.ReadAllLines)
            .Select(fileLines => new Dialog(fileLines)).ToList();
    }

    public void InvokeInteraction()
    {
        if (awaitingQuest)
        {
            if (QuestSystem.completedQuests.Contains(currentQuest))
            {
                awaitingQuest = false;
                QuestSystem.Instance.HideQuest();
            }
            else
            {
                DialogManager.Instance.ShowDialog(afterDialogs[dialogIndex-1], null);
                return;
            }
        }

        if (dialogIndex >= commonDialogs.Count || dialogIndex == threshold)
        {
            var dialogNum = rnd.Next(defaultDialogs.Count);
            currentDialog = defaultDialogs[dialogNum];
        }
        else
        {
            currentDialog = commonDialogs[dialogIndex];
            dialogIndex++;
        }
        if (dialogIndex < commonDialogs.Count)
            DialogManager.Instance.ShowDialog(currentDialog, BeginAwaitingResults);
        else
        {
            DialogManager.Instance.ShowDialog(currentDialog, null);
        }
    }

    public void BeginAwaitingResults()
    {
        if (currentQuest is null) return;
        awaitingQuest = true;
    }
}
