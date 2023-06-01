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
    IEnumerator Start()
    {
        commonDialogs = Directory.GetFiles("Assets/Dialogs/Player/CommonDialogs", "*.txt")
            .Select(File.ReadAllLines)
            .Select(fileLines => new Dialog(fileLines)).ToList();
        yield return new WaitForSeconds(3);
        var quest = transform.Find(commonDialogs.First().GetQuestToStartAfter())?.GetComponent<Quest>();
        // StartFirstDialog(quest);
    }

    public void StartFirstDialog(Quest quest)
    {
        DialogManager.Instance.ShowDialog(commonDialogs.First(), null, quest);
    }
}