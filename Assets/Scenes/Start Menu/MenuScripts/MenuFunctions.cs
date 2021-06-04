using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Handles the script-based functions of the main menu
public class MenuFunctions : MonoBehaviour
{
    // Loads Main Scene
    public void LoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Quits game
    public void Exit()
    {
        Application.Quit();
    }

}
