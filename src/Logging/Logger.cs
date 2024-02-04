using System.Globalization;

namespace CursedMoose.MASR.Logging
{
    public class Logger
    {
        private readonly string Name;
        public static readonly CultureInfo LOG_FORMAT = new("en-GB");
        public static LogLevel Level = LogLevel.Info;
        public Logger(string name)
        {
            Name = name;
        }

        public void Debug(string message)
        {
            if (Level <= LogLevel.Debug)
            {
                var timestamp = DateTime.Now.ToString(LOG_FORMAT);
                Console.WriteLine($"{timestamp} [{Name}] [DEBUG] {message}");
            }
        }

        public void Info(string message)
        {
            if (Level <= LogLevel.Info) 
            {
                var timestamp = DateTime.Now.ToString(LOG_FORMAT);
                Console.WriteLine($"{timestamp} [{Name}] {message}");
            }
        }

        public void Error(string message)
        {
            if (Level <= LogLevel.Error)
            {
                var timestamp = DateTime.Now.ToString(LOG_FORMAT);
                Console.WriteLine($"{timestamp} [{Name}] [ERROR] {message}");
            }
        }
    }

    public enum LogLevel
    {
        Debug,
        Info,
        Error
    }
}
