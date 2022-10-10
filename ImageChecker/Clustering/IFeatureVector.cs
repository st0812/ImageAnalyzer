using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clustering
{
    public interface IFeatureVector<T>
    {
        double Distance(T other);
        T Add(T other);
        T Mutiply(double ratio);
    }
}
