using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ErinTurn : PlayerTurn
{
    List<BaseTile> outerRing = new List<BaseTile>();
    List<BaseTile> path = new List<BaseTile>();
    public int specialRange = 5;
    //bool specialActive = false;

    public override void StartTurn()
    {
        outerRing.Clear();
        base.StartTurn();
    }

    public override void Attack()
    {
        range = 4;
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

            //Only does these calculations if we can move
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.GetComponent<BaseTile>())
                {
                    BaseTile hitTile = hit.transform.GetComponent<BaseTile>();

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
                        path.AddRange(player.currentTile.LinePath(hitTile));        //new line pathing
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

                if (rResults.Count == 0 && hit.transform.GetComponent<BaseTile>())
                {
                    if (hit.transform.GetComponent<BaseTile>() == path[path.Count - 1])
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
                        AudioSource.PlayClipAtPoint(specialSFX, player.transform.position);
                        stats.SubtractActionPoints(stats.block.actionsCost[1]);
                        outerRing.Clear();
                        if (!enemiesHit)
                        {
                            player.EnableMovement();
                            uiManager.ActivateButtons();
                            specialActive = false;
                        }
                        //specialActive = false;
                        //player.EnableMovement();
                        //uiManager.ActivateButtons();
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

                outerRing.Clear();
                specialActive = false;
                player.EnableMovement();
                uiManager.ActivateButtons();
            }           
        }
    }
    
    void DoDamageForSpecial(GameObject enemy)
    {
        bool crit = false;
        StartCoroutine(HitRateDealDamageSpecial(enemy.GetComponent<Health>(), 10, crit));
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
        specialActive = false;
        player.EnableMovement();
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
            outerRing = player.currentTile.Ring(specialRange);
            foreach (var item in outerRing)
            {
                item.colorManager.SetTileState(BaseTile.ColorID.AttackRange, true);
            }
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
