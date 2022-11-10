using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Deactivates and activates the tiles as the player moves in and out of range
/// </summary>
public class ActiveRange : MonoBehaviour
{
    //Make sure this is bigger than vision range
    public int range;
    //A buffer to check tiles outside of this ring as well.. for safety
    public int buffer;

    public HexGrid grid;
    private TestCharacterController player;
    private List<Hex> activeChunks;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player.GetComponent<TestCharacterController>();
        activeChunks = new List<Hex>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.stopped)
        {
            if (grid.useChunks)
            {
                //If the player isn't in the center chunk, recalculate the active chunks
                if (activeChunks.Count == 0)
                {
                    activeChunks = HexMathLib.GetChunkCircle(player.currentTile.hex.chunk, range);
                    ActivateChunks(activeChunks);
                }
                else if (player.currentTile.hex.chunk.center.hex != activeChunks[0])
                {
                    //New chunks
                    List<Hex> newTiles = HexMathLib.GetChunkCircle(player.currentTile.hex.chunk, range);
                    foreach (Hex hex in activeChunks)
                    {
                        //Deactivate the old chunks
                        if (!HexMathLib.HexInList(hex, newTiles))
                        {
                            HexChunk chunk = grid.GetChunk(hex);
                            if (chunk)
                            {
                                chunk.DisableChunk();
                            }
                        }
                    }
                    //Set the new chunks as the active ones
                    activeChunks = newTiles;
                    ActivateChunks(activeChunks);
                }
            }
            //For non-chunk generation
            else
            {
                List<Hex> hexes = HexMathLib.HexRing(player.currentTile.hex, range, buffer);
                foreach (Hex hex in hexes)
                {
                    BaseTile tile = grid.GetTile(hex);
                    if (tile)
                    {
                        if (tile.gameObject.activeSelf)
                        {
                            tile.Deactivate();
                            tile.gameObject.SetActive(false);
                        }
                    }
                }
                hexes = HexMathLib.HexRing(player.currentTile.hex, range - buffer, buffer - 1);
                foreach (Hex hex in hexes)
                {
                    BaseTile tile = grid.GetTile(hex);
                    if (tile)
                    {
                        if (!tile.gameObject.activeSelf)
                        {
                            tile.gameObject.SetActive(true);
                            tile.Activate();
                        }
                    }
                }
            }
        }
        else if (activeChunks.Count == 0)
        {
            activeChunks = HexMathLib.GetChunkCircle(player.currentTile.hex.chunk, range);
            ActivateChunks(activeChunks);
        }
        else
        {
            ActivateChunks(activeChunks);
        }
    }

    

    private void ActivateChunks(List<Hex> chunks, bool activate = true)
    {
        //Activate the new chunks
        foreach (Hex chunk in chunks)
        {
            HexChunk activeChunk = grid.GetChunk(chunk);
            if (activeChunk)
            {
                if (activate)
                {
                    activeChunk.ActivateChunk();
                }
                else
                {
                    activeChunk.DisableChunk();
                }
            }
        }
    }
}
