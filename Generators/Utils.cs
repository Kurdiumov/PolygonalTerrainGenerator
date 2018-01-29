using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        public static int Floor(float x)
        {
            return (x > 0) ? ((int)x) : (((int)x) - 1);
        }


        public static int Round(float f)
        {
            return (f >= 0) ? (int)(f + (float)0.5) : (int)(f - (float)0.5);
        }

        public static float InterpQuinticFunc(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        public  static float Lerp(float a, float b, float t)
        {
            return a + t * (b - a);
        }

        public static float FindMin(float[][] arr)
        {
            var min = arr[0][0];
            for(var i = 0; i < arr.Length; i++)
            for (var j = 0; j < arr[i].Length; j++)
                if (arr[i][j] < min)
                    min = arr[i][j];
            return min;
        }
        public static float FindMax(float[][] arr)
        {
            var max = arr[0][0];
            for (var i = 0; i < arr.Length; i++)
            for (var j = 0; j < arr[i].Length; j++)
                if (arr[i][j] > max)
                    max = arr[i][j];
            return max;
        }

        public static float[][] ShiftTerrain(float[][] arr)
        {
            var difference = FindMax(arr) - FindMin(arr);
            for (var i = 0; i < arr.Length; i++)
            for (var j = 0; j < arr[i].Length; j++)
                arr[i][j] -= difference;

            return arr;
        }
    }
}
