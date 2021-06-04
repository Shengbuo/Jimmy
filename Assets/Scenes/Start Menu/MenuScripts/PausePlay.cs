using UnityEngine;

//Pauses and Resumes the game
public class PausePlay : MonoBehaviour
{

    //keeps track of whether the game is paused or not
    public static bool isPaused = false;

    //GameObjects that need to be enabled for the menu to work
    public GameObject pauseMenu;
    public GameObject pauseBackground;
    public GameObject pauseOptions;

    //GameObjects that need to be disabled for the menu to work
    public GameObject pausePlayer;
    public GameObject pauseGUI;

    //unpauses game
    public void Resume()
    {
        //deactivates pause menu
        pauseMenu.SetActive(false);
        pauseBackground.SetActive(false);
        pauseOptions.SetActive(false);

        //activates in-game GameObjects
        pausePlayer.SetActive(true);
        pauseGUI.SetActive(true);

        //resets cursor to crosshair placement
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //unfreezes time
        Time.timeScale = 1f;
        isPaused = false;
    }

    //pauses game
    public void Pause()
    {
        //activates pause menu
        pauseMenu.SetActive(true);
        pauseBackground.SetActive(true);

        //deactivates other UI elements and the player (to stop firing on click)
        pausePlayer.SetActive(false);
        pauseGUI.SetActive(false);

        //frees cursor to adjust sliders and click buttons
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        //freezes time
        Time.timeScale = 0f;
        isPaused = true;
    }

   
    private void Start()
    {
        //unpauses on start (scenes set to pause naturally)
            Resume();
    }

    void Update()
    {

        //pauses/unpauses on escape button press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

}
