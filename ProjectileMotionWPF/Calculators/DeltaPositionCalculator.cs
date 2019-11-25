namespace ProjectileMotionWPF.Calculators
{
    public static class DeltaPositionCalculator
    {
        public static double GetDeltaPositionAfterDeltaTime(double deltaTime, double Y_Velocity)
        {
            // v = s/t <=> s = v * t

            var deltaY = Y_Velocity * deltaTime;

            return deltaY;

        }
    }
}
