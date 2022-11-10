using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//This will be a circle/hexagon of tiles to represent a chunk
public class HexChunk : MonoBehaviour
{
    public Hex chunkIndex;
    public int radius = 3;

    public List<BaseTile> tiles;
    public BaseTile center;

    [SerializeField] private bool active = true;

    //Gets all the tiles that are in the chunk
    public BaseTile[] CollectTiles()
    {
        BaseTile[] collected = GetComponentsInChildren<BaseTile>();
        tiles = collected.ToList();
        InitializeHexes();
        return collected;
    }

    private void InitializeHexes()
    {
        center.hex = new Hex(0, 0);

        List<BaseTile> neighborsToCheck = new List<BaseTile>();
        neighborsToCheck.Add(center);
        while (neighborsToCheck.Count > 0)
        {
            if (tiles.Contains(neighborsToCheck[0]))
            {
                neighborsToCheck[0].hex.chunk = this;
                for (int i = 0; i < 6; i++)
                {
                    if (neighborsToCheck[0].neighbors[i] && tiles.Contains(neighborsToCheck[0].neighbors[i]))
                    {
                        if (neighborsToCheck[0].neighbors[i].hex == null)
                        {
                            neighborsToCheck[0].neighbors[i].hex = HexMathLib.HexAdd(neighborsToCheck[0].hex, HexMathLib.HexDirections((HexMathLib.Directions)i));
                            neighborsToCheck.Add(neighborsToCheck[0].neighbors[i]);
                        }
                    }
                }
            }
            neighborsToCheck.RemoveAt(0);
        }
    }

    //Changes the cube coordinates to of the chunk to match it's actual position in the world
    public void CoordAssign(Hex hexcenter)
    {
        if (tiles.Count == 0)
        {
            CollectTiles();
        }
        if (HexMathLib.CompareHex(hexcenter, HexMathLib.Zero))
        {
            GameManager.instance.player.GetComponent<TestCharacterController>().currentTile = center;
        }
        //Assumes the center of the chunk is currently (0, 0, 0)
        foreach (BaseTile tile in tiles)
        {
            tile.hex = HexMathLib.HexAdd(tile.hex, hexcenter);
            tile.gameObject.name = "Tile " + tile.hex.x + " | " + tile.hex.y + " | " + tile.hex.z;
            tile.hex.chunk = this;
        }
    }

    public void DisableChunk()
    {
        if (active)
        {
            //Debug.Log("Disabling " + gameObject.name);
            active = false;
            foreach (BaseTile tile in tiles)
            {
                tile.Deactivate();
            }
            transform.parent.gameObject.SetActive(false);
        }
    }

    public void ActivateChunk()
    {
        if (!active)
        {
            active = true;
            //Debug.Log("Activating " + gameObject.name);
            transform.parent.gameObject.SetActive(true);
            foreach (BaseTile tile in tiles)
            {
                tile.Activate();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            center.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, !center.colorManager.GetTileState(BaseTile.ColorID.SpecialAttack));
        }
    }
}
