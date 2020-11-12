namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;

    // This sample shows how to copy pages within PDF document.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\fw4(copy).pdf";

            // open and load the file
            File.Copy( @"..\..\..\..\OutputDocuments\fw4.pdf", out_path, true );
            using (FileStream fs = new FileStream(out_path, FileMode.Open, FileAccess.ReadWrite))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument(fs);

                // incremental update
                Page page = document.Pages[0];
                document.Pages.Insert(0, page);
                document.Save();
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
