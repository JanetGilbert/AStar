using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// States a tile can be in.
public enum TileType { Normal, Obstacle, Start, End, Path };

/* Display and store data about tiles */

public class Tile : MonoBehaviour, IEquatable<Tile>
{
    [SerializeField]
    private GameData gameData; // The scriptable object that holds data about tile definitions.


    private TileType _type;
    public TileType Type
    {
        get
        {
            return _type;
        }

        set
        {
            _type = value;

            spriteRenderer.color = gameData.GetTypeColor(_type);
        }
    }

    private SpriteRenderer spriteRenderer; // Cache the renderer of the sprite so we can change its color.

    private Vector2Int _pos; // Position in grid

    public Vector2Int Pos
    {
        get
        {
            return _pos;
        }

        set
        {
            _pos = value;
        }
    }

   


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
       
    }

   
    void Update()
    {
        
    }


    // A* stuff
    public Tile prev;
    public int distFromStart;
    public int distFromEnd;
    public int Cost
    {
        get
        {
            return distFromStart + distFromEnd;
        }
    }

    // From https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/how-to-define-value-equality-for-a-type
    public override bool Equals(object obj)
    {
        return this.Equals(obj as Tile);
    }

    public bool Equals(Tile t)
    {
        // If parameter is null, return false.
        if (System.Object.ReferenceEquals(t, null))
        {
            return false;
        }

        // Optimization for a common success case.
        if (System.Object.ReferenceEquals(this, t))
        {
            return true;
        }

        // If run-time types are not exactly the same, return false.
        if (this.GetType() != t.GetType())
        {
            return false;
        }

        // Return true if the fields match.
        // Note that the base class is not invoked because it is
        // System.Object, which defines Equals as reference equality.
        return (t.Pos == Pos);
    }

    public override int GetHashCode()
    {
        return Pos.x * 0x00010000 + Pos.y;
    }
}


/*public class PathTile
{
    public Tile tile;
    public int distFromStart;
    public int distFromEnd;
    public int Value
    {
        get
        {
            return distFromStart + distFromEnd;
        }
    }

    public PathTile(Tile linkedTile, PathTile prev, Tile dest)
    {
        tile = linkedTile;
        distFromStart = prev.distFromStart + 1;
        distFromEnd = Mathf.Abs(tile.Pos.x - dest.Pos.x) + Mathf.Abs(tile.Pos.y - dest.Pos.y);
    }

}*/
