using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCharacterAnimManager : MonoBehaviour
{
    public Animator anim;
    public string move;
    public string stance;
    public string death;
    public List<string> attackAnimations;

    public void SetAnimationTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    public void SetRandomAttackTrigger()
    {
        string attack = attackAnimations[Random.Range(0, attackAnimations.Count)];
        anim.SetTrigger(attack);
    }

    public void SetMovementTrigger(bool isStopped)
    {
        string trigger = isStopped ? stance : move;
        anim.SetTrigger(trigger);
    }

    public void SetDeathTrigger()
    {
        anim.SetTrigger(death);
    }
}
