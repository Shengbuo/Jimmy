using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//Closes the elevator after the player has interacted with it
public class ElevatorCloseLoad : MonoBehaviour
{
    //hitbox to prevent player from leaving after entering the elevator
    public GameObject leaveHitbox;

    public GameObject door1;
    public GameObject door2;

    
    public ElevatorMechanics elevatorMechanics;

    bool closingDoors = false;
    bool nearElevator = false;

    int doorMoveProgress = 0;


    // Update is called once per frame
    void Update()
    {
        //closes doors if player is inside elevator while the doors have finished moving
        if (nearElevator && !(elevatorMechanics.movingDoors))
        {
            closingDoors = true;
        }
    }

    private void FixedUpdate()
    {
        //closes the doors
        if (closingDoors && doorMoveProgress < 150)
        {
            door1.transform.position = new Vector3(door1.transform.position.x, door1.transform.position.y, door1.transform.position.z + 0.01074f);
            door2.transform.position = new Vector3(door2.transform.position.x, door2.transform.position.y, door2.transform.position.z - 0.01074f);
            doorMoveProgress++;
        }
        //loads the next scene after the player has completed the level
        else if (doorMoveProgress >= 200)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        //stalls for time after closing doors (around a second)
        else if (doorMoveProgress >= 150)
        {
            closingDoors = false;
            doorMoveProgress++;
        }
    }




    private void OnTriggerStay(Collider other)
    {
        //disables the player from leaving the elevator once entered,
        //detects if player is inside elevator
        if (other.tag == "Player")
        {
            leaveHitbox.SetActive(true);
            nearElevator = true;
        }
    }

}
