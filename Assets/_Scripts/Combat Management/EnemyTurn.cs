using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// This is a very generic, very basic enemy turn. This will most likely get deleted in the future
/// </summary>
public class EnemyTurn : Turn
{
    public AIEnemyController controller;
    private StatsHolder stats;
    private EnemyAnimationHandler anim;

    public int damage = 3;
    bool crit = false;
    private bool pathSet;

    private TestCharacterController player;
    private List<BaseTile> neighbors;

    protected override void Start()
    {
        base.Start();
        stats = GetComponent<StatsHolder>();
        pathSet = false;
        anim = GetComponent<EnemyAnimationHandler>();
    }

    public override void StartTurn()
    {
        controller.ResetDirections();
        stats.TurnReset();
        if (Random.Range(0, 100) < (this.gameObject.GetComponent<StatsHolder>().CalculateCritChance()))
        {
            damage = this.gameObject.GetComponent<StatsHolder>().CalculatePhyCritDamage();
            crit = true;
        }
        else
        {
            damage = this.gameObject.GetComponent<StatsHolder>().CalculatePhyDamage();
        }
        damage /= stats.HitRate;
        List<GameObject> party = GameManager.instance.partyManager.active.GetMembers();
        if (party.Count > 0)
        {
            int closest = 64;
            int index = 0;
            for (int i = 0; i < party.Count; i++)
            {
                int t = party[i].GetComponent<TestCharacterController>().currentTile.TileDistance(this.GetComponent<AIEnemyController>().currentTile);
                if(t < closest)
                {
                    closest = t;
                    index = i;
                }
            }
            player = party[index].GetComponent<TestCharacterController>();
            
        }

        pathSet = false;
        base.StartTurn();
    }

    public override void TakeTurn()
    {
        if (!player)
        {
            player = GameManager.instance.player.GetComponent<TestCharacterController>();
        }
        neighbors = player.currentTile.neighbors.ToList();
        //If we are right next to the player
        if (neighbors.Contains(controller.directions[0]))
        {
            //Attack
            transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);
            //transform.Rotate(Vector3.up, -90);
            anim.SetAnimationTrigger("Attack");
            StartCoroutine(HitRateDealDamage(damage, crit));
            EndTurn();
        }
        else if (!pathSet)
        {
            List<BaseTile> path = controller.currentTile.GreedyPath(player.currentTile, stats.GetMovementLeft());
            //Go to a neighbor
            path.Remove(player.currentTile);
            controller.AddDirections(path);
            pathSet = true;
        }
        else if ((!stats.CanMove() || pathSet) && controller.stopped)
        {
            EndTurn();
        }
    }

    IEnumerator HitRateDealDamage(int damage, bool crit)
    {
        for (int i = 0; i < stats.HitRate; i++)
        {
            if(player)
                player.GetComponent<Health>().TakeDamage(damage, stats.HitRate, crit);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
