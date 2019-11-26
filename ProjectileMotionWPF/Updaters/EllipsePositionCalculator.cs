using ProjectileMotionWPF.Data;
using System.Windows;

namespace ProjectileMotionWPF.Updaters
{
    public static class EllipsePositionCalculator
    {
        public static Thickness NewElipseElementMarginCalculator(Position newPosition)
        {
            var x = newPosition.X;
            var y = newPosition.Y;

            //return new Thickness(1, 0, 0, 1);
            return new Thickness(x * 5 + 243, 0, 0, y * 5 + 43);
        }
    }
}
