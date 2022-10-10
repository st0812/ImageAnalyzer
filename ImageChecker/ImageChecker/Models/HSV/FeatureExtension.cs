using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageChecker.Models
{
    public static class FeatureExtension
    {
        public static T Average<T>(this IEnumerable<T> source) where T : IFeature<T>
        {
            T feature = source.FirstOrDefault();
            foreach (var f in source.Skip(1))
            {
                feature = feature.Add(f);
            }
            feature = feature.Mutiply(1.0 / source.Count());
            return feature;
        }

        public static IEnumerable<Color> Flatten(this Bitmap source)
        {
            List<Color> pixels = new List<Color>();
            for (int x = 0; x < source.Width; x++)
            {
                for (int y = 0; y < source.Height; y++)
                {
                    pixels.Add(source.GetPixel(x, y));
                }
            }
            return pixels;
        }

        public static Bitmap Reshape(IEnumerable<Color> source, int width, int height)
        {
            var pixels = source.ToList();
            Bitmap dst = new Bitmap(width, height, PixelFormat.Format32bppRgb);
            int count = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    dst.SetPixel(x, y, pixels[count++]);
                }
            }
            return dst;
        }
    }
}
