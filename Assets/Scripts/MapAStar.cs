using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Implements the A Star algorithm without edge weights, so each tile is equally easy to move to. */
public class MapAStar : Map
{
    // Tiles with information needed to run the A* algorithm.
    private AStarTile[,] aStarTiles;

    // Overriden so the base class can be called.
    protected override void Start()
    {
        base.Start();

        aStarTiles = new AStarTile[sizeX, sizeY];

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                aStarTiles[x, y] = new AStarTile(tileGrid[x, y]);
            }
        }
    }

    // Overriden so the base class can be called.
    protected override void Update()
    {
        base.Update();


    }

    // Find the path to the destination using the A* algorithm.
    private void CalculateAStar()
    {
        AStarTile start = aStarTiles[startPos.x, startPos.y];
        AStarTile end = aStarTiles[destPos.x, destPos.y];
        List<AStarTile> open = new List<AStarTile>(); // List of tiles we need to check.
        List<AStarTile> closed = new List<AStarTile>(); // List of tiles we have already checked.
        AStarTile current = start;

        // Add the starting tile to the open list.
        //  *FILL IN*

        // Repeat until we have either checked all tiles or found the end.
        while (open.Count > 0 && !open.Contains(end))
        {
            // *FILL IN*
                // Find the tile on the open list with the least cost.
                // Move to the tile with least cost.
                // Remove it from the open list.
                // Add it to the closed list.
                // Find all valid adjacent tiles. (use IsValidTile())
                // Add the best adjacent tile to the path.
        }

        // Build display path.
        pathTiles = new List<Tile>();


        if (open.Contains(end))
        {
            current = end;
            while (current.prev != null)
            {
                pathTiles.Add(current.displayTile);
                current = current.prev;
            }

            pathTiles.Reverse(); // Reverse display path as it is built from the destination to the start.
        }


    }
    // Local class for containing extra details about tiles that the A* algorithm needs.
    // (only accessible to the MapAStar class)
    private class AStarTile
    {
        public Tile displayTile; // Link to the actual Monobehaviour tile object.

        public AStarTile prev; // The previous tile in the path.
        public int distFromStart; // How far have we come from the start?
        public int distFromEnd; // A guess at how far we are from the destination.
        public int Cost // How good is this tile? The lower the better.
        {
            get
            {
                return distFromStart + distFromEnd;
            }
        }

        // Constructor
        public AStarTile(Tile linkedTile)
        {
            displayTile = linkedTile;
            prev = null;
            distFromStart = 0;
            distFromEnd = 0;
        }

        // Get the display tile grid position.
        public Vector2Int Pos
        {
            get
            {
                return displayTile.Pos;
            }

            set
            {
                displayTile.Pos = value;
            }
        }
    }

    public override void RunPathfindingAlgorithm()
    {
        ClearDisplay();
        CalculateAStar();
        DisplayRoute();
    }
}


