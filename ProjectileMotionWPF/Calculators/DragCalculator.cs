using System;
using ProjectileMotionWPF.Data;

namespace ProjectileMotionWPF.Calculators
{
    public static class DragCalculator
    {
        public static double CalculateDragAtVelocity(double currentVelocity, InitialValues initialValues)
        {
            var drag = .5d *
                initialValues.DensityOfTheMedium *
                Math.Pow(currentVelocity, 2) *
                initialValues.CrossSectionArea *
                initialValues.DragCoefficient;

            return drag;
        }
    }
}
