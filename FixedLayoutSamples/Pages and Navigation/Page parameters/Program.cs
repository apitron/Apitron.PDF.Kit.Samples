namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Resources.XObjects;

    // This sample shows how to setup a page size, orientation or rotation.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\PageParameters.pdf";
            // open and load the file
            using (FileStream inPdf = new FileStream(@"..\..\..\..\OutputDocuments\vaf1a.pdf", FileMode.Open, FileAccess.Read))
            using (FileStream outPdf = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument(inPdf);

                // Resize page
                Page page = document.Pages[0];
                page.Resize(new PageBoundary(Boundaries.A3));
                page.Transform(1, 0, 0, 1, 100, 100);

                // Rotate page
                Page page2 = document.Pages[1];
                page2.Rotate = PageRotate.Rotate90;

                // Transform page
                Page page3 = document.Pages[2];
                page3.Transform(0.5, 0, 0, 0.5, 0, 0);
                page3.Transform(1, 0, 0, 1, 100, 100);
                

                // Modify page
                FixedContent stamp = new FixedContent("Stamp", new Boundary(0, 0, 100, 100));
                stamp.Content.SetDeviceNonStrokingColor(new double[] { 0.5, 0.8, 0.1 });
                FixedLayout.Content.Path path = new FixedLayout.Content.Path();
                path.AppendRectangle(10, 10, 80, 80);
                stamp.Content.FillPath(path);

                // Register new resource
                document.ResourceManager.RegisterResource(stamp);

                // Add new object on page
                Page page4 = document.Pages[3];
                page4.Content.ModifyCurrentTransformationMatrix(1, 0, 0, 1, 100, 100);
                page4.Content.AppendXObject("Stamp");

                document.Pages.Add(page);
                document.Pages.Add(page2);
                document.Pages.Add(page3);
                document.Pages.Add(page4);
                document.Save(outPdf);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
