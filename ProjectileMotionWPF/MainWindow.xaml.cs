using System;
using System.Windows;
using ProjectileMotionWPF.Data;
using ProjectileMotionWPF.Calculators;
using ProjectileMotionWPF.Updaters;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace ProjectileMotionWPF
{
    public partial class MainWindow : Window
    {
        //private Position chartStartingPoint = new Position(250, 800 - 100);
        
        private int CurrentIteration;
        
        private InitialValues initialValues;
        
        private SpaceTimePoint[] spaceTimePoints;

        private double maximumX;
        private double maximumY;
        private double timeTotal;
        private double deltaTime; //chart time step

        public MainWindow()
        {
            InitializeComponent();
        }
        private void InitializeStartingValues()
        {
            initialValues = new InitialValues
            {
                Mass = (double)ProjectilesMassBox.Value,
                Gravity = (double)GravityBox.Value,
                DragCoefficient = 0.47,
                CrossSectionArea = Math.Pow((double)ProjectilesRadiusBox.Value, 2) * Math.PI,
                InitialVelocityX = (double)InitialVelocityVectorValueBox.Value * MathHelper.CosValueOfDegreeAngle((double)InitialVelocityVectorAngleBox.Value),
                InitialVelocityY = (double)InitialVelocityVectorValueBox.Value * MathHelper.SinValueOfDegreeAngle((double)InitialVelocityVectorAngleBox.Value),
                DensityOfTheMedium = (double)DensityOfTheMediumBox.Value,
                RadiusOfTheProjectile = (double)ProjectilesRadiusBox.Value,
                InitialVelocityVectorValue = (double)InitialVelocityVectorValueBox.Value,
                InitialVelocityVectorAngle = (double)InitialVelocityVectorAngleBox.Value,
            };
        }
        private void InitializeSpaceTimePoints()
        {
            spaceTimePoints = new SpaceTimePoint[500];

            for (int i = 0; i < spaceTimePoints.Length; i++)
            {
                spaceTimePoints[i] = new SpaceTimePoint();
            }
        }

        private void CalculateProjectileTrajectory(object sender, RoutedEventArgs e)
        {
            InitializeStartingValues();
            InitializeSpaceTimePoints();
            CalculateTotalTime();
            CalculateTrajectory();
        }

        public void CalculateTrajectory()
        {
            deltaTime = timeTotal / spaceTimePoints.Length;

            var initialPoint = new SpaceTimePoint
            {
                Velocity = new Velocity
                {
                    Vx = initialValues.InitialVelocityX,
                    Vy = initialValues.InitialVelocityY
                },
                Position = new Position
                {
                    X = 0,
                    Y = 0,
                }
            };

            spaceTimePoints[0] = initialPoint;

            for (int i = 0; i < spaceTimePoints.Length - 1; i++)
            {
                spaceTimePoints[i+1] = CalculateSpaceTimePoint(spaceTimePoints[i]);
            }

            maximumX = spaceTimePoints[spaceTimePoints.Length - 1].Position.X;
        }

        public SpaceTimePoint CalculateSpaceTimePoint(SpaceTimePoint previousSpaceTimePoint)
        {
            var spaceTimePoint = new SpaceTimePoint
            {
                Position = CalculatePosition(previousSpaceTimePoint),
                Velocity = CalculateVelocity(previousSpaceTimePoint.Velocity)
            };

            return spaceTimePoint;
        }

        public Velocity CalculateVelocity(Velocity previousVelocity)
        {
            var x_netForce = DragCalculator.CalculateDragAtVelocity(previousVelocity.Vx, initialValues);
            var y_netForce = (initialValues.Gravity) + DragCalculator.CalculateDragAtVelocity(previousVelocity.Vy, initialValues);

            var velocity = new Velocity
            {
                Vx = previousVelocity.Vx - DeltaVelocityCalculator.CalculateDeltaVelocity(deltaTime, x_netForce),
                Vy = previousVelocity.Vy - DeltaVelocityCalculator.CalculateDeltaVelocity(deltaTime, y_netForce),
            };

            return velocity;
        }

        public Position CalculatePosition(SpaceTimePoint previousSpaceTimePoint)
        {
            var calculatedX = previousSpaceTimePoint.Position.X + DeltaPositionCalculator.GetDeltaPositionAfterDeltaTime(deltaTime, previousSpaceTimePoint.Velocity.Vx);
            var calculatedY = previousSpaceTimePoint.Position.Y + DeltaPositionCalculator.GetDeltaPositionAfterDeltaTime(deltaTime, previousSpaceTimePoint.Velocity.Vy);

            var position = new Position
            {
                X = calculatedX,
                Y = calculatedY > 0 ? calculatedY : 0d
            };

            return position;
        }

        public void CalculateTotalTime()
        {
            // Total time is the sum of ascending time and descending time. Horizontal motion does not affect the time total.

            var ascendingTime = AscendingTimeCalculator.CalculateAscendingTime(initialValues, out maximumY);
            var descendingTime = DescendingTimeCalculator.CalculateDescendingTime(initialValues, maximumY, terminalVelocityCheckbox);

            ascendingTimeBox.Text = ascendingTime.ToString();
            descendingTimeBox.Text = descendingTime.ToString();
            timeTotal = (ascendingTime + descendingTime);
            finalTimeBox.Text = timeTotal.ToString();
        }

        private void iterationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            currentSliderValue.Text = iterationSlider.Value.ToString();
            CurrentIteration = (int)iterationSlider.Value;

            var position = new Position();

            if (spaceTimePoints != null)
            {
                var spaceTimePoint = spaceTimePoints[CurrentIteration];

                position.X = spaceTimePoint.Position.X;
                position.Y = spaceTimePoint.Position.Y;

                velocityXAtIterationBox.Text = spaceTimePoint.Velocity.Vx.ToString();
                velocityYAtIterationBox.Text = spaceTimePoint.Velocity.Vy.ToString();

                positionXAtIterationBox.Text = position.X.ToString();
                positionYAtIterationBox.Text = position.Y.ToString();
            }

            UpdateEllipseElement(position, maximumX, maximumY);
        }

        private void UpdateEllipseElement(Position newPosition, double maximumX, double maximumY)
        {
            try
            {
                ellipse.Margin = EllipsePositionCalculator.NewElipseElementMarginCalculator(newPosition, maximumX, maximumY);
            }
            catch (Exception)
            {
            }
            
        }
    }
}