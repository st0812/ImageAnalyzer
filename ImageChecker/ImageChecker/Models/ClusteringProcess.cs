using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ImageChecker.Models
{
    public class ClusteringProcess<S,T> where T : IFeature<T> where S:IClustering<S,T>
    {
        private IClustering<S, T> cluster;
        private int numberOfLoops;
        public ClusteringProcess(int numberOfLoops,IClustering<S,T> clustering)
        {
            this.cluster = clustering;
            this.numberOfLoops = numberOfLoops;
        }

        public IClustering<S,T> Learn()
        {
            for (int i = 0; i < numberOfLoops; i++)
            {
                var nextCluster = cluster.Next();
                if (cluster.IsConverged(nextCluster)) return cluster;
                cluster = nextCluster;
            }
            return cluster;
        }

    }

    public interface IClustering<S,T> where T : IFeature<T>
    { 
        S Next();
        bool IsConverged(S other);
        T Fit(T src);
    }
    
}
