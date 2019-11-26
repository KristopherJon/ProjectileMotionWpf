using System;
using ProjectileMotionWPF.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProjectileMotionWPF.Calculators
{
    class DescendingTimeCalculator
    {
        public static double CalculateDescendingTime(InitialValues initialValues, double Y_Maximum, CheckBox terminalVelocityCheckbox)
        {
            var time = 0.0d;
            var timeStep = 0.001d;
            var Y_Velocity = 0.000d;
            var Y_Position = Y_Maximum;
            terminalVelocityCheckbox.IsChecked = false;

            while (Y_Position >= 0d)
            {
                var dragAtVelocity = DragCalculator.CalculateDragAtVelocity(Y_Velocity, initialValues);
                var dragInducedAcceleration = dragAtVelocity / initialValues.Mass;
                
                var netAcceleration = initialValues.Gravity - dragInducedAcceleration;

                Y_Velocity += DeltaVelocityCalculator.CalculateDeltaVelocity(timeStep, netAcceleration);
                Y_Position -= DeltaPositionCalculator.GetDeltaPositionAfterDeltaTime(timeStep, Y_Velocity);

                time += timeStep;

                if (Y_Velocity >= TerminalVelocityCalculator.CalculateTerminalVelocity(initialValues))
                {
                    terminalVelocityCheckbox.IsChecked = true;
                }
            }

            return time;
        }
    }
}
