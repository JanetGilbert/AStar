using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// States a tile can be in.
public enum TileType { Normal, Obstacle, Start, End };

/* Display and store data about tiles */

public class Tile : MonoBehaviour
{
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

    private SpriteRenderer spriteRenderer; // Get the renderer of the sprite so we can change its color.


    [SerializeField]
    private GameData gameData; // The scriptable object that holds data about tile definitions.

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



}
