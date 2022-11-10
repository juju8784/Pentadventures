using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIActionInitiateCombat : AIAction
{
    public override void RunAction()
    {
        stateMachine.EnemyController.directions.Clear();
        //enemyStateMachine.EnemyController.directions.RemoveRange(1, enemyStateMachine.EnemyController.directions.Count - 1);
        GameManager.instance.rogueInCombat = gameObject;
        GameManager.instance.enemyManager.EnemyEnterCombat(gameObject);
        //GameManager.instance.isCombat = true;
        //GameManager.instance.combatManagement.InitiateTurns(gameObject);
        ActionFinished = true;
        //GameManager.instance.CombatManager.StartCombat();
    }
}
