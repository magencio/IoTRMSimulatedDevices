using System;

namespace Simulation.Core.Helpers
{
    public static class RandomHelper
    {
        private static Random random = new Random();

        /// <summary>
        /// Average +/- percentage between min and max values
        /// </summary>
        /// <param name="avg">Average</param>
        /// <param name="percentage">Percentage of the average value</param>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value</param>
        /// <returns></returns>
        public static double Vary(double avg, double percentage, double min, double max)
        {
            var value = avg * (1 + ((percentage / 100) * (2 * random.NextDouble() - 1)));
            value = Math.Max(value, min);
            value = Math.Min(value, max);
            return value;
        }
    }
}
