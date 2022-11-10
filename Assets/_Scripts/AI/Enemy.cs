using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will handle any information that needs to be set for any
/// enemy that I may make. For now it is just the enemyManager
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] private bool active = true;
    private EnemyManager manager;
    public List<GameObject> spawns;
    public EnemyAnimationHandler anim;
    public EnemyStatsHolder stats;
    public Loot drops;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        manager = GameManager.instance.enemyManager;
        manager.AddEnemy(gameObject, GameManager.instance.isCombat);
        GameManager.instance.aiTurns.AddAITurn(GetComponent<StateMachine>());
        anim = GetComponent<EnemyAnimationHandler>();
        stats = GetComponent<EnemyStatsHolder>();
    }

    public virtual void LostCombat()
    {
        GameManager.instance.combatManagement.RemoveTurn(GetComponent<Turn>());
        GameManager.instance.combatManagement.AddLoot(drops.GetRandomLoot());
        if (GetComponent<Health>().healthBar)
        {
            Destroy(GetComponent<Health>().healthBar.gameObject);
        }
        Cleanup();
        Destroy(gameObject, 2.5f);
    }
    public virtual void Die()
    {
        anim.SetAnimationTrigger("Death");
        LostCombat();
    }

    public void Cleanup()
    {
        manager.RemoveEnemy(gameObject);
        GetComponent<AIEnemyController>().currentTile.RemoveEntity(GetComponent<AIEnemyController>());
    }

    public void Activate()
    {
        if (!GameManager.instance.aiTurns.aiTurns.ContainsKey(GetComponent<StateMachine>()))
        {
            GameManager.instance.aiTurns.AddAITurn(GetComponent<StateMachine>());
        }
        active = true;
    }

    public void Deactivate()
    {
        if (GameManager.instance.aiTurns.aiTurns.ContainsKey(GetComponent<StateMachine>()))
        {
            GameManager.instance.aiTurns.RemoveAITurn(GetComponent<StateMachine>());
        }
        active = false;
    }

}
