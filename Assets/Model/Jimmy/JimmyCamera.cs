using UnityEngine;

public class JimmyCamera : MonoBehaviour
{
    //Transform of the player and camera
    public Transform player;
    public Transform cameraTransform;
    //For turning on/off the crosshair and the block message
    public GameObject Crosshair;
    public GameObject Block;
    //Offset of the camera at the start
    public Vector3 offset;
    //For raycasting of camera clipping
    public LayerMask mask;

    //Camera Rotation
    public float camYawX, camYawY, convertedYawY, camYawYCut;
    public float camDist, startAngle;
    //Camera Position
    private float camPosX, camPosY, camPosZ;
    private float camSmooth = 1000f;
    //Player Position
    private Vector3 playerPos;


    // Start is called before the first frame update
    void OnEnable()
    {
        //Set up the cursor
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
        //Starting Angle on the Horizontal view
        startAngle = Mathf.Atan2(-offset.z, -offset.x) * Mathf.Rad2Deg;
        camYawX = startAngle; 
        //Distance between camera and player
        camDist = 2.3f; 
        //Turn on/off crosshair and block message
        Crosshair.SetActive(false);
        Block.SetActive(true);
    }
    
    // Update is called once per frame
    void Update()
    {
        playerPos = new Vector3(player.position.x, player.position.y + 1.5f, player.position.z); //Player position
        camYawX -= Input.GetAxis("Mouse X") * OptIonsFunctions.MouseSens; //Camera angle degree for Horizontal mouse movement
        camYawX = Mathf.Repeat(camYawX, 360f); //Make sure the camera Yaw for Horizontal mouse movement is within 0 and 360 degrees
        camYawY -= Input.GetAxis("Mouse Y") * OptIonsFunctions.MouseSens; //Camera angle degree for Vertical mouse movement
        camYawY = Mathf.Clamp(camYawY, -85f, 85f); //Clamp the Camera angle degree for Vertical mouse movement to the range of -90 to 90 degree
        convertedYawY = Mathf.Repeat(camYawY, 360f); //Converts from range -90 to 90 to the range of 0 to 360 for trig calculation
        
        
        camPosY = Mathf.Sin(convertedYawY * Mathf.Deg2Rad) * 1.2f + playerPos.y; //Finds the camera position on the Y axis and add offset
        camYawYCut = 1.2f - Mathf.Cos(convertedYawY * Mathf.Deg2Rad) * 1.2f; //Finds the camera position cutoff for X and Z axis
        camPosX = Mathf.Cos(camYawX * Mathf.Deg2Rad) * (camDist-camYawYCut) + playerPos.x; //Finds the camera position on the X axis and add offset
        camPosZ = Mathf.Sin(camYawX * Mathf.Deg2Rad) * (camDist-camYawYCut) + playerPos.z; //Finds the camera position on the Z axis and add offset
        
        Vector3 targetPos = new Vector3(camPosX, camPosY, camPosZ); //Combining all the co-ordinates to one Vector3 point
        Quaternion targetRot = Quaternion.Euler(camYawY, -(camYawX - startAngle), 0f); //Store the target camera rotation as a quaternion angle
        
        if (Physics.Linecast(playerPos, targetPos, out RaycastHit cameraHit, mask.value)) //Check if the new camera position is inside an obstacle
            targetPos = cameraHit.point + (playerPos - cameraHit.point).normalized * 0.1f; //If it hits, change the camera position to the place where the ray hits so camera don't go into objects like walls
        
        
        targetPos = Vector3.Lerp(transform.position, targetPos, camSmooth * Time.deltaTime); //Set the camera smoothing
        
        
        transform.position = targetPos; //Set the camera position to the new camera position
        transform.rotation = targetRot; //Rotate the camera to the Yaw degree
    }

    void LateUpdate()
    {
        //Move the actual camera to this object
        cameraTransform.position = transform.position;
        cameraTransform.rotation = transform.rotation;
    }
}
