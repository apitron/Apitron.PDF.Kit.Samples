namespace Apitron.PDF.Kit.Samples
{
    using System;
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;

    // This sample shows how to list names of all files attached to PDF document.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string test_file = @"..\..\..\..\OutputDocuments\testfile.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(@"..\..\..\..\OutputDocuments\ListAttachmentsAndFileAnnotationsNames.pdf", FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();

                // add page
                document.Pages.Add(new Page(new PageBoundary(Boundaries.A4)));
                
                // add attachment
                document.Names.Attachments.Add("Attachment # 1", new EmbeddedFile(test_file, "type/pdf"));
                document.Names.Attachments.Add("Attachment # 2", new EmbeddedFile(test_file, "type/pdf"));
                document.Names.Attachments.Add("Attachment # 3", new EmbeddedFile(test_file, "type/pdf"));

                // save document
                document.Save(fs);


                // get list of attachments
                Console.WriteLine("The count of attachments is : " + document.Names.Attachments.Count);
                Console.WriteLine("List of attachments : ");
                foreach (var attachment in document.Names.Attachments)
                {
                    Console.WriteLine(attachment.Key);
                }

                Console.ReadKey();
            }
        }
    }
}
