namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.Interactive.Annotations;

    // This samples shows how to attach a file and add a file annotation to your PDF document.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\AddAttachmentsAndFileAnnotationsToPDF.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();

                // add page
                Page page = new Page();
                document.Pages.Add(page);

                // create attachment
                FileAttachmentAnnotation attachment = new FileAttachmentAnnotation(new Boundary(10, 440, 30, 460));
                attachment.FileSpecification = new EmbeddedFile(@"..\..\..\..\OutputDocuments\testfile.pdf");
                attachment.Contents = "This is attachment annotation";

                // add attachment
                document.Pages[0].Annotations.Add(attachment);

                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
