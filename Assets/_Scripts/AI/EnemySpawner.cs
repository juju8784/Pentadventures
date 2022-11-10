using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{
    public HexGrid grid;

    public Enemy baseEnemy;

    //This gets randomly chosen for now
    public List<BaseTile> spawnPositions;

    private BaseTile[] tiles;

    public List<GameObject> SpawnEnemies()
    {
        if (grid)
        {
            tiles = grid.GetComponentsInChildren<BaseTile>();
        }
        else
        {
            Debug.Log("The grid hasn't been set in the Enemy Spawner script of " + gameObject.name);
        }

        int mapSize = 6;
        int numEnemies = UnityEngine.Random.Range(0, 5);
        numEnemies += (int)(baseEnemy.stats.StarLevel / 2.0f + 0.5f);

        //BaseTile[] spawnArea = new BaseTile[tiles.Length / 2];
        //Array.Copy(tiles, spawnArea, tiles.Length / 2);

        List<GameObject> spawnedEnemies = new List<GameObject>();

        for (int i = 0; i < numEnemies; i++)
        {
            FindRandomTile(mapSize, new Vector3(0, 1, -1));
            GameObject enemy = Instantiate(baseEnemy.spawns[0]);
            Vector3 pos = spawnPositions[i].transform.position;
            pos += new Vector3(0, 1.04f, 0);
            enemy.transform.position = pos;
            spawnedEnemies.Add(enemy);
            enemy.GetComponent<EnemyStatsHolder>().StarLevel = baseEnemy.stats.StarLevel;
        }

        return spawnedEnemies;

    }

    //Make one of the signs 0 for this to work
    //Finds a random tile within the radius and has matching signs on each axis. 1 = positive, -1 = negative, 0 = doesn't matter
    private void FindRandomTile(int radius, Vector3 signs)
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
        
        //Grab the tile
        BaseTile tile = grid.GetTile(randomValues[0], randomValues[1], randomValues[2]);
        if (spawnPositions.Contains(tile) && tile != null)
        {
            FindRandomTile(radius, signs);
        }
        else
        {
            spawnPositions.Add(tile);
            //Debug.Log(randomValues[0] + " | " + randomValues[1] + " | " + randomValues[2]);
        }
    }

    private void FindRandomTile(BaseTile[] spawnArea)
    {
        BaseTile tile = spawnArea[UnityEngine.Random.Range(0, spawnArea.Length)];
        if (spawnPositions.Contains(tile))
        {
            FindRandomTile(spawnArea);
        }
        else
        {
            spawnPositions.Add(tile);
        }
    }
}
