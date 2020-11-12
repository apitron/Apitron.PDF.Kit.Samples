namespace SoftMasks
{
    using System.Diagnostics;
    using System.IO;

    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.Content;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Resources.GraphicsStates;
    using Apitron.PDF.Kit.FixedLayout.Resources.Transparency;

    using Path = Apitron.PDF.Kit.FixedLayout.Content.Path;

	// This sample shows how to draw using soft mask.
    internal class Program
    {
        private static void Main( string[] args )
        {
            string IMG1 = "IMG1";
            string IMG0 = "IMG0";
                        
            using (FileStream stream = new FileStream( @"..\..\..\..\OutputDocuments\SoftMasks.pdf", FileMode.Create ))
            {
                FixedDocument document = new FixedDocument();

                Page page = new Page();
                
                // Alpha source
                Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image im1 = new Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image( IMG1, @"..\..\..\..\OutputDocuments\softMask.png" );
                document.ResourceManager.RegisterResource( im1 );

                Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image im0 = new Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image( IMG0, @"..\..\..\..\OutputDocuments\image.jpg" );
                // To have alpha for the alpha soft mask
                im0.SoftMaskResourceID = IMG1;
                document.ResourceManager.RegisterResource( im0 );

                DrawUsingAlphaSoftMask(IMG0, document, page);
                DrawUsingLuminositySoftMask(IMG0, document, page);

                document.Pages.Add( page );
                document.Save( stream );
            }
            Process.Start( @"..\..\..\..\OutputDocuments\SoftMasks.pdf" );
        }

        private static void DrawUsingLuminositySoftMask(string IMG0, FixedDocument document, Page page)
        {
            Content pageContent = page.Content;
            Boundary smBoundary = new Boundary(0, 0, page.Boundary.MediaBox.Width, page.Boundary.MediaBox.Height / 2);
            Path path = new Path();
            path.AppendRectangle(0, 0, smBoundary.Width, smBoundary.Height);

            string SM1 = "SM1";
            string GS1 = "GS1";

            SoftMask sm1 = new SoftMask(SM1, SoftMaskSubtype.Luminosity, smBoundary);
            sm1.BackgroundColor = new double[] { 0, 0, 0 };

            sm1.Group.Content.AppendImage(IMG0, 0, 0, smBoundary.Width, smBoundary.Height);
            document.ResourceManager.RegisterResource(sm1);

            GraphicsState gs1 = new GraphicsState(GS1);
            gs1.SoftMaskResourceID = SM1;
            gs1.LineWidth = 10;
            document.ResourceManager.RegisterResource(gs1);

            pageContent.SaveGraphicsState();
            pageContent.SetGraphicsState(GS1);
            pageContent.SetDeviceNonStroking(1, 0, 0);
            pageContent.SetDeviceStroking(1, 1, 0);
            pageContent.FillAndStrokePath(path);
            pageContent.RestoreGraphicsState();
        }

        private static void DrawUsingAlphaSoftMask(string IMG0, FixedDocument document, Page page)
        {
            Content pageContent = page.Content;
            Boundary smBoundary = new Boundary(0, 0, page.Boundary.MediaBox.Width, page.Boundary.MediaBox.Height / 2);
            Path path = new Path();
            path.AppendRectangle(0, 0, smBoundary.Width, smBoundary.Height);

            string SM0 = "SM0";
            string GS0 = "GS0";

            SoftMask sm0 = new SoftMask(SM0, SoftMaskSubtype.Alpha, smBoundary);
            sm0.Group.Content.AppendImage(IMG0, 0, 0, smBoundary.Width, smBoundary.Height);
            document.ResourceManager.RegisterResource(sm0);

            GraphicsState gs0 = new GraphicsState(GS0);
            gs0.SoftMaskResourceID = SM0;
            gs0.LineWidth = 10;
            document.ResourceManager.RegisterResource(gs0);

            pageContent.SaveGraphicsState();
            pageContent.Translate(0, smBoundary.Height);
            pageContent.SetGraphicsState(GS0);
            pageContent.SetDeviceNonStroking(0, 0, 1);
            pageContent.SetDeviceStroking(0, 1, 1);
            pageContent.FillAndStrokePath(path);
            pageContent.RestoreGraphicsState();
        }
    }
}
