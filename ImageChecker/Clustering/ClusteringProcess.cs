using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clustering
{
    public class ClusteringProcess<S, T> where T : IFeatureVector<T> where S : IClusteringState<S, T>
    {
        private IClusteringState<S, T> cluster;
        private int numberOfLoops;
        public ClusteringProcess(int numberOfLoops, IClusteringState<S, T> clustering)
        {
            this.cluster = clustering;
            this.numberOfLoops = numberOfLoops;
        }

        public IClusteringState<S, T> Learn()
        {
            for (int i = 0; i < numberOfLoops; i++)
            {
                var nextCluster = cluster.GenerateNextState();
                if (cluster.IsConverged(nextCluster)) return cluster;
                cluster = nextCluster;
            }
            return cluster;
        }
    }
}
