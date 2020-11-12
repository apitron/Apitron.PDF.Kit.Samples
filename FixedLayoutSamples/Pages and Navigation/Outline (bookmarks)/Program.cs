namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.Interactive.Navigation.DocumentLevel;

    // This sample shows how to build PDF document's bookmarks
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\Bookmarks.pdf";
            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();

                for (int i = 0; i < 2; i++)
                {
                    // create new page and add it to the document
                    Page page = new Page(new PageBoundary(new Boundary(0, 0, 210.0, 297.0)));
                    document.Pages.Add(page);

                    // Add simple bookmark
                    document.Bookmarks.AddLast(new Bookmark(page, $"Simple bookmark {i}"));
                }

                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
