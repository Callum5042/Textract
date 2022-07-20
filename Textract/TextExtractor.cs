using Tesseract;

namespace Textract
{
    public static class TextExtractor
    {
        public static string ExtractFromBytes(byte[] bytes)
        {
            ArgumentNullException.ThrowIfNull(bytes);

            using var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);
            using var img = Pix.LoadFromMemory(bytes);
            using var page = engine.Process(img);
            return page.GetText();
        }
    }
}