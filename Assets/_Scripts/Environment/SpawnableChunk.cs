using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpawnableChunk
{
    public GameObject chunk;

    [Tooltip("The weight the object has. Higher weight means more likely to be chosen")]
    public int weight;

    public SpawnableChunk(SpawnableChunk copy)
    {
        chunk = copy.chunk;
        weight = copy.weight;
    }

    public SpawnableChunk()
    {
        chunk = null;
        weight = 0;
    }
}
