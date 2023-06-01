using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    // Start is called before the first frame update
    // [SerializeField] DialogManager dialogManager;
    void Awake()
    {
        GameStateController.DialogMode = false;
        GameStateController.NormalMode = true;
        // InitializeFirstDialog();
    }

    // public IEnumerator InitializeFirstDialog()
    // {
    //     
    // }
}
