namespace Apitron.PDF.Kit.Samples
{
    using System;
    using System.IO;
    using Apitron.PDF.Kit;

    // This sample shows how to remove pages from your PDF documents.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\RemovePages.pdf";
            // open and load the file
            using (FileStream inPdf = new FileStream(@"..\..\..\..\OutputDocuments\vaf1a.pdf", FileMode.Open, FileAccess.Read ))
            using (FileStream outPdf = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument(inPdf);
                
                Console.WriteLine("Pages count (before): " + document.Pages.Count);
                if (document.Pages.Count != 2)
                {
                    document.Pages.Remove(document.Pages[1]);// remove 2nd page
                    document.Save(outPdf);
                }
                Console.WriteLine("Pages count (after): " + document.Pages.Count);
                Console.ReadKey();
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}