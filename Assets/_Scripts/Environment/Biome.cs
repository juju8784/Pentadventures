using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Biome : MonoBehaviour
{
    //contains the gameobjects and the weights
    public List<SpawnableChunk> chunks = new List<SpawnableChunk>();
    private int sum = 0;

    private void Awake()
    {
        //https://stackoverflow.com/questions/3163922/sort-a-custom-class-listt
        //for custom sort
        chunks.Sort(delegate (SpawnableChunk chunk1, SpawnableChunk chunk2) { return chunk1.weight.CompareTo(chunk2.weight); });
        chunks.Reverse();

        //Change the sorted list weights to account for the sum

        List<SpawnableChunk> temp = new List<SpawnableChunk>();
        foreach (var chunk in chunks)
        {
            SpawnableChunk copy = new SpawnableChunk(chunk);
            copy.weight += sum;
            temp.Add(copy);
            sum += chunk.weight;
        }
        chunks = temp;

    }

    public GameObject GetRandomChunk()
    {
        int r = Random.Range(0, sum);

        foreach (SpawnableChunk chunk in chunks)
        {
            if (chunk.weight >= r)
            {
                return chunk.chunk;
            }
        }
        return null;
    }
}

