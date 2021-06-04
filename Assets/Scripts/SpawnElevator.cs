using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//manages the functions of the elevator that the player spawns in through levels 2 and 3
public class SpawnElevator : MonoBehaviour
{
    public GameObject door1;
    public GameObject door2;

    int doorMoveProgress = 0;

    void FixedUpdate()
    {
        //always moves doors at the beginning of each scene (stops based on counter)
        if (doorMoveProgress < 150)
        {
            door1.transform.position = new Vector3(door1.transform.position.x, door1.transform.position.y, door1.transform.position.z - 0.01074f);
            door2.transform.position = new Vector3(door2.transform.position.x, door2.transform.position.y, door2.transform.position.z + 0.01074f);
            doorMoveProgress++;
        }
    }
}
