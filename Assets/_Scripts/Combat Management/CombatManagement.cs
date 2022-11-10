using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManagement : MonoBehaviour
{
    //a collection of all of the turns
    public List<Turn> turns = new List<Turn>();
    public int Round = 0;

    //uses id's to keep track of the turn order
    public List<int> order = new List<int>();

    public int currentTurnID = 0;
    private int nextIDToGive = 0;

    private EnemySpawner spawner;
    [SerializeField] private PlayerSpawner plSpawner;

    //Save camera data
    private Vector3 prevCamera;

    public EndCombatUI combatUI;

    //Loot
    public List<int> lootGained;
    public List<string> deadCharacters = new List<string>();
    public Wallet wallet;

    private void Start()
    {
        spawner = GetComponent<EnemySpawner>();
    }

    public void InitiateTurns(GameObject enemy)
    {
        List<GameObject> characters = GameManager.instance.partyManager.active.GetMembers();
        for (int i = 0; i < characters.Count; i++)
        {
            //characters[i].transform.position = new Vector3(56.2f, 1f, (-18 + (i * 1.2f)));
            characters[i].SetActive(true);
            AddTurn(characters[i].GetComponent<Turn>());
            characters[i].GetComponent<Turn>().toggle = false;
            characters[i].GetComponent<StatsHolder>().ResetActionPointsNewCombat();
            characters[i].transform.position = plSpawner.GetRandomPositionInCombat();
        }

        //Enemy spawns
        List<GameObject> enemies = spawner.SpawnEnemies();
        for (int i = 0; i < enemies.Count; i++)
        {
            AddTurn(enemies[i].GetComponent<Turn>());
            //spawner.SetRandomStats(enemies[i]);
        }

        //Ui
        GameManager.instance.uiManager.UpdatePartyUI();

        //Camera Stuff
        prevCamera = GameManager.instance.cameraParent.transform.position;
        GameManager.instance.cameraParent.transform.position = new Vector3(63, -33, -28);

        lootGained = new List<int>();
        for (int i = 0; i < 5; i++)
        {
            lootGained.Add(0);
        }

        //dead characters
        deadCharacters = new List<string>();

        //Starting at the first turn
        for (int i = 0; i < turns.Count; i++)
        {
            if (turns[i].id == currentTurnID)
            {
                turns[i].StartTurn();
            }
        }
    }

    public void NextTurn()
    {
        //need to code this for speed/initiative later
        
        if (currentTurnID == order[order.Count - 1])
        {
            currentTurnID = order[0];
            Round++;
        }
        else
        {
            currentTurnID = order[order.IndexOf(currentTurnID) + 1];
        }

        for (int i = 0; i < turns.Count; i++)
        {
            if (turns[i].id == currentTurnID)
            {
                turns[i].StartTurn();
            }
        }
        GameManager.instance.uiManager.UpdatePartyUI();
    }

    public void AddTurn(Turn toAdd)
    {
        turns.Add(toAdd);
        toAdd.id = nextIDToGive;
        nextIDToGive++;
        toAdd.cm = this;
        int initiative = toAdd.GetComponent<StatsHolder>().CalculateMovement();
        //Change later once we have speed/initiative
        for (int i = 0; i < order.Count; i++)
        {
            Turn compare = FindTurn(order[i]);
            if (compare)
            {
                if (compare.GetComponent<StatsHolder>().CalculateMovement() < initiative)
                {
                    order.Insert(i, toAdd.id);
                    return;
                }
            }
        }
        order.Add(toAdd.id);

        GameManager.instance.uiManager.UpdatePartyUI();
    }

    public void RemoveTurn(Turn toRemove)
    {
        if (toRemove.id == currentTurnID)
        {
            NextTurn();
        }
        order.Remove(toRemove.id);
        turns.Remove(toRemove);
        GameManager.instance.uiManager.UpdatePartyUI();
    }

    public Turn FindTurn(int id)
    {
        for (int i = 0; i < turns.Count; i++)
        {
            if (turns[i].id == id)
            {
                return turns[i];
            }
        }
        return null;
    }

    public void EndCombat(bool playerWon)
    {
        for (int i = 0; i < turns.Count; i++)
        {
            PlayerTurn p = turns[i].GetComponent<PlayerTurn>();
            if (p)
            {
                p.ResetOnEndCombat();
            }
        }
        ResetCombat();
        List<string> tempNames = new List<string>();
        tempNames.Add("Leonidas");
        tempNames.Add("Erin");
        combatUI.CombatEnded(playerWon, GameManager.instance.enemyManager.combatEnemiesDefeated, deadCharacters, lootGained);
        wallet.AddLoot(lootGained);
        //update party stuff
        //turn back wolrd turn stuff
        //update the world party object script called party object
        
    }

    public void ResetCombat()
    {
        for (int i = 0; i < turns.Count; i++)
        {
            turns[i].isActive = false;
            turns[i].toggle = false;
        }
        currentTurnID = 0;
        nextIDToGive = 0;
        turns.Clear();
        order.Clear();
    }

    public void MoveToWorld()
    {
        GameManager.instance.isCombat = false;
        GameManager.instance.cameraParent.transform.position = prevCamera;
    }

    public void AddLoot(List<int> loot)
    {
        for (int i = 0; i < loot.Count; i++)
        {
            lootGained[i] += loot[i];
        }
    }
}
