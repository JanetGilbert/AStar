using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

/* A class to contain the tile grid and serve as a base class for the path algorithms.*/
public class Map : MonoBehaviour
{
    [HeaderAttribute("Prefab to create individual tiles")]
    [SerializeField]
    private Tile tilePrefab;

    [HeaderAttribute("Grid size")]
    [SerializeField]
    protected int sizeX = 10;
    [SerializeField]
    protected int sizeY = 10;

    // Store tiles in grid.
    protected Tile[,] tileGrid;

    // Start and end of path.
    protected Vector2Int startPos;
    protected Vector2Int destPos;

    // The calculated path to display.
    protected List<Tile> pathTiles;
    protected int displayIndex; // Current path object to display.

    // All possible directions. (could be simplified to just up, down, left, right)
    protected readonly Vector2Int[] allDirections =
                                           {Vector2Int.up,
                                            Vector2Int.up + Vector2Int.left,
                                            Vector2Int.up + Vector2Int.right,
                                            Vector2Int.down,
                                            Vector2Int.down + Vector2Int.left,
                                            Vector2Int.down + Vector2Int.right,
                                            Vector2Int.left,
                                            Vector2Int.right };

    // Virtual so that it can be overriden in derived class.
    protected virtual void Start()
    {
        // Create simple grid test layout.
        InitGrid();
        PlaceObstacle(2,3,4,2);

        SetStartPos(4, 1);
        SetDestPos(5, 7);
    }

    // Virtual so that it can be overriden in derived class.
    protected virtual void Update()
    {
        // Display next tile in path when user presses space.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceDisplay();
        }
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
                newTile.Pos = new Vector2Int(x, y);
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
                tileGrid[x, y].State = TileType.Obstacle;
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
                if (tileGrid[x, y].State == TileType.Start) // If Start is already set, clear it.
                {
                    tileGrid[x, y].State = TileType.Normal;
                }
            }
        }

        tileGrid[posX, posY].State = TileType.Start;
    }

    // Set Destination position.
    private void SetDestPos(int posX, int posY)
    {
        destPos = new Vector2Int(posX, posY);

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (tileGrid[x, y].State == TileType.End) // If Destination is already set, clear it.
                {
                    tileGrid[x, y].State = TileType.Normal;
                }
            }
        }

        tileGrid[posX, posY].State = TileType.End;
    }


    // Is the position on the board and not blocked?
    public bool IsValidTile(Vector2Int pos)
    {
        if ((pos.x >= 0) &&
            (pos.y >= 0) &&
            (pos.x < sizeX) &&
            (pos.y < sizeY))
        {
            if ((tileGrid[pos.x, pos.y].State == TileType.Normal) ||
                (tileGrid[pos.x, pos.y].State == TileType.End))
            {
                return true;
            }
        }

        return false;
    }

    // Reset display path display.
    protected void ClearDisplay()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (tileGrid[x, y].State == TileType.Path)
                {
                    tileGrid[x, y].State = TileType.Normal;
                }
            }
        }

        displayIndex = 0;
    }

    // Display next point in path.
    private void AdvanceDisplay()
    {
        if (displayIndex < pathTiles.Count)
        {
            tileGrid[pathTiles[displayIndex].Pos.x, pathTiles[displayIndex].Pos.y].State = TileType.Path;
            displayIndex++;
        }
    }




}

