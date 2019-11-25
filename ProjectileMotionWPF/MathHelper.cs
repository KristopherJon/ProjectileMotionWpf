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
                initialValues.CrossSectionArea *
                initialValues.DragCoefficient;

            return drag;
        }
        public static double CalculateTerminalVelocity(InitialValues initialValues)
        {
            //For the reference: https://www.grc.nasa.gov/WWW/K-12/airplane/termv.html

            var numerator = 2 * initialValues.Mass;
            var denominator = initialValues.DragCoefficient * initialValues.DensityOfTheMedium * initialValues.CrossSectionArea;

            return Math.Sqrt(numerator / denominator);
        }
        public static double Get_Y_PositionAfterDeltaTime(double current_Y_Position, double deltaTime, double Y_Velocity)
        {
            // v = s/t <=> s = v * t

            var deltaY = Y_Velocity * deltaTime;

            return current_Y_Position + deltaY;

        }
        public static double GetDeltaVelocity(double deltaTime, double netForce)
        {
            return deltaTime * netForce;
        }

    }
}
