using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private TMP_Text textBox;
    private TMP_Text header;
    private List<string> replicas;
    public int lettersPerSecond = 40;
    [SerializeField] private PlayerController playerController;
    public static DialogManager Instance { get; private set; }
    private Dialog dialog;
    private IEnumerator replicasFlow;


    public void Setup()
    {
        textBox = transform.Find("MainText").GetComponent<TMP_Text>();
        header = transform.Find("HeaderText").GetComponent<TMP_Text>();
        Instance = this;
    }

    public void ShowDialog(Dialog d, string characterName)
    {
        gameObject.SetActive(true);
        header.text = characterName;
        GameStateController.EnterDialogMode();
        dialog = d;
        replicas = dialog.Lines;
        replicasFlow = NextReplica();
        replicasFlow.MoveNext();
    }

    public void ShowNextReplica()
    {
        replicasFlow.MoveNext();
    }
    public IEnumerator NextReplica()
    {
        for (var replicaIndex = 0;; replicaIndex++)
        {
            if (replicaIndex >= replicas.Count)
            {
                replicaIndex = 0;
                GameStateController.LeaveDialogMode();
                gameObject.SetActive(false);
                yield break;
            }
            var coroutine = StartCoroutine(TypeReplica(replicas[replicaIndex]));
            yield return coroutine;
            StopCoroutine(coroutine);
        }
    } 

    public IEnumerator TypeReplica(string replica)
    {
        // add ready field
        textBox.text = "";
        foreach (var letter in replica.ToCharArray())
        {
            textBox.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }
}
