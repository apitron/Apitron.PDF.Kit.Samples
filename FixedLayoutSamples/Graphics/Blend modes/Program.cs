namespace Apitron.PDF.Kit.Samples
{
    using System;
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.Content;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using System.Diagnostics;

    using Apitron.PDF.Kit.FixedLayout.Resources;
    using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;
    using Apitron.PDF.Kit.FixedLayout.Resources.GraphicsStates;
    using Apitron.PDF.Kit.FixedLayout.Resources.XObjects;

    using Path = Apitron.PDF.Kit.FixedLayout.Content.Path;

    //This sample shows how to customize a way in which semi-transparent objects will blend on a canvas.
    internal class Program
    {
        private static void Main( string[] args )
        {
            // open and load the file
            using (FileStream fs = new FileStream( @"..\..\..\..\OutputDocuments\BlendModes.pdf", FileMode.Create ))
            {
                // this objects represents a PDF fixed document
                FixedDocument document = new FixedDocument();

                Image image = new Image( "IMG0", @"..\..\..\..\OutputDocuments\image1.jpeg" );
                document.ResourceManager.RegisterResource( image );

                Page page = new Page();

                BlendMode[] modes = (BlendMode[])Enum.GetValues( typeof( BlendMode ) );
                page.Content.Translate( 50, 50 );
                int count = 0;
                int maxCount = 6;

                for (int i = 0; i < modes.Length; i++)
                {
                    PaintImage( document.ResourceManager, page, modes[i], "IMG0" );
                    page.Content.Translate( 0, 130 );
                    count++;
                    
                    if(count == maxCount)
                    {
                        page.Content.Translate(0, -130*count);
                        page.Content.Translate(200, 0);
                        count = 0;
                    }

                }

                document.Pages.Add( page );
                document.Save( fs );
            }

            Process.Start( @"..\..\..\..\OutputDocuments\BlendModes.pdf" );
        }

        private static void PaintImage( ResourceManager resourceManager, Page page, BlendMode blendMode, string imageResourceID )
        {
            string gsID = "gs" + blendMode.ToString();
            GraphicsState gs = new GraphicsState( gsID );
            gs.BlendMode = blendMode;
            resourceManager.RegisterResource( gs );

            Path path = new Path();
            path.AppendRectangle( 0, 0, 100, 100 );

            TextObject textObject = new TextObject( StandardFonts.Helvetica, 14 );
            textObject.SetTextMatrix( 1, 0, 0, 1, 30, -15 );
            textObject.AppendText( blendMode.ToString() );

            page.Content.SaveGraphicsState();
            page.Content.SetDeviceNonStroking( 0.3, 0.3, 0.75 );
            page.Content.FillPath( path );
           
            page.Content.SetGraphicsState( gsID );
            page.Content.AppendImage( imageResourceID, 10, 10, 80, 70 );

            page.Content.SetDeviceNonStroking( 0, 0, 0 );
            page.Content.AppendText( textObject );

            page.Content.RestoreGraphicsState();
        }
    }
}
