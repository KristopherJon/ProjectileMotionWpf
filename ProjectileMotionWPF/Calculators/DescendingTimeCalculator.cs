using System;
using ProjectileMotionWPF.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectileMotionWPF.Calculators
{
    class DescendingTimeCalculator
    {
        public static double CalculateDescendingTime(InitialValues initialValues, double Y_Maximum)
        {
            var time = 0.0d;
            var timeStep = 0.001d;
            var Y_Velocity = 0.000d;
            var Y_Position = Y_Maximum;

            while (Y_Position >= 0)
            {
                var dragAtVelocity = DragCalculator.CalculateDragAtVelocity(Y_Velocity, initialValues);
                var netForce = initialValues.Gravity - dragAtVelocity;

                Y_Velocity += DeltaVelocityCalculator.CalculateDeltaVelocity(timeStep, netForce);
                Y_Position -= DeltaPositionCalculator.GetDeltaPositionAfterDeltaTime(timeStep, Y_Velocity);

                time += timeStep;
            }

            return time;
        }
    }
}
