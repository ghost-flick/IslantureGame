using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateController
{
    public static bool DialogMode = false;

    public static bool NormalMode = true;
    // Start is called before the first frame update
    public static void EnterDialogMode()
    {
        DialogMode = true;
        NormalMode = false;
        PlayerController.Instance.canMove = false;
        PlayerController.Instance.EnterDialog();
        PlayerController.Instance.actions.AddListener(DialogManager.Instance.ShowNextReplica);
    }

    public static void LeaveDialogMode()
    {
        DialogMode = false;
        NormalMode = true;
        PlayerController.Instance.canMove = true;
        PlayerController.Instance.actions.RemoveListener(DialogManager.Instance.ShowNextReplica);
    }
}
