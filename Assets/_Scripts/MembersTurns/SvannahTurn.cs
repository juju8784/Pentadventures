using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SvannahTurn : PlayerTurn
{
    bool dealPoison = false;
    public int poisonPercentage = 20;
    public int poisonTicks = 3;
    public int specialLast = 5;
    int specialCount = 0;

    public override void ResetOnEndCombat()
    {
        DeactivateSpecial();
        base.ResetOnEndCombat();
    }
    // Start is called before the first frame update
    public override void StartTurn()
    {
        if (dealPoison)
        {
            specialCount++;
            if(specialCount == specialLast)
            {
                DeactivateSpecial();
                specialCount = 0;
            }
        }
        base.StartTurn();
    }

    public override void Attack()
    {
        range = 1;
        base.Attack();
    }
    public override void Special()
    {
        if (!stats)
        {
            stats = GetComponent<StatsHolder>();
        }
        if (stats.CheckForPoints(stats.block.actionsCost[1]))
        {
            stats.SubtractActionPoints(stats.block.actionsCost[1]);
            dealPoison = true;
            Vector3 offset = new Vector3(0, 1.6f, 0.7f);
            GameObject dmgRedText = Instantiate(textSpawnPrefab, player.transform.position + offset, Quaternion.identity);
            dmgRedText.transform.SetParent(canvas.transform);
            dmgRedText.GetComponent<TextMeshProUGUI>().text = "Poison Activated";
            dmgRedText.GetComponent<TextMeshProUGUI>().color = Color.green;
            dmgRedText.transform.position = Camera.main.WorldToScreenPoint(dmgRedText.transform.position);
            Destroy(dmgRedText, 2.5f);
        }
        // activate poison damage on regular attacks for 5 turns
        // ticks on enemies for 3 turns
        // no stacking poison damage
    }

    public override IEnumerator HitRateDealDamage(Health target, int damage, bool crit)
    {
        if (!target)
        {
            StopCoroutine(HitRateDealDamage(target, damage, crit));
        }
        if (dealPoison)
        {
            target.GetComponent<StatsHolder>().AddPoisonEffect(poisonPercentage);
        }
        else
        {
            target.GetComponent<StatsHolder>().RemovePoisonEffect();
        }
        return base.HitRateDealDamage(target, damage, crit);
    }

    public void DeactivateSpecial()
    {
        dealPoison = false;
        Vector3 offset = new Vector3(0, 1.6f, 0.7f);
        GameObject dmgRedText = Instantiate(textSpawnPrefab, player.transform.position + offset, Quaternion.identity);
        dmgRedText.transform.SetParent(canvas.transform);
        dmgRedText.GetComponent<TextMeshProUGUI>().text = "Poison Deactivated";
        dmgRedText.GetComponent<TextMeshProUGUI>().color = Color.green;
        dmgRedText.transform.position = Camera.main.WorldToScreenPoint(dmgRedText.transform.position);
        Destroy(dmgRedText, 2.5f);
    }
}
