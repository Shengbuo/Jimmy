using UnityEngine;

public class JimmyCollidePoint : MonoBehaviour
{
    /// <summary>
    /// This script is on the hand and leg bones of Jimmy where
    /// there is a trigger collider for melee collision check
    /// </summary>
    
    //Get all the information for this trigger collider
    public Animator anime;
    public string partName;
    private TheBossFinal bossScript;
    private float nextCheckTime = -1f; //Gap between collision check
    
    
    // Start is called before the first frame update
    void OnEnable()
    {
        bossScript = GameObject.FindGameObjectWithTag("The Boss Final").GetComponent<TheBossFinal>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //When there is a collision with the Boss
        if (other.transform.gameObject.CompareTag("The Boss Final"))
        {
            //The time gap has passed and do damage according to current animation playing
            if (nextCheckTime <= Time.time)
            {
                //For Punches
                if (anime.GetCurrentAnimatorStateInfo(0).IsName("Punches"))
                {
                    if (partName.Equals("Left Hand") && anime.GetFloat("PunchKickNum") == 0f)
                    {
                        bossScript.curHealth -= Jimmy.punchDamage;
                        nextCheckTime = Time.time + 0.2f;
                    }
                
                    else if (partName.Equals("Right Hand") && anime.GetFloat("PunchKickNum") == 1f)
                    {
                        bossScript.curHealth -= Jimmy.punchDamage;
                        nextCheckTime = Time.time + 0.2f;
                    }
                }
                //For the leg kick
                else if (partName.Equals("Right Leg") && anime.GetCurrentAnimatorStateInfo(0).IsName("Kick"))
                {
                    bossScript.curHealth -= Jimmy.punchDamage;
                    nextCheckTime = Time.time + 0.15f;
                }
            }
        }
    }
}
