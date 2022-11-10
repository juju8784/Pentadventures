using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDecisionVisionRange : AIDecision
{
    //change this to the stat ranges in the future
    public int visionRange = 3;
    
    public override bool RunDecision()
    {
        CurrentValue = CheckIfPlayerIsInVisonRange();
        return CurrentValue;
    }

    public bool CheckIfPlayerIsInVisonRange()
    {
        BaseTile playerTile = GameManager.instance.player.GetComponent<TestCharacterController>().currentTile;
        return stateMachine.EnemyController.currentTile.TileDistance(playerTile) <= visionRange;
    }
}
