namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Content;

    // This sample shows how to add or insert pages to your PDF documents.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\AddNewPages.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                
                // AddPage method adds a new page to the end of the PDF document (by default)
                document.Pages.Add(new Page(new PageBoundary(new Boundary(0,0,210,297))));

                // AddPage method adds a new page to the end of the PDF document (2nd page letter papersize)
                document.Pages.Add(new Page(new PageBoundary(Boundaries.Letter)));

                document.Pages[0].Rotate = PageRotate.Rotate90;
                TextObject to = new TextObject("TimesNewRoman", 12);
                to.AppendText("Hello world!");
                document.Pages[0].Content.AppendText(to);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
