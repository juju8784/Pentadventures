using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class VivienneTurn : PlayerTurn
{
    public int specialRange = 3;
    //bool specialActive = false;
    List<BaseTile> outerRing = new List<BaseTile>();
    List<BaseTile> path = new List<BaseTile>();
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
            if (path.Count == 0)
            {
                path.Add(player.currentTile);
            }
            else if (path[0] != player.currentTile)
            {
                if (path.Count >= 2)
                {
                    if (path[1] != player.currentTile)
                    {
                        foreach (var item in path)
                        {
                            item.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, false);
                        }
                        path.Clear();
                        path.Add(player.currentTile);
                    }
                }
                else
                {
                    foreach (var item in path)
                    {
                        item.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, false);
                    }
                    path.Clear();
                    path.Add(player.currentTile);
                }
            }

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            BaseTile hitTile;

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

                    //If the tile is unwalkable
                    if (hitTile.weight == -1 || !outerRing.Contains(hitTile))
                    {
                        foreach (var item in path)
                        {
                            item.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, false);
                        }
                        path.Clear();
                    }
                    else if (path.Count == 1 || hitTile != path[path.Count - 1])
                    {
                        if (path.Count >= 1)
                        {
                            foreach (var item in path)
                            {
                                item.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, false);
                            }
                        }
                        path.Clear();
                        path.AddRange(player.currentTile.LinePath(hitTile));            //New line pathing
                        //path.AddRange(player.currentTile.GreedyPath(hitTile, specialRange, true));
                        foreach (var item in path)
                        {
                            item.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, true);
                        }
                    }
                }
                else
                {
                    //Removes highlighted colors if mouse isn't over a tile
                    foreach (var item in path)
                    {
                        item.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, false);
                    }
                    path.Clear();
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

                if (hit.transform.GetComponent<Entity>())
                {
                    hitTile = hit.transform.GetComponent<Entity>().currentTile;
                }
                else
                {
                    hitTile = hit.transform.GetComponent<BaseTile>();
                }

                if (rResults.Count == 0 && hitTile)
                {
                    if (outerRing.Contains(hitTile))
                    {
                        if (hitTile.entities.Count == 0)
                        {
                            foreach (var item in path)
                            {
                                item.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, false);
                            }
                            foreach (var item in outerRing)
                            {
                                item.colorManager.SetTileState(BaseTile.ColorID.AttackRange, false);
                            }
                            enemiesHit = false;
                            for (int i = 0; i < path.Count; i++)
                            {
                                if (path[i].entities.Count > 0)
                                {
                                    if (path[i].entities[0].GetComponent<AIEnemyController>())
                                    {
                                        enemiesHit = true;
                                        DoDamageForSpecial(path[i].entities[0].gameObject);
                                    }
                                }
                            }
                            stats.ReduceMovementLeft(3);
                            player.transform.position = hitTile.transform.position;
                            player.transform.position += new Vector3(0, 1, 0);
                            if (!enemiesHit)
                            {
                                player.EnableMovement();
                                specialActive = false;
                                uiManager.ActivateButtons();
                            }
                            //player.EnableMovement();
                            //specialActive = false;
                            //uiManager.ActivateButtons();
                        }
                        else
                        {
                            Vector3 offset = new Vector3(0, 1.6f, 0.7f);
                            GameObject dmgRedText = Instantiate(textSpawnPrefab, player.transform.position + offset, Quaternion.identity);
                            dmgRedText.transform.SetParent(canvas.transform);
                            dmgRedText.GetComponent<TextMeshProUGUI>().text = "Tile Occupied. Choose empty tile";
                            dmgRedText.GetComponent<TextMeshProUGUI>().color = Color.magenta;
                            dmgRedText.transform.position = Camera.main.WorldToScreenPoint(dmgRedText.transform.position);
                            Destroy(dmgRedText, 2.5f);
                        }
                    }
                }
            }
            // right click cancel
            if (Input.GetMouseButtonDown(1))
            {
                foreach (var item in path)
                {
                    item.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, false);
                }
                foreach (var item in outerRing)
                {
                    item.colorManager.SetTileState(BaseTile.ColorID.AttackRange, false);
                }
                Vector3 offset = new Vector3(0, 1.6f, 0.7f);
                GameObject dmgRedText = Instantiate(textSpawnPrefab, player.transform.position + offset, Quaternion.identity);
                dmgRedText.transform.SetParent(canvas.transform);
                dmgRedText.GetComponent<TextMeshProUGUI>().text = "Special Canceled";
                dmgRedText.GetComponent<TextMeshProUGUI>().color = Color.green;
                dmgRedText.transform.position = Camera.main.WorldToScreenPoint(dmgRedText.transform.position);
                Destroy(dmgRedText, 2.5f);

                player.EnableMovement();
                specialActive = false;
                GameManager.instance.combatUIManager.ActivateButtons();
            }
        }
    }
    public override IEnumerator HitRateDealDamage(Health target, int damage, bool crit)
    {
        for (int i = 0; i < stats.HitRate; i++)
        {
            if (Random.Range(0, 100) < (player.gameObject.GetComponent<StatsHolder>().CalculateCritChance()))
            {
                damage = player.gameObject.GetComponent<StatsHolder>().CalculatePhyCritDamage();
                stats.AddMovementLeft(3);
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
    void DoDamageForSpecial(GameObject enemy)
    {
        bool crit = false;
        StartCoroutine(HitRateDealDamageSpecial(enemy.GetComponent<Health>(), 5, crit));
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
            damage = (int)((float)damage * 1.5f);
            if (target)
            {
                target.TakeDamage(damage, hitRate, crit, false);
            }
            else
            {
                StopCoroutine(HitRateDealDamage(target, hitRate, crit));
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
        if (stats.GetMovementLeft() >= stats.block.actionsCost[1])
        {
            outerRing = player.currentTile.Ring(specialRange + 1);
            foreach (var item in outerRing)
            {
                item.colorManager.SetTileState(BaseTile.ColorID.AttackRange, true);
            }
            player.DisableMovement();
            specialActive = true;
            uiManager.DeactivateButtons();
        }
    }
}
