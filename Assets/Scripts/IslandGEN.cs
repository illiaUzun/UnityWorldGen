using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IslandGEN {
    public static float[,] GetIslandMap(int chunkSize) {
        float[,] map = new float[chunkSize, chunkSize];
        for (int i = 0; i < chunkSize; i++) {
            for (int j = 0; j < chunkSize; j++) {
                float x = i / (float)chunkSize * 2 - 1;
                float y = j / (float)chunkSize * 2 - 1;

                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                map[i, j] = CalculateCurve(value);
            }
        }
        return map;
    }

    static float CalculateCurve (float value) {
        float a = 3;
        float b = 2.2f;

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
    }
}
