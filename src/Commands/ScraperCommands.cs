using CursedMoose.MASR.Commands;
using CursedMoose.MASR.ScreenCapture;

namespace MultiAgentScreenRegionReader.Commands
{
    internal class ScraperCommands : ServerCommand
    {
        public ScraperCommands() : base("scraper")
        {

        }

        public override void Handle(string command)
        {
            var subCommand = StripCommandFromMessage(command);

            switch (subCommand)
            {
                case "start":
                    ScreenCapturer.Instance.StartScraper();
                    break;
                case "stop":
                    ScreenCapturer.Instance.StopScraper(); 
                    break;
            }
        }
    }
}
