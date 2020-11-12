namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Resources;

    // This sample shows how to move (reorder) and clear pages in a PDF document.
    internal class Program
    {
        private static void Main(string[] args)
        {
            // open and load the file
            using (FileStream inPdf = new FileStream(@"..\..\..\..\OutputDocuments\vaf1a.pdf", FileMode.Open, FileAccess.Read))
            using (FileStream outPdf = new FileStream(@"..\..\..\..\OutputDocuments\MovePages.pdf", FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument(inPdf, new ResourceManager());

                Page page = document.Pages[2];

                // remove 3rd page
                document.Pages.Remove(page);

                // insert 3rd page as 11th page
                document.Pages.Insert(10, page);
                
                // clear 4th page
                document.Pages[3].Clear();

                // resize 5th page
                document.Pages[4].Resize(new PageBoundary(Boundaries.Ledger));

                document.Save(outPdf);
            }

            System.Diagnostics.Process.Start(@"..\..\..\..\OutputDocuments\MovePages.pdf");
        }
    }
}
