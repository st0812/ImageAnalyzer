using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageChecker.Models
{
    public class HueFeatureFilter : IFeatureFilter<HSVFeature>
    {
        public double HueStart
        {
            get; 
        }

        public double HueEnd
        {
            get; 
        }

        public HueFeatureFilter(double hueStart, double hueEnd)
        {
            HueStart = hueStart;
            HueEnd = hueEnd;
        }

        public bool IsPassed(HSVFeature feature)
        {
            var h = feature.H.Value;
            if (this.HueStart < this.HueEnd)
            {
                return (this.HueStart < h) && (h < this.HueEnd);
            }
            else
            {
                return (h < this.HueEnd) || (this.HueStart < h);
            }
        }
    }

    public class SaturationFeatureFileter : IFeatureFilter<HSVFeature>
    {
        public double SaturationStart
        {
            get; 
        }

        public double SaturationEnd
        {
            get; 
        }

        public SaturationFeatureFileter(double saturationStart,double saturationEnd)
        {
            SaturationStart = saturationStart;
            SaturationEnd = saturationEnd;
        }


        public bool IsPassed(HSVFeature feature)
        {
            return (this.SaturationStart < feature.S) && (feature.S < this.SaturationEnd);
        }
    }

    public class ValueFeatureFilter : IFeatureFilter<HSVFeature>
    {
        public double ValueStart
        {
            get; 
        }

        public double ValueEnd
        {
            get; 
        }

        public ValueFeatureFilter(double valueStart,double valueEnd)
        {
            ValueStart = valueStart;
            ValueEnd = valueEnd;
        }

        public bool IsPassed(HSVFeature feature)
        {
            return (this.ValueStart < feature.V) && (feature.V < this.ValueEnd);
        }

    }
}
