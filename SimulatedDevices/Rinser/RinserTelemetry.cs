namespace Rinser
{
    public class RinserTelemetry
    {
        public bool online { get; set; }
        public bool running { get; set; }
        public bool producing { get; set; }
        public double emptybottlecounterin { get; set; }
        public double emptybottlecounterout { get; set; }
        public double machinespeed { get; set; }
        public double electricityconsumption { get; set; }
        public double waterconsumption { get; set; }
    }
}