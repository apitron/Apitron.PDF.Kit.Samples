namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;

	// This sample shows how to change metadata of a PDF document (properties such as "Author", "Title").
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\DocumentProperties.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this objects represents a PDF fixed document
                FixedDocument document = new FixedDocument
                {
                    Author   = "Apitron Developer",
                    Keywords = "C# PDF Report",
                    Subject  = "Annual Report 2015",
                    Title    = "Annual Report"
                };
                document.Pages.Add(new Page(new PageBoundary(Boundaries.A4)));
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
