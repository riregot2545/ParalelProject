using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Lab1Filters
{
    class Program
    {
       static void Main(string[] args)
        {
            string path = "image\\Lab1.jpg";
            ImageReader reader = new ImageReader();
            Bitmap image = reader.importFromFile(path);
            MatrixCutter matrixCutter = new MatrixCutter(image);

            Color[][] square = matrixCutter.cutSquareFromImage(0, 0, 9);

            Console.ReadLine();

        }


        class ImageReader
        {
            public Bitmap importFromFile(string path)
            {
                return new Bitmap(path);
            }
        }

        class MatrixCutter
        {
            private Bitmap image;

            public MatrixCutter(Bitmap image)
            {
                this.image = image;
            }

            //x,y - coordinates of CENTER corner of square
            public Color[][] cutSquareFromImage(int x, int y, int size)
            {
                Color[][] matrix = new Color[size][];
                for (int i = 0; i < size; i++)
                {
                    matrix[i] = new Color[size];
                }

                int topLeftX = x - size / 2;
                int topLeftY = y - size / 2;

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        int xCord = topLeftX + i;
                        int yCord = topLeftY + j;
                        if (Utils.inImage(xCord, yCord, image))
                        {
                            matrix[i][j] = image.GetPixel(i, j);
                        }
                        else
                        {
                            matrix[i][j] = Color.Black;
                        }
                    }
                }

                return matrix;

            }
        }

        class Utils
        {
            public static bool inImage(int indexX, int indexY, Bitmap image)
            {
                return indexX > -1 && indexY > -1 && indexX < image.Width && indexY < image.Height;
            }
        }
    }
}
