using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private List<GameObject> combatEnemies;

    private GameObject enemyInCombat;

    private EnemySpawner combatSpawner;
    private CombatManagement combatManager;

    public int combatEnemiesDefeated = 0;

    public List<Loot> lootTable;


    private void Awake()
    {
        enemies = new List<GameObject>();
        combatEnemies = new List<GameObject>();
    }

    private void Update()
    {
        //DEBUG STUFF DELETE LATER
        if (Input.GetKeyDown(KeyCode.K))
        {
            combatEnemies[0].GetComponent<Health>().Die();
        }
    }

    public List<GameObject> GetEnemiesInCombat()
    {
        return combatEnemies;
    }

    //Adds an enemy into the list. Doesn't add it if there's a duplicate. returns true if it succeeded
    public bool AddEnemy(GameObject enemy, bool combat = false)
    {
        //Out of combat
        if (!enemies.Contains(enemy) && !combat)
        {
            enemies.Add(enemy);
            return true;
        }
        //In combat of combat
        if (!combatEnemies.Contains(enemy))
        {
            combatEnemies.Add(enemy);
            enemy.GetComponent<Enemy>().drops = lootTable[enemy.GetComponent<EnemyStatsHolder>().StarLevel - 1];
            return true;
        }
        return false;
    }

    //Removes an enemy from the list if it exists. returns true if it succeeded
    public bool RemoveEnemy(GameObject enemy)
    {
        //Out of combat
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            GameManager.instance.aiTurns.RemoveAITurn(enemy.GetComponent<StateMachine>());
            CheckGameStatus();
            return true;
        }
        //In combat
        if (combatEnemies.Contains(enemy))
        {
            combatEnemies.Remove(enemy);
            combatEnemiesDefeated++;
            CheckCombatStatus();
            //Handle true and false cases here
            return true;
        }
        return false;
    }

    //This function will eventually be obsolete as we'll be using the quest system
    //Checks to see if all enemies are gone
    private bool CheckGameStatus()
    {
        //if (enemies.Count <= 0)
        //{
        //    SceneManager.LoadScene("WinScene");
        //    return true;
        //}
        return false;
    }

    private bool CheckCombatStatus()
    {
        if (combatEnemies.Count <= 0)
        {
            enemies.Remove(enemyInCombat);
            enemyInCombat.GetComponent<Enemy>().Cleanup();
            Destroy(enemyInCombat);
            enemyInCombat = null;
            GameManager.instance.combatManagement.EndCombat(true);
            CheckGameStatus();
            return true;
        }
        return false;
    }

    public void EnemyEnterCombat(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            AddEnemy(enemy);
        }
        enemyInCombat = enemy;
        combatManager = GameManager.instance.combatManagement;
        combatSpawner = combatManager.GetComponent<EnemySpawner>();
        Enemy data = enemy.GetComponent<Enemy>();

        //sends all of the data to the enemy spawner
        combatSpawner.baseEnemy = data;

        GameManager.instance.isCombat = true;

        GameManager.instance.aiTurns.EndAllAITurn();

        combatEnemiesDefeated = 0;

        //foreach (GameObject rogue in enemies)
        //{
        //    rogue.GetComponent<AIEnemyController>().FinishActions();
        //}
        combatManager.InitiateTurns(enemy);
    }
}
