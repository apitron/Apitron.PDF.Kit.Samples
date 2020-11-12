namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.Resources;

    // This sample shows how to swap pages (interchange positions of two pages) in a PDF document.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\SwapPages.pdf";

            // open and load the file
            using (FileStream inPdf = new FileStream(@"..\..\..\..\OutputDocuments\vaf1a.pdf", FileMode.Open, FileAccess.Read))
            using (FileStream outPdf = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument(inPdf, new ResourceManager());

                Page page = document.Pages[0];
                Page page2 = document.Pages[4];
                document.Pages.Remove(page);
                document.Pages.Remove(page2);
				
                document.Pages.Insert(4, page);
                document.Pages.Insert(0, page2);
                document.Save(outPdf);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
