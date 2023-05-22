using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInterfaceManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] DialogManager dialogManager;
    void Start()
    {
        dialogManager.Setup();
    }
}
