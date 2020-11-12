namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;


    // This samples shows how to use color range to mask parts of the image.
    class Program
    {
        static void Main( string[] args )
        {
            string out_path = @"..\..\..\..\OutputDocuments\MaskedByColorRange.pdf";

            using (FileStream stream = new FileStream(out_path, FileMode.Create))
            {
                // create document
                FixedDocument document = new FixedDocument();

                // register image resource
                string resourceID = "IMG0";
                Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image image = new Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image(resourceID, @"..\..\..\..\OutputDocuments\image.jpg");
                document.ResourceManager.RegisterResource(image);
               
                // Red colors will be masked.
                image.MaskColorRanges = new double[] {100, 255, 0, 255, 0, 255};
                image.UseInvertedDecode = false;

                // add boundaries
                Boundary boundary = new Boundary(0, 0, image.Width + 20, image.Height + 60);
                Page page = new Page(new PageBoundary(boundary));
                page.Content.AppendImage(resourceID, 10, 50, image.Width, image.Height);

                document.Pages.Add(page);
                document.Save(stream);
            }
            System.Diagnostics.Process.Start(out_path);
        }
    }
}
