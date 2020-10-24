using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Drawing;

namespace Lab1Filters
{
    class MedianFiltration
    {
        private Bitmap image;
        private Color[,] colors;
        private byte[][,] byteColors;
        private Bitmap result;

        public MedianFiltration(string path)
        {
            image = new Bitmap(path);
            ArrayInitialization();
            ByteArrayInitialization();
            
        }

        public MedianFiltration(Stream path)
        {
            image = new Bitmap(path);
            ArrayInitialization();
            ByteArrayInitialization();
        }

        private void ArrayInitialization()
        {
            colors = new Color[image.Width, image.Height];
            for(int i =0; i < colors.GetLength(0); i++)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    colors[i, j] = image.GetPixel(i, j);
                }
            }
        }

        private void ByteArrayInitialization()
        {
            byteColors = new byte[3][,];
            for (int k = 0; k < 3; k++)
            {
                byteColors[k] = new byte[image.Width, image.Height];
            }
            for (int i = 0; i < colors.GetLength(0); i++)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    byteColors[0][i, j] = colors[i,j].R;
                    byteColors[1][i, j] = colors[i,j].G;
                    byteColors[2][i, j] = colors[i,j].B;
                }
            }
        }

        private Color OneIncrementation(int windowWidth, int windowHeight)
        {
            int edgex = (int)Math.Ceiling(windowWidth / 2.0);
            int edgey = (int)Math.Ceiling(windowHeight / 2.0);
            for (int x = edgex; x < image.Width - edgex; x++)
            {
                for (int y = 0; y < image.Height - edgey; y++)
                {

                }
            }

            return new Color();
        }
    }

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

            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream("Lab1Filters.image.Lab1.jpg");
            MedianFiltration medianFiltration = new MedianFiltration(myStream);
            /*Bitmap img = new Bitmap(myStream);
            Console.WriteLine("{0}; {1}", img.Height, img.Width);
            Color color = img.GetPixel(959, 1279);
            byte r = color.R;
            byte g = color.G;
            byte b = color.B;
            byte a = color.A;
            string text = String.Format("Slate Blue has these ARGB values: Alpha:{0}, " +
                "red:{1}, green: {2}, blue: {3}", new object[] { a, r, g, b });
            Console.WriteLine(text);*/
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
