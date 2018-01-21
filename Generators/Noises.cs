using System;

namespace Generators
{
    public static class Noises
    {
        public static float[][] GenerateWhiteNoise(int width, int height)
        {
            Random random = new Random(0); //Seed to 0 for testing
            float[][] noise = Utils.GetEmptyArray(width, height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    noise[i][j] = (float)random.NextDouble() % 1;
                }
            }

            return noise;
        }

        public static float[][] GenerateRandom(int gridSize, int maxSize)
        {
            var random = new Random();
            var arr = Utils.GetEmptyArray(gridSize, gridSize);
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                    arr[i][j] = random.Next(maxSize);
            }
            return arr;
        }

        public static float[][] GeneratePerlinNoise(float[][] baseNoise, int octaveCount)
        {
            int width = baseNoise.Length;
            int height = baseNoise[0].Length;

            float[][][] smoothNoise = new float[octaveCount][][];

            float persistance = 0.5f;

            for (int i = 0; i < octaveCount; i++)
            {
                smoothNoise[i] = PostModifications.Smooth(baseNoise, i);
            }

            float[][] perlinNoise = Utils.GetEmptyArray(width, height);
            float amplitude = 1.0f;
            float totalAmplitude = 0.0f;

            //blend noise together
            for (int octave = octaveCount - 1; octave >= 0; octave--)
            {
                amplitude *= persistance;
                totalAmplitude += amplitude;

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        perlinNoise[i][j] += smoothNoise[octave][i][j] * amplitude;
                    }
                }
            }

            //normalisation
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    perlinNoise[i][j] /= totalAmplitude;
                }
            }

            return perlinNoise;
        }
    }
}
