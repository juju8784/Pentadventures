using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEnemyParty : MonoBehaviour
{
    public List<Enemy> enemies;

    private CombatManagement combatManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isCombat)
        {
            if (enemies.Count <= 0)
            {
                combatManager.EndCombat(true);
            }
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    public void ResetEnemies()
    {
        enemies.Clear();
    }
}
