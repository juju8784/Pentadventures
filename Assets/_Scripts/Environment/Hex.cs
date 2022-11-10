using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the positional data for the hex in cube coordinates
/// https://www.redblobgames.com/grids/hexagons/
/// x + y + z = 0
/// </summary>

public class Hex
{
    //Cube Coordinates
    public int x, y, z;
    public HexChunk chunk;

    #region Contructors
    public Hex(int _x, int _y, int _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }

    public Hex(int _x, int _z)
    {
        x = _x;
        z = _z;
        y = -x - z;
    }

    public Hex(Cube cube)
    {
        x = (int)cube.x;
        y = (int)cube.y;
        z = (int)cube.z;
    }
    #endregion

    public long CreateHashCode()
    {
        int hq = x;
        int hr = y;
        return hq * 8290956017 + hr * 2814095477;
        //return hq ^ (hr + 0x9e3779b9 + (hq << 6) + (hq >> 2));
    }

    public void SetValues (int _x, int _y, int _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }
    public void SetValues(int _x, int _y)
    {
        x = _x;
        y = _y;
        z = -x - y;
    }
}

public struct Cube
{
    public float x, y, z;

    public Cube(float _x, float _y, float _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }

    public Cube(float _x, float _z)
    {
        x = _x;
        z = _z;
        y = -x - z;
    }

    public Cube(Hex hex)
    {
        x = hex.x;
        y = hex.y;
        z = hex.z;
    }

    public static Cube operator +(Cube a, Cube b)
    {
        return HexMathLib.CubeAdd(a, b);
    }

    public static Cube operator -(Cube a, Cube b)
    {
        return HexMathLib.CubeSubtract(a, b);
    }

    public static Cube operator *(Cube a, float value)
    {
        return HexMathLib.CubeMultiply(a, value);
    }
}
