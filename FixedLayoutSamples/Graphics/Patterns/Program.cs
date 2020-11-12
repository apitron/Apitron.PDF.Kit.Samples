namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Content;
    using Apitron.PDF.Kit.Styles;
    using Apitron.PDF.Kit.FixedLayout.Resources.Patterns;
    using Path = Apitron.PDF.Kit.FixedLayout.Content.Path;

    // This sample shows how to use colored and uncolored tiling patterns.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\Patterns.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                Page page = new Page(new PageBoundary(Boundaries.A4));
                document.Pages.Add(page);
                
                // set line caps
                page.Content.SetLineWidth(2);
                page.Content.SetLineDashPattern(new float[] { 2, 2, 4, 2 }, 3);
                page.Content.SetLineCapStyle(LineCapStyle.Round);
                page.Content.SetLineJoinStyle(LineJoinStyle.Bevel);
                page.Content.SetMitterLimit(2);

                DrawColoredTilingPattern(page, document);

                DrawUncoloredTilingPattern(page, document);

                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }

        /// <summary>
        /// Draws the uncolored tiling pattern.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="document">The document.</param>
        private static void DrawUncoloredTilingPattern(Page page, FixedDocument document)
        {
            string uncoloredRId = "UnColoredTilingPattern";
            TilingPattern uncolored = new TilingPattern(uncoloredRId, new Boundary(0, 0, 30, 20), 30, 20, false);
            Path patternPath = new Path();
            patternPath.AppendRectangle(-15, -5, 25, 7);
            patternPath.AppendRectangle(15, -5, 25, 7);
            patternPath.AppendRectangle(-15, 15, 25, 7);
            patternPath.AppendRectangle(15, 15, 25, 7);
            patternPath.AppendRectangle(0, 5, 25, 7);
            uncolored.Content.SetDeviceStrokingColor(0.4);
            uncolored.Content.FillAndStrokePath(patternPath);

            document.ResourceManager.RegisterResource(uncolored);

            // Use uncolored Pattern color to fill
            page.Content.SetNonStrokingColorSpace(PredefinedColorSpaces.RGBPattern);
            page.Content.SetNonStrokingColor(uncoloredRId, 0.1, 0.7, 0.2);

            // Use RGB color to stroke
            page.Content.SetStrokingColorSpace(PredefinedColorSpaces.RGB);
            page.Content.SetStrokingColor(0.44, 0.16, 0.38);

            // Draw rectangle
            Path path = new Path();
            path.AppendRectangle(100, 210, 200, 50);
            page.Content.FillAndStrokePath(path);

            // Set one more color for the uncolred pattern
            page.Content.SetNonStrokingColor(uncoloredRId, 1, 0, 0);

            // Draw rectangle
            path = new Apitron.PDF.Kit.FixedLayout.Content.Path();
            path.AppendRectangle(100, 260, 200, 50);
            page.Content.FillAndStrokePath(path);
        }

        /// <summary>
        /// Draws the colored tiling pattern.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="document">The document.</param>
        private static void DrawColoredTilingPattern(Page page, FixedDocument document)
        {
            // Create and register tiling patterns
            string coloredRId = "ColoredTilingPattern";

            TilingPattern colored = new TilingPattern(coloredRId, new Boundary(0, 0, 30, 20), 30, 20);
            Path patternPath = new Path();
            patternPath.AppendRectangle(-15, -5, 25, 7);
            patternPath.AppendRectangle(15, -5, 25, 7);
            patternPath.AppendRectangle(-15, 15, 25, 7);
            patternPath.AppendRectangle(15, 15, 25, 7);
            patternPath.AppendRectangle(0, 5, 25, 7);
            colored.Content.SetDeviceNonStrokingColor(0.7);
            colored.Content.FillAndStrokePath(patternPath);

            document.ResourceManager.RegisterResource(colored);

            // Use colored Pattern color to fill
            page.Content.SetNonStrokingColorSpace(PredefinedColorSpaces.Pattern);
            page.Content.SetNonStrokingColor(coloredRId);

            // Use RGB color to stroke
            page.Content.SetStrokingColorSpace(PredefinedColorSpaces.RGB);
            page.Content.SetStrokingColor(0.44, 0.16, 0.38);

            // Draw rectangle
            Apitron.PDF.Kit.FixedLayout.Content.Path path = new Apitron.PDF.Kit.FixedLayout.Content.Path();
            path.AppendRectangle(100, 100, 200, 100);
            page.Content.FillAndStrokePath(path);
        }
    }
}
