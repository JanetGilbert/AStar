﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// States a tile can be in.
public enum TileType { Normal, Obstacle, Start, End };

/* Display and store data about tiles */
public class Tile : MonoBehaviour
{
    [SerializeField]
    [HeaderAttribute("The scriptable object that holds data about tile definitions.")]
    private GameData gameData = null; 

    [SerializeField]
    [HeaderAttribute("The overlay sprite that displays the path.")]
    private SpriteRenderer routeRenderer = null;

    [SerializeField]
    [HeaderAttribute("The overlay sprite that displays the current end of the path")]
    private SpriteRenderer cursorRenderer = null;

    private SpriteRenderer spriteRenderer; // Cache the renderer of the sprite so we can change its color.
    private Map map;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
       
    }

    void Start()
    {
        // Ensure we get an active map if some may be inactive.
        Map [] maps = GetComponentsInParent<Map>();

        foreach (Map m in maps)
        {
            if (m.enabled)
            {
                map = m;
            }
        }

        ClearRouteOverlay();
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

    // Reset overlay display.
    public void ClearRouteOverlay()
    {
        cursorRenderer.enabled = false;
        routeRenderer.enabled = false;
    }

    // Display an overlay to show that this tile is the current end of the path.
    public void DisplayAsCursor()
    {
        cursorRenderer.enabled = true;
        routeRenderer.enabled = false;
    }

    // Display an overlay to show that this tile is on the path.
    public void DisplayAsRoute()
    {
        cursorRenderer.enabled = false;
        routeRenderer.enabled = true;
    }

    // Draw obstacles onto map.
    public void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            map.TileClick(this);
        }
    }



}

