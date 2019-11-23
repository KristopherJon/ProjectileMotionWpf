using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectileMotionWPF
{
    public static class MathHelper
    {
        public static double SinValueOfDegreeAngle(double angle)
        {
            return Math.Sin(DegreeToRadian(angle));
        }
        public static double CosValueOfDegreeAngle(double angle)
        {
            return Math.Cos(DegreeToRadian(angle));
        }
        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0f;
        }
    }
}
