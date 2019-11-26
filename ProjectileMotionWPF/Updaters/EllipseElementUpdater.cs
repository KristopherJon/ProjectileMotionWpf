using ProjectileMotionWPF.Data;
using System.Windows;

namespace ProjectileMotionWPF.Updaters
{
    public static class EllipsePositionCalculator
    {
        public static Thickness NewElipseElementMarginCalculator(Position newPosition, double maxDistance, double maxHeight)
        {
            var x = newPosition.X;
            var y = newPosition.Y;

            //return new Thickness(1, 0, 0, 1);
            return new Thickness(x * 500 / maxDistance + 243, 0, 0, y * 300 / maxHeight + 43);
        }
    }
}
