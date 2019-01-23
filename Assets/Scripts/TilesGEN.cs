using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesGEN : MonoBehaviour
{

    public void GenerateTileMap(float[,] heightMap, float heightMultiplier, AnimationCurve animationCurve, Transform tilePrefab, Transform mapHolder, TerrainType[] regions) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        System.Random prng = new System.Random(MapGEN.seed);

        int sortOrder;
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                float tileHeight = heightMap[x, y];
                TerrainType tileTerrain;
                for (int i = 1; i < regions.Length; i++) {
                    if (tileHeight > regions[i - 1].height && tileHeight <= regions[i].height) {
                        tileTerrain = regions[i];
                        Vector2 tilePosition = CoordToPosition(x, y, height, width);
                        float evaluatedHeight = animationCurve.Evaluate(tileHeight);
                        float z = 0;
                        int blockCount = 0;

                        ///Если что - поменять порядок обхода по Z
                            for (z = evaluatedHeight; z > 0; z -= .05f) {
                                Transform newTile = Instantiate(tilePrefab, tilePosition + ( new Vector2(0f, heightMultiplier) * z ), Quaternion.identity) as Transform;
                                newTile.parent = mapHolder;
                                SpriteRenderer spriteRenderer = newTile.GetComponent<SpriteRenderer>();
                                spriteRenderer.sprite = tileTerrain.sprite;
                                sortOrder = ( ( ( x + 1 ) * ( y + 1 ) ) + ( int )z ) / 5;
                                spriteRenderer.sortingOrder = sortOrder;
                            if (i > 4 && z < evaluatedHeight - .15f) {
                                break;
                            }
                            }
                        //   }

                        //Objectspawn
                        if (prng.Next(0, 100) < .1f * 100)
                        {
                            float foliageRandomNumber = prng.Next(100) * .05f;
                            for (int f = 0; f < regions[i].spawnedObjects.Length; f++)
                            {
                                if (foliageRandomNumber <= regions[i].spawnedObjects[f].cumulativeWeight)
                                {
                                    Vector2 topSmoothTileAddedHeight;
                                        topSmoothTileAddedHeight = new Vector2(0, .15f);
                                    Transform newFoliage = Instantiate(tilePrefab, tilePosition + ( new Vector2(0f, heightMultiplier) * ( z + .05f) ) + topSmoothTileAddedHeight, Quaternion.identity) as Transform;
                                    newFoliage.parent = mapHolder;
                                    SpriteRenderer spriteRenderer2 = newFoliage.GetComponent<SpriteRenderer>();
                                    spriteRenderer2.sprite = regions[i].spawnedObjects[f].sprite;
                                    spriteRenderer2.sortingOrder = (( x + 1 ) * ( y + 1 ) ) + (int) z + 1;
                                    break;
                                }
                            }
                        }



                        break;
                    }
                }
            }
        }
    }

    Vector2 CoordToPosition(int x, int y, int height, int width)
    {
        // return new Vector3(-width / 2 + 0.5f + x, 0, -height / 2 + 0.5f + y);
         float zeroX = 0f + .5f * x - .5f * y;
         float zeroY = (.25f * height + .25f * width) / 2f - .25f * y - .25f * x;
         return new Vector2(zeroX, zeroY);
    }
}




