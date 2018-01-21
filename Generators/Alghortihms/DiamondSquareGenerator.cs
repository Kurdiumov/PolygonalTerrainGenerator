using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Generators
{
    public class DiamondSquareGenerator : IGenerator
    {
        private readonly GraphicsDevice _graphicDevice;
        private readonly GraphicsDeviceManager _graphicDeviceManeger;

        private readonly Random rand = new Random();

        public float Height = 10;
        public float Displacement = 300;
        public int Iterations = 9;

        public DiamondSquareGenerator(GraphicsDevice graphicDevice, GraphicsDeviceManager graphics)
        {
            _graphicDevice = graphicDevice;
            _graphicDeviceManeger = graphics;
        }

        public IGameObject Generate(float offsetX = 0, float offsetY = 0)
        {
            var arr = Utils.GetEmptyArray(2, 2, -1);

            for (var i = 0; i < arr.Length; i++)
                for (var j = 0; j < arr.Length; j++)
                    arr[i][j] = GetRandom();


            for (int i = 1; i < Iterations; i++)
            {
                var Divided = DivideToSquares(arr);

                var squaredArray = new List<float[][]>();
                for (int j = 0; j < Divided.Count; j++)
                {
                    var div = Square(Divided.ElementAt(j));
                    squaredArray.Add(div);
                }

                var middedArray = new List<float[][]>();
                for (int j = 0; j < squaredArray.Count; j++)
                {
                    var mid = Mid(squaredArray.ElementAt(j));
                    middedArray.Add(mid);
                }

                var diamondedArray = new List<float[][]>();
                for (int j = 0; j < middedArray.Count; j++)
                {
                    var diamond = Diamond(middedArray.ElementAt(j));
                    diamondedArray.Add(diamond);
                }
                arr = ConnectArrays(diamondedArray);
                Displacement /= 2;
            }

            return new PrimitiveBase(_graphicDevice, _graphicDeviceManeger, arr, arr.Length, offsetX, offsetY);
        }


        private float[][] Square(float[][] input)
        {
            input = ExpandSquare(input);
            return Mid(input);
        }

        private float[][] Diamond(float[][] input)
        {
            var sizeInRow = input.Length;

            for (int i = 0; i < sizeInRow; i++)
            {
                for (int j = 0; j < sizeInRow; j++)
                {
                    if (input[i][j] == -1)
                    {
                        if (i == 0)
                            input[i][j] = (input[i][j - 1] + input[i][j + 1] + input[i + 1][j]) / 3;
                        else if (i == sizeInRow - 1)
                            input[i][j] = (input[i][j - 1] + input[i][j + 1] + input[i - 1][j]) / 3;
                        else if (j == 0)
                            input[i][j] = (input[i][j + 1] + input[i - 1][j] + input[i + 1][j]) / 3;
                        else if (j == sizeInRow - 1)
                            input[i][j] = (input[i][j - 1] + input[i - 1][j] + input[i + 1][j]) / 3;
                        else
                            input[i][j] = (input[i][j - 1] + input[i][j + 1] + input[i - 1][j] + input[i + 1][j]) / 4;

                    }
                }
            }


            return input;
        }

        private float[][] Mid(float[][] input)
        {
            if (input.Length != 3 || input[0].Length != 3 || input[1].Length != 3 || input[2].Length != 3)
                throw new ArgumentException("Cant Mid non 3 dimension array");

            var average = (input[0][0] + input[0][2] + input[2][0] + input[2][2]) / 4;
            input[1][1] = average + GetDisplacement();

            return input;
        }

        private float[][] ExpandSquare(float[][] input)
        {
            if (input.Length != 2 || input[0].Length != 2 || input[1].Length != 2)
                throw new ArgumentException("Cant Expand non Diamond");

            float[][] output = Utils.GetEmptyArray(3, 3, -1);

            output[0][0] = input[0][0];
            output[0][2] = input[0][1];
            output[2][0] = input[1][0];
            output[2][2] = input[1][1];

            return output;
        }

        private List<float[][]> DivideToSquares(float[][] input)
        {
            if (input.Length < 2)
                throw new ArgumentException("Too low number of vertices to divide to square");

            List<float[][]> SquaresList = new List<float[][]>();
            var numberOfSqaures = Utils.Square(Utils.Sqrt(input.Length * input.Length) - 1);
            var numberInRow = Utils.Sqrt((int)numberOfSqaures);

            for (int i = 0; i < numberOfSqaures; i++)
                SquaresList.Add(Utils.GetEmptyArray(2, 2, -3));

            int square = 0;
            for (var i = 0; i < numberInRow; i++)
            {
                for (var j = 0; j < numberInRow; j++)
                {
                    SquaresList.ElementAt(square)[0][0] = input[i][j];
                    SquaresList.ElementAt(square)[0][1] = input[i][j + 1];
                    SquaresList.ElementAt(square)[1][0] = input[i + 1][j];
                    SquaresList.ElementAt(square)[1][1] = input[i + 1][j + 1];

                    square++;
                }
            }

            return SquaresList;
        }

        private float[][] ConnectArrays(List<float[][]> ListOfSquares)
        {
            var numberOfSquares = ListOfSquares.Count; //can be 4, 9, 16, 25, 36, 49, 64, 81, 100 etc

            if (!Utils.IsPowerOfTwo(numberOfSquares))
                throw new ArgumentException("number of squares should be power of two!");

            var NumberInRow = 3;
            var DestinationNuberInRow = (Utils.Sqrt(numberOfSquares) * 2) + 1;

            var output = Utils.GetEmptyArray(DestinationNuberInRow, DestinationNuberInRow, -2);


            if (DestinationNuberInRow == 3)
            {
                for (int i = 0; i < DestinationNuberInRow; i++)
                {
                    for (int j = 0; j < DestinationNuberInRow; j++)
                    {
                        output[i][j] = ListOfSquares.First()[i][j];

                    }

                }
            }
            else
            {
                int k = 0;
                for (int i = 0; i < DestinationNuberInRow; i++)
                {
                    if (i % 2 == 0 && i != 0 && i != DestinationNuberInRow -1 )
                        k+= Utils.Sqrt(numberOfSquares);

                    int l = k;
                    for (int j = 0; j < DestinationNuberInRow; j++)
                    {
                        if (j != 0 && j != DestinationNuberInRow - 1 && j % 2 == 0)
                            l++;
                        if (l >= numberOfSquares)
                            throw new ArgumentException("L is more that number of squares");

                        var tmpI = i;
                        var tmpJ = j;

                        while (tmpI >= 2)
                            tmpI -= 2;

                        while (tmpJ >= 2)
                            tmpJ -= 2;
                        if (tmpI == 0 && i == DestinationNuberInRow - 1)
                            tmpI = 2;

                        if (tmpJ == 0 && j == DestinationNuberInRow - 1)
                            tmpJ = 2;


                        output[i][j] = ListOfSquares.ElementAt(l)[tmpI][tmpJ];

                    }

                }
            }

            return output;

        }


        private float GetRandom()
        {
            return (float)rand.NextDouble() * Height;
        }

        private float GetDisplacement()
        {
            return ((float)rand.NextDouble() * Displacement)- (Displacement/2);
        }
    }
}

