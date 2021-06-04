using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    private float camPitch = 0f;
    private Transform curTransform;

    public float startingPitch = 0f;
    public Transform playerTransform;
    public Transform camTransform;


    void rotatePitch()
    {
        //Update the Y rotation of the Camera
        float mouseY = Input.GetAxis("Mouse Y") * (OptIonsFunctions.MouseSens);
        camPitch -= mouseY;
        camPitch = Mathf.Clamp(camPitch, -90f, 90f);
        curTransform.rotation = Quaternion.Euler(camPitch, playerTransform.eulerAngles.y, 0f);
    }

    void moveCamera()
    {
        camTransform.position = transform.position;
        camTransform.rotation = transform.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        camPitch = startingPitch;
        curTransform = this.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        rotatePitch();
        moveCamera();
    }
}
