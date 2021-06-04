using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//updates the text besides setting sliders to display the value
public class TextToSlider : MonoBehaviour
{
    public Slider slider;
    public Text display;

    private void Start()
    {
        //initial display value after a scene load
        display.text = (slider.value * 200).ToString();
    }

    //updates text when slider is interacted with
    public void ChangeTextValue()
    {
        display.text = (slider.value*200).ToString();
    }

}
