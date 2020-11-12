namespace Apitron.PDF.Kit.Samples
{
    using System;
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;

    // This sample shows how to save/remove attachment bytes (extract attached file).
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\SaveAttachment.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                // add page
                document.Pages.Add(new Page(new PageBoundary(Boundaries.A4)));

                // add attachment
                document.Names.Attachments.Add("Attachment # 1", new EmbeddedFile(@"..\..\..\..\OutputDocuments\testfile.pdf", "type/pdf"));
                document.Names.Attachments.Add("Attachment # 2", new EmbeddedFile(@"..\..\..\..\OutputDocuments\testfile.pdf", "type/pdf"));
                document.Names.Attachments.Add("Attachment # 3", new EmbeddedFile(@"..\..\..\..\OutputDocuments\testfile.pdf", "type/pdf"));

                // save document
                document.Save(fs);
                
                // get list of attachments
                Console.WriteLine("The count of attachments before is : " + document.Names.Attachments.Count);
                Console.WriteLine("List of attachments : ");
                foreach (var attachment in document.Names.Attachments)
                {
                    Console.WriteLine(attachment.Key);
                }
            }

            using (FileStream outPdf = new FileStream(out_path, FileMode.Open))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument(outPdf);
                
                // get attached document as stream
                System.IO.Stream attachmentStream = document.Names.Attachments["Attachment # 2"].GetStream();
                
                //  remove attachment from document
                document.Names.Attachments.Remove("Attachment # 3");
                
                // get list of attachments
                Console.WriteLine("The count of attachments is : " + document.Names.Attachments.Count);
                Console.WriteLine("List of attachments : ");

                foreach (var attachment in document.Names.Attachments)
                {
                    Console.WriteLine(attachment.Key);
                }
                document.Save(outPdf);
            }
            Console.ReadKey();

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
