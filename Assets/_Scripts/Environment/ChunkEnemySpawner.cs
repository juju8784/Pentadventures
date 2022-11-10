using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkEnemySpawner : MonoBehaviour
{
    public HexChunk chunk;
    public float spawnChance;
    public GameObject enemy;
    public int starLevelRange;
    public int breathingRoom = 10;

    // Start is called before the first frame update
    void Start()
    {
        foreach (BaseTile tile in chunk.tiles)
        {
            if (tile.weight >= 0 && tile.entities.Count == 0)
            {
                float chance = Random.Range(0.0f, 1.0f);
                if (chance <= spawnChance)
                {
                    int distance = HexMathLib.Distance(tile.hex, HexMathLib.Zero);
                    if (distance < breathingRoom)
                    {
                        break;
                    }

                    GameObject spawn = Instantiate(enemy);
                    spawn.transform.position = tile.transform.position;
                    
                    //Move it up so it's at the right height
                    spawn.transform.Translate(Vector3.up * 1.03f);

                    //star level stuff

                    int starLevel = Mathf.Min(5, (distance - breathingRoom) / starLevelRange + 1);

                    spawn.GetComponent<EnemyStatsHolder>().StarLevel = starLevel;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
