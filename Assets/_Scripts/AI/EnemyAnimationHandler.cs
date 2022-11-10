using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationHandler : MonoBehaviour
{
    public Animator anim;

    public void SetAnimationTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }
    
    public void SetMovementAnimation(bool isStopped)
    {
        anim.SetBool("Running", !isStopped);
    }
}
