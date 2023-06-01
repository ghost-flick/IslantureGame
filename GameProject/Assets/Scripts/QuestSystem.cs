using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class QuestSystem : MonoBehaviour
{
    [SerializeField] private Quest currentQuest;
    [SerializeField] private GameObject questBox;
    [SerializeField] private TMP_Text questTitleText;
    [SerializeField] private TMP_Text questInfoText;
    [SerializeField] private TMP_Text questProgress;
    public static QuestSystem Instance { get; private set; }

    public void Start()
    {
        Instance = this;
    }

    [SerializeField] public Quest Quest
    {
        set
        {
            ShowQuest(value);
            value.questSystem = this;
            currentQuest = value;
            value.eventOnCompletion ??= new UnityEvent();
            value.eventOnCompletion.AddListener(ShowQuestCompleted);
            currentQuest.Setup();
        }
        get => currentQuest;
    }

    public void ShowQuest(Quest quest)
    {
        questBox.SetActive(true);
        questTitleText.text = quest.Title;
        questInfoText.text = quest.Description;
        questProgress.text = quest.Progress;
    }

    public bool CheckCompleted()
    {
        return currentQuest.isCompleted;
    }

    public void HideQuest()
    {
        questBox.SetActive(false);
    }

    public void UpdateProgressText()
    {
        questProgress.text = $"{currentQuest.CurrentProgress}/{currentQuest.Target}";
    }

    public void ShowQuestCompleted()
    {
        questInfoText.text += " (completed)";
    }
}