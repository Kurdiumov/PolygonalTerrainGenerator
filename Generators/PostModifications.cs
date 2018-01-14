using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generators
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
    }
}
