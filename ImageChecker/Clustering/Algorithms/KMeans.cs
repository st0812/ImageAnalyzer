using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clustering.Algorithms
{
    public class KMeansState<T> : IClusteringState<KMeansState<T>, T> where T : IFeatureVector<T>
    {
        public IEnumerable<T> CenterFeatures { get; }
        private IEnumerable<T> Features { get; }
        public KMeansState(IEnumerable<T> features, IEnumerable<T> centerFeatures)
        {
            CenterFeatures = centerFeatures;
            Features = features;
        }

        public KMeansState<T> GenerateNextState()
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

            return new KMeansState<T>(Features, newCenters.ToList());
        }

        public T CalculateNearestCenterVector(T src)
        {
            return CenterFeatures.MinBy(k => src.Distance(k)).First();
        }

        public bool IsConverged(KMeansState<T> other)
        {
            return CenterFeatures.All(feature => other.CenterFeatures.Contains(feature));
        }

        public static KMeansState<T> CreateKMeansInitialState(IEnumerable<T> features, int numberOfClusters, int seed)
        {
            var rand = new Random(seed);
            var centers = features.OrderBy(v => rand.Next()).Take(numberOfClusters);
            var cluster = new KMeansState<T>(features, centers.ToList());
            return cluster;
        }
    }
}
