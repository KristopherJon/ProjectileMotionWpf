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

        private const float G = 9.80665f;
        private float initialVelocityVectorValue;
        private float initialVelocityVectorAngle;

        private float velocityX;
        private float initialVelocityY;
        private float finalTime;

        private int CurrentIteration;
        private SpaceTimePoint[] spaceTimePoints;

        public void SetCurrentIteration(int i)
        {
            CurrentIteration = i;
        }

        public MainWindow()
        {
            InitializeComponent();
            var theObject = thrownObject;
            Canvas.SetLeft(theObject, 20);
        }

        private void CalculateProjectileTrajectory(object sender, RoutedEventArgs e)
        {
            ReadInitialValues();
            CalculateTimeTotal();
            PopulateSpaceTimePointsTable();
            
            finalTimeBox.Text = finalTime.ToString();

            PopulateResultFields(CurrentIteration);
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
        private void ReadInitialValues()
        {
            initialVelocityVectorAngle = (float)InitialVelocityVectorAngle.Value;
            initialVelocityVectorValue = (float)InitialVelocityVectorValue.Value;
            velocityX = initialVelocityVectorValue * CosValueOfDegreeAngle(initialVelocityVectorAngle); // x velocity is a constant for we ommit aeordynamic resistance
            initialVelocityY = initialVelocityVectorValue * SinValueOfDegreeAngle(initialVelocityVectorAngle); // is not constant for vertical velocity is affected by gavity
            CurrentIteration = (int)iterationBox.Value;
            spaceTimePoints = new SpaceTimePoint[100];
        }
        private void CalculateTimeTotal()
        {
            finalTime = (2.0f * initialVelocityVectorValue) * SinValueOfDegreeAngle(initialVelocityVectorAngle) / G;
        }
        private void PopulateSpaceTimePointsTable()
        {
            for (int i = 0; i < spaceTimePoints.Length; i++)
            {
                spaceTimePoints[i] = CreateSpaceTimePointAtIteration(i);
            }
        }
        private float SinValueOfDegreeAngle(float angle)
        {
            return (float)Math.Sin(DegreeToRadian(angle));
        }
        private float CosValueOfDegreeAngle(float angle)
        {
            return (float)Math.Cos(DegreeToRadian(angle));
        }
        private float DegreeToRadian(float angle)
        {
            return (float)Math.PI * angle / 180.0f;
        }
        private void iterationBox_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (spaceTimePoints != null)
            {
                SetCurrentIteration((int)iterationBox.Value);

                PopulateResultFields(CurrentIteration);
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
