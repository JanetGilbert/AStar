using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* A class to contain the tile grid and the pathfinding functions.*/
public class Map : MonoBehaviour
{
    [HeaderAttribute("Prefab to create individual tiles")]
    [SerializeField]
    private Tile tilePrefab;

    [HeaderAttribute("Grid size")]
    [SerializeField]
    private int sizeX = 10;
    [SerializeField]
    private int sizeY = 10;

    // Store tiles in grid.
    private Tile[,] tileGrid;

    // Start and end of path.
    private Vector2Int startPos;
    private Vector2Int destPos;


    void Start()
    {
        // Create simple grid test layout.
        InitGrid();
        PlaceObstacle(2,3,4,2);

        SetStartPos(0, 1);
        SetDestPos(5, 7);
    }

    
    void Update()
    {
        
    }

    // Initialize empty grid.
    private void InitGrid()
    {
        SpriteRenderer spriteRenderer = tilePrefab.GetComponent<SpriteRenderer>();
        float gridW = spriteRenderer.bounds.size.x; // Get size of sprite.
        float gridH = spriteRenderer.bounds.size.y;
        Vector3 tilePos = transform.position;

        tileGrid = new Tile[sizeX, sizeY];

        for (int x = 0; x < sizeX; x++)
        {            
            for (int y = 0; y < sizeY; y++)
            {
                Tile newTile = Instantiate(tilePrefab, tilePos, Quaternion.identity);
                newTile.transform.parent = transform;
                tileGrid[x, y] = newTile;

                tilePos.y += gridH;
            }

            tilePos.x += gridW;
            tilePos.y = transform.position.y;
        }

    }

    // Place a rectangular obstacle.
    private void PlaceObstacle(int posX, int posY, int sizeX, int sizeY)
    {
        for (int x = posX; x < posX + sizeX; x++)
        {
            for (int y = posY; y < posY + sizeY; y++)
            {
                tileGrid[x, y].Type = TileType.Obstacle;
            }
        }
    }

    // Set Starting position.
    private void SetStartPos(int posX, int posY)
    {
        startPos = new Vector2Int(posX, posY);

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (tileGrid[x, y].Type == TileType.Start) // If Start is already set, clear it.
                {
                    tileGrid[x, y].Type = TileType.Normal;
                }
            }
        }

        tileGrid[posX, posY].Type = TileType.Start;
    }

    // Set Destination position.
    private void SetDestPos(int posX, int posY)
    {
        destPos = new Vector2Int(posX, posY);

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (tileGrid[x, y].Type == TileType.End) // If Destination is already set, clear it.
                {
                    tileGrid[x, y].Type = TileType.Normal;
                }
            }
        }

        tileGrid[posX, posY].Type = TileType.End;
    }



}
