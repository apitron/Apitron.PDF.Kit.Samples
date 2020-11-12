namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Resources.ColorSpaces.Device;

    // This sample shows how to construct and use graphics path.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\Path.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                Page page = new Page(new PageBoundary(Boundaries.A4));

                string rgbID = "CS_RGB";
                document.ResourceManager.RegisterResource(new RgbColorSpace(rgbID));
                page.Content.SetNonStrokingColorSpace(rgbID);
                page.Content.SetNonStrokingColor(0.33, 0.66, 0.33);

                document.Pages.Add(page);
                Apitron.PDF.Kit.FixedLayout.Content.Path path = new Apitron.PDF.Kit.FixedLayout.Content.Path();
                path.AppendRectangle(10, 10, page.Boundary.MediaBox.Width-20, page.Boundary.MediaBox.Height-20);
                page.Content.FillAndStrokePath(path);

                Apitron.PDF.Kit.FixedLayout.Content.Path path2 = new Apitron.PDF.Kit.FixedLayout.Content.Path(70, 80);
                path2.AppendLine(70,80);
                path2.AppendCubicBezier(70,80,140,400,280,260);
                path2.ClosePath();
                page.Content.StrokePath(path2);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
