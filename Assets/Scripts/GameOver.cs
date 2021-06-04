using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public static string deathScene = null;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(deathScene);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
