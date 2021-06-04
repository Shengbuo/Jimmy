using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LockOnHand : MonoBehaviour
{

    public float gunDistance;
    public float turnSpeed;
    public Transform rotatePoint;
    public Transform cameraTransform;

    void Update()
    {
        this.transform.rotation = Quaternion.Lerp(transform.rotation, cameraTransform.rotation, turnSpeed * Time.deltaTime);
        this.transform.position = Vector3.Lerp(this.transform.position, rotatePoint.position + cameraTransform.forward * gunDistance, turnSpeed * Time.deltaTime);
    }
}
