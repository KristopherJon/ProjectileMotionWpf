using System;
using ProjectileMotionWPF.Data;

namespace ProjectileMotionWPF.Calculators
{
    public static class TerminalVelocityCalculator
    {
        public static double CalculateTerminalVelocity(InitialValues initialValues)
        {
            //For the reference: https://www.grc.nasa.gov/WWW/K-12/airplane/termv.html

            var numerator = 2 * initialValues.Mass;
            var denominator = initialValues.DragCoefficient * initialValues.DensityOfTheMedium * initialValues.CrossSectionArea;

            return Math.Sqrt(numerator / denominator);
        }
    }
}
