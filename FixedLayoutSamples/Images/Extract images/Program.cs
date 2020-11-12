namespace Apitron.PDF.Kit.Samples
{
    using System;
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.Extraction;

	//This sample shows how to extract images from PDf document.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string in_path = @"..\..\..\..\OutputDocuments\testfile.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(in_path, FileMode.Open))
            {
                // open PDF document
                FixedDocument document = new FixedDocument(fs);
                int counter = 0;
                foreach (Page page in document.Pages)
                {
                    // extract images from selected document
                    foreach (ImageInfo extractImage in page.ExtractImages())
                    {
                        // save extracted images
                        extractImage.SaveToBitmap(new FileStream($@"..\..\..\..\OutputDocuments\extracted_image_{counter}.bmp", FileMode.Create));
                        counter++;
                    }
                }
                Console.WriteLine("Images count : {0}", counter);
            }

            // show extracted images
            System.Diagnostics.Process.Start(@"..\..\..\..\OutputDocuments\extracted_image_0.bmp");
            System.Diagnostics.Process.Start(@"..\..\..\..\OutputDocuments\extracted_image_1.bmp");
        }
    }
}
