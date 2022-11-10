using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TempRogueHealth : Health
{
    [SerializeField] ThomasTurn tturn;
    EnemySpawner enemySpawner;
    CombatManagement cmanager;
    protected override void Start()
    {
        base.Start();
        if (!tturn)
        {
            tturn = FindObjectOfType<ThomasTurn>();
        }
        cmanager = GameManager.instance.combatManagement;
    }


    public override void TakeLeoExplosionDamage(int damage, int hitRate = 1, bool crit = false)
    {
        int final = 0;
        final = (damage - (stats.CalculateArmor() / hitRate));
        if (dmgReduction > 0)
        {
            final -= (int)(((float)damage / 100f) * dmgReduction);
        }
        if (final < 0)
        {
            final = 0;
        }
        else if (final > 0)
        {
            currentHealth -= final;
        }
        GameObject hurtEffect = Instantiate(effect, transform);
        hurtEffect.transform.Translate(0, 1.0f, 0);
        if (healthBar)
        {
            healthBar.BarFillAmmount();
        }
        if (sfx)
        {
            AudioSource.PlayClipAtPoint(sfx, cam.transform.position);
        }
        // for now the damage indicator is being hardcoded;

        Vector3 offset = new Vector3(0.7f, 1.6f, 0.7f);
        GameObject damageEffect = Instantiate(damagePrefab, transform.position + offset, Quaternion.identity);
        damageEffect.transform.SetParent(canvas.transform);

        Vector3 armoroffset = new Vector3(0.7f, 0.8f, 0.7f);
        GameObject armor = Instantiate(damagePrefab, transform.position + armoroffset, Quaternion.identity);
        armor.transform.SetParent(canvas.transform);

        // set text
        damageEffect.GetComponent<TextMeshProUGUI>().text = final.ToString();
        if (crit)
        {
            damageEffect.GetComponent<TextMeshProUGUI>().color = new Color(0.6f, 0.196f, 0.8f); // purple
        }
        else
        {
            damageEffect.GetComponent<TextMeshProUGUI>().color = Color.blue; // blue
        }
        armor.GetComponent<TextMeshProUGUI>().text = "-" + (stats.CalculateArmor() / hitRate).ToString();
        armor.GetComponent<TextMeshProUGUI>().color = Color.white;
        armor.GetComponent<TextMeshProUGUI>().fontSize -= 8;
        armor.transform.position = cam.WorldToScreenPoint(armor.transform.position);
        Destroy(armor, 2.5f);

        damageEffect.transform.position = cam.WorldToScreenPoint(damageEffect.transform.position);
        Destroy(damageEffect, 2.5f);


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public override void TakeDamage(int damage, int hitRate = 1, bool crit = false, bool magic = false)
    {
        if (currentHealth > 0)
        {
            int final = 0;
            bool evaded = (Random.Range(0, 100) < stats.CalculateEvasion());
            if (!evaded)
            {
                final = (damage - (stats.CalculateArmor() / hitRate));
                if (dmgReduction > 0)
                {
                    final -= (int)(((float)damage / 100f) * dmgReduction);
                }
                if (stats.poisoned)
                {
                    stats.poisonDamage = (int)(((float)damage / 100f) * stats.poisonEffect);
                    final += stats.poisonDamage;
                }
                if (final < 0)
                {
                    final = 0;
                }
                else if (final > 0)
                {
                    currentHealth -= final;
                }
                GameObject hurtEffect = Instantiate(effect, transform);
                hurtEffect.transform.Translate(0, 1.0f, 0);
                if (healthBar)
                {
                    healthBar.BarFillAmmount();
                }
                if (sfx)
                {
                    AudioSource.PlayClipAtPoint(sfx, cam.transform.position);
                }
            }
            // for now the damage indicator is being hardcoded;

            Vector3 offset = new Vector3(0, 1.6f, 0.7f);
            GameObject damageEffect = Instantiate(damagePrefab, transform.position + offset, Quaternion.identity);
            damageEffect.transform.SetParent(canvas.transform);

            if (evaded)
            {
                damageEffect.GetComponent<TextMeshProUGUI>().text = "Dodged";
                damageEffect.GetComponent<TextMeshProUGUI>().color = Color.white;
                damageEffect.GetComponent<TextMeshProUGUI>().fontSize -= 5;
                GetComponent<EnemyAnimationHandler>().SetAnimationTrigger("Dodge");
            }
            else
            {
                Vector3 armoroffset = new Vector3(0, 0.8f, 0.7f);
                GameObject armor = Instantiate(damagePrefab, transform.position + armoroffset, Quaternion.identity);
                armor.transform.SetParent(canvas.transform);

                // set text
                damageEffect.GetComponent<TextMeshProUGUI>().text = final.ToString();
                if (crit)
                {
                    if (magic)
                    {
                        damageEffect.GetComponent<TextMeshProUGUI>().color = new Color(0.6f, 0.196f, 0.8f); // purple
                    }
                    else
                        damageEffect.GetComponent<TextMeshProUGUI>().color = new Color(1, 0.647f, 0); // orange
                }
                else
                {
                    if (magic)
                    {
                        damageEffect.GetComponent<TextMeshProUGUI>().color = Color.blue; // blue
                    }
                    else
                        damageEffect.GetComponent<TextMeshProUGUI>().color = Color.red; // red
                }
                if (stats.poisoned)
                {
                    damageEffect.GetComponent<TextMeshProUGUI>().color = Color.green;
                }
                armor.GetComponent<TextMeshProUGUI>().text = "-" + (stats.CalculateArmor() / hitRate).ToString();
                armor.GetComponent<TextMeshProUGUI>().color = Color.white;
                armor.GetComponent<TextMeshProUGUI>().fontSize -= 8;
                armor.transform.position = cam.WorldToScreenPoint(armor.transform.position);
                Destroy(armor, 2.5f);
            }
            damageEffect.transform.position = cam.WorldToScreenPoint(damageEffect.transform.position);
            Destroy(damageEffect, 2.5f);


            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public override void Die()
    {
        GameManager.instance.questManager.RogueDie();
        switch(stats.StarLevel)
        {
            case 1:
                GameManager.instance.questManager.Rogue1StarDie();
                break;
            case 2:
                GameManager.instance.questManager.Rogue2StarDie();
                break;
            case 3:
                GameManager.instance.questManager.Rogue3StarDie();
                break;
            case 4:
                GameManager.instance.questManager.Rogue4StarDie();
                break;
            case 5:
                GameManager.instance.questManager.Rogue5StarDie();
                break;
        }
        //play die animation/sfx
        if (tturn)
        {
            tturn.RemoveEntityFromLists(this.gameObject);
        }
        GetComponent<Enemy>().Die();
    }
}
