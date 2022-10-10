using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageChecker.Models
{
    public class KMeansHSVProcessor : IImageProcessor
    {
        private FeatureFilters<HSVFeature> Filters;
        private int NumberOfClusters;
        private int NumberOfLoops;
        private int Seed;

        public KMeansHSVProcessor(FeatureFilters<HSVFeature> filters, int numberOfClusters, int numberOfLoops, int seed)
        {
            Filters = filters;
            NumberOfClusters = numberOfClusters;
            NumberOfLoops = numberOfLoops;
            Seed = seed;
        }

        public (Bitmap, IEnumerable<Color>) BeginProcess(Bitmap src)
        {
            var features = src.Flatten().Select(pixel => HSVFeature.RGBtoHSV(pixel)).ToList();
            var learningFeatures = features.Where(feature => Filters.IsPassed(feature)).ToList();

            var kmean = KMeans<HSVFeature>.CreateKMeansState(learningFeatures, NumberOfClusters, Seed);
            var process = new ClusteringProcess<KMeans<HSVFeature>, HSVFeature>(NumberOfLoops, kmean);
            var result = process.Learn();

            var dstFeatures = features.Select(feature => Filters.IsPassed(feature) ? result.Fit(feature) : new HSVFeature(new Phase(0),0,0)).Select(feature => feature.Color);
            var colors = (result as KMeans<HSVFeature>).CenterFeatures.Select(feature => feature.Color);
            return (FeatureExtension.Reshape(dstFeatures, src.Width, src.Height), colors);
        }
    }
}
