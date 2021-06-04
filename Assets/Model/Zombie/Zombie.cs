using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    //Components for Zombie and player
    private NavMeshAgent NMAgent;
    private Animator anime;
    private Transform playerTransform;
    private float checkPunchTime = -1;
    private System.Random random = new System.Random();
    private PlayerStats playerStats;
    
    //Stats
    public float curHealth;
    public float fullHealth = 100f;
    public float punchDamage = 10f;
    public float enableDistance = 10f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //Assign Variables
        NMAgent = GetComponent<NavMeshAgent>();
        anime = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        curHealth = fullHealth;
        NMAgent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Dies
        if (curHealth <= 0f) { Death(); }

        //When just moving
        if (anime.GetCurrentAnimatorStateInfo(0).IsName("Basic Moves"))
        {
            anime.applyRootMotion = true;
            NMAgent.SetDestination(playerTransform.position);
            //Punch if close
            if (NMAgent.remainingDistance <= enableDistance)
            {
                Movement(NMAgent.desiredVelocity);
                punch();
            }
            else
            {
                NMAgent.velocity = Vector3.zero;
                NMAgent.speed = 0f;
                NMAgent.acceleration = 0f;
            }
        }
        
        //If punching then don't move
        else if (anime.GetCurrentAnimatorStateInfo(0).IsName("Punches"))
        {
            LookAtPlayer();
            anime.applyRootMotion = false;
            
            NMAgent.speed = 1f;
            NMAgent.acceleration = 0f;
            NMAgent.velocity = Vector3.zero;
            
            if (checkPunchTime < Time.time)
            {
                applyPunchDamage();
                checkPunchTime = Time.time + 100000f;
            }
        }
    }
    
    //Same as TheBossFinal.cs
    Vector3 getGroundPlane()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hit);
        return hit.normal;
    }
    
    //Same as TheBossFinal.cs
    void Movement(Vector3 move)
    {
        NMAgent.SetDestination(playerTransform.position);

        //Prepare the move vector
        if (move.magnitude > 1f) { move.Normalize(); }
        else { move = (move * 999999f).normalized; }

        move = transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, getGroundPlane());
        
        float turnAmount = Mathf.Atan2(move.x, move.z);
        float forwardAmount = move.z;
        
        //Force Rotation
        float turnSpeed = Mathf.Lerp(180f, 360f, forwardAmount);
        transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        
        //Set Animation
        anime.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        anime.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
    }

    //Plays death animation and destroy the object
    void Death()
    {
        anime.SetTrigger("Death");
        GetComponent<CapsuleCollider>().enabled = false;
        NMAgent.ResetPath();
        Destroy(gameObject, 5f);
    }
    
    //Punching
    void punch()
    {
        if (NMAgent.remainingDistance < NMAgent.stoppingDistance)
        {
            LookAtPlayer();
            anime.SetFloat("PunchNum", random.Next(2));
            anime.SetTrigger("Punch");
            checkPunchTime = Time.time + 0.2f;
        }
    }
    
    void applyPunchDamage()
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position + Vector3.up, transform.position + Vector3.up + transform.forward * 2f, out hit))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                playerStats.playerHealth = Mathf.Max(0f, playerStats.playerHealth - punchDamage);
            }
        }
    }

    //Manage the speed of NM Agent based on the root animation
    void OnAnimatorMove()
    {
        if (anime.GetCurrentAnimatorStateInfo(0).IsName("Basic Moves"))
        {
            NMAgent.speed = (anime.deltaPosition / Time.deltaTime).magnitude * 5f;
            NMAgent.acceleration = 8;
        }
    }
    
    void LookAtPlayer()
    {
        Vector3 lookVector = playerTransform.position - transform.position;
        lookVector.y = transform.position.y;
        Quaternion rot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1f);
    }
}
