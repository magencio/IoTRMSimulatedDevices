using System.IO;

namespace Simulation.Core.Helpers
{
    public static class FileHelper
    {
        public static string ReadFile(string fileName)
        {
            using (var r = new StreamReader(fileName))
            {
                return r.ReadToEnd();
            }
        }
    }
}
