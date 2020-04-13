using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Struct to associate type with color (since we can't serialize dictionaries)
[Serializable]
public struct TypeColor
{
    public TileType type;
    public Color color;
}

/*
 * Game data definitions.
 */
[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [SerializeField]
    private TypeColor[] typeColors; // Link Tile type to display color.

    // Find the correct color for the current tile type.
    public Color GetTypeColor(TileType tileType)
    {
        foreach (TypeColor typeColor in typeColors)
        {
            if (typeColor.type == tileType)
            {
                return typeColor.color;
            }
        }

        return Color.black;
    }
}
