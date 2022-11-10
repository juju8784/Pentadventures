using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class SomaTurn : PlayerTurn
{
    [SerializeField] private HexGrid grid;
    //List<BaseTile> outerRing = new List<BaseTile>();
    List<BaseTile> coneArea = new List<BaseTile>();
    public int specialRange = 5;
    //bool specialActive = false;

    protected override void Start()
    {
        grid = GameManager.instance.combatGrid;
        base.Start();
    }
    public override IEnumerator HitRateDealDamage(Health target, int damage, bool crit)
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
            damage = (int)((float)damage * 1.75f);
            damage /= stats.HitRate;
            if (target)
            {
                target.TakeDamage(damage, stats.HitRate, crit, false);
            }
            yield return new WaitForSeconds(0.7f);
        }
    }

    public override void Attack()
    {
        range = 1;
        base.Attack();
    }

    public override void TakeTurn()
    {
        base.TakeTurn();
        if (specialActive)
        {
            // error checking
            if (coneArea.Count == 0)
            {
                coneArea.Add(player.currentTile);
            }
            else if (coneArea[0] != player.currentTile)
            {
                if (coneArea.Count >= 2)
                {
                    if (coneArea[1] != player.currentTile)
                    {
                        foreach (var item in coneArea)
                        {
                            item.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, false);
                        }
                        coneArea.Clear();
                        coneArea.Add(player.currentTile);
                    }
                }
                else
                {
                    foreach (var item in coneArea)
                    {
                        item.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, false);
                    }
                    coneArea.Clear();
                    coneArea.Add(player.currentTile);
                }
            }

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Only does these calculations if we can move
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.GetComponent<BaseTile>())
                {
                    BaseTile hitTile = hit.transform.GetComponent<BaseTile>();

                    //If the tile is unwalkable
                    if (hitTile.weight == -1)
                    {
                        foreach (var item in coneArea)
                        {
                            item.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, false);
                        }
                        coneArea.Clear();
                    }
                    else if (coneArea.Count == 1 || hitTile != coneArea[coneArea.Count - 1])
                    {
                        if (coneArea.Count >= 1)
                        {
                            foreach (var item in coneArea)
                            {
                                item.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, false);
                            }
                        }
                        coneArea.Clear();
                        // show affected area
                        List<Hex> lHex = HexMathLib.HexCone(player.currentTile.hex, hitTile.hex, specialRange);
                        coneArea = grid.GetTiles(lHex);
                        foreach (var item in coneArea)
                        {
                            item.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, true);
                        }
                    }
                }
                else
                {
                    //Removes highlighted colors if mouse isn't over a tile
                    foreach (var item in coneArea)
                    {
                        item.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, false);
                    }
                    coneArea.Clear();
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

                if (rResults.Count == 0 && hit.transform.GetComponent<BaseTile>())
                {
                    if (coneArea.Contains(hit.transform.GetComponent<BaseTile>()))
                    {
                        foreach (var item in coneArea)
                        {
                            item.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, false);
                        }
                        //foreach (var item in outerRing)
                        //{
                        //    item.colorManager.SetTileState(BaseTile.ColorID.AttackRange, false);
                        //}
                        enemiesHit = false;
                        for (int i = 0; i < coneArea.Count; i++)
                        {
                            if (coneArea[i].entities.Count > 0)
                            {
                                if (coneArea[i].entities[0].GetComponent<AIEnemyController>())
                                {
                                    enemiesHit = true;
                                    DoDamageForSpecial(coneArea[i].entities[0].gameObject);
                                }
                            }

                        }
                        stats.SubtractActionPoints(stats.block.actionsCost[1]);
                        if (!enemiesHit)
                        {
                            player.EnableMovement();
                            uiManager.ActivateButtons();
                        }
                        //outerRing.Clear();
                        specialActive = false;
                    }
                }
            }
            // right click cancel
            if (Input.GetMouseButtonDown(1))
            {
                foreach (var item in coneArea)
                {
                    item.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, false);
                }
                //foreach (var item in outerRing)
                //{
                //    item.colorManager.SetTileState(BaseTile.ColorID.AttackRange, false);
                //}
                Vector3 offset = new Vector3(0, 1.6f, 0.7f);
                GameObject dmgRedText = Instantiate(textSpawnPrefab, player.transform.position + offset, Quaternion.identity);
                dmgRedText.transform.SetParent(canvas.transform);
                dmgRedText.GetComponent<TextMeshProUGUI>().text = "Special Canceled";
                dmgRedText.GetComponent<TextMeshProUGUI>().color = Color.green;
                dmgRedText.transform.position = Camera.main.WorldToScreenPoint(dmgRedText.transform.position);
                Destroy(dmgRedText, 2.5f);

                //outerRing.Clear();
                player.EnableMovement();
                specialActive = false;
                uiManager.ActivateButtons();
            }
        }
    }
    void DoDamageForSpecial(GameObject enemy)
    {
        bool crit = false;
        StartCoroutine(HitRateDealDamageSpecial(enemy.GetComponent<Health>(), 3, crit));
        transform.rotation = Quaternion.LookRotation(enemy.transform.position - transform.position, Vector3.up);
    }

    public IEnumerator HitRateDealDamageSpecial(Health target, int hitRate, bool crit = false)
    {
        int damage = 0;
        for (int i = 0; i < hitRate; i++)
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
            damage /= hitRate;
            damage = (int)((float)damage * 1.75f);
            if (target)
            {
                target.TakeDamage(damage, hitRate, crit, false);
            }
            else
            {
                StopCoroutine(HitRateDealDamageSpecial(target, hitRate, crit));
            }
            yield return new WaitForSeconds(0.7f);
        }
        player.EnableMovement();
        specialActive = false;
        uiManager.ActivateButtons();
    }
    public override void Special()
    {
        if (!stats)
        {
            stats = GetComponent<StatsHolder>();
        }
        if (stats.CheckForPoints(stats.block.actionsCost[1]))
        {
            //List<BaseTile> tilesInRange = gameObject.GetComponent<TestCharacterController>().currentTile.BreadthFirst(specialRange);
            //for (int i = tilesInRange.Count - 1 - (6 * specialRange); i < tilesInRange.Count; i++)
            //{
            //    if (tilesInRange[i].GreedyPathDistance(player.currentTile, specialRange, true) == specialRange)
            //    {
            //        outerRing.AddRange(tilesInRange.GetRange(i, tilesInRange.Count - i));
            //        break;
            //    }
            //}
            //foreach (var item in outerRing)
            //{
            //    item.colorManager.SetTileState(BaseTile.ColorID.AttackRange, true);
            //}
            player.DisableMovement();
            specialActive = true;
            uiManager.DeactivateButtons();
        }
    }
    public override void ResetOnEndCombat()
    {
        specialActive = false;
        base.ResetOnEndCombat();
    }
}
