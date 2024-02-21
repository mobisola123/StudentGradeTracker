namespace StudentGradeTracker.AppOptions
{
    public class JwtConfiguration
    {
        public string Secret { get; set; }

        public double JwtExpiryTime { get; set; }
    }
}
