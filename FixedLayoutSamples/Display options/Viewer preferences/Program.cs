namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;

    // This sample shows how to instruct a PDF viewer to display the pages in two columns, 
	// with odd-numbered pages on the left.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\ViewerPreferences.pdf";

            // open and load the file
            using (FileStream inPdf = new FileStream(@"..\..\..\..\OutputDocuments\vaf1a.pdf", FileMode.Open))
            using (FileStream outPdf = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument(inPdf);
                
                // change viewer preferences
                document.PageLayout = PageLayout.TwoPageLeft;
                document.PageMode   = PageMode.UseThumbs;
                document.ViewerPreferences.HideMenubar = true;
                document.ViewerPreferences.HideToolbar = true;

                document.Save(outPdf);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}