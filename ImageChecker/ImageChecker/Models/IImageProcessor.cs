using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageChecker.Models
{
    interface IImageProcessor
    {
        (Bitmap, IEnumerable<Color>) BeginProcess(Bitmap src);
    }
}
