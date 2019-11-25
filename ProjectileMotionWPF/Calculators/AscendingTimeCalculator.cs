using System;
using ProjectileMotionWPF.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectileMotionWPF.Calculators
{
    public static class AscendingTimeCalculator
    {
        /// <summary>
        /// Calculates time it takes the projectile's speed to reach a number "close" to zero.
        /// </summary>
        public static double CalculateAscendingTime(InitialValues initialValues, out double maximumY)
        {
            var time = 0.0d;
            var deltaTime = 0.001d;
            var Y_Velocity = initialValues.InitialVelocityY;
            var Y_Position = 0d;

            while (Y_Velocity >= 0.001d)
            {
                var dragAtVelocity = DragCalculator.CalculateDragAtVelocity(Y_Velocity, initialValues);
                var netForce = initialValues.Gravity + dragAtVelocity;

                Y_Velocity -= DeltaVelocityCalculator.CalculateDeltaVelocity(deltaTime, netForce);
                Y_Position += DeltaPositionCalculator.GetDeltaPositionAfterDeltaTime(deltaTime, Y_Velocity);

                time += deltaTime;
            }

            maximumY = Y_Position;

            return time;
        }
    }
}
