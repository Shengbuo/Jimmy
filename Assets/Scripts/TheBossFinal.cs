using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class TheBossFinal : MonoBehaviour
{
    //Stats for the Jump Attack
    public float airTime;
    public float jumpToApexTime;
    public float JumpAttackDamage;
    public GameObject landingBlast;
    
    //GameObjects to know which player form the player is at
    public GameObject player;
    public GameObject jimmy;
    
    //The Final Boss's stats
    public float fullHealth;
    public float curHealth;
    public float punchGap; //Time Gap between punches
    public float punchReach;
    public float punchDmg;
    
    
    public System.Random random = new System.Random();
    public float deathWait;
    
    
    //Variables for the Jump Attack Animation
    private float landTime = 0f;
    private Vector3 startPos;
    private Vector3 apex;
    private Vector3 dropPos;
    private int jumpState = -1;
    private float jumpAnimationCounter = 0f;
    
    //The Final Boss's Components
    private Animator anime;
    private NavMeshAgent NMAgent;
    
    //Capsule Player Form's Components
    private Transform playerTransform;
    private PlayerStatsBoss playerStats;
    
    //Jimmy Player Form's Components
    private Jimmy jimmyStats;
    private Transform JimmyTransform;
    
    //Variables for punching
    private float nextPunches;
    private int punchCounter;
    private int punchAmt = -1;
    
    //Variable for Special Move used
    private int stage = 1;
    //A time gap until player go to the Victory Scene
    private float deathTime = -1f;
    
    
    //Static variable for punchDamage to link with TheBossFinalCollidePoint.cs
    public static float punchDamage;
    
    //Assigns punchDamage with punchDmg
    private void Awake()
    {
        punchDamage = punchDmg;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        //Initialize the Final Boss's variables
        curHealth = fullHealth;
        anime = GetComponent<Animator>();
        NMAgent = GetComponent<NavMeshAgent>();
        NMAgent.updateRotation = false;
        
        //Initialize Capsule Player's variables
        playerTransform = player.GetComponent<Transform>();
        playerStats = player.GetComponent<PlayerStatsBoss>();
        
        //Initialize Modeled Player's variables
        JimmyTransform = jimmy.GetComponent<Transform>();
        jimmyStats = jimmy.GetComponent<Jimmy>();
    }

    // Update is called once per frame
    void Update()
    {
        //If player is in Capsule form
        if (player.activeSelf)
        {
            //Set the playerTransform to the Capsule player's Transform
            playerTransform = player.GetComponent<Transform>();
            //While in the Basic Movements (Idle, Walking, Running)
            if (anime.GetCurrentAnimatorStateInfo(0).IsName("Basic Moves"))
            {
                //Start Jump Attack
                jumpState = 0;
            } 
        }

        //If player is in Jimmy form
        else
        {
            //Set the playerTransform to the Modeled player's Transform
            playerTransform = JimmyTransform;
            //While in the Basic Movements (Idle, Walking, Running)
            if (anime.GetCurrentAnimatorStateInfo(0).IsName("Basic Moves"))
            {
                if (NMAgent.enabled)
                {
                    //Move the Boss
                    Movement(NMAgent.desiredVelocity);
                    //If the Boss is in attack range
                    if (NMAgent.remainingDistance <= NMAgent.stoppingDistance)
                    {
                        //Stop the boss from moving and try Punching
                        NMAgent.velocity = Vector3.zero;
                        Punch();
                    }
                }
                
                //Use Jump Attack every a third health gone
                if (curHealth <= 0f) { jumpState = 0; }
                
                else if (curHealth <= fullHealth / 3)
                {
                    if (stage != 3)
                    {
                        stage = 3;
                        jumpState = 0;
                    }
                }
                
                else if (curHealth <= 2 * fullHealth / 3)
                {
                    if (stage != 2)
                    {
                        stage = 2;
                        jumpState = 0;
                    }
                }
            }
            
            //Look at the player while punching
            else if (anime.GetCurrentAnimatorStateInfo(0).IsName("Punches")) { LookAtPlayer(); }
            
            //If dead
            else if (anime.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                //Wait for a certain amount of time then to the Victory Scene
                if (deathTime < 0f)
                {
                    deathTime = Time.time + deathWait;
                }
                else if (deathTime < Time.time)
                {
                    Death();
                }
            }
        }
        
        JumpAttack();
    }

    //Go to Victory Scene
    void Death()
    {
        SceneManager.LoadScene("Victory");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    
    //Function for punching
    void Punch()
    {
        RaycastHit hit;
        //Use positions to find the direct line between the boss and the player 
        Vector3 JimmyCenter = JimmyTransform.position + Vector3.up * 0.82f;
        Vector3 theBossCenter = transform.position + Vector3.up * 0.82f;
        Vector3 BtoJDir = Vector3.Normalize(JimmyCenter - theBossCenter);
        
        //If the punch gap is waited and the Boss can see the player with Linecast
        if (nextPunches <= Time.time && Physics.Linecast(theBossCenter, theBossCenter + BtoJDir * punchReach, out hit))
        {
            //Linecast hits Jimmy
            if (hit.transform.gameObject.CompareTag("Jimmy"))
            {
                //Move the Boss near the player and Look at it
                transform.position = JimmyTransform.position - BtoJDir * 1f;
                LookAtPlayer();
                
                //If it hasn't punched yet since the last punch cycle
                if (punchAmt == -1)
                {
                    //The amount of punch this cycle and reset the counter for it
                    punchAmt = random.Next(6) + 1;
                    punchCounter = 0;
                }
                //In the punching cycle
                else
                {
                    //Punch the player with random punching action
                    if (punchCounter < punchAmt)
                    {
                        anime.SetTrigger("Punch");
                        anime.SetFloat("PunchNum", random.Next(4));
                        punchCounter++;
                    }
                    //Completes the punch cycle
                    else
                    {
                        nextPunches = Time.time + punchGap;
                        punchAmt = -1;
                    }
                }
            }
        }
    }

    //Applies damage to player from Jump Attack
    private void ApplyJumpDamage()
    {
        if (player.activeSelf)
        {
            playerStats.playerHealth -= 25f;
        }
        else if (!Input.GetMouseButton(1))
        {
            jimmyStats.curHealth -= JumpAttackDamage;
        }
    }

    //Jump Attack States
    void JumpAttack()
    {
        switch (jumpState)
        {
            //Ready the boss for jumping up
            case 0:
                startPos = transform.position;
                apex = transform.position + Vector3.up * 10f;

                jumpState = 1;
                jumpAnimationCounter = 0f;
                
                anime.SetTrigger("Jump Up");
                NMAgent.enabled = false;
                break;
            
            //Move the Boss up in the Air
            case 1:
                if (jumpAnimationCounter >= jumpToApexTime)
                {
                    jumpState = 2;
                    landTime = Time.time + airTime;
                }
                else
                {
                    LookAtPlayer();
                    jumpAnimationCounter += Time.deltaTime;
                    transform.position = Vector3.Lerp(startPos, apex, Mathf.Min(1f, jumpAnimationCounter / jumpToApexTime));
                }
                break;
            
            //Up in the apex of the jump waiting
            case 2:
                if (landTime <= Time.time)
                {
                    startPos = transform.position;
                    dropPos = playerTransform.position - Vector3.up * 1f;
                    jumpState = 3;
                    jumpAnimationCounter = 0f;
                    anime.SetTrigger("Jump Down");
                }
                
                LookAtPlayer();
                break;
            
            //Punch down
            case 3:
                if (jumpAnimationCounter >= 1f)
                {
                    jumpState = -1;
                    LookAtPlayer();
                    GameObject blast = Instantiate(landingBlast, transform.position, transform.rotation);
                    Destroy(blast, 1f);
                    if (!player.activeSelf) { anime.SetBool("Tired", true); }
                    ApplyJumpDamage();
                }
                else
                {
                    jumpAnimationCounter += Time.deltaTime;
                    transform.position = Vector3.Lerp(startPos, dropPos, Mathf.Min(1f, jumpAnimationCounter / jumpToApexTime));
                }
                break;
            
            //Make sure every variable is set back from case 1
            default:
                anime.ResetTrigger("Jump Up");
                anime.ResetTrigger("Jump Down");
                NMAgent.enabled = true;
                break;
        }
    }
    
    //Gets the plane normal of the ground
    Vector3 GetGroundPlane()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hit);
        return hit.normal;
    }
    
    //Used for the movement and animation
    void Movement(Vector3 move)
    {
        //Ready for moving
        NMAgent.SetDestination(playerTransform.position);
        anime.applyRootMotion = NMAgent.remainingDistance > NMAgent.stoppingDistance;

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
    
    //Looks at the player
    void LookAtPlayer()
    {
        Vector3 lookVector = playerTransform.position - transform.position;
        lookVector.y = transform.position.y;
        Quaternion rot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1f);
    }
}
