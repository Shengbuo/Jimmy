using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStatsBoss : MonoBehaviour
{
    //Capsule player Health values for the boss scene
    public float playerFullHealth;
    public float playerHealth;
    //Blood Effect Value
    public Image bloodEffect;
    //Normal Boss and the transformed player with model
    public GameObject normalBoss;
    public GameObject Jimmy;

    private void Awake()
    {
        //Sets the final form to inactive when starting the scene
        Jimmy.SetActive(false);
    }

    
    
    /// <summary>
    /// Everything is the same as PlayerStats.cs but just for the boss scene
    /// Look into PlayerStats.cs for more detail comments
    /// I will only comment parts different from PlayerStats.cs
    /// </summary>

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = playerFullHealth;
    }

    // Update is called once per frame
    void Update()
    {
        BloodScreen();
        if (playerHealth <= 0f)
        {
            //If the player died from the Original Boss Form then Go to Game Over
            if (normalBoss.activeSelf)
            { 
                toGameOver();
            }
            
            //If the player died from the Boss's final form then it transform the capsule player to
            //the Jimmy form (Modeled Player)
            else
            {
                Jimmy.transform.position = transform.position;
                Jimmy.transform.rotation = transform.rotation;
                Jimmy.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
    
    void BloodScreen()
    { 
        Color curColour = bloodEffect.color;
        curColour.a = Mathf.Lerp(1f, 0f, playerHealth / playerFullHealth);
        bloodEffect.color = curColour;
    }

    void toGameOver()
    {
        GameOver.deathScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("GameOver");
    }
}