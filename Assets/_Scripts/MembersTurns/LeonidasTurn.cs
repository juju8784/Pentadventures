using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeonidasTurn : PlayerTurn
{
    //bool specialActive = false;
    int specialSpearsCount = 10;
    private List<GameObject> enemiesWithStacks = new List<GameObject>();
    [SerializeField] private GameObject stackVFX;

    public override void Attack()
    {
        range = 3;
        base.Attack();
    }
    public override void DoDamage(GameObject enemy)
    {
        AddStack(enemy);
        int damage = 0;
        bool crit = false;
        Health h = enemy.GetComponent<Health>();
        StartCoroutine(HitRateDealDamage(h, damage, crit));
        transform.rotation = Quaternion.LookRotation(enemy.transform.position - transform.position, Vector3.up);
    }

    public override void DoMagDamage(GameObject enemy)
    {
        LeoStacks ls = enemy.GetComponent<LeoStacks>();
        if (ls)
        {
            if (ls.AddStack(specialActive))
            {
                enemiesWithStacks.Remove(enemy);
            }
        }
        else
        {
            enemy.AddComponent<LeoStacks>();
            enemy.GetComponent<LeoStacks>().AddStack(specialActive);
            enemiesWithStacks.Add(enemy);
        }
        base.DoMagDamage(enemy);
    }
    public override IEnumerator HitRateDealDamage(Health target, int damage, bool crit)
    {
        if (!target)
        {
            Debug.Log("Target in null in leonidas turn script");
        }
        for (int i = 0; i < stats.HitRate; i++)
        {
            if (Random.Range(0, 100) < (player.gameObject.GetComponent<StatsHolder>().CalculateCritChance()))
            {
                damage = player.gameObject.GetComponent<StatsHolder>().CalculatePhyCritDamage();
                crit = true;
            }
            else
            {
                damage = player.gameObject.GetComponent<StatsHolder>().CalculatePhyDamage();
                crit = false;
            }
            damage /= stats.HitRate;
            if (target)
            {
                target.TakeDamage(damage, stats.HitRate, crit, false);
            }
            else
            {
                StopCoroutine(HitRateDealDamage(target, damage, crit));
            }
            yield return new WaitForSeconds(0.7f);
            if (!isActive)
            {
                break;
            }
        }
    }
    public override void Special()
    {
        if (!stats)
        {
            stats = GetComponent<StatsHolder>();
        }
        if (stats.SubtractActionPoints(stats.block.actionsCost[1])) // check for points
        {
            specialActive = true;
            List<GameObject> enemiesWOStacks = new List<GameObject>();
            specialSpearsCount = Random.Range(6, 12);
            foreach (var enemy in GameManager.instance.enemyManager.GetEnemiesInCombat())
            {
                if (!enemiesWithStacks.Contains(enemy))
                {
                    enemiesWOStacks.Add(enemy);
                }
            }
            if (enemiesWithStacks.Count > 0)
            {
                while (enemiesWithStacks.Count > 0 && specialSpearsCount > 0)
                {
                    if (AddStack(enemiesWithStacks[0]))
                    {
                        specialSpearsCount--;
                    }
                    else
                    {
                        enemiesWithStacks.RemoveAt(0);
                    }
                    if (enemiesWithStacks.Count == 0)
                    {
                        break;
                    }
                    if (specialSpearsCount == 0)
                    {
                        break;
                    }
                }
            }
            for (int i = 0; (i < enemiesWOStacks.Count) && (specialSpearsCount > 0); i++)
            {
                for (int j = 0; (j < 5) && (specialSpearsCount > 0); j++)
                {
                    if (AddStack(enemiesWOStacks[i]))
                    {
                        specialSpearsCount--;
                    }
                    else
                    {
                        enemiesWOStacks.RemoveAt(i);
                        i--;
                    }
                }
            }
            
            ExplodeStacks();
        }
    }

    public void SpecialDoDamage(GameObject enemy, int damage, bool crit, bool explosion = false)
    {
        //bool crit = false;
        Health h = enemy.GetComponent<Health>();
        StartCoroutine(HitRateDealDamageForSpecial(h, damage, crit, explosion));
        transform.rotation = Quaternion.LookRotation(enemy.transform.position - transform.position, Vector3.up);
    }
    public IEnumerator HitRateDealDamageForSpecial(Health target, int damage, bool crit, bool explosion = false)
    {
        if (!target)
        {
            Debug.Log("Target in null in leonidas turn script");
        }
        damage /= stats.HitRate;
        for (int i = 0; i < stats.HitRate; i++)
        {
            if (target)
            {
                if (explosion)
                {
                    target.TakeLeoExplosionDamage(damage, stats.HitRate, crit);
                }
                else
                {
                    target.TakeDamage(damage, stats.HitRate, crit, true);
                }
            }
            yield return new WaitForSeconds(0.9f);
            if (!isActive)
            {
                break;
            }
        }
        specialActive = false;
    }

    public bool AddStack(GameObject enemy)
    {
        int damage = 0;
        if (!enemy)
        {
            return false;
        }
        LeoStacks ls = enemy.GetComponent<LeoStacks>();
        if (ls)
        {
            if (ls.AddStack(specialActive))
            {
                enemiesWithStacks.Remove(enemy);
                int temp;
                bool crit;
                if (Random.Range(0, 100) < (player.gameObject.GetComponent<StatsHolder>().CalculateCritChance()))
                {
                    temp = player.gameObject.GetComponent<StatsHolder>().CalculatePhyCritDamage();
                    crit = true;
                    Debug.Log("LeoCrit");
                }
                else
                {
                    temp = player.gameObject.GetComponent<StatsHolder>().CalculatePhyDamage();
                    crit = false;
                    Debug.Log("Leo Not Crit");
                }
                temp = (int)((float)temp * 0.7f);
                for (int i = 0; i < ls.maxStacks; i++)
                {
                    damage += temp;
                }
                if (specialActive)
                {
                    SpecialDoDamage(enemy, damage, crit);
                }
                else
                {
                    enemy.GetComponent<Health>().TakeDamage(damage, stats.HitRate, crit, true);
                }
                for (int i = 0; i < 6; i++)
                {
                    if (enemy.GetComponent<AIEnemyController>().currentTile.neighbors[i].entities.Count > 0)
                    {
                        GameObject neightbor = enemy.GetComponent<AIEnemyController>().currentTile.neighbors[i].entities[0];
                        if (neightbor)
                        {
                            if (neightbor.GetComponent<AIEnemyController>())
                            {
                                if (specialActive)
                                {
                                    SpecialDoDamage(neightbor, damage, crit);
                                }
                                else
                                {
                                    neightbor.GetComponent<Health>().TakeDamage(damage, stats.HitRate, crit, true);
                                }
                            }
                        }
                    }
                }
                Destroy(enemy.GetComponent<LeoStacks>());
            }
        }
        else
        {
            enemy.AddComponent<LeoStacks>();
            enemy.GetComponent<LeoStacks>().AddStack(specialActive);
            enemiesWithStacks.Add(enemy);
            if (stackVFX)
            {
              // add the spear VXF to show the stacks   
            }
        }
        return true;
    }

    public void ExplodeStacks()
    {
        int temp;
        bool crit;
        if (Random.Range(0, 100) < (player.gameObject.GetComponent<StatsHolder>().CalculateCritChance()))
        {
            temp = player.gameObject.GetComponent<StatsHolder>().CalculatePhyCritDamage();
            crit = true;
            Debug.Log("LeoCrit");
        }
        else
        {
            temp = player.gameObject.GetComponent<StatsHolder>().CalculatePhyDamage();
            crit = false;
            Debug.Log("Leo Not Crit");
        }
        temp = (int)((float)temp * 0.7f);
        int damage = 0;
        for (int d = 0; d < enemiesWithStacks.Count; d++)
        {
            GameObject enemy = enemiesWithStacks[d];
            if (!enemy)
            {
                continue;
            }
            LeoStacks ls = enemy.GetComponent<LeoStacks>();
            damage = 0;
            for (int i = 0; i < ls.maxStacks; i++)
            {
                damage += temp;
            }
            for (int i = 0; i < 6; i++)
            {
                if (enemy.GetComponent<AIEnemyController>().currentTile.neighbors[i].entities.Count > 0)
                {
                    GameObject neightbor = enemy.GetComponent<AIEnemyController>().currentTile.neighbors[i].entities[0];
                    if (neightbor)
                    {
                        if (neightbor.GetComponent<AIEnemyController>())
                        {
                            SpecialDoDamage(neightbor, damage, crit);
                        }
                    }
                }
            }
            SpecialDoDamage(enemy, damage, crit);
            if (enemy)
            {
                Destroy(enemy.GetComponent<LeoStacks>());
            }
        }
    }
    public override void ResetOnEndCombat()
    {
        enemiesWithStacks.Clear();
        specialActive = false;
        base.ResetOnEndCombat();
    }
}
