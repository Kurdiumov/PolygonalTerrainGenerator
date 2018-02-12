using System;

namespace Generators
{
    public static class Noises
    {
        private  const float F2 = (float)(1.0 / 2.0);
        private  const float G2 = (float)(1.0 / 4.0);

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

        public static float Simplex(int seed, float x, float y)
        {
            float t = (x + y) * F2;
            int i = Utils.Floor(x + t);
            int j = Utils.Floor(y + t);

            t = (i + j) * G2;
            float X0 = i - t;
            float Y0 = j - t;

            float x0 = x - X0;
            float y0 = y - Y0;

            int i1, j1;
            if (x0 > y0)
            {
                i1 = 1;
                j1 = 0;
            }
            else
            {
                i1 = 0;
                j1 = 1;
            }

            float x1 = x0 - i1 + G2;
            float y1 = y0 - j1 + G2;
            float x2 = x0 - 1 + F2;
            float y2 = y0 - 1 + F2;

            float n0, n1, n2;

            t = (float)0.5 - x0 * x0 - y0 * y0;
            if (t < 0) n0 = 0;
            else
            {
                t *= t;
                n0 = t * t * Utils.GradCoord2D(seed, i, j, x0, y0);
            }

            t = (float)0.5 - x1 * x1 - y1 * y1;
            if (t < 0) n1 = 0;
            else
            {
                t *= t;
                n1 = t * t * Utils.GradCoord2D(seed, i + i1, j + j1, x1, y1);
            }

            t = (float)0.5 - x2 * x2 - y2 * y2;
            if (t < 0) n2 = 0;
            else
            {
                t *= t;
                n2 = t * t * Utils.GradCoord2D(seed, i + 1, j + 1, x2, y2);
            }

            return 50 * (n0 + n1 + n2);
        }

        public static float Perlin(int seed, float x, float y)
        {
            int x0 = Utils.Floor(x);
            int y0 = Utils.Floor(y);
            int x1 = x0 + 1;
            int y1 = y0 + 1;

            float xs, ys;

            xs = Utils.InterpQuinticFunc(x - x0);
            ys = Utils.InterpQuinticFunc(y - y0);


            float xd0 = x - x0;
            float yd0 = y - y0;
            float xd1 = xd0 - 1;
            float yd1 = yd0 - 1;

            float xf0 = Utils.Lerp(Utils.GradCoord2D(seed, x0, y0, xd0, yd0), Utils.GradCoord2D(seed, x1, y0, xd1, yd0), xs);
            float xf1 = Utils.Lerp(Utils.GradCoord2D(seed, x0, y1, xd0, yd1), Utils.GradCoord2D(seed, x1, y1, xd1, yd1), xs);

            return Utils.Lerp(xf0, xf1, ys);
        }

        public static float GetWhiteNoise(int seed, float x, float y)
        {
            int xi = Utils.FloatCast2Int(x);
            int yi = Utils.FloatCast2Int(y);

            return Utils.ValCoord2D(seed, xi, yi);
        }
    }
}
