using Apitron.PDF.Kit;
using System.Diagnostics;
using System.IO;

namespace UsingPDFAValidationSettings
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Stream stream = File.Open(@"../../data/document.pdf", FileMode.Open, FileAccess.Read))
            {
                // create document object and specify the output format
                FixedDocument doc = new FixedDocument(stream, PdfStandard.PDFA);

                // save document
                using (Stream outputStream = File.Create(@"pdfa_document.pdf"))
                {
                    // turn off cross reference stream usage
                    doc.IsCompressedStructure = false;
                    doc.Save(outputStream);
                }
            }
            Process.Start("pdfa_document.pdf");
        }
    }
}
