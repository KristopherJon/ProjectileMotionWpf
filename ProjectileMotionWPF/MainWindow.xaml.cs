using System;
using System.Windows;
using ProjectileMotionWPF.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectileMotionWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private Position chartStartingPoint = new Position(250, 800 - 100);

        private InitialValues initialValues;

        //private double finalTime;
        //private double maxDistance;
        //private double maxHeight;

        private int CurrentIteration;
        
        private SpaceTimePoint[] spaceTimePoints;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void CalculateProjectileTrajectory(object sender, RoutedEventArgs e)
        {
            InitializeStartingValues();
            //finalTimeBox.Text = CalculateTotalTime().ToString();
            descendingTimeBox.Text = CalculateDescendingTime(initialValues).ToString();
            //ascendingTimeBox.Text = CalculateAscendingTime(initialValues).ToString();
        }

        private void InitializeStartingValues()
        {
            initialValues = new InitialValues
            {
                Gravity = (double)GravityBox.Value,
                InitialVelocityVectorValue = (double)InitialVelocityVectorValueBox.Value,
                InitialVelocityVectorAngle = (double)InitialVelocityVectorAngleBox.Value,
                InitialVelocityX = (double)InitialVelocityVectorValueBox.Value * MathHelper.CosValueOfDegreeAngle((double)InitialVelocityVectorValueBox.Value),
                InitialVelocityY = (double)InitialVelocityVectorValueBox.Value * MathHelper.SinValueOfDegreeAngle((double)InitialVelocityVectorAngleBox.Value),
                RadiusOfTheProjectile = (double)ProjectilesRadiusBox.Value,
                CrossSectionArea = Math.Pow((double)ProjectilesRadiusBox.Value, 2) * Math.PI,
                DensityOfTheMedium = (double)DensityOfTheMediumBox.Value,
                Mass = (double)ProjectilesMassBox.Value,
                DragCoefficient = 0.47 // sphere
            };
            
            CurrentIteration = (int)iterationBox.Value;
            
            spaceTimePoints = new SpaceTimePoint[500];
            for (int i = 0; i < spaceTimePoints.Length; i++)
            {
                spaceTimePoints[i] = new SpaceTimePoint();
            }
        }

        private double CalculateTotalTime()
        {
            // Total time is the sum of ascending time and descending time. Horizontal motion does not affect the time total.
            return CalculateAscendingTime(initialValues) + CalculateDescendingTime(initialValues);
        }

        /// <summary>
        /// Calculates time it takes the projectile's speed to reach a number "close" to zero.
        /// </summary>
        public double CalculateAscendingTime(InitialValues initialValues)
        {
            var time = 0.0d;
            var timeStep = 0.001d;
            var yVelocity = initialValues.InitialVelocityY;

            while (yVelocity >= 0.001d)
            {
                var dragAtVelocity = MathHelper.CalculateDragAtVelocity(yVelocity, initialValues);
                var netForce = initialValues.Gravity + dragAtVelocity ;
                var velocityChangeAfterTimeStep = netForce * timeStep;
                yVelocity -= velocityChangeAfterTimeStep;
                time += timeStep;
            }

            velocityYAtIterationBox.Text = yVelocity.ToString();

            return time;
        }


        public double CalculateDescendingTime(InitialValues initialValues)
        {
            var time = 0.0d;
            var timeStep = 0.001d;
            var yVelocity = 0.000d;

            while (yVelocity <= initialValues.InitialVelocityY)
            {
                var dragAtVelocity = MathHelper.CalculateDragAtVelocity(yVelocity, initialValues);
                var netForce = initialValues.Gravity - dragAtVelocity;
                var velocityChangeAfterTimeStep = netForce * timeStep;
                yVelocity += velocityChangeAfterTimeStep;
                time += timeStep;

                if (velocityChangeAfterTimeStep <= 0.001)
                {
                    terminalVelocityBox.IsChecked = true;
                }
            }

            return time;
        }

        private double CalculateTerminalVelocity(InitialValues initialValues)
        {
            //For the reference: https://www.grc.nasa.gov/WWW/K-12/airplane/termv.html

            var numerator = 2 * initialValues.Mass;
            var denominator = initialValues.DragCoefficient * initialValues.DensityOfTheMedium * initialValues.CrossSectionArea;

            return Math.Sqrt(numerator / denominator);
        }

        private double CalculatePositionY()
        {


            return 1d;
        }

        private void IterationBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //if (spaceTimePoints != null && iterationBox.Value != null)
            //{
            //    SetCurrentIteration((int)iterationBox.Value);

            //    PopulateResultFields(CurrentIteration);

            //    UpdateEllipseElement(spaceTimePoints[CurrentIteration].Position);
            //}
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

        //private void ReadInitialValues()
        //{
        //    initialVelocityVectorAngle = (float)InitialVelocityVectorAngle.Value;
        //    initialVelocityVectorValue = (float)InitialVelocityVectorValue.Value;
        //    initialVelocityX = initialVelocityVectorValue * MathHelper.CosValueOfDegreeAngle(initialVelocityVectorAngle); // x velocity is a constant for we ommit aeordynamic resistance
        //    initialVelocityY = initialVelocityVectorValue * MathHelper.SinValueOfDegreeAngle(initialVelocityVectorAngle); // is not constant for vertical velocity is affected by gavity
        //    CurrentIteration = (int)iterationBox.Value;
        //    //G = GravityBox.Value;
        //    spaceTimePoints = new SpaceTimePoint[500];
        //    G = (float)GravityBox.Value;
        //}
        //private void CalculateTimeTotal()
        //{
        //    finalTime = (2.0f * initialVelocityVectorValue) * MathHelper.SinValueOfDegreeAngle(initialVelocityVectorAngle) / G;
        //}
        //private void PopulateSpaceTimePointsTable()
        //{
        //    for (int i = 0; i < spaceTimePoints.Length; i++)
        //    {
        //        spaceTimePoints[i] = CreateSpaceTimePointAtIteration(i);
        //    }
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