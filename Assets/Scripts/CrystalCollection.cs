using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//lets the player "collect" the crystal
public class CrystalCollection : MonoBehaviour
{

    public GameObject Crystal;

    private void OnTriggerEnter(Collider other)
    {
        //disables the crystal on collision with player
        if(other.tag == "Player")
        {
            Crystal.SetActive(false);
        }
    }
}
