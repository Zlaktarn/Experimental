using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Biome Preset", menuName = "New Block Preset")]
public class BiomePreset : ScriptableObject
{

    public GameObject[] tiles;
    public float minHeight;
    public float minMoisture;
    public float minHeat;

    public GameObject GetTileObject()
    {
        return tiles[Random.Range(0, tiles.Length)];
    }

    public bool MatchCondition(float height, float moisture, float heat)
    {
        return height >= minHeight && moisture >= minMoisture && heat >= minHeat;
    }
}
