using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGEN : MonoBehaviour {

    public enum DrawMode { NoiseMap, IslandFalloffMap, ColourMap, Mesh, TileMap};
    public DrawMode drawmode;

    const int chunkSize = 50;
    [Range(0,6)]
    public int levelOfDetail;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public float objectsSpawnRate;
    public Vector2 offset;

    public bool island;
    float[,] islandFalloffMap;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public TerrainType[] regions;

    public float objectSpawnRate;

    public Transform tilePrefab;

    private void Awake() {
        islandFalloffMap = IslandGEN.GetIslandMap(chunkSize);
    }

    public void Generate() {
        float[,] noiseMap = PerlinNoise.GenerateNoiseMap(seed, chunkSize, chunkSize, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colourMap = new Color[chunkSize * chunkSize];
        for (int y = 0; y < chunkSize; y++) {
            for (int x = 0; x < chunkSize; x++) {
                if(island) {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - islandFalloffMap[x, y]);
                }
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++) {
                    if (currentHeight <= regions[i].height) {
                        colourMap[y * chunkSize + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawmode == DrawMode.NoiseMap) {
            display.DrawTexture(TextureGEN.TextureFromHeightMap(noiseMap));
        } else if (drawmode == DrawMode.IslandFalloffMap) {
            display.DrawTexture(TextureGEN.TextureFromHeightMap(islandFalloffMap));
        } else if (drawmode == DrawMode.ColourMap) {
            display.DrawTexture(TextureGEN.TextureFromColourMap(colourMap, chunkSize, chunkSize));
        } else if (drawmode == DrawMode.Mesh) {
            display.DrawMesh(MeshGEN.GenerateMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGEN.TextureFromColourMap(colourMap, chunkSize, chunkSize));
      //      StupidSpawn.SpawnObjects(noiseMap, meshHeightMultiplier, meshHeightCurve, tilePrefab, regions);
        } else if (drawmode == DrawMode.TileMap) {
            string holderName = "TiledMap";
            if (transform.Find(holderName)) {
                DestroyImmediate(transform.Find(holderName).gameObject);
            }
            Transform mapHolder = new GameObject(holderName).transform;
            mapHolder.parent = transform;
            TilesGEN tilesGEN = new TilesGEN();
            tilesGEN.GenerateTileMap(noiseMap, meshHeightMultiplier, meshHeightCurve, tilePrefab, mapHolder, regions);
        }

        ////
        Player player = new Player();
        player.Spawn(noiseMap, meshHeightMultiplier, meshHeightCurve, tilePrefab, regions[0].spawnedObjects[0].sprite);
        ////
        

}

    private void OnValidate() {
        if (noiseScale <= 0) {
            noiseScale = 0.0001f;
        }
        if (octaves < 1) {
            octaves = 1;
        }
        if (lacunarity < 1) {
            lacunarity = 1;
        }
        islandFalloffMap = IslandGEN.GetIslandMap(chunkSize);
    }
}

[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color colour;
    public Sprite sprite;

    public SpawnedObjects[] spawnedObjects;

}


[System.Serializable]
public struct SpawnedObjects
{
    public string name;
    public Object obj;
    public Sprite sprite;

    public float cumulativeWeight;
}
