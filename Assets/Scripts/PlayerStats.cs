using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    //Player Health Values
    public float playerFullHealth;
    public float playerHealth;
    //Blood Effect on Screen
    public Image bloodEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        //Set the player health
        playerHealth = playerFullHealth;
    }

    // Update is called once per frame
    void Update()
    {
        BloodScreen();
        //If the player dies
        if (playerHealth <= 0f) { toGameOver(); }
    }
    
    //Updates the blood effect opacity
    void BloodScreen()
    {
        Color curColour = bloodEffect.color;
        curColour.a = Mathf.Lerp(1f, 0f, playerHealth / playerFullHealth);
        bloodEffect.color = curColour;
    }

    //Switch to Game Over Scene
    void toGameOver()
    {
        GameOver.deathScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("GameOver");
    }
}
