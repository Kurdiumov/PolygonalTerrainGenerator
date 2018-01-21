﻿namespace Generators
{
    public static class PostModifications
    {
        public static float[][] NormalizeAndFlatten(float[][] arr, int GridSize, float Flattening, float Height)
        {
            arr = Normalize(arr, GridSize);
            arr = Flatten(arr, GridSize, Flattening);
            for (var x = 0; x < GridSize; ++x)
            for (var y = 0; y < GridSize; ++y)
                arr[x][y] = arr[x][y] * Height;
            return arr;
        }

        public static float[][] Normalize(float[][] arr, int GridSize, float Height)
        {
            arr = Normalize(arr, GridSize);
            for (var x = 0; x < GridSize; ++x)
            for (var y = 0; y < GridSize; ++y)
                arr[x][y] = arr[x][y] * Height;
            return arr;
        }

        public static float[][] Normalize(float[][] arr, int GridSize)
        {
            float min = 0;
            float max = 0;

            for (var x = 0; x < GridSize; ++x)
            for (var y = 0; y < GridSize; ++y)
            {
                var z = arr[x][y];
                if (z < min) min = z;
                if (z > max) max = z;
            }


            if (min != max)
            {
                for (var x = 0; x < GridSize; ++x)
                for (var y = 0; y < GridSize; ++y)
                {
                    arr[x][y] = (arr[x][y] - min) / (max - min);
                }
            }
            else
            {
                for (var x = 0; x < GridSize; ++x)
                for (var y = 0; y < GridSize; ++y)
                    arr[x][y] = min;
            }

            return arr;
        }

        public static float[][] Flatten(float[][] arr, int GridSize, float Flattening)
        {
            if (Flattening <= 1) return arr;
            for (var x = 0; x < GridSize; ++x)
            {
                for (var y = 0; y < GridSize; ++y)
                {
                    var flat = 1f;
                    var original = arr[x][y];

                    for (var i = 0; i < Flattening; ++i)
                        flat *= original;

                    arr[x][y] = flat;
                }
            }

            return arr;
        }

        public static float[][] Smooth(float[][] input, int octave)
        {
            int width = input.Length;
            int height = input[0].Length;

            float[][] smoothNoise = Utils.GetEmptyArray(width, height);

            int samplePeriod = 1 << octave;
            float sampleFrequency = 1.0f / samplePeriod;

            for (int i = 0; i < width; i++)
            {

                int sample_i0 = (i / samplePeriod) * samplePeriod;
                int sample_i1 = (sample_i0 + samplePeriod) % width; 
                float horizontal_blend = (i - sample_i0) * sampleFrequency;

                for (int j = 0; j < height; j++)
                {

                    int sample_j0 = (j / samplePeriod) * samplePeriod;
                    int sample_j1 = (sample_j0 + samplePeriod) % height; 
                    float vertical_blend = (j - sample_j0) * sampleFrequency;


                    float top = Utils.Interpolate(input[sample_i0][sample_j0],
                        input[sample_i1][sample_j0], horizontal_blend);


                    float bottom = Utils.Interpolate(input[sample_i0][sample_j1],
                        input[sample_i1][sample_j1], horizontal_blend);

                    smoothNoise[i][j] = Utils.Interpolate(top, bottom, vertical_blend);
                }
            }

            return smoothNoise;
        }
    }
}
