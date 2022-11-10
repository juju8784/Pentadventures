using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// This class is a work in progress for now
/// It's supposed to show the attack range of the player when hovering over the button
/// </summary>

public class ShowAttackRange : MonoBehaviour
{
    private TestCharacterController player;

    private BaseTile center;
    private List<BaseTile> tiles;

    public int range;

    private bool showRange;

    // Start is called before the first frame update
    void Start()
    {
        tiles = new List<BaseTile>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            player = GameManager.instance.player.GetComponent<TestCharacterController>();
        }


        if (showRange)
        {
            foreach (BaseTile tile in tiles)
            {
                tile.colorManager.SetTileState(BaseTile.ColorID.AttackRange, true);
            }
        }
        else
        {
            foreach (BaseTile tile in tiles)
            {
                tile.colorManager.SetTileState(BaseTile.ColorID.AttackRange, false);
            }
        }
    }

    //Highlights the attack range of the player. Only shows if the player is not moving
    public void ShowRange()
    {
        if (player.currentTile != center)
        {
            center = player.currentTile;
            tiles.Clear();
            for (int i = 0; i < 6; i++)
            {
                NextNeighbor(center, center.neighbors[i], range);
            }
        }

        //Check for enemies in circle
        GetComponent<Button>().interactable = false;
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].entities.Count >= 1)
            {
                GetComponent<Button>().interactable = true;
                break;
            }
        }

        showRange = true;

    }

    public void HideRange()
    {
        GetComponent<Button>().interactable = true;
        showRange = false;
    }

    private void NextNeighbor(BaseTile center, BaseTile current, int range)
    {
        //Checks duplicate
        if (tiles.Contains(current) || current == center)
        {
            return;
        }

        //Checks distance
        int distance = center.TileDistance(current);
        if (distance > range)
        {
            return;
        }
        else if (distance <= range)
        {
            tiles.Add(current);
            if (distance < range)
            {
                for (int i = 0; i < 6; i++)
                {
                    NextNeighbor(center, current.neighbors[i], range);
                }
            }
        }


        return;
    }
}
