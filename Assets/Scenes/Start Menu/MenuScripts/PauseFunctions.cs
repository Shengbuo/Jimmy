using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Controls the script-based functions of the pause menu
public class PauseFunctions : MonoBehaviour
{
    public GameObject Options;
    //Loads the Main menu (index 0 in the build settings)
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }


    //Opens the options menu
    public void OpenOptions()
    {
        //disables the pause/main menu GameObject
        gameObject.SetActive(false);
        //enables the options menu GameObject
        Options.SetActive(true);
    }
}
