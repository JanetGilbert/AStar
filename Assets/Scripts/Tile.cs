using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// States a tile can be in.
public enum TileType { Normal, Obstacle, Start, End };

/* Display and store data about tiles */
public class Tile : MonoBehaviour
{

    [SerializeField]
    [HeaderAttribute("The overlay sprite that displays the path.")]
    private SpriteRenderer routeRenderer = null;

    private SpriteRenderer spriteRenderer; // Cache the renderer of the sprite so we can change its color.
    private Map map;
    public Map TileMap
    {
        set
        {
            map = value;
        }
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
       
    }

    void Start()
    {
        RouteOverlay = false;
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

            spriteRenderer.color = map.gameData.GetTypeColor(value);
        }
    }

    // Overlay tile with route marker so we can see the route that has been calculated.
    public bool RouteOverlay
    {
        set
        {
            routeRenderer.enabled = value;
        }
    }

    // Paint features onto the map.
    public void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            map.TileClick(this);
        }
    }
}

