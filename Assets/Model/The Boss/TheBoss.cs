using UnityEngine;
using UnityEngine.AI;

public class TheBoss : MonoBehaviour
{
    //Same as TheBossFinal.cs
    private NavMeshAgent NMAgent;
    private Animator anime;
    private float checkPunchTime;
    private float nextSpinTime = 20f;
    private float deathTime = -1f;
    private float spinStopTime; //When stop spinning
    private System.Random random = new System.Random();
    
    //Same as TheBossFinal.cs
    public PlayerStatsBoss playStats;
    public Transform playerTransform;
    
    //Final form and flame effect
    public GameObject finalForm;
    public GameObject flame;
    
    //Stats
    public float fullHealth;
    public float curHealth;
    public float punchDamage;
    public float punchReach;
    public float spinDuration;
    public float spinDamage;
    public float spinReach;


    // Start is called before the first frame update
    void Start()
    {
        //Assign all the variables
        NMAgent = GetComponent<NavMeshAgent>();
        anime = GetComponent<Animator>();
        NMAgent.updateRotation = false;
        curHealth = fullHealth;
    }



    // Update is called once per frame
    void Update()
    {
        if (curHealth <= 0f) { Death(); }
        else { AnimationBasedSwitch(); }
    }

    //States for the death animation to become the final form
    void Death()
    {
        if (deathTime < 0f)
        {
            deathTime = Time.time;
            NMAgent.velocity = Vector3.zero;
            anime.SetTrigger("Death");
        }

        else
        {
            if (deathTime + 6f <= Time.time)
            {
                finalForm.SetActive(true);
                finalForm.transform.position = transform.position;
                finalForm.transform.rotation = transform.rotation;
                gameObject.SetActive(false);
            }
            
            //Make flame
            else if (deathTime + 4.5f <= Time.time)
            {
                GameObject rebornFlame = Instantiate(flame, transform.position, transform.rotation);
                Destroy(rebornFlame, 1.7f);
            }


        }
    }

    //Same as TheBossFinal.cs
    Vector3 GetGroundPlane()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hit);
        return hit.normal;
    }

    //Same as TheBossFinal.cs
    void Movement(Vector3 move)
    {
        NMAgent.SetDestination(playerTransform.position);
        NMAgent.speed = 1f;
        
        //Prepare the move vector
        if (move.magnitude > 1f) { move.Normalize(); }
        else { move = (move * 999999f).normalized; }
        
        move = transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, GetGroundPlane());
        
        float turnAmount = Mathf.Atan2(move.x, move.z);
        float forwardAmount = move.z;
        
        //Force Rotation
        float turnSpeed = Mathf.Lerp(180f, 360f, forwardAmount);
        transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        
        //Set Animation
        anime.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        anime.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
    }

    //When reaches the player, punch
    void Punch()
    {
        if (NMAgent.remainingDistance <= NMAgent.stoppingDistance + 0.1f)
        {
            LookAtPlayer();
            anime.SetFloat("PunchNum", random.Next(4));
            anime.SetTrigger("Punch");
            checkPunchTime = Time.time + 0.35f;
        }
    }
    
    void ApplyPunchDamage()
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position + Vector3.up * 1.33f, transform.position + Vector3.up * 1.33f + transform.forward * punchReach, out hit))
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                playStats.playerHealth = Mathf.Max(0f, playStats.playerHealth - punchDamage);
            }
        }
    }

    void ApplySpinDamage()
    {
        if (Vector3.Distance(playerTransform.position, transform.position) <= spinReach)
        {
            playStats.playerHealth = Mathf.Max(0f, playStats.playerHealth - spinDamage * Time.deltaTime);
        }
    }
    
    
    void AnimationBasedSwitch()
    {
        //When in basic movements
        if (anime.GetCurrentAnimatorStateInfo(0).IsName("Basic Moves"))
        {
            anime.applyRootMotion = true;
            Movement(NMAgent.desiredVelocity);
            if (nextSpinTime < Time.time) { anime.SetTrigger("Spin"); }
            else { Punch(); }
        }
        
        //When punching
        else if (anime.GetCurrentAnimatorStateInfo(0).IsName("Punches"))
        {
            anime.applyRootMotion = false;
            LookAtPlayer();
            if (checkPunchTime < Time.time)
            {
                ApplyPunchDamage();
                checkPunchTime = Time.time + 100000f;
            }
        }
        
        //Starting to spin
        else if (anime.GetCurrentAnimatorStateInfo(0).IsName("Start Spinning"))
        {
            NMAgent.ResetPath();
            spinStopTime = Time.time + spinDuration;
        }
        
        //Spinning
        else if (anime.GetCurrentAnimatorStateInfo(0).IsName("Spinning"))
        {
            NMAgent.speed = 8f;
            NMAgent.acceleration = 8f;
            if (Time.time > spinStopTime)
            {
                anime.SetTrigger("Stop Spinning");
            }
            else
            {
                NMAgent.SetDestination(playerTransform.position);
                ApplySpinDamage();
            }
        }
        
        //End spinning
        else if (anime.GetCurrentAnimatorStateInfo(0).IsName("End Spinning"))
        {
            NMAgent.speed = 0f;
            NMAgent.acceleration = 0f;
            NMAgent.velocity = Vector3.zero;
            
            anime.ResetTrigger("Punch");
            anime.ResetTrigger("Stop Spinning");
            anime.ResetTrigger("Spin");
            
            nextSpinTime = Time.time + 20f;
        }
    }
    
    //Same as TheBossFinal.cs
    void LookAtPlayer()
    {
        Vector3 lookVector = playerTransform.position - transform.position;
        lookVector.y = transform.position.y;
        Quaternion rot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1f);
    }
}
