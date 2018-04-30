namespace Filler
{
    public class FillerTelemetry
    {
        public bool online { get; set; }
        public bool running { get; set; }
        public bool producing { get; set; }
        public double fullbottlecounterin { get; set; }
        public double fullbottlecounterout { get; set; }
        public double bowltemperature { get; set; }
        public double bowlpressure { get; set; }
        public double beverageinfeedvolume { get; set; }
        public double co2consumption { get; set; }
        public double qainspectorlevel { get; set; }
        public double qainspectorcap { get; set; }
        public double qacapcounter { get; set; }
        public double machinespeed { get; set; }
        public double electricityconsumption { get; set; }
        public double waterconsumption { get; set; }
    }
}