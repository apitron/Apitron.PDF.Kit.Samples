namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using System.Diagnostics;

    using Apitron.PDF.Kit.FixedLayout.Resources.GraphicsStates;

    // This sample shows how to draw on a canvas using semi-transparent colors.
    internal class Program
    {
        private static void Main(string[] args)
        {
            // open and load the file
            using (FileStream fs = new FileStream(@"..\..\..\..\OutputDocuments\Transparency.pdf", FileMode.Create))
            {
                // this objects represents a PDF fixed document
                FixedDocument document = new FixedDocument();

                GraphicsState gs = new GraphicsState("gs01");
                document.ResourceManager.RegisterResource(gs);

                // Set stroking & non stroking alpha
                gs.CurrentNonStrokingAlpha = 0.5;
                gs.CurrentStrokingAlpha = 0.5;
                gs.LineWidth = 10;

                Page page = new Page();
                page.Content.SetGraphicsState("gs01");

                FixedLayout.Content.Path path = new FixedLayout.Content.Path();
                path.AppendRectangle( 100, 100, 200, 200 );

                page.Content.SetDeviceNonStroking( 0, 0, 1 );
                page.Content.FillAndStrokePath( path );

                page.Content.ModifyCurrentTransformationMatrix( 1, 0, 0, 1, 100, 0 );
                page.Content.SetDeviceNonStroking( 0, 1, 0 );
                page.Content.FillAndStrokePath( path );

                page.Content.ModifyCurrentTransformationMatrix( 1, 0, 0, 1, -50, 100 );
                page.Content.SetDeviceNonStroking( 1, 0, 0 );
                page.Content.FillAndStrokePath( path );

                document.Pages.Add(page);
                document.Save(fs);
            }

            Process.Start(@"..\..\..\..\OutputDocuments\Transparency.pdf");
        }
    }
}
