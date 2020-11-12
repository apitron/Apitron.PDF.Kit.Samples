namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;

    // This samples shows how to use images in your PDF documents.
    internal class Program
    {
        private static void Main( string[] args )
        {
            string out_path = @"..\..\..\..\OutputDocuments\AddAndDrawImages.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create, FileAccess.Write))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();

                // register image resource
                string resourceID = "IMG0";
                FixedLayout.Resources.XObjects.Image image = new FixedLayout.Resources.XObjects.Image(resourceID, @"..\..\..\..\OutputDocuments\image.jpg");
                document.ResourceManager.RegisterResource(image);

                // add boundaries
                Boundary boundary = new Boundary(0, 0, image.Width + 20, image.Height + 60);
                Page page = new Page(new PageBoundary(boundary));
                page.Content.AppendImage(resourceID, 10, 50, image.Width, image.Height);
                document.Pages.Add(page);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
