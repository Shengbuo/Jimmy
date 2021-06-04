using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Jimmy : MonoBehaviour
{
    //Move Velocity
    private float moveFB = 0f, moveLR = 0f;
    //Jimmy's components
    private Animator anime;
    private CharacterController cc;
    //The final boss's components
    private Animator bossAnime;
    private Transform bossTransform;
    //Track current combo for 2 punch 1 kick
    private int curCombo = 1;
    //Random Generator
    private System.Random random = new System.Random();
    //Time tracker for next punch
    private float nextPunch = -1f;
    //Where to move jimmy when punching
    private Vector3 punchMove = Vector3.negativeInfinity;
    
    //Health from last update
    private float lastHealth;
    //Special move counter
    private float specialMoveCounter = 0f;
    
    private float gravity = 9.81f;
    
    //Camera
    public Transform camTransform;
    
    //Stats punch stats
    public float punchReach;
    public float punchGap;
    public float punchDmg;
    
    //Health Stats
    public float fullHealth;
    public float curHealth;
    
    //Blood Effect
    public Image bloodEffect;
    
    //Punch damage of Jimmy for collision check
    public static float punchDamage;
    
    private void Awake()
    {
        punchDamage = punchDmg;
    }

    void OnEnable()
    {
        //Set the components and health up
        curHealth = fullHealth;
        lastHealth = fullHealth;
        anime = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        bossTransform = GameObject.FindGameObjectWithTag("The Boss Final").GetComponent<Transform>();
        bossAnime = GameObject.FindGameObjectWithTag("The Boss Final").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Don't move when doing special combo
        anime.applyRootMotion = !anime.GetCurrentAnimatorStateInfo(0).IsName("Special Combo");
        //Check movement keys
        CheckKeys();
        Vector3 moveVel = camTransform.TransformPoint(new Vector3(moveLR, 0, moveFB)) - camTransform.position;
        //Move the player with animations
        Movement(moveVel);
        //Check Punch and blood screen
        Punch();
        BloodScreen();
        
        //If health goes to 0
        if (curHealth <= 0f) { toGameOver(); }

        //If it has taken damage then play hit animation
        if (Math.Abs(lastHealth - curHealth) > 0.5f)
        {
            anime.SetTrigger("Hit");
        }
        //Sync up special combo with fall and death animation
        if (anime.GetCurrentAnimatorStateInfo(0).IsName("Special Combo"))
        {
            if (specialMoveCounter <= 1f)
            {
                if (anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.702f)
                {
                    bossAnime.SetBool("Tired", false);
                }
            }
            
            else if (specialMoveCounter <= 2f)
            {
                if (anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.467f)
                {
                    bossAnime.SetBool("Tired", false);
                }
            }
            
            else
            {
                if (anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.050f)
                {
                    bossAnime.SetTrigger("Death");
                }
            }
        }
        //Sets the last health to this frame's health
        lastHealth = curHealth;
        //Apply gravity
        cc.Move(gravity * Vector3.down * Time.deltaTime);
    }

    //Update the blood screen
    void BloodScreen()
    {
        Color curColour = bloodEffect.color;
        curColour.a = Mathf.Lerp(1f, 0f, curHealth / fullHealth);
        bloodEffect.color = curColour;
    }

    //Switch to game over
    void toGameOver()
    {
        GameOver.deathScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("GameOver");
    }
    
    void Punch()
    {
        //Same as TheBossFinal.cs
        Vector3 JimmyCenter = transform.position + Vector3.up * 0.82f;
        Vector3 theBossCenter = bossTransform.position + Vector3.up * 0.82f;
        Vector3 JtoBDir = Vector3.Normalize(theBossCenter - JimmyCenter);

        RaycastHit hit;

        if (Input.GetMouseButtonDown(0) && nextPunch <= Time.time)
        {
            if (Physics.Linecast(JimmyCenter, JimmyCenter + JtoBDir * punchReach, out hit))
            {
                //Same as TheBossFinal.cs
                if (hit.transform.gameObject.CompareTag("The Boss Final"))
                {
                    transform.position = bossTransform.position - JtoBDir * 1f;
                    LookAtBoss();
                    //Use special combo if the boss is tired
                    if (bossAnime.GetBool("Tired") && !anime.GetCurrentAnimatorStateInfo(0).IsName("Special Combo"))
                    {
                        anime.SetFloat("Stage", specialMoveCounter);
                        anime.SetTrigger("Special Combo");
                        specialMoveCounter++;
                    }
                    
                    else { ApplyMeleeAnimation(); }
                }
            }
            
            else { ApplyMeleeAnimation(); }
        }
        
        //Smooth out player moving to punch
        if (Vector3.Distance(punchMove, Vector3.negativeInfinity) > 1f)
        {
            if (Vector3.Distance(punchMove, transform.position) <= 0.1f)
            {
                punchMove = Vector3.negativeInfinity;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, punchMove, Time.deltaTime);
            }
        }
        //If right click then block
        if (Input.GetMouseButton(1))
        {
            anime.ResetTrigger("Punch");
            anime.ResetTrigger("Kick");
            anime.SetBool("Blocking", true);
        }
        else
        {
            anime.SetBool("Blocking", false);
        }
    }
    
    void ApplyMeleeAnimation()
    {
        anime.SetFloat("PunchKickNum", random.Next(2));
        nextPunch = Time.time + punchGap;
        if (curCombo != 3)
        {
            anime.ResetTrigger("Kick");
            anime.SetTrigger("Punch");
            curCombo++;
        }
        else
        {
            anime.ResetTrigger("Punch");
            anime.SetTrigger("Kick");
            curCombo = 1;
        }
    }

    //For movements
    void CheckKeys()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) { moveFB = 0f; }
        else if (Input.GetKey(KeyCode.W)) { moveFB = 1f; }
        else if (Input.GetKey(KeyCode.S)) { moveFB = -1f; }
        else { moveFB = 0f; }

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) { moveLR = 0f; }
        else if (Input.GetKey(KeyCode.D)) { moveLR = 1f; }
        else if (Input.GetKey(KeyCode.A)) { moveLR = -1f; }
        else { moveLR = 0f; }
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
    
    //Same as TheBossFinal.cs
    void LookAtBoss()
    {
        Vector3 lookVector = bossTransform.position - transform.position;
        lookVector.y = transform.position.y;
        Quaternion rot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1f);
    }
}
