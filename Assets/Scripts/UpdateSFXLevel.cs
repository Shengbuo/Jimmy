using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//standardizes SFX volume based on the SFX slider volume
//placed on every object with a SFX audio source
public class UpdateSFXLevel : MonoBehaviour
{

    //gets gameObject of an AudioSource 
    public AudioSource sounds;

    void Update()
    {
        //gets volume from OptIonsFunctions (the slider data hub!)
        sounds.volume = OptIonsFunctions.sounds;
    }
}
