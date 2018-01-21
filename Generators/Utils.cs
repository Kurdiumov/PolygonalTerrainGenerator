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

        public static int Sqrt(int number)
        {
            return (int)Math.Sqrt(number);
        }

        public static float[][] GetEmptyArray(int width, int height, float initValue = 0)
        {
            var arr = new float[width][];
            for (int i = 0; i < width; i++)
            {
                arr[i] = new float[height];
                if (initValue != 0)
                    for (int j = 0; j < width; j++)
                        arr[i][j] = initValue;
            }
            return arr;
        }

        public static float Interpolate(float x0, float x1, float alpha)
        {
            return x0 * (1 - alpha) + alpha * x1;
        }

        public static bool IsPowerOfTwo(int val)
        {
            return (val != 0) && (val & (val - 1)) == 0;
        }
    }
}
