using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Struct to associate type with color (since we can't serialize dictionaries)
[Serializable]
public struct TypeData
{
    public TileType type;
    public Color color;
    public char typeChar;
}

/*
 * Game data definitions.
 */
[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [HeaderAttribute("Link a Tile Type to information about it.")]
    [SerializeField]
    private TypeData[] typeData = null; 

    // Find the correct color for the current tile type.
    public Color GetTypeColor(TileType tileType)
    {
        foreach (TypeData typeColor in typeData)
        {
            if (typeColor.type == tileType)
            {
                return typeColor.color;
            }
        }

        return Color.black;
    }

    // Map from a char to a tile type.
    public TileType GetTypeFromChar(char tileChar)
    {
        foreach (TypeData typeColor in typeData)
        {
            if (typeColor.typeChar == tileChar)
            {
                return typeColor.type;
            }
        }

        return TileType.Normal;
    }
}
