﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGEN : MonoBehaviour {

    public enum DrawMode { NoiseMap, ColourMap };
    public DrawMode drawmode;

    public int mapWidth;
    public int mapHeight;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;

    public TerrainType[] regions;

    public void Generate() {
        float[,] noiseMap = PerlinNoise.GenerateNoiseMap(seed, mapWidth, mapHeight, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colourMap = new Color[mapWidth * mapHeight];
        for (int y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++) {
                    if (currentHeight <= regions[i].height) {
                        colourMap[y * mapWidth + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawmode == DrawMode.NoiseMap) {
            display.DrawTexture(TextureGEN.TextureFromHeightMap(noiseMap));
        } else if (drawmode == DrawMode.ColourMap) {
            display.DrawTexture(TextureGEN.TextureFromColourMap(colourMap, mapWidth, mapHeight));
        }

    }

    private void OnValidate() {
        if (mapWidth < 1) {
            mapWidth = 1;
        }
        if (mapHeight < 1) {
            mapHeight = 1;
        }
        if (noiseScale <= 0) {
            noiseScale = 0.0001f;
        }
        if (octaves < 1) {
            octaves = 1;
        }
        if (lacunarity < 1) {
            lacunarity = 1;
        }
    }
}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color colour;
}