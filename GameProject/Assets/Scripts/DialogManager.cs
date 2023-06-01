using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField]private TMP_Text textBox;
    [SerializeField]private TMP_Text header;
    [SerializeField] private GameObject dialogBox;
    private List<(string, string)> replicas;
    public int lettersPerSecond = 40;
    public static DialogManager Instance { get; private set; }
    private Dialog dialog;
    private IEnumerator replicasFlow;
    public UnityEvent orderedActions;
    
    
    public void Awake()
    {
        Instance = this;
    }

    public void ShowDialog(Dialog d)
    {
        dialogBox.SetActive(true);
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
                orderedActions.Invoke();
                replicaIndex = 0;
                GameStateController.LeaveDialogMode();
                dialogBox.SetActive(false);
                yield break;
            }
            var coroutine = StartCoroutine(TypeReplica(replicas[replicaIndex]));
            yield return coroutine;
            StopCoroutine(coroutine);
        }
    }

    public IEnumerator TypeReplica((string, string) replica)
    {
        var charName = replica.Item1;
        var charReplica = replica.Item2;
        header.text = charName;
        textBox.text = "";
        foreach (var letter in charReplica.ToCharArray())
        {
            textBox.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }
}
