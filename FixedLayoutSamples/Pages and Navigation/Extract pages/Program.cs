namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;

    // This sample shows how to extract pages from one PDF document to another.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\ExtractPages.pdf";

            // open and load the file
            using (FileStream inPDF = new FileStream(@"..\..\..\..\OutputDocuments\testfile.pdf", FileMode.Open))
            using (FileStream outPDF = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument(inPDF);

                // create new document
                FixedDocument outDocument = new FixedDocument();
                
                // export page from source document and insert to the destination
                outDocument.Pages.Insert(0, Page.Export(outDocument, document.Pages[0]));

                outDocument.Save(outPDF);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
