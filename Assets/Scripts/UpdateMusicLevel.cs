using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//standardizes music volume based on the music slider volume
//placed on every object with a music audio source
public class UpdateMusicLevel : MonoBehaviour
{
    //gets gameObject of an AudioSource 
    public AudioSource music;

    // Update is called once per frame
    void Update()
    {
        //gets volume from OptIonsFunctions (the slider data hub!)
        music.volume = OptIonsFunctions.music;
    }
}
