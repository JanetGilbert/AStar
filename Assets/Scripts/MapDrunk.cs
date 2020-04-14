using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* The drunkard's walk algorithm, a simple and bad algorithm that walks randomly and hopes to hit the destination.*/
public class MapDrunk : Map
{
    // Overriden so the base class can be called.
    protected override void Start()
    {
        base.Start();
    }

    // Overriden so the base class can be called.
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.D)) // Trigger algorithm.
        {
            ClearDisplay();
            CalculateDrunkardsWalk();
        }
    }


    // Find a random path and maybe, hopefully, end up at the destination *hic*
    public void CalculateDrunkardsWalk()
    {
        Tile start = tileGrid[startPos.x, startPos.y];
        Tile end = tileGrid[destPos.x, destPos.y];
        Tile current = start;
        int sanityCheck = 999; // Make sure we don't wander endlessly if the route is completely blocked.

        pathTiles = new List<Tile>();

        // Repeat until the destination is found.
        while (!pathTiles.Contains(end) && sanityCheck > 0)
        {
            List<Tile> adjacent = new List<Tile>();

            // Make a list of all the adjacent tiles we could move to.
            foreach (Vector2Int dir in allDirections)
            {
                Vector2Int pos = current.Pos + dir;
                if (IsValidTile(pos))
                {
                    adjacent.Add(tileGrid[pos.x, pos.y]);
                }
            }

            // Randomly choose one a possible adjacent tile to move to.
            if (adjacent.Count > 0)
            {
                Tile newCurrent = adjacent[Random.Range(0, adjacent.Count)];
                pathTiles.Add(newCurrent);
                current = newCurrent;
            }

            sanityCheck--;
        }
    }
}
