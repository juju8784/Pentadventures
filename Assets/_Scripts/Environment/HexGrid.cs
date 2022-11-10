using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    [Header("Spawning Variables")]
    public Transform hexBasePrefab;
    public List<GameObject> chunks;

    [Header("Misc Variables")]
    [Tooltip("A percentage of the tile width and length")]
    public float gap;
    public int size;
    public bool CreateOnStart;
    public bool useChunks;

    [Tooltip("Center of the grid when it spawns")]
    public Vector3 startPos;

    //Dimensions of the base tile
    private float hexWidth;
    private float hexLength;

    private Hashtable hexes = new Hashtable();
    public int counter = 0;
    public bool debug;

    private void Start()
    {
        //Grabs the dimensions from the mesh
        hexLength = hexBasePrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.x;
        hexWidth = hexBasePrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.z;
        hexLength *= hexBasePrefab.localScale.x;
        hexWidth *= hexBasePrefab.localScale.z;

        //Adds the gap to the length and width
        hexLength += hexLength * gap;
        hexWidth += hexWidth * gap;

        if (CreateOnStart)
        {
            if (useChunks)
            {
                CreateChunkMap(size);
            }
            else
            {
                CreateMap(size);
            }
        }
    }

    //Returns the tile at the given position
    public BaseTile GetTile(int x, int y, int z)
    {
        Hex test = new Hex(x, y, z);
        if (hexes.ContainsKey(test.CreateHashCode()))
        {
            return (BaseTile)hexes[test.CreateHashCode()];
        }
        return null;
    }

    //Returns the tile at the given position
    public BaseTile GetTile(int x, int y)
    {
        Hex test = new Hex(x, y);
        if (hexes.ContainsKey(test.CreateHashCode()))
        {
            return (BaseTile)hexes[test.CreateHashCode()];
        }
        return null;
    }

    public BaseTile GetTile(Hex hex)
    {
        if (hexes.ContainsKey(hex.CreateHashCode()))
        {
            return (BaseTile)hexes[hex.CreateHashCode()];
        }
        return null;
    }

    public Hex GetHex(Hex hex)
    {
        if (hexes.ContainsKey(hex.CreateHashCode()))
        {
            return ((BaseTile)hexes[hex.CreateHashCode()]).hex;
        }
        return null;
    }

    public HexChunk GetChunk(Hex center)
    {
        Hex hex = GetHex(center);
        if (hex != null)
        {
            return hex.chunk;
        }
        return null;
    }

    public List<BaseTile> GetTiles(List<Hex> hexes)
    {
        List<BaseTile> tiles = new List<BaseTile>();
        foreach (Hex hex in hexes)
        {
            BaseTile tile = GetTile(hex);
            if (tile != null)
            {
                tiles.Add(tile);
            }
        }
        return tiles;
    }

    public List<Hex> GetHexes(List<Hex> hexList)
    {
        List<Hex> results = new List<Hex>();
        foreach (Hex hex in hexList)
        {
            Hex result = GetHex(hex);
            if (result != null)
            {
                results.Add(result);
            }
        }
        return results;
    }

    public bool StoreTile(BaseTile tile)
    {
        if (GetTile(tile.hex) == null)
        {
            tile.grid = this;
            hexes.Add(tile.hex.CreateHashCode(), tile);
            return true;
        }
        Debug.Log("Tile: " + tile.gameObject.name + " already exists in hashtable. This is the guy that's already there " + GetTile(tile.hex).gameObject.name);
        return false;
    }

    public bool StoreChunk(HexChunk chunkToStore, Hex centerPos)
    {
        chunkToStore.CoordAssign(centerPos);
        foreach (BaseTile tile in chunkToStore.tiles)
        {
            if (!StoreTile(tile))
            {
                return false;
            }
        }
        return true;
    }

    //Creates a basic map with radius
    public void CreateMap(int radius)
    {
        //https://www.redblobgames.com/grids/hexagons/implementation.html
        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Mathf.Max(-radius, -q - radius);
            int r2 = Mathf.Min(radius, -q + radius);
            for (int r = r1; r <= r2; r++)
            {
                Transform tile = Instantiate(hexBasePrefab);
                tile.parent = transform;
                Hex hex = new Hex(q, r);
                tile.GetComponent<BaseTile>().hex = hex;
                tile.name = "Tile " + hex.x + " | " + hex.y + " | " + hex.z;
                if (debug)
                {
                    if (q == 0 && r == 0)
                    {
                        tile.GetComponent<BaseTile>().colorManager.SetTileState(BaseTile.ColorID.Unwalkable, true);
                    }
                    else if (q == 0)
                    {
                        tile.GetComponent<BaseTile>().colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, true);
                    }
                    else if (r == 0)
                    {
                        tile.GetComponent<BaseTile>().colorManager.SetTileState(BaseTile.ColorID.AttackRange, true);
                    }
                    else if (-q - r == 0)
                    {
                        tile.GetComponent<BaseTile>().colorManager.SetTileState(BaseTile.ColorID.Entity, true);
                    }
                }
                tile.position = CalculateTilePosition(hex);
                StoreTile(tile.GetComponent<BaseTile>());
                counter++;
            }
        }
    }

    public void CreateChunkMap(int radius)
    {
        List<Hex> toSpawn = new List<Hex>();
        List<Hex> spawned = new List<Hex>();
        toSpawn.Add(new Hex(0, 0));
        while (toSpawn.Count > 0)
        {
            if (GetTile(toSpawn[0]) == null)
            {
                //Spawn the chunk
                GameObject chunkToSpawn = chunks[Random.Range(0, chunks.Count)];
                if (HexMathLib.CompareHex(toSpawn[0], HexMathLib.Zero))
                {
                    chunkToSpawn = chunks[0];
                }
                SpawnChunk(toSpawn[0], chunkToSpawn);
                spawned.Add(toSpawn[0]);

                //Check the neighboring chunk positions
                List<Hex> neighbors = HexMathLib.CalculateChunkNeighbors(toSpawn[0]);
                for (int i = 0; i < 6; i++)
                {
                    if (!spawned.Contains(neighbors[i]) && HexMathLib.Distance(neighbors[i], HexMathLib.Zero) <= radius * 7)
                    {
                        //Add them if they haven't been spawned and still in the radius
                        toSpawn.Add(neighbors[i]);
                    }
                }
            }
            toSpawn.RemoveAt(0);
        }
    }

    //Calculates the tile position based on the hex coords
    public Vector3 CalculateTilePosition(Hex hex)
    {
        Vector3 position = startPos;

        int zVector = hex.z + (hex.x) / 2;

        //a thicc boi
        float offset = (hex.x % 2 != 0) ? hexWidth / 2 * ((hex.x < 0) ? -1 : 1) : 0;

        position.x += hex.x * hexLength * 0.75f;
        position.z -= zVector * hexWidth + offset;

        return position;
    }

    public bool SpawnTile(Hex hex)
    {
        if (GetTile(hex) == null)
        {
            Transform tile = Instantiate(hexBasePrefab);
            tile.parent = transform;
            tile.GetComponent<BaseTile>().hex = hex;
            tile.name = "Tile " + hex.x + " | " + hex.y + " | " + hex.z;
            tile.position = CalculateTilePosition(hex);
            hexes.Add(hex.CreateHashCode(), tile.GetComponent<BaseTile>());
            counter++;
            return true;
        }
        return false;
    }

    public bool SpawnChunk(Hex center, GameObject chunkPrefab)
    {
        if (GetTile(center) == null)
        {
            GameObject spawn = Instantiate(chunkPrefab);
            spawn.transform.parent = transform;
            HexChunk spawnChunk = spawn.GetComponent<HexChunk>();
            if (spawnChunk == null)
            {
                spawnChunk = spawn.GetComponentInChildren<HexChunk>();
            }
            spawn.gameObject.name = "Chunk: " + center.x + " | " + center.y + " | " + center.z + " " + spawn.gameObject.name;
            spawnChunk.gameObject.name = "Chunk: " + center.x + " | " + center.y + " | " + center.z;
            //Store everything
            StoreChunk(spawnChunk, center);

            //move it to the right position
            spawn.transform.position = CalculateTilePosition(center);
            counter += spawnChunk.tiles.Count;

            //Debug
            if (debug)
            {
                
            }
            return true;
        }
        return false;
    }

    public void SpawnChunkNeighbors(HexChunk chunk)
    {
        List<Hex> neighbors = HexMathLib.CalculateChunkNeighbors(chunk.center.hex);
        foreach (Hex hex in neighbors)
        {
            GameObject chunkToSpawn = chunks[Random.Range(0, chunks.Count)];
            SpawnChunk(hex, chunkToSpawn);
        }
    }
}
