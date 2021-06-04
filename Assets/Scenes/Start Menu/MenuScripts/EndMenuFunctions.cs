using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Handles script-based functions on the end screen
public class EndMenuFunctions : MonoBehaviour
{
        //Loads the main menu
       public void LoadMain()
    {
        SceneManager.LoadScene(0);
    }
}
