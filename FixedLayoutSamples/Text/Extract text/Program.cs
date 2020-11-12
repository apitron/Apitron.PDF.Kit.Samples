namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.Extraction;

	//This sample shows how to extract text from a page or from entire PDF document.
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (FileStream fs = new FileStream(@"..\..\..\..\OutputDocuments\vaf1a.pdf", FileMode.Open, FileAccess.Read))
            {
                // open and load PDF document
                FixedDocument document = new FixedDocument(fs);
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

                // extract text from selected PDF document
                foreach (Page page in document.Pages)
                {
                    // Extracted text will be formatted (Alternative methods are ExtractText(TextExtractionOptions.RawText)).
                    // Formatting means that all relative text positions will be kept after extraction and text will look more readable.
                    // Extracting text with formatting may be especially useful for PDF documents with tabular data.
                    string text = page.ExtractText(TextExtractionOptions.FormattedText);
                    stringBuilder.Append(text);
                }

                System.Console.WriteLine(stringBuilder.ToString());
                System.Console.ReadKey();
            }
        }
    }
}
