using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Saves the slider-based settings through loading scenes
public class SettingsSave : MonoBehaviour
{

    public Slider SenSlider;
    public Slider MusSlider;
    public Slider SFXSlider;

    void Awake()
    {
        //Changes slider value based on their static value given by OptIonsFunctions
        SenSlider.value = (OptIonsFunctions.MouseSens) / 5;
        MusSlider.value = (OptIonsFunctions.music);
        SFXSlider.value = (OptIonsFunctions.sounds);
    }


}
