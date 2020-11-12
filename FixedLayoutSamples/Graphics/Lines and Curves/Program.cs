namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Content;
    using Apitron.PDF.Kit.FixedLayout.Resources.ColorSpaces.Device;

    /// <summary>
    /// This sample shows how to draw straight lines and cubic Bezier curves.
    /// All drawings within PDF document is being done using methods and properties of <see cref="FixedLayout.Content.Path"/> class.     
    /// You create a set of drawing commands and add the resulting path to the <see cref="Page.Content"/> by filling or stroking it.
    /// If you need to apply clipping, you may create <see cref="ClippedContent"/> object based on path and draw to this clipped content.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\LinesAndCurves.pdf";
            
            // create new PDF file
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
                path.AppendRectangle(10, 10, page.Boundary.MediaBox.Width - 20, page.Boundary.MediaBox.Height - 20);
                page.Content.FillAndStrokePath(path);

                Apitron.PDF.Kit.FixedLayout.Content.Path line = new Apitron.PDF.Kit.FixedLayout.Content.Path();
                line.MoveTo(250,250);
                line.AppendLine(280,250);
                line.AppendLine(280,280);
                line.ClosePath();
                page.Content.StrokePath(line);
                
                Apitron.PDF.Kit.FixedLayout.Content.Path path2 = new Apitron.PDF.Kit.FixedLayout.Content.Path(70, 80);
                path2.AppendLine(70, 80);
                path2.AppendCubicBezier(70, 80, 140, 400, 280, 260);
                path2.ClosePath();                
                page.Content.StrokePath(path2);                

                document.Save(fs);                                
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
