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
        private Bitmap result;

        MedianFiltration(string path)
        {
            image = new Bitmap(path);
        }

        MedianFiltration(Stream path)
        {
            image = new Bitmap(path);
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
    }

    class Program
    {
        static void Main(string[] args)
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream("Lab1Filters.image.Lab1.jpg");
            Bitmap img = new Bitmap(myStream);
            Console.WriteLine("{0}; {1}", img.Height, img.Width);
            Color color = img.GetPixel(959, 1279);
            byte r = color.R;
            byte g = color.G;
            byte b = color.B;
            byte a = color.A;
            string text = String.Format("Slate Blue has these ARGB values: Alpha:{0}, " +
                "red:{1}, green: {2}, blue: {3}", new object[] { a, r, g, b });
            Console.WriteLine(text);
            Console.ReadLine();
        }
    }
}
