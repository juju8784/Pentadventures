using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToGuildHallTile : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Canvas canvas;
    GameObject prompt;

    public void ReturnToGuildHallPrompt(BaseTile tile)
    {
        if (tile.entities.Count > 0)
        {
            if (tile.entities[0].GetComponent<TestCharacterController>())
            {
                if (!canvas)
                {
                    canvas = GameObject.FindObjectOfType<Canvas>();
                }
                prompt = Instantiate(prefab);
                prompt.transform.SetParent(canvas.transform);
                prompt.transform.localPosition = Vector3.zero;
                prompt.GetComponent<ReturnToGHButtons>().Initiate(this.GetComponent<ReturnToGuildHallTile>());
            }
        }
    }

    public void Return()
    {
        SceneManager.LoadScene("GuildHall", LoadSceneMode.Single);
    }

    public void Dismiss()
    {
        Destroy(prompt);
    }
}
