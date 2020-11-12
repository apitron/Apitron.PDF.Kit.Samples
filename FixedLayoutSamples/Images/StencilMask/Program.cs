namespace StencilMask
{
    using System.IO;

    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;

    // This sample masks page content using an stencil mask based on other image.
    class Program
    {
        static void Main( string[] args )
        {
            string out_path = @"..\..\..\..\OutputDocuments\StencilMask.pdf";

            using (FileStream stream = new FileStream(out_path , FileMode.Create))
            {
                // create document
                FixedDocument document = new FixedDocument();

                // register image
                string maskID = "IMG1";
                Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image stencilMask = new Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image(maskID, @"..\..\..\..\OutputDocuments\mask.png");
                stencilMask.IsStencilMask = true;
                document.ResourceManager.RegisterResource(stencilMask);

                // add boundaries
                Boundary boundary = new Boundary(0, 0, stencilMask.Width + 20, stencilMask.Height + 60);
                Page page = new Page(new PageBoundary(boundary));

                // add image as stencil mask
                page.Content.SetDeviceNonStrokingColor(0.4, 0.2, 0.7);
                page.Content.AppendImage(maskID, 10, 50, stencilMask.Width, stencilMask.Height);

                document.Pages.Add(page);
                document.Save(stream);
            }
            System.Diagnostics.Process.Start(out_path);
        }
    }
}
