using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    // [SerializeField] DialogManager dialogManager;
    private List<Dialog> commonDialogs;
    public int dialogIndex;
    void Start()
    {
        commonDialogs = Directory.GetFiles("Assets/Dialogs/Player/CommonDialogs", "*.txt")
            .Select(File.ReadAllLines)
            .Select(fileLines => new Dialog(fileLines)).ToList();
        ShowNextDialog();
    }

    public void ShowNextDialog()
    {
        StartCoroutine(ShowNextPlayerDialog());
    }

    public IEnumerator ShowNextPlayerDialog()
    {
        if (dialogIndex > commonDialogs.Count)
            yield break;
        yield return new WaitForSeconds(3);
        DialogManager.Instance.ShowDialog(commonDialogs[dialogIndex], null);
        dialogIndex++;
    }
}