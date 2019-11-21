using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectileMotionWPF
{
    public static class MathHelper
    {
        public static float SinValueOfDegreeAngle(float angle)
        {
            return (float)Math.Sin(DegreeToRadian(angle));
        }
        public static float CosValueOfDegreeAngle(float angle)
        {
            return (float)Math.Cos(DegreeToRadian(angle));
        }
        public static float DegreeToRadian(float angle)
        {
            return (float)Math.PI * angle / 180.0f;
        }
    }
}
