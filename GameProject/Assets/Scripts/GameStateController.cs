using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateController
{
    public static bool DialogMode = false;
    // Start is called before the first frame update
    public static void EnterDialogMode()
    {
        DialogMode = true;
        PlayerController.Instance.canMove = false;
        PlayerController.Instance.actions.AddListener(DialogManager.Instance.ShowNextReplica);
    }

    public static void LeaveDialogMode()
    {
        DialogMode = false;
        PlayerController.Instance.canMove = true;
        PlayerController.Instance.actions.RemoveListener(DialogManager.Instance.ShowNextReplica);
    }
}
