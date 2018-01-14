using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generators
{
    public static class Utils
    {
        public static float Square(float number)
        {
            return number * number;
        }

        public static float[][] GetEmptyArray(int width, int height)
        {
            var arr = new float[width][];
            for (int i = 0; i < width; i++)
            {
                arr[i] = new float[height];
            }
            return arr;
        }

        public static float Interpolate(float x0, float x1, float alpha)
        {
            return x0 * (1 - alpha) + alpha * x1;
        }
    }
}
