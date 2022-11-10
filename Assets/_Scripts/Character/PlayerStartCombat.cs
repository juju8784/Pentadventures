using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStartCombat : MonoBehaviour
{
    private TestCharacterController controller;
    public GameObject textPrefab;
    GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<TestCharacterController>();

        text = Instantiate(textPrefab, textPrefab.transform.position, Quaternion.identity);
        Canvas canvas = FindObjectOfType<Canvas>();
        text.transform.SetParent(canvas.transform);
        text.GetComponent<TextMeshProUGUI>().text = "Press 'F' to start combat";
        text.GetComponent<TextMeshProUGUI>().color = Color.black;
        text.GetComponent<TextMeshProUGUI>().fontSize = 30;
        //text.transform.position = Camera.main.WorldToScreenPoint(text.transform.position);
        text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        text.SetActive(false);
        if (!GameManager.instance.isCombat && GameManager.instance.TurnManager.isPlayersTurn)
        {
            for (int i = 0; i < 6; i++)
            {
                if (controller.currentTile)
                {
                    if (controller.currentTile.neighbors[i])
                    {
                        if (controller.currentTile.neighbors[i].entities.Count == 1)
                        {
                            if (controller.currentTile.neighbors[i].entities[0].GetComponent<AIEnemyController>())
                                text.SetActive(true);
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.F) && controller.stopped)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (controller.currentTile.neighbors[i].entities.Count == 1)
                    {
                        EnemyManager enemyManager = GameManager.instance.enemyManager;
                        text.SetActive(false);
                        enemyManager.EnemyEnterCombat(controller.currentTile.neighbors[i].entities[0].gameObject);
                        return;
                    }
                }
            }
        }
    }
}
