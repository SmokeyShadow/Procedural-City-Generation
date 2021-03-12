using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise
{

    private static Noise _instance;
    public static Noise Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Noise();
            return _instance;
        }
    }

    public float[,] GenerateNoiseMap(int width, int height, float scale, int seed, float offsetX, float offsetY)
    {
        float[,] noiseMap = new float[width, height];

        System.Random prng = new System.Random(seed);


        offsetX += prng.Next(-100000, 100000);
        offsetY += prng.Next(-100000, 100000);


        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float sampleX = (x) / scale  + offsetX;
                float sampleY = (y) / scale  + offsetY;

                float perlinVal = Mathf.PerlinNoise(sampleX, sampleY) * 10;
                noiseMap[x, y] = perlinVal;

                if (noiseMap[x, y] > maxNoiseHeight)
                    maxNoiseHeight = noiseMap[x, y];
                else if (noiseMap[x, y] < minNoiseHeight)
                    minNoiseHeight = noiseMap[x, y];        
            }

        }
        //scale the number (noisemap) between [0,1]
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }

}
