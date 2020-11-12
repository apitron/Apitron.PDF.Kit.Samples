namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;

    // This sample masks an image using a mask based on other image.
    class Program
    {
        static void Main( string[] args )
        {
            string out_path = @"..\..\..\..\OutputDocuments\MaskedImage.pdf";

            using (FileStream stream = new FileStream(out_path, FileMode.Create))
            {
                // create document
                FixedDocument document = new FixedDocument();

                // register image resource
                string resourceID = "IMG0";
                Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image image = new Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image(resourceID, @"..\..\..\..\OutputDocuments\image.jpg");
                document.ResourceManager.RegisterResource(image);

                // register image mask resource
                string maskID = "IMG1";
                Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image explicitMask = new Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image(maskID, @"..\..\..\..\OutputDocuments\mask.png");
                document.ResourceManager.RegisterResource(explicitMask);

                image.MaskResourceID = maskID;
                image.UseInvertedDecode = true; // set special flag

                // set boundaries
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
