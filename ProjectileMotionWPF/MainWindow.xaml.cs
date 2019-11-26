using System;
using System.Windows;
using ProjectileMotionWPF.Data;
using ProjectileMotionWPF.Calculators;
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
                Gravity = (double)GravityBox.Value,
                DragCoefficient = 0.47,
                CrossSectionArea = Math.Pow((double)ProjectilesRadiusBox.Value, 2) * Math.PI,
                InitialVelocityX = (double)InitialVelocityVectorValueBox.Value * MathHelper.CosValueOfDegreeAngle((double)InitialVelocityVectorValueBox.Value),
                InitialVelocityY = (double)InitialVelocityVectorValueBox.Value * MathHelper.SinValueOfDegreeAngle((double)InitialVelocityVectorAngleBox.Value),
                DensityOfTheMedium = (double)DensityOfTheMediumBox.Value,
                RadiusOfTheProjectile = (double)ProjectilesRadiusBox.Value,
                InitialVelocityVectorValue = (double)InitialVelocityVectorValueBox.Value,
                InitialVelocityVectorAngle = (double)InitialVelocityVectorAngleBox.Value,
            };

            CurrentIteration = (int)iterationBox.Value;
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
        }

        public SpaceTimePoint CalculateSpaceTimePoint(SpaceTimePoint previousSpaceTimePoint)
        {
            var spaceTimePoint = new SpaceTimePoint
            {
                Position = CalculatePosition(previousSpaceTimePoint.Position),
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


        public Position CalculatePosition(Position previousPosition)
        {
            var position = new Position
            {
                X = 1d,
                Y = 1d
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

        private void IterationBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            CurrentIteration = (int)iterationBox.Value;

            if (spaceTimePoints != null)
            {
                var spaceTimePoint = spaceTimePoints[CurrentIteration];
            
                velocityXAtIterationBox.Text = spaceTimePoint.Velocity.Vx.ToString();
                velocityYAtIterationBox.Text = spaceTimePoint.Velocity.Vy.ToString();

                positionXAtIterationBox.Text = spaceTimePoint.Position.X.ToString();
                positionYAtIterationBox.Text = spaceTimePoint.Position.Y.ToString();
            }
        }

        //private void CalculateProjectileTrajectory(object sender, RoutedEventArgs e)
        //{
        //    ReadInitialValues();
        //    CalculateTimeTotal();
        //    PopulateSpaceTimePointsTable();
        //    maxDistance = spaceTimePoints[spaceTimePoints.Length - 1].Position.X;
        //    maxHeight = spaceTimePoints[spaceTimePoints.Length / 2].Position.Y;
        //    finalTimeBox.Text = finalTime.ToString();

        //    PopulateResultFields(CurrentIteration);
        //}
        
        //private void UpdateEllipseElement(Position newPosition)
        //{
        //    //ellipse.Margin = EllipsePositionCalculator.NewElipseElementMarginCalculator(newPosition, maxDistance, maxHeight);
        //    ellipse.Margin = EllipsePositionCalculator.NewElipseElementMarginCalculator(newPosition, maxDistance, maxHeight);
        //}
        //private SpaceTimePoint CreateSpaceTimePointAtIteration(int i)
        //{
        //    var currentTime = GetTimeAtIteration(i);

        //    var currentX = initialVelocityX * currentTime;
        //    var currentY = initialVelocityY * currentTime - (G/2)*((float)Math.Pow(currentTime, 2));
        //    var position = new Position(currentX, currentY);

        //    var currentVelocityY = initialVelocityY - (G * currentTime);
        //    var velocity = new Velocity(initialVelocityX, currentVelocityY);

        //    return new SpaceTimePoint(position, velocity);
        //}
        //private void PopulateResultFields(int iterationNumber)
        //{
        //    velocityXAtIterationBox.Text = spaceTimePoints[iterationNumber].Velocity.Vx.ToString();
        //    velocityYAtIterationBox.Text = spaceTimePoints[iterationNumber].Velocity.Vy.ToString();

        //    positionXAtIterationBox.Text = spaceTimePoints[iterationNumber].Position.X.ToString();
        //    positionYAtIterationBox.Text = spaceTimePoints[iterationNumber].Position.Y.ToString();
        //}
        //private double GetTimeAtIteration(int i)
        //{
        //    return finalTime * (i / (float)spaceTimePoints.Length);
        //}

        //public void SetCurrentIteration(int i)
        //{
        //    CurrentIteration = i;
        //}
    }
}