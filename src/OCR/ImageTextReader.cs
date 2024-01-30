namespace CursedMoose.MASR.OCR
{
    public interface ImageTextReader
    {
        public static ImageTextReader Instance = new TesseractImageReader();
        Task<string> ReadText(Bitmap image);
        Task<string> ReadText(string filePath = "images/screenshots/screenreader.png");
    }
}
