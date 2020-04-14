using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// States a tile can be in.
public enum TileType { Normal, Obstacle, Start, End, Path };

/* Display and store data about tiles */

public class Tile : MonoBehaviour
{
    [SerializeField]
    private GameData gameData; // The scriptable object that holds data about tile definitions.

    private SpriteRenderer spriteRenderer; // Cache the renderer of the sprite so we can change its color.

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

    // Position in grid
    private Vector2Int _pos; 

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

    // Tile state
    private TileType _state;
    public TileType State
    {
        get
        {
            return _state;
        }

        set
        {
            _state = value;

            spriteRenderer.color = gameData.GetTypeColor(value);
        }
    }

}

