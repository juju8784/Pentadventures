using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    //An empty gameobject where you want the grid to build from
    public GameObject Grid;
    //The tile you want to build with
    public GameObject Tile;
    //The list of tiles
    public List<BaseTile> tiles;

    //Total size of the grid
    public int size;

    // Start is called before the first frame update
    void Start()
    {
        tiles = new List<BaseTile>();
        
        BuildGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BuildGrid()
    {
        Mesh mesh = Tile.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Bounds bounds = mesh.bounds;
        //gets the edge distance from the lesser of the 2
        float edgeDistance = (bounds.size.x < bounds.size.z) ? bounds.size.x : bounds.size.z;

        GameObject previousTile = Instantiate(Tile);

        for (int i = 0; i < size - 1; i++)
        {
            Vector3 position = previousTile.transform.position;
            
        }

    }
}
