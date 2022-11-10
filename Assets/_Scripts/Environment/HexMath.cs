using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A lot of the math and some of the functions are inspired by this guy
/// https://www.redblobgames.com/grids/hexagons/
/// I've pasted this link in other parts of code, but I thought I'd post it here as well
/// Really helped me learn a much more efficient way of dealing with hex grids
/// </summary>
public static class HexMath
{
    //The different directions a tile can go
    //{ top-right, bottom-right, bottom, bottom-left, top-left, top }
    private static Hex[] hex_directions =
    {
        new Hex(+1, -1, 0), new Hex(+1, 0, -1), new Hex(0, +1, -1),
        new Hex(-1, +1, 0), new Hex(-1, 0, +1), new Hex(0, -1, +1)
    };

    //Use these to help navigate hex_directions
    public enum Directions
    {
        TopRight,
        BotRight,
        Bottom,
        BotLeft,
        TopLeft,
        Top
    }

    //Returns a Hex with the specified direction
    public static Hex HexDirections(Directions direction)
    {
        return hex_directions[(int)direction];
    }

    //Returns the potential neighbor in the specified direction
    public static Hex GetHexNeighbor(Hex current, Directions direction)
    {
        return HexAdd(current, HexDirections(direction));
    }

    //Adds the 2 hexes together
    public static Hex HexAdd(Hex a, Hex b)
    {
        return new Hex(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static Hex HexMultiply(Hex a, int multiply)
    {
        return new Hex(a.x * multiply, a.y * multiply, a.z * multiply);
    }

    //Returns the distance between the 2 hexes
    public static int Distance(Hex a, Hex b)
    {
        //The max of the difference between the 2 hexes
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y), Mathf.Abs(a.z - b.z));
    }

    //Draw a line from start length amount of tiles in specified direction. Returns the list of tiles hit. Gives null if the tile doesn't exist
    public static List<Hex> DrawLine(Hex start, Directions direction, int length)
    {
        List<Hex> tilesHit = new List<Hex>();

        //TODO

        return tilesHit;
    }


    public static List<Hex> HexRing(Hex center, int radius)
    {
        List<Hex> results = new List<Hex>();

        //Gets a hex at the edge of the ring
        Hex hex = HexAdd(center, HexMultiply(HexDirections(Directions.TopLeft), radius));

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

        return results;
    }

}
