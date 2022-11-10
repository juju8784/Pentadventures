using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalahadTurn : PlayerTurn
{
    // Start is called before the first frame update
    public override void Attack()
    {
        range = 1;
        base.Attack();
    }
    public override void Special()
    {
        if (!stats)
        {
            stats = GetComponent<StatsHolder>();
        }
        if (stats.SubtractActionPoints(stats.block.actionsCost[2]))
        {
            uiManager.UpdateButtonAPStats(stats.APointsLeft);
            float heal = stats.CalculateIntDamage();
            heal = heal * 0.3f;
            player.gameObject.GetComponent<PlayerHealth>().Heal((int)heal);
            Debug.Log("I have healed");
        }

    }
}
