using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void UnPause()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
