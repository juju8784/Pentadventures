using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    public int maxHealth;
    [SerializeField]protected int currentHealth;
    public GameObject effect;
    public GameObject damagePrefab;
    public HealthBar healthBar;
    [SerializeField] private GameObject barPrefab;
    [SerializeField] protected Camera cam;
    protected Canvas canvas;
    public StatsHolder stats;
    public AudioClip sfx;
    public AudioClip healsfx;
    public int dmgReduction = 0;
    int poisonDamage = 0;

    //Leaving all of these virtual so they may be overriden later for specific creatures and abilities
    protected virtual void Start()
    {
        if (!stats)
        {
            this.gameObject.GetComponent<StatsHolder>();
        }
        if (barPrefab)
        {
            GameObject temp = GameObject.Instantiate(barPrefab);
            canvas = GameObject.FindObjectOfType<Canvas>();
            temp.transform.SetParent(canvas.transform);
            temp.transform.SetAsFirstSibling();
            healthBar = temp.GetComponent<HealthBar>();
            healthBar.HealthBarPositioning(this.gameObject);
            healthBar.BarFillAmmount();
        }
        if (!cam)
        {
            cam = GameManager.instance.cameraParent.GetComponentInChildren<Camera>();
        }

        stats.SetStats();
        maxHealth = this.gameObject.GetComponent<StatsHolder>().CalculateHealth();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (healthBar)
        {
            if (GameManager.instance.isCombat && currentHealth > 0)
            {
                healthBar.HealthBarPositioning(this.gameObject);
                healthBar.ShowBar();
                healthBar.UpdateActionsLeft(stats.APointsLeft);
            }
            else
            {
                healthBar.HideBar();
            }
        }
    }
    public virtual void TakePoisonDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
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
            Vector3 offset = new Vector3(0, 1.6f, 0.7f);
            GameObject damageEffect = Instantiate(damagePrefab, transform.position + offset, Quaternion.identity);
            damageEffect.transform.SetParent(canvas.transform);
            damageEffect.GetComponent<TextMeshProUGUI>().text = damage.ToString();
            damageEffect.GetComponent<TextMeshProUGUI>().color = Color.green;
            damageEffect.transform.position = cam.WorldToScreenPoint(damageEffect.transform.position);
            Destroy(damageEffect, 2.5f);
        }
    }
    public virtual void TakeLeoExplosionDamage(int damage, int hitRate = 1, bool crit = false)
    {
        // placeholder
    }
    //Use this for taking any and all damage
    public virtual void TakeDamage(int damage, int hitRate = 1, bool crit = false, bool magic = false)
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
                damageEffect.GetComponent<TextMeshProUGUI>().color = new Color(1,0.647f,0); // orange
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

    public virtual void Heal(int healAmount)
    {
        if (healsfx)
        {
            AudioSource.PlayClipAtPoint(healsfx, cam.transform.position);
        }
        currentHealth += healAmount;
        if (healthBar)
        {
            healthBar.BarFillAmmount();
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public virtual void Die()
    {
        if (healthBar)
        {
            healthBar.HideBar();
        }
        //Send message to everything that this monster/player is dead
        //Handle death case
    }

    public virtual int GetHealth()
    {
        return currentHealth;
    }
}
