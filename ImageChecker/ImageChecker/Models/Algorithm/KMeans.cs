using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ImageChecker.Models
{
    public class KMeans<T> : IClustering<KMeans<T>, T> where T : IFeature<T>
    {
        public IEnumerable<T> CenterFeatures { get; }
        private IEnumerable<T> Features { get; }
        public KMeans(IEnumerable<T> features, IEnumerable<T> centerFeatures)
        {
            CenterFeatures = centerFeatures;
            Features = features;
        }

        public KMeans<T> Next()
        {
            var dic = new Dictionary<T, List<T>>();
            foreach (var center in CenterFeatures)
            {
                dic.Add(center, new List<T>());
            }

            foreach (var feature in Features)
            {
                var nearest = dic.Keys.MinBy(k => feature.Distance(k)).First();
                dic[nearest].Add(feature);
            }

            var newCenters = dic.Values.Select(fs => fs.Average<T>());

            return new KMeans<T>(Features, newCenters.ToList());
        }

        public T Fit(T src)
        {
            return CenterFeatures.MinBy(k => src.Distance(k)).First();
        }

        public static KMeans<T> CreateKMeansState(IEnumerable<T> features, int numberOfClusters, int seed)
        {
            var rand = new Random(seed);
            var centers = features.OrderBy(v => rand.Next()).Take(numberOfClusters);
            var cluster = new KMeans<T>(features, centers.ToList());
            return cluster;
        }

        public bool IsConverged(KMeans<T> other)
        {
            return CenterFeatures.All(feature => other.CenterFeatures.Contains(feature));
        }
    }
}
