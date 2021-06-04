using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Allows for interaction with the elevator at the end of the level
public class ElevatorMechanics : MonoBehaviour
{

    //text used when player is in proximity of elevator
    public GameObject GUITextcollected;
    public GameObject GUITextactive;

    //elevator opening doors
    public GameObject door1;
    public GameObject door2;

    //used to check if crystal is collected
    public GameObject crystal;

    //tracks whether or not the doors are currently moving
    public bool movingDoors = false;

    //tracks whether or not the player is near the elevator
    bool nearElevator = false;

    int doorMoveProgress = 0;
    

    // Update is called once per frame
    void Update()
    {
        //Elevator activates by pressing the E key
        if (nearElevator && Input.GetKeyDown(KeyCode.E))
        {
            movingDoors = true;
            GUITextcollected.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        //opens doors
        if (movingDoors && doorMoveProgress < 150)
        {
            door1.transform.position = new Vector3(door1.transform.position.x, door1.transform.position.y, door1.transform.position.z - 0.01074f);
            door2.transform.position = new Vector3(door2.transform.position.x, door2.transform.position.y, door2.transform.position.z + 0.01074f);
            doorMoveProgress++;
        }
        //stalls to end animation smoothly
        else if(doorMoveProgress >= 200)
        {
            doorMoveProgress++;
        }
        //marks the doors as still
        else if(doorMoveProgress >= 150)
        {
            movingDoors = false;
        }
    }



    //checks if player triggers the elevators isTrigger hitbox (for proximity detection)
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //makes sure players can't proceed without collecting the current level's crystal
            if (crystal.activeInHierarchy)
            {
                GUITextactive.SetActive(true);
            }
            else
            {
                GUITextcollected.SetActive(true);
                nearElevator = true;
            }
            
        }
    }

    //checks if the player leaves the elevators isTrigger hitbox
    private void OnTriggerExit(Collider other)
    {
        //disables proximity based text
        if(other.tag == "Player")
        {
            GUITextactive.SetActive(false);
            GUITextcollected.SetActive(false);
            nearElevator = false;
        }
    }
}
