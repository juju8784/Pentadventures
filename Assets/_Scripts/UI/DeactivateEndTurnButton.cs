using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeactivateEndTurnButton : MonoBehaviour
{
    public Button button;
    public TestCharacterController player;
    bool PlayerAttacking = false;
    bool playerUsingSpecial = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!button)
        {
            button = GetComponent<Button>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isCombat)
        {
            Turn te = GameManager.instance.combatManagement.FindTurn(GameManager.instance.combatManagement.currentTurnID);
            if (te)
            {
                player = te.GetComponent<TestCharacterController>();
            }
            if (player)
            {
                PlayerAttacking = player.GetComponent<PlayerTurn>().attacking;
                playerUsingSpecial = player.GetComponent<PlayerTurn>().specialActive;
            }
        }
        else
        {
            player = GameManager.instance.player.GetComponent<TestCharacterController>();
            //PlayerAttacking = true;
        }

        if (player)
        {
            if (GameManager.instance.isCombat)
            {
                if (playerUsingSpecial || PlayerAttacking)
                {
                    button.interactable = false;
                }
                else if (player.stopped)
                {
                    button.interactable = true;
                }
                else
                {
                    button.interactable = false;
                }
            }
            else
            {
                if (player.stopped && GameManager.instance.TurnManager.isPlayersTurn)
                {
                    button.interactable = true;
                }
                else
                {
                    button.interactable = false;
                }
            }
        }
    }
}
