using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesGEN : MonoBehaviour
{

    public void GenerateTileMap(float[,] heightMap, float heightMultiplier, AnimationCurve animationCurve, Transform tilePrefab, Transform mapHolder, TerrainType[] regions) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float tileHeight = heightMap[x, y];
                TerrainType tileTerrain;
                for (int i = 1; i < regions.Length; i++)
                {
                    if (tileHeight > regions[i-1].height && tileHeight <= regions[i].height)
                    {
                        tileTerrain = regions[i];
                        Vector2 tilePosition = CoordToPosition(x, y, height, width);
                        float evaluatedHeight = animationCurve.Evaluate(tileHeight);
                        float z = 0;
                        for (z = 0; z < evaluatedHeight; z += .05f)
                        {
                            Transform newTile = Instantiate(tilePrefab, tilePosition + (new Vector2(0f, heightMultiplier) * z), Quaternion.identity) as Transform;
                            newTile.parent = mapHolder;
                            SpriteRenderer spriteRenderer = newTile.GetComponent<SpriteRenderer>();
                            spriteRenderer.sprite = tileTerrain.sprite;
                            spriteRenderer.sortingOrder = ((x + 1) * (y + 1)) + (int) z;
                        }

                        if (evaluatedHeight % 1 != 0f)
                        {
                         /*   Transform newTile = Instantiate(tilePrefab, tilePosition + (new Vector2(0f, heightMultiplier) * (z - 1)) + new Vector2(0f, (evaluatedHeight % 1) * heightMultiplier), Quaternion.identity) as Transform;
                            newTile.parent = mapHolder;
                            SpriteRenderer spriteRenderer = newTile.GetComponent<SpriteRenderer>();
                            spriteRenderer.sprite = tileTerrain.sprite;
                            spriteRenderer.sortingOrder = ((x + 1) * (y + 1)) + (int) z;
                            z++;*/
                        }


                               }
                            }
            }
        }
        

   /*       for (int x = 0; x < width; x++) {
               for (int y = 0; y < height; y++) {
                //   Vector3 tilePos = new Vector3(-width / 2 + 0.5f + x, 0, -height / 2 + 0.5f + y);
                   for (int z = 0; z < Mathf.FloorToInt(animationCurve.Evaluate(heightMap[x,y])); z++)
                   {
                       Transform newTile = Instantiate(tilePrefab, new Vector3(-width / 2 + 0.5f + x, animationCurve.Evaluate(heightMap[x, y]) * heightMultiplier, -height / 2 + 0.5f + y), Quaternion.Euler(Vector3.right * 90)) as Transform;
                       newTile.parent = mapHolder;
                        SpriteRenderer spriteRenderer = newTile.GetComponent<SpriteRenderer>();
                        spriteRenderer.sprite = regions[0].sprite;
                        spriteRenderer.sortingOrder = ((x + 1) * (y + 1));
                   }

            }

           }
           */

        /*      for (int x = 0; x < width; x++) {
                  for (int y = 0; y < height; y++) {
                  //       Vector3 tilePos = new Vector3(-width / 2 + 0.5f + x, animationCurve.Evaluate(heightMap[x, y]) * heightMultuplier, -height / 2 + 0.5f + y); // CoordToPosition(x, y, height, width); 
                      float tileHeight = heightMap[x, y];
                      for (int i = 0; i < 1; i++) {
                          if (tileHeight <= regions[i].height) { 
                              Vector3 tilePos = CoordToPosition(x, y, height, width); // new Vector3(-width / 2 + 0.5f + x, animationCurve.Evaluate(heightMap[x, y]) * heightMultuplier, -height / 2 + 0.5f + y);
                              float evaluatedHeight = animationCurve.Evaluate(tileHeight) * 10;
                              int z = 0;
                              for (z = 0; z < Mathf.FloorToInt(evaluatedHeight); z++) {
                                  Transform newTile = Instantiate(tilePrefab, tilePos + (new Vector3(0f, z * heightMultuplier, 0f)), Quaternion.identity) as Transform; // Instantiate(tilePrefab, tilePos + (new Vector2(0f, heightMultuplier) * z), Quaternion.identity) as Transform;
                                  newTile.parent = mapHolder;
                                  SpriteRenderer spriteRenderer = newTile.GetComponent<SpriteRenderer>();
                                  spriteRenderer.sprite = regions[0].sprite;
                                  spriteRenderer.sortingOrder = ((x + 1) * (y + 1)) + z;
                              }

                              //    Transform newTile = Instantiate(tilePrefab, tilePos, Quaternion.Euler(Vector3.right * 90)) as Transform;
                              //    newTile.parent = mapHolder;
                          }
                      }
                  }
              }*/
    }

    Vector2 CoordToPosition(int x, int y, int height, int width)
    {
        // return new Vector3(-width / 2 + 0.5f + x, 0, -height / 2 + 0.5f + y);

         float zeroX = 0f + .5f * x - .5f * y;
         float zeroY = (.25f * height + .25f * width) / 2f - .25f * y - .25f * x;
         return new Vector2(zeroX, zeroY);
    }
}




