using System;
using System.IO;
using Apitron.PDF.Kit;

namespace PDFACheckingSample
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Stream fileStream = File.Open("../../Data/document_pdfa.pdf",FileMode.Open))
            {
                using (FixedDocument doc = new FixedDocument(fileStream))
                {
                    // PdfStandard property can be used to check whether the document uses PDFA standard.
                    // It will return PDfStandard.Default if the doc doesn't use PDFA or PdfStandard.PDFA if it does.
                    Console.WriteLine(string.Format("Document uses '{0}' standard",Enum.GetName(typeof(PdfStandard),doc.PdfStandard)));
                }
            }

            Console.ReadLine();
        }
    }
}