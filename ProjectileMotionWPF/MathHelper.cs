using System;
using ProjectileMotionWPF.Data;
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
        public static double CalculateDragAtVelocity(double currentVelocity, InitialValues initialValues)
        {
            var drag = .5d *
                initialValues.DensityOfTheMedium *
                Math.Pow(currentVelocity, 2) *
                initialValues.CrossSection *
                initialValues.DragCoefficient;

            return drag;
        }
    }
}
