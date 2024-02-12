using CursedMoose.MASR.Logging;

namespace CursedMoose.MASR.ScreenCapture
{
    internal class ClipboardScraper
    {
        readonly Logger log = new("ClipboardScraper");
        private Image previousImage;
        private bool Scraper_Running = false;

        internal ClipboardScraper(Image defaultImage)
        {
            previousImage = defaultImage;
        }
        internal bool HasNewImage()
        {
            bool hasNewImage = false;
            Thread clipboardThread = new Thread(
                delegate ()
                {
                    if (Clipboard.ContainsImage())
                    {
                        var clipboardImage = Clipboard.GetImage();
                        hasNewImage = true;
                    }
                });
            clipboardThread.SetApartmentState(ApartmentState.STA);
            clipboardThread.Start();
            clipboardThread.Join();
            return hasNewImage;
        }

        internal Image GetImage()
        {
            Image newImage = previousImage;
            Thread clipboardThread = new Thread(
                delegate ()
                {
                    if (Clipboard.ContainsImage())
                    {
                        var clipboardImage = Clipboard.GetImage();
                        Clipboard.Clear();
                        newImage = clipboardImage;
                        previousImage = clipboardImage;
                    }
                    else
                    {
                        log.Error("No image was found on the clipboard.");
                    }
                });
            clipboardThread.SetApartmentState(ApartmentState.STA);
            clipboardThread.Start();
            clipboardThread.Join();
            return newImage;
        }

        internal void ClearClipboard()
        {
            Thread clipboardThread = new Thread(
                delegate ()
                {
                    Clipboard.Clear();
                });
            clipboardThread.SetApartmentState(ApartmentState.STA);
            clipboardThread.Start();
            clipboardThread.Join();
        }
        private async Task Run()
        {
            ClearClipboard();
            Scraper_Running = true;
            do
            {
                await Scrape_Clipboard();
            } while (Scraper_Running);

            return;
        }

        private async Task Scrape_Clipboard()
        {
            if (HasNewImage())
            {
                var image = GetImage();
                var bmp = new Bitmap(image);
                await ElevenLabs.ElevenLabs.Clippy.ReadImage(bmp);
            }
            await Task.Delay(1_000);
        }
        internal async Task Start()
        {
            if (!Scraper_Running)
            {
                log.Info($"Hello at {DateTime.Now}");
                Scraper_Running = true;
                await Task.Run(Run);
            }
            return;
        }

        internal bool IsRunning()
        {
            return Scraper_Running;
        }

        internal async Task Stop()
        {
            log.Info($"Goodbye at {DateTime.Now}");
            Scraper_Running = false;
            return;
        }
    }
}
