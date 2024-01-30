using CursedMoose.MASR.Logging;
using System.Drawing.Imaging;

namespace CursedMoose.MASR.ScreenCapture
{
    public class ScreenCapturer
    {
        public static readonly ScreenCapturer Instance = new ScreenCapturer();
        readonly Logger log = new("ScreenCapturer");
        private ClipboardScraper scraper;
        private ScreenRegionSelector selectorForm;
        private Rectangle selectedRegion; // = new Rectangle(0, 0, 100, 100);
        private static readonly string ImagePath = "images/region.png";

        public int SelectedRegionArea
        {
            get { return selectedRegion.Height * selectedRegion.Width; }
        }

        public ScreenCapturer()
        {
            scraper = new(CaptureScreen());
            selectorForm = new ScreenRegionSelector(CaptureScreen());
            selectedRegion = new Rectangle(0, 0, 0, 0);

            if (!Directory.Exists("images"))
            {
                Directory.CreateDirectory("images");
            }
        }

        private Bitmap CaptureScreen()
        {
            Bitmap bmp = new(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
            }
            return bmp;
        }

        private Bitmap CaptureScreenRegion()
        {
            Bitmap bmp = new(selectedRegion.Width, selectedRegion.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(selectedRegion.Left, selectedRegion.Top, 0, 0, selectedRegion.Size);
            }
            return bmp;
        }

        public string TakeScreenRegion()
        {
            var filePath = ImagePath;
            var img = CaptureScreenRegion();
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                img.Save(fs, ImageFormat.Png);
            }
            return filePath;
        }

        public bool ClipboardHasNewImage()
        {
            return scraper.HasNewImage();
        }

        public Image GetClipboardImage()
        {
            return scraper.GetImage();
        }

        public async Task StartScraper()
        {
            if (!scraper.IsRunning())
            {
                log.Info($"Scraper started at {DateTime.Now}");
                await Task.Run(scraper.Start);
            }
            return;
        }

        public async Task StopScraper()
        {
            log.Info($"Goodbye at {DateTime.Now}");
            await scraper.Stop();
            return;
        }

        public void SelectScreenRegion()
        {
            Task mytask = Task.Run(() =>
            {
                selectorForm = new ScreenRegionSelector(CaptureScreen());
                selectorForm.ShowDialog();
            });
            var screen = CaptureScreen();
            var graphics = Graphics.FromImage(screen);
            graphics.CopyFromScreen(0, 0, 0, 0, screen.Size);

            using (MemoryStream s = new MemoryStream())
            {
                screen.Save(s, ImageFormat.Bmp);
            }
        }

        public void SetScreenRegion(Rectangle rect)
        {
            selectedRegion = rect;
            log.Info($"Set selectedRegion to {rect.Location}{rect.Size.Width}x{rect.Size.Height}");
        }
    }
}
