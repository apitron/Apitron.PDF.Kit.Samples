namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.Content;

	// This sample shows how to create and save PDF documents using Apitron.PDF.Kit library.
    // The most important classes in Apitron.Pdf.Kit library are FixedDocument and FlowDocument. 
    // Each newly created PDF document should contain at least one page, 
    // you can access it using FixedDocument.Pages collection. 
    // When you're done with document, you can save it to a file or a stream using FixedDocument.Save(FileStream fs) method.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\CreateAndSavePDFDocument.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document.
                FixedDocument document = new FixedDocument();
                
                // add page
                document.Pages.Add(new Page());

                // add some text content
                TextObject text = new TextObject("Helvetica", 38);
                text.SetTranslation(10, 550);
                text.SetTextRenderingMode(RenderingMode.FillText);
                text.AppendText("Very simple PDF creation process");

                // register some image content
                FixedLayout.Resources.XObjects.Image image = new FixedLayout.Resources.XObjects.Image("Image1", @"..\..\..\..\OutputDocuments\image.jpg");
                document.ResourceManager.RegisterResource(image);

                // append text and image
                document.Pages[0].Content.AppendText(text);
                document.Pages[0].Content.AppendImage("Image1", 10, 50, Boundaries.A4.Width-20, image.Height);
				
                // save document
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
