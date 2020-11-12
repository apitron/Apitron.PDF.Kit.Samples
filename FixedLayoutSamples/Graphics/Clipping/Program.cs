namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.Content;

    // This sample shows how to create clipped content.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\Clipping.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                Page page = new Page();
                document.Pages.Add(page);

                // register image resource
                FixedLayout.Resources.XObjects.Image image = new FixedLayout.Resources.XObjects.Image("Image1", @"..\..\..\..\OutputDocuments\image.jpg");
                document.ResourceManager.RegisterResource(image);
                
                // add clipped content
                ClippedContent clippedContent = new ClippedContent(0, 0, 170, 170);
                clippedContent.ModifyCurrentTransformationMatrix(1, 0, 0, 1, 0, 0);
                clippedContent.SaveGraphicsState();
                clippedContent.ModifyCurrentTransformationMatrix(170, 0, 0, 170, 0, 0);
                clippedContent.AppendXObject("Image1");
                clippedContent.RestoreGraphicsState();

                page.Content.AppendContent(clippedContent);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
