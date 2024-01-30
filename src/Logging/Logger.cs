using System.Globalization;

namespace CursedMoose.MASR.Logging
{
    public class Logger
    {
        private readonly string Name;
        public static readonly CultureInfo LOG_FORMAT = new("en-GB");
        public Logger(string name)
        {
            Name = name;
        }

        public void Info(string message)
        {
            var timestamp = DateTime.Now.ToString(LOG_FORMAT);
            Console.WriteLine($"{timestamp} [{Name}] {message}");
        }

        public void Error(string message)
        {
            var timestamp = DateTime.Now.ToString(LOG_FORMAT);
            Console.WriteLine($"{timestamp} [{Name}] [ERROR] {message}");
        }
    }
}
