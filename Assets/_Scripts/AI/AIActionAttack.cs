using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIActionAttack : AIAction
{
    public int damage;
    public override void RunAction()
    {
        GameManager.instance.player.GetComponent<Health>().TakeDamage(damage);
    }
}
