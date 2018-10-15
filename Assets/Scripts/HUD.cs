using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour {

    public GameObject gameOverPanel;

    public void showGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }
    public void Onclick_Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Run");
       
    }
    public void Onclick_Quit()
    {
        Application.Quit();
    }
}
