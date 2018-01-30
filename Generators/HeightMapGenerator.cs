using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Generators
{
    internal static class HeightMapGenerator
    {
        public static void Generate(GraphicsDevice gd, float[][] arr, string algorithm = "")
        {
            try
            {
                int width = arr.Length;
                int height = arr.Length;

                if (!String.IsNullOrWhiteSpace(algorithm))
                    algorithm = algorithm + " - ";

                using (Texture2D image = new Texture2D(gd, width, height))
                {
                    var copy2D = arr.Select(a => a.ToArray()).ToArray();
                    var imgArr = ToOneDimentionalArray(PostModifications.Normalize(copy2D, width, 255));

                    image.SetData(ToGrayScale(imgArr, width, height));

                    if (!Directory.Exists("./HeightMaps/"))
                        Directory.CreateDirectory("./HeightMaps/");

                    using (Stream stream = File.Create("./HeightMaps/" + algorithm +
                                                       DateTime.Now.ToString("H;mm;ss") + ".png"))
                    {
                        image.SaveAsPng(stream, width, height);
                    }
                }
            }
            catch
            {
                
            }
        }

        private  static float[] ToOneDimentionalArray(float[][] arr)
        {
            float[] output = new float[arr.Length * arr.Length];

            int k = 0;
            for (int i = 0; i < arr.Length; i++)
            for (int j = 0; j < arr[i].Length; j++, k++)
                output[k] = arr[i][j];
            return output;
        }

        private static Color[] ToGrayScale(float[]arr, int width, int height)
        {
            Color[] heightMapColors = new Color[width * height];
            for (int i = 0; i < arr.Length; i++)
            {
                heightMapColors[i].R = (byte)arr[i];
                heightMapColors[i].G = (byte)arr[i];
                heightMapColors[i].B = (byte)arr[i];
                heightMapColors[i].A = 255;
            }

            return heightMapColors;
        }
    }
}
