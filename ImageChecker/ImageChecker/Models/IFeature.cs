using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageChecker.Models
{
    public interface IFeature<T>
    {
        double Distance(T other);
        T Add(T other);
        T Mutiply(double ratio);
    }
}
    
