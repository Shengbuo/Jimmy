using UnityEngine;

//Stores the universal values for mouse sensitivity, music volume, and sound effect volume
public class OptIonsFunctions : MonoBehaviour
{
    public static float MouseSens = 5;
    public static float music = 1f;
    public static float sounds = 1f;
    
    // Sensitivity
    public void ChangeSens(float NewSens)
    {
        MouseSens = 5 * NewSens;
    }

    //Music
    public void ChangeMus(float newMus)
    {
        music = newMus;
    }


    //Sound effects
    public void ChangeSFX(float newSFX)
    {
        sounds = newSFX;
    }

}
