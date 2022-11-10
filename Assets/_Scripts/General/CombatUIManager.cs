using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using UnityEngine.UIElements;

public class CombatUIManager : MonoBehaviour
{
    public Button endTurnButton;
    public List<Button> aButtons;
    //public Canvas canvas;
    public GameObject combatOptionsObject;
    //public List<GameObject> buttonPrefabs = new List<GameObject>();
    List<GameObject> bObjects = new List<GameObject>();

    private void Start()
    {
        //actionButtons = actionOptions.GetComponentsInChildren<Button>();
        //text = actionOptions.GetComponentsInChildren<TextMeshProUGUI>();
        if (!combatOptionsObject)
        {
            combatOptionsObject = GameObject.Find("Combat Options");
        }
        if (!endTurnButton)
        {
            endTurnButton = GameObject.Find("End Turn Button").GetComponent<Button>();
        }
    }

    public void OnPause(bool pause)
    {
        foreach (GameObject button in bObjects)
        {
            button.SetActive(!pause);
        }
        
    }


    public void ShowUIOptions()
    {
        if (!combatOptionsObject)
        {
            combatOptionsObject = GameObject.Find("Combat Options");
        }
        if (endTurnButton)
        {
            endTurnButton.interactable = true;
        }
        if (!combatOptionsObject.activeInHierarchy)
        {
            combatOptionsObject.SetActive(true);
        }
        //actionOptions.SetActive(true);
        // UI showing the attack buttons for chossing what to do 
    }

    public void UpdateButtonAPStats(int apLeft)
    {
        foreach (var item in aButtons)
        {
            item.gameObject.GetComponent<DeactivateNoMoreActionPoints>().APLeft = apLeft;
        }
    }

    public bool MakeButtonOutOfPrefab(GameObject buttonprefab)
    {
        if (buttonprefab)
        {
            GameObject newButton = Instantiate(buttonprefab);
            newButton.transform.SetParent(combatOptionsObject.transform, false);
            bObjects.Add(newButton);
            aButtons.Add(newButton.GetComponent<Button>());
            return true;
        }
        else
        {
            Debug.Log("Button prefab was null");
            return false;
        }
    }

    public void HideUIOptions()
    {
        endTurnButton.interactable = false;
        for (int i = 0; i < aButtons.Count; i++)
        {
            Destroy(aButtons[i]);
        }
        for (int i = 0; i < bObjects.Count; i++)
        {
            Destroy(bObjects[i]);
        }
        aButtons.Clear();
        bObjects.Clear();
        //actionOptions.SetActive(false);
        // turn off isActive for the combat UI
    }

    public void ChangeUIOptions(PlayerTurn playerTurn, List<GameObject> buttonPrefabs)
    {
        bool doit = true;
        doit = doit & MakeButtonOutOfPrefab(buttonPrefabs[0]);
        if (!aButtons[0])
        {
            Debug.Log("Button[0] null");
            doit = false;
        }
        doit = doit & MakeButtonOutOfPrefab(buttonPrefabs[1]);
        if (!aButtons[1])
        {
            Debug.Log("Button[1] null");
            doit = false;
        }
        doit = doit & MakeButtonOutOfPrefab(buttonPrefabs[2]);
        if (!aButtons[2])
        {
            Debug.Log("Button[2] null");
            doit = false;
        }
        if (doit)
        {
            ActivateButtons();

            endTurnButton.onClick.RemoveAllListeners();
            endTurnButton.onClick.AddListener(playerTurn.EndTurn);

            aButtons[0].onClick.RemoveAllListeners();
            aButtons[1].onClick.RemoveAllListeners();
            aButtons[2].onClick.RemoveAllListeners();
            aButtons[0].onClick.AddListener(playerTurn.Attack);
            aButtons[1].onClick.AddListener(playerTurn.Special);
            aButtons[2].onClick.AddListener(playerTurn.Heal);
            //aButtons[0].onClick.AddListener(DeactivateButtons);
            //aButtons[1].onClick.AddListener(DeactivateButtons);
            //aButtons[2].onClick.AddListener(DeactivateButtons);
        }
    }
    public void ActivateButtons()
    {
        foreach (Button button in aButtons)
        {
            button.interactable = true;
        }
    }
    public void DeactivateButtons()
    {
        foreach (Button button in aButtons)
        {
            button.interactable = false;
        }
    }

}
