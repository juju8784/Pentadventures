using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    public override void TakeDamage(int damage, int hitRate = 1, bool crit = false, bool magic = false)
    {
        base.TakeDamage(damage, hitRate, crit, magic);
        //this.gameObject.GetComponent<StatBlock>().UIUpdateVitality(GetHealth());
    }
    public override void Heal(int healAmount)
    {
        base.Heal(healAmount);
        GameManager.instance.questManager.PlayerHealed(healAmount);
        //this.gameObject.GetComponent<StatBlock>().UIUpdateVitality(GetHealth());
    }

    public override void Die()
    {
        if (GetComponent<CombatCharacterAnimManager>())
        {
            GetComponent<CombatCharacterAnimManager>().SetDeathTrigger();
        }
        GameManager.instance.partyManager.CharacterDiedDuringCombat(this.gameObject);
    }
}
