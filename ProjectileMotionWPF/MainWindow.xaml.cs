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
        private Position chartStartingPoint = new Position(250, 800 - 100);

        private float G;
        private float initialVelocityVectorValue;
        private float initialVelocityVectorAngle;

        private float velocityX;
        private float initialVelocityY;
        private float finalTime;
        private float maxDistance;
        private float maxHeight;

        private int CurrentIteration;
        private SpaceTimePoint[] spaceTimePoints;

        public void SetCurrentIteration(int i)
        {
            CurrentIteration = i;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateProjectileTrajectory(object sender, RoutedEventArgs e)
        {
            ReadInitialValues();
            CalculateTimeTotal();
            PopulateSpaceTimePointsTable();
            maxDistance = spaceTimePoints[spaceTimePoints.Length - 1].Position.X;
            maxHeight = spaceTimePoints[spaceTimePoints.Length / 2].Position.Y;
            finalTimeBox.Text = finalTime.ToString();

            PopulateResultFields(CurrentIteration);
        }

        private void ReadInitialValues()
        {
            initialVelocityVectorAngle = (float)InitialVelocityVectorAngle.Value;
            initialVelocityVectorValue = (float)InitialVelocityVectorValue.Value;
            velocityX = initialVelocityVectorValue * MathHelper.CosValueOfDegreeAngle(initialVelocityVectorAngle); // x velocity is a constant for we ommit aeordynamic resistance
            initialVelocityY = initialVelocityVectorValue * MathHelper.SinValueOfDegreeAngle(initialVelocityVectorAngle); // is not constant for vertical velocity is affected by gavity
            CurrentIteration = (int)iterationBox.Value;
            //G = GravityBox.Value;
            spaceTimePoints = new SpaceTimePoint[100];
        }
        private void CalculateTimeTotal()
        {
            finalTime = (2.0f * initialVelocityVectorValue) * MathHelper.SinValueOfDegreeAngle(initialVelocityVectorAngle) / G;
        }
        private void PopulateSpaceTimePointsTable()
        {
            for (int i = 0; i < spaceTimePoints.Length; i++)
            {
                spaceTimePoints[i] = CreateSpaceTimePointAtIteration(i);
            }
        }
        private void UpdateEllipseElement(Position newPosition)
        {
            ellipse.Margin = EllipsePositionCalculator.NewElipseElementMarginCalculator(newPosition, maxDistance, maxHeight);
        }
        private SpaceTimePoint CreateSpaceTimePointAtIteration(int i)
        {
            var currentTime = GetTimeAtIteration(i);

            var currentX = velocityX * currentTime;
            var currentY = initialVelocityY * currentTime - (G/2)*((float)Math.Pow(currentTime, 2));
            var position = new Position(currentX, currentY);

            var currentVelocityY = initialVelocityY - (G * currentTime);
            var velocity = new Velocity(velocityX, currentVelocityY);
            
            return new SpaceTimePoint(position, velocity);
        }
        private void PopulateResultFields(int iterationNumber)
        {
            velocityXAtIterationBox.Text = spaceTimePoints[iterationNumber].Velocity.VX.ToString();
            velocityYAtIterationBox.Text = spaceTimePoints[iterationNumber].Velocity.VY.ToString();

            positionXAtIterationBox.Text = spaceTimePoints[iterationNumber].Position.X.ToString();
            positionYAtIterationBox.Text = spaceTimePoints[iterationNumber].Position.Y.ToString();
        }
        private float GetTimeAtIteration(int i)
        {
            return finalTime * (i / 100f);
        }
        private void IterationBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (spaceTimePoints != null && iterationBox.Value != null)
            {
                SetCurrentIteration((int)iterationBox.Value);

                PopulateResultFields(CurrentIteration);

                UpdateEllipseElement(spaceTimePoints[CurrentIteration].Position);

                var x = (spaceTimePoints[CurrentIteration].Position.X) / initialVelocityVectorValue;

                //currentXBox.Text = x.ToString();
            }
        }
    }
}


//<Path Stroke = "Black" StrokeThickness="1" Margin="250,100,50,100" Stretch="Fill">
//            <Path.Data>
//                <PathGeometry>
//                    <PathGeometry.Figures>
//                        <PathFigureCollection>
//                            <PathFigure StartPoint = "0,0" >
//                                < PathFigure.Segments >
//                                    < PathSegmentCollection >
//                                        < QuadraticBezierSegment Point1="100,-100" Point2="200,0" />
//                                    </PathSegmentCollection>
//                                </PathFigure.Segments>
//                            </PathFigure>
//                        </PathFigureCollection>
//                    </PathGeometry.Figures>
//                </PathGeometry>
//            </Path.Data>
//        </Path>
