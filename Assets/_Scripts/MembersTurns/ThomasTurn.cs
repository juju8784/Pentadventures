using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThomasTurn : PlayerTurn
{

    public GameObject blizzardEffectPrefab;
    List<BaseTile> blizzardArea = new List<BaseTile>();
    [SerializeField] List<GameObject> entitiesAffected = new List<GameObject>();
    [SerializeField] List<GameObject> entitiesAffectedOutOfArea = new List<GameObject>();
    public int specialRange = 3;
    public int turnsLasting = 3;
    public int slowPercentage = 20;
    int turnsSinceActivation = -1;
    //bool specialActive = false;

    public override void StartTurn()
    {
        if (specialActive)
        {
            turnsSinceActivation++;
            if (turnsSinceActivation >= turnsLasting)
            {
                specialActive = false;
                DeactivateSpecial();
            }
            else
            {
                foreach (var unit in entitiesAffected)
                {
                    StatsHolder temp = unit.GetComponent<StatsHolder>();
                    temp.AddSlowEffect(slowPercentage);
                    DoMagDamage(unit);
                }
            }
        }
        base.StartTurn();
    }

    public override void TakeTurn()
    {
        if (specialActive)
        {
            // check if all entities are still valid
            foreach (var item in entitiesAffected)
            {
                if(item  == null)
                {
                    entitiesAffected.Remove(item);
                }
            }
            foreach (var item in entitiesAffectedOutOfArea)
            {
                if (item == null)
                {
                    entitiesAffectedOutOfArea.Remove(item);
                }
            }

            foreach (var item in blizzardArea)
            {
                item.colorManager.SetTileState(BaseTile.ColorID.Ice, false);
            }
            blizzardArea.Clear();
            blizzardArea = player.currentTile.BreadthFirst(specialRange); // highlights area
            entitiesAffectedOutOfArea = new List<GameObject>(entitiesAffected);
            entitiesAffected.Clear();
            foreach (var item in blizzardArea)
            {
                item.colorManager.SetTileState(BaseTile.ColorID.Ice, true);
                if (item.entities.Count > 0)
                {
                    if (item.entities[0].GetComponent<AIEnemyController>())
                    {
                        if (!entitiesAffected.Contains(item.entities[0]))
                        {
                            entitiesAffected.Add(item.entities[0]);
                        }
                    }
                }
            }
            for (int i = 0; i < entitiesAffectedOutOfArea.Count; i++)
            {
                if (entitiesAffected.Contains(entitiesAffectedOutOfArea[i]))
                {
                    StatsHolder temp = entitiesAffectedOutOfArea[i].GetComponent<StatsHolder>();
                    temp.RemoveSlowEffect();
                    entitiesAffectedOutOfArea.Remove(entitiesAffectedOutOfArea[i]);

                }
            }
            foreach (var unit in entitiesAffected)
            {
                StatsHolder temp = unit.GetComponent<StatsHolder>();
                temp.AddSlowEffect(slowPercentage);
            }
        }
        base.TakeTurn();
    }

    // call this when an enemy dies somehow
    public void RemoveEntityFromLists(GameObject pc)
    {
        entitiesAffected.Remove(pc);
        entitiesAffectedOutOfArea.Remove(pc);
    }

    public override void Attack()
    {
        range = 1;
        base.Attack();
    }

    public override void Special()
    {
        if (stats.SubtractActionPoints(stats.block.actionsCost[1])) // check for points
        {
            blizzardArea = player.currentTile.BreadthFirst(specialRange); // highlights area
            foreach (var item in blizzardArea)
            {
                item.colorManager.SetTileState(BaseTile.ColorID.Ice, true);
                if(item.entities.Count > 0)
                {
                    if (item.entities[0].GetComponent<AIEnemyController>())
                    {
                        if (!entitiesAffected.Contains(item.entities[0]))
                        {
                            entitiesAffected.Add(item.entities[0]);
                        }
                    }
                }
            }
            AudioSource.PlayClipAtPoint(specialSFX, player.transform.position);
            foreach (var unit in entitiesAffected)
            {
                StatsHolder temp = unit.GetComponent<StatsHolder>();
                temp.AddSlowEffect(slowPercentage);
                DoMagDamage(unit);
            }
            // slow down everyone in the area;
            // spawn the snow effect on each tile
            //specialActive = true;
            turnsSinceActivation = 0;
            
        }
    }

    public void DeactivateSpecial()
    {
        foreach (var item in blizzardArea)
        {
            item.colorManager.SetTileState(BaseTile.ColorID.Ice, false);
        }
        blizzardArea.Clear();
        foreach (var unit in entitiesAffected)
        {
            StatsHolder temp = unit.GetComponent<StatsHolder>();
            temp.RemoveSlowEffect();
        }
        entitiesAffected.Clear();
        foreach (var unit in entitiesAffectedOutOfArea)
        {
            StatsHolder temp = unit.GetComponent<StatsHolder>();
            temp.RemoveSlowEffect();
        }
        entitiesAffectedOutOfArea.Clear();
        // Take off slow effect of affected people;
        // destroy the snow effect on each tile
    }

    public override void Death()
    {
        DeactivateSpecial();
        base.Death();
    }
    public override void ResetOnEndCombat()
    {
        turnsSinceActivation = -1;
        DeactivateSpecial();
        specialActive = false;
        base.ResetOnEndCombat();
    }

}
