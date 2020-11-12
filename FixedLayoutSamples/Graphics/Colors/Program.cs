namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.Resources.ColorSpaces.Device;

    // This sample shows how to use colors.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\Colors.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                Page page = new Page();
                document.Pages.Add(page);

                // register RGB and CMYK color spaces. 
                // it's also possible to use Lab, Gray, Indexed, ICC based color spaces.
                document.ResourceManager.RegisterResource(new RgbColorSpace("CS_RGB"));
                document.ResourceManager.RegisterResource(new CmykColorSpace("CS_CMYK"));
                
                // use RGB color space
                page.Content.SetNonStrokingColorSpace("CS_RGB");
                page.Content.SetNonStrokingColor(0.33, 0.66, 0.33);

                FixedLayout.Content.Path path = new FixedLayout.Content.Path();
                path.AppendRectangle(10, 10, page.Boundary.MediaBox.Width - 20, page.Boundary.MediaBox.Height - 20);
                page.Content.FillAndStrokePath(path);

                // use CMYK color space
                page.Content.SetStrokingColorSpace("CS_CMYK");
                page.Content.SetStrokingColor(0.01, 0.99, 0.33, 0.67);

                page.Content.ModifyCurrentTransformationMatrix(-1, 0, 0, -1, 500, 500);

                // draw path
                FixedLayout.Content.Path path2 = new FixedLayout.Content.Path(70, 80);
                path2.MoveTo(75, 40);
                path2.AppendCubicBezier(75, 37,  70, 25,  50, 25);
                path2.AppendCubicBezier(20, 25,  20, 62.5,20, 62.5);
                path2.AppendCubicBezier(20, 80,  40, 102, 75, 120);
                path2.AppendCubicBezier(110,102, 130,80,  130,62.5);
                path2.AppendCubicBezier(130,62.5,130,25,  100,25);
                path2.AppendCubicBezier(85, 25,  75, 37,  75, 40);

                path2.ClosePath();
                page.Content.FillAndStrokePath(path2);

                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
