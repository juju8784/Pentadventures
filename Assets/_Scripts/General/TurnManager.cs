using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public int WorldTurn;
    // 0 is player's turn
    // 1 is AI's turn
    public bool isPlayersTurn;
    public GameObject playerButton;
    public WorldAITurn aiTurns;

    void Start()
    {
        WorldTurn = 0;
        playerButton = GameObject.Find("End Turn Button");
        isPlayersTurn = true;
        StartPlayerTurn();
        if (!aiTurns)
        {
            aiTurns = GameManager.instance.aiTurns;
        }
    }

    public bool IsPlayerTurn()
    {
        return isPlayersTurn;
    }
    public void StartAITurn()
    {
        if (GameManager.instance.isCombat == false)
        {
            aiTurns.StartAITurn();
        }
    }
    public void EndAITurn()
    {
        if (GameManager.instance.isCombat == false)
        {
            WorldTurn++;
            //GameManager.instance.rogue.GetComponent<StateMachine>().isMyTurn = false;
            StartPlayerTurn();
        }
    }
    public void StartPlayerTurn()
    {
        if (GameManager.instance.isCombat == false)
        {
            isPlayersTurn = true;
            playerButton = GameObject.Find("End Turn Button");
            playerButton.GetComponent<Button>().onClick.RemoveAllListeners();
            playerButton.GetComponent<Button>().onClick.AddListener(EndPlayerTurn);
            playerButton.GetComponent<Button>().interactable = true;
            GameManager.instance.player.GetComponent<StatsHolder>().TurnReset();
        }
    }
    public void EndPlayerTurn()
    {
        if (GameManager.instance.isCombat == false)
        {
            isPlayersTurn = false;
            playerButton.GetComponent<Button>().interactable = false;
            StartAITurn();
        }
    }

}
