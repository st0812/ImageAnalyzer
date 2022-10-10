using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ImageChecker.Models
{
    public class Phase : IEquatable<Phase>
    {
        public float Value { get; }
        public Phase(float value)
        {
            var rotation = Math.Floor(value / 360);
            var phase = value - rotation * 360;
            var positivePhase = (phase >= 0) ? phase : phase + 360;
            Value = (float)positivePhase;
        }
        public bool Equals(Phase other)
        {
            return Value.Equals(other.Value);
        }

        public Phase Add(Phase other)
        {
            var sin = (float)Math.Sin(Math.PI * Value / 180.0) + (float)Math.Sin(Math.PI * other.Value / 180.0);
            var cos = (float)Math.Cos(Math.PI * Value / 180.0) + (float)Math.Cos(Math.PI * other.Value / 180.0);
            var phase = Math.Atan2(sin, cos) * 180.0 / Math.PI;
            return new Phase((float)phase);
        }

        public double Distance(Phase other)
        {
            double H1 = Value;
            double H2 = other.Value;
            if (H2 > H1)
            {
                (H1, H2) = (H2, H1);
            }
            return Math.Sin(Math.PI * (H1 - H2) / 180.0 / 2);
        }

    }

    public class HSVFeature : IFeature<HSVFeature>, IEquatable<HSVFeature>
    {
        public Phase H { get; }
        public float S { get; }
        public float V { get; }

        public HSVFeature(Phase h, float s, float v)
        {
            H = h;
            S = s;
            V = v;
        }
        public override string ToString()
        {
            return  "H:" + ((int)H.Value).ToString() + ",S:" + ((int)S).ToString() + ",V:" + ((int)V).ToString();

        }
        public Color Color
        {
            get
            {
                var H = this.H.Value;
                var S = this.S / 100;
                var V = this.V / 100;

                float R = 0.0F;
                float G = 0.0F;
                float B = 0.0F;
                if (S == 0.0)
                {
                    R = G = B = V * 255;
                    return Color.FromArgb((byte)R, (byte)G, (byte)B);
                }

                int Hd = (int)Math.Floor(H / 60.0);
                float F = (float)(H / 60 - Hd);
                float M = V * (1 - S) * 255;
                float N = V * (1 - S * F) * 255;
                float K = V * (1 - S * (1 - F)) * 255;

                switch (Hd)
                {
                    case 0:
                        R = V * 255;
                        G = K;
                        B = M;
                        break;
                    case 1:
                        R = N;
                        G = V * 255;
                        B = M;
                        break;
                    case 2:
                        R = M;
                        G = V * 255;
                        B = K;
                        break;
                    case 3:
                        R = M;
                        G = N;
                        B = V * 255;
                        break;
                    case 4:
                        R = K;
                        G = M;
                        B = V * 255;
                        break;
                    case 5:
                        R = V * 255;
                        G = M;
                        B = N;
                        break;
                }
                return Color.FromArgb((byte)R, (byte)G, (byte)B);
            }
        }
        public HSVFeature Add(HSVFeature other)
        {

            return new HSVFeature(H.Add(other.H), S + other.S, V + other.V);
        }


        public double Distance(HSVFeature other)
        {
            return Math.Pow(H.Distance(other.H), 2.0) * 10000 + Math.Pow(S - other.S, 2.0) + Math.Pow(V - other.V, 2.0);
        }

        public bool Equals(HSVFeature other)
        {
            return H == other.H && V == other.V && S == other.S;
        }

        public HSVFeature Mutiply(double ratio)
        {

            return new HSVFeature(new Phase(H.Value), S * (float)ratio, V * (float)ratio);
        }
        public static HSVFeature RGBtoHSV(Color src)
        {
            var R = src.R;
            var G = src.G;
            var B = src.B;

            var MAX = new List<float> { R, G, B }.Max();
            var MIN = new List<float> { R, G, B }.Min();


            float H = 0.0F; ;
            if (R == MAX)
            {
                H = (G - B) / (MAX - MIN) * 60;
            }
            else if (G == MAX)
            {
                H = (B - R) / (MAX - MIN) * 60 + 120;
            }
            else if (B == MAX)
            {
                H = (R - G) / (MAX - MIN) * 60 + 120;
            }

            if (H < 0) H += 360;
            var S = (MAX - MIN) / MAX * 100;

            var V = MAX / 255 * 100;

            return new HSVFeature(new Phase(H), S, V);
        }


    }


    
}
