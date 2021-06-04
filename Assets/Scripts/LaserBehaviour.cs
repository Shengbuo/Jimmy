using UnityEngine;

public class LaserBehaviour : MonoBehaviour
{
    private Vector3 direction;
    private Vector3 beforeUpdate;
    
    public float speed;
    public float bulletDamage = 20f;
    public GameObject explosion;
    public Transform startPoint;
    public Transform endPoint;

    // Update is called once per frame
    void Update()
    {
        beforeUpdate = startPoint.position;
        transform.position += direction * speed * Time.deltaTime;
        
        RaycastHit hit;
        if (Physics.Linecast(beforeUpdate, endPoint.position, out hit)) { collided(hit); }
    }

    public void setPoint(Vector3 point) { direction = (point - transform.position).normalized; }
    public void setDirection(Vector3 dir) { direction = dir; }
    


    private void collided(RaycastHit hit)
    {
        if (explosion != null) { 
            GameObject newExplosion = Instantiate(explosion, hit.point, transform.rotation);
            GameObject collidedObject = hit.transform.gameObject;
            if (collidedObject.CompareTag("Zombie"))
            {
                collidedObject.GetComponent<Zombie>().curHealth -= bulletDamage;
            }
            else if (collidedObject.CompareTag("The Boss"))
            {
                collidedObject.GetComponent<TheBoss>().curHealth -= bulletDamage;
            }
            
            Destroy(gameObject); Destroy(newExplosion, 1f);
        }
    }
}