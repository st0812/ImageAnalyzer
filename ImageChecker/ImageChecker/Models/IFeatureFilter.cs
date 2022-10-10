using Clustering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageChecker.Models
{
    public interface IFeatureFilter<T> where T : IFeatureVector<T>
    {
        bool IsPassed(T feature);
    }

    public class FeatureFilters<T> where T : IFeatureVector<T>
    {
        private IEnumerable<IFeatureFilter<T>> Filters;
        public FeatureFilters(IEnumerable<IFeatureFilter<T>> filters)
        {
            Filters = filters;
        }

        public bool IsPassed(T feature)
        {
            return Filters.All(filter => filter.IsPassed(feature));
        }
    }

   
}
