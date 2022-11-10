using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIActionMoveTowardsPlayer : AIAction
{

    private TestCharacterController player;
    private List<BaseTile> neighbors;
    private AIEnemyController controller;
    private StatsHolder stats;

    private BaseTile targetTile;

    protected override void Start()
    {
        base.Start();
        stats = stateMachine.stats;
        
        controller = stateMachine.EnemyController;
    }

    public override void OnActiveChange()
    {
        targetTile = null;
    }

    //Moves 1 tile towards its destination
    public override void RunAction()
    {
        if (!player)
        {
            player = GameManager.instance.player.GetComponent<TestCharacterController>();
        }

        if (controller.stopped)
        {
            if (stats.CanMove())
            {
                if (!targetTile)
                {
                    List<BaseTile> path = controller.currentTile.GreedyPath(player.currentTile, 1);
                    if (path.Count > 0)
                    {
                        controller.AddDirections(path);
                        targetTile = path[path.Count - 1];
                    }
                    //stats.Move(1);
                }
                else if (targetTile == controller.currentTile)
                {
                    ActionFinished = true;
                }
                else
                {
                    Debug.Log("Movement gone wrong in ai " + gameObject.name);
                }
            }
            else
            {
                ActionFinished = true;
            }
        }
    }
}
