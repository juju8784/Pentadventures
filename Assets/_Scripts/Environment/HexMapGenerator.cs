using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMapGenerator : MonoBehaviour
{
    public HexGrid grid;

    //Essentially the player or whatever
    public TestCharacterController focus;

    //Change this later to be an array of all the tiles to spawn
    public GameObject tile;

    [Tooltip("This script will start generating more tiles if the player is this distance or less from the edge")]
    public int generationDistance;
    private List<Hex> previousChunks;
    private Biome biome;

    public List<BaseTile> testLine = new List<BaseTile>();

    private void Start()
    {
        if (!grid)
        {
            Debug.Log("Please set the grid variable in the HexMapGenerator component on " + gameObject.name);
        }
        previousChunks = new List<Hex>();
        biome = GetComponent<Biome>();
    }

    private void Update()
    {
        if (!GameManager.instance.isCombat)
        {
            if (!focus.stopped)
            {
                if (grid.useChunks)
                {
                    if (previousChunks.Count == 0)
                    {
                        previousChunks = HexMathLib.GetChunkCircle(focus.currentTile.hex.chunk, generationDistance);
                    }
                    if (focus.currentTile.hex.chunk.center.hex != previousChunks[0])
                    {
                        List<Hex> ring = HexMathLib.GetChunkCircle(focus.currentTile.hex.chunk, generationDistance);
                        foreach (Hex hex in ring)
                        {
                            //GameObject chunkToSpawn = grid.chunks[Random.Range(0, grid.chunks.Count)];
                            grid.SpawnChunk(hex, biome.GetRandomChunk());
                        }
                        previousChunks = ring;
                    }

                }
                else
                {
                    //Check all 6 directions for tiles that don't exist
                    for (int i = 0; i < 6; i++)
                    {
                        if (!CheckDirection(i))
                        {
                            if (grid.useChunks)
                            {
                                //Create a line to check for empty spaces
                                List<Hex> line = new List<Hex>();
                                line.Add(focus.currentTile.hex);
                                for (int j = 0; j < generationDistance; j++)
                                {
                                    Hex currentHex = HexMathLib.GetHexNeighbor(line[line.Count - 1], (HexMathLib.Directions)i);
                                    if (!grid.GetTile(currentHex))
                                    {
                                        //Spawn new chunks if there isn't a path
                                        List<Hex> newChunks = HexMathLib.CalculateChunkNeighbors(line[line.Count - 1].chunk.center.hex);
                                        for (int k = 0; k < 6; k++)
                                        {
                                            //Change this later for the patterns 
                                            //Debug.Log("Spawn chunk at center: " + newChunks[k].x + " | " + newChunks[k].y + " | " + newChunks[k].z);
                                            GameObject chunkToSpawn = grid.chunks[Random.Range(0, grid.chunks.Count)];
                                            grid.SpawnChunk(newChunks[k], chunkToSpawn);
                                        }
                                    }
                                    line.Add(grid.GetTile(currentHex).hex);
                                }
                            }
                            else
                            {
                                List<Hex> toSpawn = HexMathLib.HexRing(focus.currentTile.hex, generationDistance);
                                foreach (Hex hex in toSpawn)
                                {
                                    grid.SpawnTile(hex);
                                }
                            }
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (testLine.Count > 0)
            {
                foreach (var tile in testLine)
                {
                    tile.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, false);
                }
                testLine.Clear();
            }
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.GetComponent<BaseTile>())
                {
                    BaseTile hitTile = hit.transform.GetComponent<BaseTile>();
                    List<Hex> line = HexMathLib.DrawLine(GameManager.instance.player.GetComponent<TestCharacterController>().currentTile.hex, hitTile.hex);
                    testLine = grid.GetTiles(line);
                    foreach (var tile in testLine)
                    {
                        tile.colorManager.SetTileState(BaseTile.ColorID.SpecialAttack, true);
                    }
                }
            }
        }
    }


    private bool CheckDirection(int direction)
    {
        Hex currentHex = focus.currentTile.hex;
        if (currentHex != null)
        {
            for (int i = 0; i < generationDistance; i++)
            {
                currentHex = HexMathLib.GetHexNeighbor(currentHex, (HexMathLib.Directions)direction);
                if (grid.GetTile(currentHex.x, currentHex.y, currentHex.z) == null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    
}

public static class HexMathLib
{
    public static Hex Zero = new Hex(0, 0, 0);
    //The different directions a tile can go
    //{ top-right, bottom-right, bottom, bottom-left, top-left, top }
    private static Hex[] hex_directions =
    {
        new Hex(-1, +1, 0), new Hex(0, +1, -1), new Hex(+1, 0, -1),
        new Hex(+1, -1, 0), new Hex(0, -1, +1), new Hex(-1, 0, +1), 
    };

    //Use these to help navigate hex_directions
    public enum Directions
    {
        TopLeft,
        Top,
        TopRight,
        BotRight,
        Bot,
        BotLeft,
    }

    

    //Returns a Hex with the specified direction
    public static Hex HexDirections(Directions direction)
    {
        return hex_directions[(int)direction];
    }

    public static Hex HexDirections(int direction)
    {
        return hex_directions[direction];
    }

    public static Hex ClockwiseHexDirections(int direction)
    {
        int clockwise = direction - 4;
        if (clockwise < 0)
        {
            clockwise += 6;
        }
        return hex_directions[clockwise];
    }

    //Returns the potential neighbor in the specified direction
    public static Hex GetHexNeighbor(Hex current, Directions direction)
    {
        return HexAdd(current, HexDirections(direction));
    }

    public static Hex GetHexNeighbor(Hex current, int direction)
    {
        return HexAdd(current, HexDirections(direction));
    }

    //Calculates which of the 6 sections the vector falls in. Returns a list of 2 directions it is in between
    public static List<int> CalculateSection(Hex vector)
    {
        int max = 0;
        int second = 0;
        int maxDirection = 0;
        int secondDirection = 0;

        for (int i = 0; i < 6; i++)
        {
            int dot = HexDot(vector, HexDirections(i));
            if (dot > max)
            {
                second = max;
                max = dot;
                secondDirection = maxDirection;
                maxDirection = i;
            }
            else if (dot > second)
            {
                second = dot;
                secondDirection = i;
            }
        }
        List<int> directions = new List<int>();
        //Order them
        //0 5 is a special case
        if ((maxDirection == 0 || secondDirection == 0) && (maxDirection == 5 || secondDirection == 5))
        {
            directions.Add(5);
            directions.Add(0);
        }
        else
        {
            //Adds the smaller direction first and then the bigger one
            directions.Add((maxDirection < secondDirection) ? maxDirection : secondDirection);
            directions.Add((maxDirection > secondDirection) ? maxDirection : secondDirection);
        }
        

        return directions;
    }

    //Adds the 2 hexes together
    public static Hex HexAdd(Hex a, Hex b)
    {
        return new Hex(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    //Subtracts the 2 hexes
    public static Hex HexSubtract(Hex a, Hex b)
    {
        return new Hex(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static Hex HexMultiply(Hex a, int multiply)
    {
        return new Hex(a.x * multiply, a.y * multiply, a.z * multiply);
    }

    public static int HexDot(Hex a, Hex b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    #region Cube Math
    public static Cube CubeAdd(Cube a, Cube b)
    {
        return new Cube(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static Cube CubeSubtract(Cube a, Cube b)
    {
        return new Cube(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static Cube CubeMultiply(Cube a, float multiply)
    {
        return new Cube(a.x * multiply, a.y * multiply, a.z * multiply);
    }

    public static Cube CubeDivide(Cube a, float divide)
    {
        return new Cube(a.x / divide, a.y / divide, a.z / divide);
    }

    public static Cube CubeRound(Cube cube)
    {
        int x = Mathf.RoundToInt(cube.x);
        int y = Mathf.RoundToInt(cube.y);
        int z = Mathf.RoundToInt(cube.z);

        float deltaX = Mathf.Abs(x - cube.x);
        float deltaY = Mathf.Abs(y - cube.y);
        float deltaZ = Mathf.Abs(z - cube.z);

        if (deltaX > deltaY && deltaX > deltaZ)
        {
            x = -y - z;
        }
        else if (deltaY > deltaZ)
        {
            y = -x - z;
        }
        else
        {
            z = -x - y;
        }

        return new Cube(x, y, z);
    }

    public static Cube CubeNormalize(Cube cube)
    {
        float max = Mathf.Max(Mathf.Abs(cube.x), Mathf.Abs(cube.y), Mathf.Abs(cube.z));
        return CubeDivide(cube, max);
    }

    //A classic lerp function. Made some custom operators for this so it can look clean
    public static Cube CubeLerp(Cube a, Cube b, float ratio)
    {
        return a + (b - a) * ratio;
    }

    #endregion

    //Returns true if the hexes are the same
    public static bool CompareHex(Hex a, Hex b)
    {
        return ((a.x == b.x) && (a.y == b.y) && (a.z == b.z));
    }

    //INCOMPLETE: For now this will rotate the hex 60 degrees to the left around a center
    public static Hex RotateLeft(Hex point, Hex center)
    {
        Hex direction = HexSubtract(point, center);
        //TODO
        return point;
    }

    //Returns the distance between the 2 hexes in # of hexes
    public static int Distance(Hex a, Hex b)
    {
        //The max of the difference between the 2 hexes
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y), Mathf.Abs(a.z - b.z));
    }

    //Draw a line from start length amount of tiles in specified direction. Returns the list of tiles hit. Gives null if the tile doesn't exist
    public static List<Hex> DrawLine(Hex start, Directions direction, int length)
    {
        List<Hex> tilesHit = new List<Hex>();
        tilesHit.Add(start);
        for (int i = 0; i < length; i++)
        {
            tilesHit.Add(HexAdd(tilesHit[tilesHit.Count - 1], HexDirections(direction)));
        }

        return tilesHit;
    }

    public static List<Hex> DrawLine(Hex start, Hex end)
    {
        List<Hex> tilesHit = new List<Hex>();

        //how long the line should be 
        float distance = Distance(start, end);

        //Cube versions for floats
        Cube a = new Cube(start);
        Cube b = new Cube(end);

        //To make sure it always pushes the line in the same direction
        a = a + new Cube(Mathf.Exp(-6), 2 * Mathf.Exp(-6), -3 * Mathf.Exp(-6));
        //b = b + new Cube(Mathf.Exp(-6), 2 * Mathf.Exp(-6), -3 * Mathf.Exp(-6));


        for (int i = 0; i <= distance; i++)
        {
            tilesHit.Add(new Hex(CubeRound(CubeLerp(a, b, 1.0f / distance * i))));
        }

        return tilesHit;
    }

    public static List<Hex> HexCone(Hex center, Hex direction, int distance)
    {
        List<Hex> cone = new List<Hex>();

        //Calculate direction
        Hex vector = HexSubtract(direction, center);
        List<int> section = CalculateSection(vector);

        Hex currentTile = null;
        //Get the hexes
        for (int i = 1; i <= distance; i++)
        {
            //Move along the edge
            currentTile = HexAdd(center, HexMultiply(HexDirections(section[0]), i));
            cone.Add(currentTile);
            for (int j = 0; j < i; j++)
            {
                //moving from edge to edge
                currentTile = HexAdd(currentTile, ClockwiseHexDirections(section[0]));
                cone.Add(currentTile);
            }
        }

        return cone;
    }

    public static List<Hex> HexRing(Hex center, int radius, int numRings = 1)
    {
        List<Hex> results = new List<Hex>();

        for (int k = 0; k < numRings; k++)
        {
            //Gets a hex at the edge of the ring
            Hex hex = HexAdd(center, HexMultiply(HexDirections((Directions)4), radius));

            //omg this is so smart
            //Ok so this loops 6 times for each side of the hexring
            for (int i = 0; i < 6; i++)
            {
                //loops radius amount of times because that is the length of a side
                for (int j = 0; j < radius; j++)
                {
                    results.Add(hex);
                    //goes to the neighbor in i direction because that will move along the side
                    hex = GetHexNeighbor(hex, (Directions)i);
                }
            }
            radius++;
        }
        return results;
    }

    public static List<Hex> PartialHexRing(Hex center, Directions direction, int radius)
    {
        List<Hex> results = new List<Hex>();

        Hex hex = HexAdd(center, HexMultiply(HexDirections(direction), radius));
        for (int i = 0; i < radius; i++)
        {
            results.Add(hex);
            int sideDirection = (int)direction - 4;
            if (sideDirection < 0)
            {
                sideDirection += 6;
            }
            hex = GetHexNeighbor(hex, sideDirection);
        }

        return results;
    }

    public static List<Hex> HexSpiral(Hex center, int radius)
    {
        List<Hex> results = new List<Hex>();
        results.Add(center);
        for (int i = 1; i <= radius; i++)
        {
            results.AddRange(HexRing(center, i));
        }

        return results;
    }

    public static List<Hex> CalculateChunkNeighbors(Hex center)
    {
        List<Hex> chunkCenters = new List<Hex>();
        List<Hex> edge = HexRing(center, 7);

        int index = 3;

        for (int i = 0; i < 6; i++)
        {
            chunkCenters.Add(edge[index]);
            index += 7;
        }

        return chunkCenters;
    }

    public static List<Hex> GetChunkCircle(HexChunk chunk, int radius)
    {
        List<Hex> centers = new List<Hex>();
        centers.Add(chunk.center.hex);
        int index = 0;

        //formula for hex circle area
        int circleArea = 1 + 3 * radius * (radius + 1);

        //breath first search
        while (centers.Count < circleArea)
        {
            //Caculate chunk neighbors
            List<Hex> neighbors = HexMathLib.CalculateChunkNeighbors(centers[index]);
            foreach (Hex hex in neighbors)
            {
                if (!HexMathLib.HexInList(hex, centers) && hex != null)
                {
                    centers.Add(hex);
                }
            }
            index++;
        }
        return centers;
    }

    public static List<Hex> MovementRange(Hex center, int range)
    {
        List<Hex> results = new List<Hex>();

        for (int x = -range; x < range; x++)
        {
            for (int z = 0; z < Mathf.Min(range, -x + range); z++)
            {
                int y = -x - z;
                results.Add(HexAdd(center, new Hex(x, y, z)));
            }
        }

        return results;
    }

    public static bool HexInList(Hex toCheck, List<Hex> hexes)
    {
        foreach (Hex hex in hexes)
        {
            if (CompareHex(toCheck, hex))
            {
                return true;
            }
        }
        return false;
    }
}
