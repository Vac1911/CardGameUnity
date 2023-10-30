using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class Grid3D : MonoBehaviour
{
    public Vector3 gridSize;
    public Vector3 cellSize;
    public GameObject cell;

    public bool shouldGenerateGrid = false;

    void Generate()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++)
            {
                for( int z = 0; z < gridSize.z; z++)
                {
                    Instantiate(cell, new Vector3(cellSize.x * x, cellSize.y * y, cellSize.z * z), Quaternion.identity, this.transform);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldGenerateGrid)
        {
            Generate();
            shouldGenerateGrid = false;
        }   
    }
}
