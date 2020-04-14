using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

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

    // The calculated path;
    private List<Tile> pathTiles;
    private int displayIndex; // Current path object to display.

    void Start()
    {
        // Create simple grid test layout.
        InitGrid();
        PlaceObstacle(2,3,4,2);

        SetStartPos(4, 1);
        SetDestPos(5, 7);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            CalculateDrunkardsWalk();
            ClearDisplay();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            CalculateAStar();
            ClearDisplay();
        }

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

    private void CalculateDrunkardsWalk()
    {
        Tile start = tileGrid[startPos.x, startPos.y];
        Tile end = tileGrid[destPos.x, destPos.y];
        Tile current = start;
        int sanityCheck = 999; // Make sure we don't wander endlessly if the route is completely blocked.

        pathTiles = new List<Tile>();

        while (!pathTiles.Contains(end) && sanityCheck > 0)
        {
            List<Tile> adjacent = new List<Tile>();

            AddTileIfPossible(adjacent, current.Pos + Vector2Int.up);
            AddTileIfPossible(adjacent, current.Pos + Vector2Int.down);
            AddTileIfPossible(adjacent, current.Pos + Vector2Int.left);
            AddTileIfPossible(adjacent, current.Pos + Vector2Int.right);

            if (adjacent.Count > 0)
            {
                Tile newCurrent = adjacent[Random.Range(0, adjacent.Count)];
                pathTiles.Add(newCurrent);
                current = newCurrent;
            }

            sanityCheck--;
        }
    }

    // Is the position on the board and a valid move position.
    private bool CanAddTile(Vector2Int pos)
    {
        if ((pos.x >= 0) &&
            (pos.y >= 0) &&
            (pos.x < sizeX) &&
            (pos.y < sizeY))
        {
            if ((tileGrid[pos.x, pos.y].Type == TileType.Normal) ||
                (tileGrid[pos.x, pos.y].Type == TileType.End))
            {
                return true;
            }
        }

        return false;
    }

    private void AddTileIfPossible(List<Tile> tiles, Vector2Int pos)
    {
        if (CanAddTile(pos))
        {
            tiles.Add(tileGrid[pos.x, pos.y]);
        }
    }




    private void ClearDisplay()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (tileGrid[x, y].Type == TileType.Path)
                {
                    tileGrid[x, y].Type = TileType.Normal;
                }
            }
        }

        displayIndex = 0;
    }

    private void AdvanceDisplay()
    {
        if (displayIndex < pathTiles.Count)
        {
            tileGrid[pathTiles[displayIndex].Pos.x, pathTiles[displayIndex].Pos.y].Type = TileType.Path;
            displayIndex++;
        }
    }

    private void CalculateAStar()
    {
        Tile start = tileGrid[startPos.x, startPos.y];
        Tile end = tileGrid[destPos.x, destPos.y];
        List<Tile> open = new List<Tile>();
        List<Tile> closed = new List<Tile>();
        Tile current = start;

    //   

        open.Add(start);

        start.prev = null;

        while (open.Count > 0 && !open.Contains(end))
        {
            int minCost = 9999;
            int lowestIndex = 0;

            for (int t = 0; t < open.Count; t++)
            {
                if (open[t].Cost < minCost)
                {
                    minCost = open[t].Cost;
                    lowestIndex = t;
                }
            }

            current = open[lowestIndex];
            open.Remove(current);
            closed.Add(current);


            List<Tile> adjacent = new List<Tile>();

            AddTileIfPossibleA(adjacent, current.Pos + Vector2Int.up, closed);
            AddTileIfPossibleA(adjacent, current.Pos + Vector2Int.down, closed);
            AddTileIfPossibleA(adjacent, current.Pos + Vector2Int.left, closed);
            AddTileIfPossibleA(adjacent, current.Pos + Vector2Int.right, closed);

            foreach (Tile tile in adjacent)
            {
                if (open.Contains(tile))
                {
                    if (current.distFromStart + 1 < tile.distFromStart)
                    {
                        tile.distFromStart = current.distFromStart + 1;
                        tile.prev = current;
                    }
                }
                else
                {
                    open.Add(tile);
                    tile.prev = current;
                    tile.distFromStart = current.distFromStart + 1;
                    tile.distFromEnd = Mathf.Abs(tile.Pos.x - end.Pos.x) + Mathf.Abs(tile.Pos.y - end.Pos.y);
                }
            }
        }

        // Build path.
        pathTiles = new List<Tile>();
        current = end;
        while (current.prev != null)
        {
            pathTiles.Add(current);
            current = current.prev;
        }

        pathTiles.Reverse();
    }



    private void AddTileIfPossibleA(List<Tile> tiles, Vector2Int pos, List<Tile> closed)
    {
        if (CanAddTile(pos))
        {
            if (!closed.Contains(tileGrid[pos.x, pos.y]))
            {
                tiles.Add(tileGrid[pos.x, pos.y]);
            }
        }
    }
}
