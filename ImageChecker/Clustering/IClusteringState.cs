using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clustering
{
    public interface IClusteringState<S, T> where T : IFeatureVector<T> where S : IClusteringState<S, T>
    {
        S GenerateNextState();
        bool IsConverged(S other);
        T CalculateNearestCenterVector(T src);
    }
}
