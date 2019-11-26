namespace ProjectileMotionWPF.Calculators
{
    public static class DeltaVelocityCalculator
    {
        public static double CalculateDeltaVelocity(double deltaTime, double acceleration)
        {
            return deltaTime * acceleration;
        }
    }
}
