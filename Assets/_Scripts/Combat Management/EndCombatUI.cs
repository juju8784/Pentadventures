using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndCombatUI : MonoBehaviour
{
    public TextMeshProUGUI combatOutcome;
    public TextMeshProUGUI results;
    public TextMeshProUGUI lootGained;
    public TextMeshProUGUI lostCharacters;
    public GameObject continueButton;
    public GameObject guildHallButton;

    public void CombatEnded(bool playerWon, int enemiesDefeated, List<string> charactersLost, List<int> loot)
    {
        gameObject.SetActive(true);
        combatOutcome.text = playerWon ? "You Won!" : "You Lost :(";
        results.text = "You have defeated " + enemiesDefeated + " Rogue" + ((enemiesDefeated > 1) ? "s" : "");

        if (playerWon)
        {
            for (int i = 0; i < loot.Count; i++)
            {
                if (loot[i] == 0)
                {
                    break;
                }
                lootGained.text += loot[i];

                if (i == 0)
                {
                    lootGained.text += " copper";
                }
                else if (i == 1)
                {
                    lootGained.text += " silver";
                }
                else if (i == 2)
                {
                    lootGained.text += " electrum";
                }
                else if (i == 3)
                {
                    lootGained.text += " gold";
                }
                else
                {
                    lootGained.text += " platinum";
                }

                lootGained.text += "\n";
            }
            guildHallButton.SetActive(false);
        }
        else
        {
            continueButton.SetActive(false);
        }

        //Lost characters text
        if (charactersLost.Count > 0)
        {
            lostCharacters.text = "You have lost: ";
            for (int i = 0; i < charactersLost.Count; i++)
            {
                lostCharacters.text += charactersLost[i];
                if (i + 1 < charactersLost.Count)
                {
                    lostCharacters.text += ", ";
                }
            }
        }
    }

    public void ResetText()
    {
        combatOutcome.text = "";
        results.text = "";
        lootGained.text = "";
        lostCharacters.text = "";
        gameObject.SetActive(false);
    }
}
