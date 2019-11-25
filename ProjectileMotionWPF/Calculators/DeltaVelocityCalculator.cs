namespace ProjectileMotionWPF.Calculators
{
    public static class DeltaVelocityCalculator
    {
        public static double CalculateDeltaVelocity(double deltaTime, double netForce)
        {
            return deltaTime * netForce;
        }
    }
}
