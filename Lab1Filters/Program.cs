using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Threading;

namespace Lab1Filters
{
    class MedianFilter
    {
        private Color[,] colorMatrix;
        private Size windowSize;
        private Size imageSize;
        private Point edge;

        private int numberOfThreads;

        public MedianFilter(Bitmap image, Size windowSize, int numberOfThreads)
        {
            this.imageSize = image.Size;
            this.colorMatrix = ColorArrayBitmapConverter.ConvertToColorArray(image);
            this.windowSize = windowSize;
            this.numberOfThreads = numberOfThreads;

            int edgex = (int)Math.Ceiling(this.windowSize.Width / 2.0);
            int edgey = (int)Math.Ceiling(this.windowSize.Height / 2.0);

            this.edge = new Point(edgex, edgey);
        }

        public Bitmap Filter()
        {
            Color[,] resultColorMatrix = new Color[imageSize.Width, imageSize.Height];
            Point[] points = new Point[imageSize.Width * imageSize.Height];

            for (int x = 0, k = 0; x < imageSize.Width; x++)
            {
                for (int y = 0; y < imageSize.Height; y++, k++)
                {
                    Point point = new Point(x, y);
                    points[k] = point;
                }
            }

            int pointsPerThread = points.Length / numberOfThreads;
            Task[] threads = new Task[numberOfThreads];
            for (int i = 0; i < numberOfThreads; i++)
            {
                Point[] threadPoints = new Point[pointsPerThread];
                Array.Copy(points, i * pointsPerThread, threadPoints, 0, pointsPerThread);
                threads[i] = Task.Run(() =>
                {
                    for (int j = 0; j < threadPoints.Length; j++)
                    {
                        Pixel pixel = GetNewPixel(threadPoints[j]);
                        resultColorMatrix[pixel.Point.X, pixel.Point.Y] = pixel.Color;
                    }

                });
            }

            Task.WaitAll(threads);

            return ColorArrayBitmapConverter.ConvertToBitmap(resultColorMatrix);
        }

        private Pixel GetNewPixel(Point point)
        {
            List<Color> colorList = new List<Color>(windowSize.Width * windowSize.Height);
            for (int fx = 0; fx < windowSize.Width; fx++)
            {
                for (int fy = 0; fy < windowSize.Height; fy++)
                {
                    if (InMatrix(point.X + fx - edge.X, point.Y + fy - edge.Y))
                        colorList.Add(colorMatrix[point.X + fx - edge.X, point.Y + fy - edge.Y]);
                }
            }
            return new Pixel(GetMedian(colorList), point);
        }

        private Color GetMedian(List<Color> colorArray)
        {
            List<byte>[] byteColors = new List<byte>[3] {
                new List<byte>(), new List<byte>(), new List<byte>()
            };
            byte[] RGBvalues = new byte[3];
            colorArray.ForEach(color =>
            {
                byteColors[0].Add(color.R);
                byteColors[1].Add(color.G);
                byteColors[2].Add(color.B);
            });

            for (int k = 0; k < byteColors.Length; k++)
            {
                byteColors[k].Sort();
                RGBvalues[k] = byteColors[k][byteColors[k].Count / 2];
            }
            Color resColor = Color.FromArgb(255, RGBvalues[0], RGBvalues[1], RGBvalues[2]);
            return resColor;
        }

        public bool InMatrix(int indexX, int indexY)
        {
            return indexX > -1 && indexY > -1 && indexX < colorMatrix.GetLength(0) && indexY < colorMatrix.GetLength(1);
        }
    }

    class ColorArrayBitmapConverter
    {
        public static Bitmap ConvertToBitmap(Color[,] array)
        {
            Bitmap resultBitmap = new Bitmap(array.GetLength(0), array.GetLength(1));
            for (int x = 0; x < resultBitmap.Width - 1; x++)
            {
                for (int y = 0; y < resultBitmap.Height - 1; y++)
                {
                    resultBitmap.SetPixel(x, y, array[x, y]);
                }
            }

            return resultBitmap;
        }

        public static Color[,] ConvertToColorArray(Bitmap image)
        {
            Color[,] colors = new Color[image.Width, image.Height];
            for (int i = 0; i < colors.GetLength(0); i++)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    colors[i, j] = image.GetPixel(i, j);
                }
            }

            return colors;
        }
    }

    class Pixel
    {
        private Color color;
        private Point point;

        public Pixel(Color color, Point point)
        {
            Color = color;
            Point = point;
        }

        public Color Color { get => color; set => color = value; }
        public Point Point { get => point; set => point = value; }
    }

    class Program
    {
        private static Stopwatch stopwatch = new Stopwatch();

        static void Main(string[] args)
        {
            int numberOfThreads = 4;
            int windowSize = 10;
            Bitmap image = importBitmapFromManifest("Lab1Filters.image.Lab1.jpg");
            if (args.Length > 0)
            {
                numberOfThreads = int.Parse(args[0]);
                if (args.Length > 1)
                {
                    windowSize = int.Parse(args[1]);
                }
            }

            stopwatch.Start();
            MedianFilter medianFilter = new MedianFilter(image, new Size(windowSize, windowSize), numberOfThreads);
            Bitmap output = medianFilter.Filter();
            stopwatch.Stop();
            output.Save("Lab1Output.jpg", ImageFormat.Jpeg);

            Console.WriteLine("Фильтрация {0} потоками и окном размером в {1} пикселей завершена за {2} ms.",
                numberOfThreads, windowSize, stopwatch.Elapsed.TotalMilliseconds);
            Console.ReadLine();
        }

        static Bitmap importBitmapFromManifest(string manifestPath)
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Stream imageStream = myAssembly.GetManifestResourceStream(manifestPath);
            return new Bitmap(imageStream);
        }
    }
}
