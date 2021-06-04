using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBossFinalCollidePoint : MonoBehaviour
{
    public Animator anime;
    public string partName;
    public Jimmy jimmyScript;
    private float nextCheckTime = -1f;

    /// <summary>
    /// Just like the JimmyCollidePoint.cs but for the Final Boss
    /// </summary>

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.CompareTag("Jimmy"))
        {
            if (nextCheckTime <= Time.time)
            {
                if (anime.GetCurrentAnimatorStateInfo(0).IsName("Punches") && !Input.GetMouseButton(1))
                {
                    if (partName.Equals("Right Hand") && anime.GetFloat("PunchNum") <= 2f)
                    {
                        jimmyScript.curHealth -= TheBossFinal.punchDamage;
                        nextCheckTime = Time.time + 0.15f;
                    }
                
                    else if (partName.Equals("Left Hand") && anime.GetFloat("PunchNum") > 2f)
                    {
                        jimmyScript.curHealth -= TheBossFinal.punchDamage;
                        nextCheckTime = Time.time + 0.15f;
                    }
                }
            }
        }
    }
}
