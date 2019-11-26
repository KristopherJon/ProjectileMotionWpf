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
        
        int CurrentIteration;
        
        InitialValues initialValues;
        
        List<SpaceTimePoint> spaceTimePoints;

        double maximumX;
        double maximumY;
        double finalTime = 0;
        readonly double deltaTime = 0.01d ; //physics time step

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
            spaceTimePoints = new List<SpaceTimePoint>();
        }

        private void CalculateProjectileTrajectory(object sender, RoutedEventArgs e)
        {
            InitializeStartingValues();
            InitializeSpaceTimePoints();
            CalculateTrajectory();
        }

        public void CalculateTrajectory()
        {
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

            spaceTimePoints.Add(initialPoint);

            int i = 0;
            finalTime = 0;

            while (spaceTimePoints[spaceTimePoints.Count - 1].Position.Y >= 0)
            {
                finalTime += deltaTime;
                var newPoint = CalculateSpaceTimePoint(spaceTimePoints[i]);
                spaceTimePoints.Add(newPoint);
                if (newPoint.Position.Y <= 0d)
                {
                    newPoint.Position.Y = 0d;
                    break;
                }
                i++;
            };

            iterationSlider.Maximum = spaceTimePoints.Count - 1;
            finalTimeBox.Text = finalTime.ToString();
            maximumX = spaceTimePoints[spaceTimePoints.Count - 1].Position.X;
            maximumY = spaceTimePoints[spaceTimePoints.Count - 1].Position.Y;
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
                Y = calculatedY
            };

            return position;
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