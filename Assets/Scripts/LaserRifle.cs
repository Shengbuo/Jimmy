using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserRifle : MonoBehaviour
{
    private RaycastHit hit;
    private float nextShotTime;

    public Camera playerCam;
    public float firingRate;
    public GameObject laserPrefab;
    public GameObject muzzleFlashPrefab;
    public AudioSource shootSound;

    
    void shootLaser()
    {
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        
        //Laser
        GameObject laser = Instantiate(laserPrefab, transform.position, transform.rotation);

        if (Physics.Raycast(ray, out hit))
        {
            laser.GetComponent<LaserBehaviour>().setPoint(hit.point); 
        }
        else
        {
            laser.GetComponent<LaserBehaviour>().setDirection(ray.direction); 
        }



        Destroy(laser, 2f);
        
        //Muzzle Flash
        GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, transform.position, transform.rotation);
        muzzleFlash.transform.parent = gameObject.transform;
        Destroy(muzzleFlash, 0.1f);
        
        shootSound.Play();
        
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time > nextShotTime)
        {
            shootLaser();
            nextShotTime = Time.time + firingRate;
        }
    }
}
