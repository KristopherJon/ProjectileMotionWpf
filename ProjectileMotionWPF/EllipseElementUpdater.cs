using ProjectileMotionWPF.Data;
using System.Windows;

namespace ProjectileMotionWPF
{
    public static class EllipsePositionCalculator
    {
        public static Thickness NewElipseElementMarginCalculator(Position newPosition, float maxDistance, float maxHeight)
        {
            var x = newPosition.X;
            var y = newPosition.Y;

            return new Thickness(x * 500 / maxDistance + 243, 0, 0, y * 100 / maxHeight + 43);
        }
    }
}
