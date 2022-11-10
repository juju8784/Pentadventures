using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldSpawning : MonoBehaviour
{
    public HexGrid grid;

    public List<GameObject> enemies;
    public List<GameObject> lostCharas;
    public List<BaseTile> spawnPositions = new List<BaseTile>();

    //private BaseTile[] tiles;

    public void RepositionEntities()
    {
        int mapSize = 20;

        for (int i = 0; i < lostCharas.Count; i++)
        {
            if (lostCharas[i])
            {
                FindRandomTile(mapSize, new Vector3(0, 0, 0));
                Vector3 pos = spawnPositions[i].transform.position;
                pos += new Vector3(0, 0.7f, 0);
                lostCharas[i].transform.position = pos;
            }
        }

    }

    //Make one of the signs 0 for this to work
    //Finds a random tile within the radius and has matching signs on each axis. 1 = positive, -1 = negative, 0 = doesn't matter
    private void FindRandomTile(int radius, Vector3 signs)
    {
        int[] randomValues = new int[3];
        int thirdValue = 0;

        for (int i = 0; i < 2; i++)
        {
            randomValues[i] = UnityEngine.Random.Range(-radius / 2, radius / 2);
            thirdValue -= randomValues[i];
        }

        randomValues[2] = thirdValue;
        

        //Grab the tile
        BaseTile tile = grid.GetTile(randomValues[0], randomValues[1], randomValues[2]);
        if ((spawnPositions.Contains(tile) && tile != null) || tile.weight == -1 || tile.weight < 0 || HexMathLib.CompareHex(tile.hex, HexMathLib.Zero))
        {
            FindRandomTile(radius, signs);
        }
        else
        {
            spawnPositions.Add(tile);
            //Debug.Log(randomValues[0] + " | " + randomValues[1] + " | " + randomValues[2]);
        }
    }
}
