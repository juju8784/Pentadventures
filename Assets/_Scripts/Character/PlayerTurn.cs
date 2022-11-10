using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerTurn : Turn
{
    private string[] buttonText;
    protected CombatUIManager uiManager;
    protected TestCharacterController player;
    protected StatsHolder stats;
    public Button[] actionButtons;
    public List<GameObject> buttonPrefabs;
    public GameObject textSpawnPrefab;

    public AudioClip specialSFX;
    public AudioClip attackSFX;
    private GameObject rogue;
    protected List<BaseTile> area = new List<BaseTile>();
    protected int range = 1;
    public bool attacking = false;
    public bool specialActive = false;
    protected bool enemiesHit = false;
    protected BaseTile hitTile;
    protected BaseTile previoushitTile;
    public bool magicalBaseAttack = false;
    public Canvas canvas;

    protected override void Start()
    {
        uiManager = GameManager.instance.combatUIManager;
        player = GetComponent<TestCharacterController>();
        stats = GetComponent<StatsHolder>();
        canvas = GameManager.instance.canvas.GetComponent<Canvas>();
    }

    public override void StartTurn()
    {
        if (!uiManager)
        {
            uiManager = GameObject.FindObjectOfType<CombatUIManager>();
        }
        uiManager.ChangeUIOptions(this, buttonPrefabs);
        if (attacking)
        {
            uiManager.DeactivateButtons();
        }
        uiManager.ShowUIOptions();
        if (!player)
        {
            player = GetComponent<TestCharacterController>();
        }
        player.ResetDirections();
        if (!stats)
        {
            stats = GetComponent<StatsHolder>();
        }
        stats.TurnReset();
        uiManager.UpdateButtonAPStats(stats.APointsLeft);
        rogue = GameManager.instance.rogueInCombat;
        base.StartTurn();
    }

    public override void EndTurn()
    {
        if (isActive)
        {
            player.ResetDirections();
            uiManager.HideUIOptions();
            uiManager.UpdateButtonAPStats(stats.APointsLeft);
            base.EndTurn();
        }       
    }

    public override void TakeTurn()
    {
        if (attacking)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool onTarget = false;
            //Only does these calculations if we can move
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.GetComponent<BaseTile>() || hit.transform.GetComponent<Entity>())
                {
                    if (hit.transform.GetComponent<Entity>())
                    {
                        hitTile = hit.transform.GetComponent<Entity>().currentTile;
                    }
                    else
                    {
                        hitTile = hit.transform.GetComponent<BaseTile>();
                    }
                    if (previoushitTile)
                    {
                        previoushitTile.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, false);
                    }
                    previoushitTile = hitTile;
                    //If the tile is unwalkable
                    if (hitTile.weight == -1 || !area.Contains(hitTile))
                    {
                       hitTile.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, false);
                    }
                    else
                    {
                       hitTile.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, true);   
                    }
                }
                else
                {
                    //Removes highlighted colors if mouse isn't over a tile
                    if (hitTile)
                    {
                        hitTile.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, false);
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                PointerEventData pEventData = new PointerEventData(player.eSystem);
                pEventData.position = Input.mousePosition;

                List<RaycastResult> rResults = new List<RaycastResult>();
                if (!player.gRaycaster)
                {
                    player.gRaycaster = GameManager.instance.gRaycaster;
                }
                player.gRaycaster.Raycast(pEventData, rResults);
                

                if (rResults.Count == 0)
                {
                    if (hit.transform.GetComponent<BaseTile>())
                    {
                        if ((hit.transform.GetComponent<BaseTile>() == hitTile))
                        {
                            onTarget = true;
                        }
                    }
                    else if (hit.transform.GetComponent<Entity>())
                    {
                        if(hit.transform.GetComponent<Entity>().currentTile == hitTile)
                        {
                            onTarget = true;
                        }
                    }
                    if (onTarget)
                    {
                        if (hitTile.entities.Count > 0 && area.Contains(hitTile))
                        {
                            if (hitTile.entities[0].GetComponent<Enemy>())
                            {
                                hitTile.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, false);
                                foreach (var item in area)
                                {
                                    item.colorManager.SetTileState(BaseTile.ColorID.AttackRange, false);
                                }
                                if (GetComponent<CombatCharacterAnimManager>())
                                {
                                    GetComponent<CombatCharacterAnimManager>().SetRandomAttackTrigger();
                                }
                                if (magicalBaseAttack)
                                {
                                    DoMagDamage(hitTile.entities[0].gameObject);
                                }
                                else
                                {
                                    DoDamage(hitTile.entities[0].gameObject);
                                }
                                AudioSource.PlayClipAtPoint(attackSFX, player.transform.position);
                                stats.SubtractActionPoints(stats.block.actionsCost[0]);
                                area.Clear();
                                player.EnableMovement();
                                attacking = false;
                                uiManager.ActivateButtons();
                            }
                            else if (hitTile.entities[0].GetComponent<TestCharacterController>())
                            {
                                Vector3 offset = new Vector3(0, 1.6f, 0.7f);
                                GameObject thisIsAllyText = Instantiate(textSpawnPrefab, hitTile.entities[0].transform.position + offset, Quaternion.identity);
                                
                                thisIsAllyText.transform.SetParent(canvas.transform);
                                thisIsAllyText.GetComponent<TextMeshProUGUI>().text = "This is an ally";
                                thisIsAllyText.GetComponent<TextMeshProUGUI>().color = Color.magenta;
                                thisIsAllyText.transform.position = Camera.main.WorldToScreenPoint(thisIsAllyText.transform.position);
                                Destroy(thisIsAllyText, 2.5f);
                            }
                        }
                        else
                        {
                            Vector3 offset = new Vector3(0, 0.6f, 0.7f);
                            GameObject noEntityText = Instantiate(textSpawnPrefab, hitTile.transform.position + offset, Quaternion.identity);
                            noEntityText.transform.SetParent(canvas.transform);
                            if (player.currentTile.TileDistance(hitTile) <= range)
                            {
                                noEntityText.GetComponent<TextMeshProUGUI>().text = "No enemy on this tile";
                            }
                            else
                            {
                                noEntityText.GetComponent<TextMeshProUGUI>().text = "Out of range";
                            }
                            noEntityText.GetComponent<TextMeshProUGUI>().color = Color.magenta;
                            noEntityText.transform.position = Camera.main.WorldToScreenPoint(noEntityText.transform.position);
                            Destroy(noEntityText, 2.5f);
                        }
                    }
                }
            }
            // right click cancel
            if (Input.GetMouseButtonDown(1))
            {
                hitTile.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, false);
                
                foreach (var item in area)
                {
                    item.colorManager.SetTileState(BaseTile.ColorID.AttackRange, false);
                }
                Vector3 offset = new Vector3(0, 1.6f, 0.7f);
                GameObject cancelAttackText = Instantiate(textSpawnPrefab, player.transform.position + offset, Quaternion.identity);
                cancelAttackText.transform.SetParent(canvas.transform);
                cancelAttackText.GetComponent<TextMeshProUGUI>().text = "Attack Canceled";
                cancelAttackText.GetComponent<TextMeshProUGUI>().color = Color.green;
                cancelAttackText.transform.position = Camera.main.WorldToScreenPoint(cancelAttackText.transform.position);
                Destroy(cancelAttackText, 2.5f);

                area.Clear();
                player.EnableMovement();
                attacking = false;
                uiManager.ActivateButtons();
            }
        }
    }

    public virtual void DoDamage(GameObject enemy)
    {
        bool crit = false;
        int damage = 0;
        Health t = enemy.GetComponent<Health>();
        StartCoroutine(HitRateDealDamage(t, damage, crit));
        transform.rotation = Quaternion.LookRotation(enemy.transform.position - transform.position, Vector3.up);
    }
    public virtual void DoMagDamage(GameObject enemy)
    {
        bool crit = false;
        int damage = 0;
        Health t = enemy.GetComponent<Health>();
        StartCoroutine(HitRateDealMagicalDamage(t, damage, crit));
        transform.rotation = Quaternion.LookRotation(enemy.transform.position - transform.position, Vector3.up);
    }

    public override void Attack()
    {
        if (!stats)
        {
            stats = GetComponent<StatsHolder>();
        }

        if (stats.CheckForPoints(stats.block.actionsCost[0])) // check for points
        {
            area = player.currentTile.BreadthFirst(range); // highlights area
            area.Remove(player.currentTile);
            foreach (var item in area)
            {
                item.colorManager.SetTileState(BaseTile.ColorID.AttackRange, true);
            }
            //
            player.DisableMovement();
            attacking = true;
            uiManager.DeactivateButtons();
            //for (int i = 0; i < area.Count; i++)
            //{
            //    if (area[i].entities.Count == 1)
            //    {
            //        StateMachine enemy = area[i].entities[0].GetComponent<StateMachine>();
            //        if (enemy)
            //        {

            //                uiManager.UpdateButtonAPStats(stats.APointsLeft);
            //                int damage = 0;
            //                bool crit = false;
            //                //if (Random.Range(0, 100) < (player.gameObject.GetComponent<StatsHolder>().CalculateCritChance()))
            //                //{
            //                //    damage = player.gameObject.GetComponent<StatsHolder>().CalculatePhyCritDamage();
            //                //    crit = true;
            //                //}
            //                //else
            //                //{
            //                //    damage = player.gameObject.GetComponent<StatsHolder>().CalculatePhyDamage();
            //                //}

            //                Debug.Log("I have attacked");
            //                //damage /= stats.HitRate;
            //                StartCoroutine(HitRateDealDamage(enemy.GetComponent<Health>(), damage, crit));
            //                transform.rotation = Quaternion.LookRotation(enemy.transform.position - transform.position, Vector3.up);
            //                if (attack)
            //                {
            //                    attack.PlaySound();
            //                }
            //        }
            //        return;
            //    }
            //}
        }
        
    }

    public virtual IEnumerator HitRateDealDamage(Health target, int damage, bool crit)
    {
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
        }
    }

    public IEnumerator HitRateDealMagicalDamage(Health target, int damage, bool crit)
    {
        for (int i = 0; i < stats.HitRate; i++)
        {
            if (Random.Range(0, 100) < (player.gameObject.GetComponent<StatsHolder>().CalculateCritChance()))
            {
                damage = player.gameObject.GetComponent<StatsHolder>().CalculateMagCritDamage();
                crit = true;
            }
            else
            {
                damage = player.gameObject.GetComponent<StatsHolder>().CalculateIntDamage();
                crit = false;
            }
            damage = (int)((float)damage * 0.5f);
            damage /= stats.HitRate;
            if (target)
            {
                target.TakeDamage(damage, stats.HitRate, crit, true);
            }
            else
            {
                StopCoroutine(HitRateDealMagicalDamage(target, damage, crit));
            }
            yield return new WaitForSeconds(0.7f);
        }
    }

    public override void Special()
    {
        //       ------------------------------ code for regular attack -----------------------------
        //    implement actual specials in each characters' scripts, which will be children of this scrip
        //                          override this function pretty much
        if (!stats)
        {
            stats = GetComponent<StatsHolder>();
        }
        if (stats.CheckForPoints(stats.block.actionsCost[0])) // check for points
        {
            area = player.currentTile.BreadthFirst(range); // highlights area
            foreach (var item in area)
            {
                item.colorManager.SetTileState(BaseTile.ColorID.AttackRange, true);
            }
            player.DisableMovement();
            attacking = true;
            uiManager.DeactivateButtons();
        }
    }

    public override void Heal()
    {
        // TODO
        // Heal based on 'CheckForHealingPotionsInInventory' (to be created)
        if (!stats)
        {
            stats = GetComponent<StatsHolder>();
        }
        if (stats.SubtractActionPoints(stats.block.actionsCost[2]))
        {
            uiManager.UpdateButtonAPStats(stats.APointsLeft);
            float heal = stats.CalculateHealth();
            heal = heal * 0.1f;
            player.gameObject.GetComponent<PlayerHealth>().Heal((int)heal);
            Debug.Log("I have healed");
        }
        //if(QuestGiver.giver.questsList[2].isActive == true)
        //{
        //    QuestGiver.giver.questsList[2].goal.PlayerHealed(300);
        //}
    }

    public virtual void Death()
    {

    }
    public virtual void ResetOnEndCombat()
    {
        player.ResetDirections();
        uiManager.HideUIOptions();
        uiManager.UpdateButtonAPStats(stats.APointsLeft);
    }
}
