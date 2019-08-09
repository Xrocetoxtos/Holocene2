using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGeneration : MonoBehaviour
{

	public float[,] GeneratePerlinNoiseMap (int width, int depth, float scale, float offsetX, float offsetZ,Wave[] waves)
    {
		float[,] noiseMap = new float[width, depth];

		for (int xIndex = 0; xIndex < width; xIndex++)
        {
			for (int zIndex = 0; zIndex < depth; zIndex++)
            {
				float sampleX = (xIndex + offsetX) / scale;
				float sampleZ = (zIndex + offsetZ) / scale;

                //float noise = Mathf.PerlinNoise (sampleX, sampleZ);
                float noise = 0;
                float normalization = 0f;
                foreach (Wave wave in waves)
                {
                    noise += wave.amplitude * Mathf.PerlinNoise(sampleX * wave.frequence + wave.seed, sampleZ * wave.frequence + wave.seed);
                    normalization += wave.amplitude;
                }
                noise /= normalization;
				noiseMap [xIndex, zIndex] = noise;
			}
		}
		return noiseMap;
	}
}

[System.Serializable]
public class Wave
{
    public float seed;
    public float frequence;
    public float amplitude;
}
