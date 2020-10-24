using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace Lab1Filters
{
    class MedianFiltration
    {
        private Bitmap image;
        private Color[,] colors;
        private Color[,] resultColors;
        private Bitmap resultBitmap;

        public MedianFiltration(string path, int windowWidth, int windowHeight)
        {
            image = new Bitmap(path);
            ColorArrayInitialization();
            resultColors = MethodImplementation(windowWidth, windowHeight);
            BitmapInitialization();
            resultBitmap.Save("Lab1Result.jpg", ImageFormat.Jpeg);
        }

        public MedianFiltration(Stream path, int windowWidth, int windowHeight)
        {
            image = new Bitmap(path);
            ColorArrayInitialization();
            resultColors = MethodImplementation(windowWidth, windowHeight);
            BitmapInitialization();
            resultBitmap.Save("Lab1Result.jpg", ImageFormat.Jpeg);
        }

        private void ColorArrayInitialization()
        {
            colors = new Color[image.Width, image.Height];
            for (int i = 0; i < colors.GetLength(0); i++)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    colors[i, j] = image.GetPixel(i, j);
                }
            }
        }

        private void BitmapInitialization()
        {
            resultBitmap = new Bitmap(resultColors.GetLength(0), resultColors.GetLength(1));
            for (int x = 0; x < resultBitmap.Width - 1; x++)
            {
                for (int y = 0; y < resultBitmap.Height - 1; y++)
                {
                    resultBitmap.SetPixel(x, y, resultColors[x, y]);
                }
            }
        }

        private Color[,] MethodImplementation(int windowWidth, int windowHeight)
        {
            Color[,] result = new Color[image.Width, image.Height];
            int edgex = (int)Math.Ceiling(windowWidth / 2.0);
            int edgey = (int)Math.Ceiling(windowHeight / 2.0);
            for (int x = edgex; x < image.Width - edgex; x++)
            {
                for (int y = edgey; y < image.Height - edgey; y++)
                {
                    Color[,] greyArray = new Color[windowWidth, windowHeight];
                    for (int fx = 0; fx < windowWidth; fx++)
                    {
                        for (int fy = 0; fy < windowHeight; fy++)
                        {
                            greyArray[fx, fy] = colors[x + fx - edgex, y + fy - edgey];
                        }
                    }
                    result[x, y] = GettingMedian(greyArray);
                }
            }

            return result;
        }

        private Color GettingMedian(Color[,] greyArray)
        {
            List<byte>[] byteColors = new List<byte>[3] {
                new List<byte>(), new List<byte>(), new List<byte>()
            };
            byte[] RGBvalues = new byte[3];
            for (int i = 0; i < greyArray.GetLength(0); i++)
            {
                for (int j = 0; j < greyArray.GetLength(1); j++)
                {
                    byteColors[0].Add(greyArray[i, j].R);
                    byteColors[1].Add(greyArray[i, j].G);
                    byteColors[2].Add(greyArray[i, j].B);
                }
            }
            for (int k = 0; k < byteColors.Length; k++)
            {
                byteColors[k].Sort();
                RGBvalues[k] = byteColors[k][byteColors[k].Count / 2];
            }
            Color resColor = Color.FromArgb(255, RGBvalues[0], RGBvalues[1], RGBvalues[2]);
            return resColor;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream("Lab1Filters.image.Lab1.jpg");
            MedianFiltration medianFiltration = new MedianFiltration(myStream, 10, 10);
            Console.WriteLine("Фильтрация завершена");
            Console.ReadLine();
        }
    }
}
