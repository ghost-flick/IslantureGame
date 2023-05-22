using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class Npc : MonoBehaviour, IInteractable
{
    [SerializeField] private Dialog dialog;
    private string charackterName = "Old man";
    public void InvokeInteraction()
    {
        DialogManager.Instance.ShowDialog(dialog, charackterName);
    }
}
