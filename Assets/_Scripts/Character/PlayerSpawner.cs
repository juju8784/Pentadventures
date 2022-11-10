using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform spawnPosition;


    public HexGrid grid;
    public Vector3 hexoffset = new Vector3(0, 0, 0);

    private Vector3 FindRandomTile(int radius, Vector3 signs)
    {
        int[] randomValues = new int[3];
        int thirdValue = 0;
        //int randX = UnityEngine.Random.Range(0, radius) * ((xSign != 0) ? xSign : UnityEngine.Random.Range(0, 1) * 2 - 1);
        //int randY = UnityEngine.Random.Range(0, radius) * ((ySign != 0) ? ySign : UnityEngine.Random.Range(0, 1) * 2 - 1);
        //int randZ = -randX - randY;
        //quick maths
        for (int i = 0; i < 3; i++)
        {
            if (signs[i] != 0)
            {
                randomValues[i] = UnityEngine.Random.Range(0, radius) * (int)signs[i];
                thirdValue -= (int)randomValues[i];
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (randomValues[i] == 0)
            {
                randomValues[i] = thirdValue;
                break;
            }
        }
        int x = (int)hexoffset.x;
        int y = (int)hexoffset.y;
        int z = (int)hexoffset.z;

        //Grab the tile
        Vector3 newTilePos;
        BaseTile tile = grid.GetTile(randomValues[0] + x, randomValues[1] + y, randomValues[2] + z);
        if(tile.entities.Count > 0 || tile.weight < 0)
        {
            newTilePos = FindRandomTile(radius, signs);
        }
        else
        {
            newTilePos = tile.transform.position;
        }
        return newTilePos;
    }

    public Vector3 GetRandomPositionInCombat()
    {
        // bottom of the screen
        return FindRandomTile(5, new Vector3(0, -1, 1)) + new Vector3(0,1,0);
    }
}
