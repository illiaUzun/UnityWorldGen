using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public void Spawn(float[,] heightMap, float heightMultiplier, AnimationCurve animationCurve, Transform tilePrefab, Sprite sprite) {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Vector2 tilePosition = CoordToPosition(width / 2, height / 2, height, width);
        Vector3 mapCenter = tilePosition + new Vector2(0f,heightMap[width / 2, height / 2]);

        Transform player = Instantiate(tilePrefab, mapCenter, Quaternion.identity) as Transform;
        SpriteRenderer spriteRenderer2 = player.GetComponent<SpriteRenderer>();
        spriteRenderer2.sprite = sprite;
        spriteRenderer2.sortingOrder = 500;
    }

    Vector2 CoordToPosition(int x, int y, int height, int width)
    {
        // return new Vector3(-width / 2 + 0.5f + x, 0, -height / 2 + 0.5f + y);
        float zeroX = 0f + .5f * x - .5f * y;
        float zeroY = ( .25f * height + .25f * width ) / 2f - .25f * y - .25f * x;
        return new Vector2(zeroX, zeroY);
    }
}
