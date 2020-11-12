namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Content;
    using Apitron.PDF.Kit.FixedLayout.Resources.ColorSpaces.Device;

    /// <summary>
    /// This sample shows how to draw simple figures.
    /// All drawings within PDF document is being done using methods and properties of <see cref="FixedLayout.Content.Path"/> class.     
    /// You create a set of drawing commands and add the resulting path to the <see cref="Page.Content"/> by filling or stroking it.
    /// If you need to apply clipping, you may create <see cref="ClippedContent"/> object based on path and draw to this clipped content.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\Figures.pdf";
            
            // create new PDF file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                Page page = new Page(new PageBoundary(Boundaries.A4));

                string rgbID = "CS_RGB";
                document.ResourceManager.RegisterResource(new RgbColorSpace(rgbID));
                page.Content.SetNonStrokingColorSpace(rgbID);

                // draw the circle
                page.Content.SetNonStrokingColor(0.33, 0.66, 0.33);
                page.Content.FillPath(FixedLayout.Content.Path.CreateCircle(300, 400, 50, false));

                // draw the ellipse 
                page.Content.SetDeviceNonStrokingColor(0.2, 0.15, 0.98);
                page.Content.FillPath(FixedLayout.Content.Path.CreateEllipse(200, 400, 150, 50, false));

                // draw the round rect
                page.Content.SetDeviceStrokingColor(0.29, 0.85, 0.18);
                page.Content.StrokePath(FixedLayout.Content.Path.CreateRoundRect(10, 10, 300, 300, 10, 10, 5, 5, false));


                // draw the arc
                // Create the circle
                FixedLayout.Content.Path circle =  FixedLayout.Content.Path.CreateCircle(150, 50, 45);
                FixedLayout.Content.Path ellipse = FixedLayout.Content.Path.CreateEllipse(150, 50, 20, 30);

                // Initialize clipping to cut the circle
                ClippedContent content = new ClippedContent(new Boundary(200, 50));

                content.SetDeviceNonStrokingColor(0.2, 0.5, 0);
                content.SetDeviceStrokingColor(0.4, 0, 0.7);

                content.FillAndStrokePath(circle);
                content.StrokePath(ellipse);

                // Set point to draw
                page.Content.SetTranslation(100, 100);

                // Rotate on 30 degree
                page.Content.SetRotation(30 * System.Math.PI / 180);

                page.Content.AppendContent(content);


                document.Pages.Add(page);
                document.Save(fs);                             
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
