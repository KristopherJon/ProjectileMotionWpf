using System;
using ProjectileMotionWPF.Data;

namespace ProjectileMotionWPF.Calculators
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
