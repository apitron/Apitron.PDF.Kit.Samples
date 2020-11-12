namespace TransparencyGroups
{
    using System.Diagnostics;
    using System.IO;

    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.Content;
    using Apitron.PDF.Kit.FixedLayout.Resources;
    using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;
    using Apitron.PDF.Kit.FixedLayout.Resources.GraphicsStates;
    using Apitron.PDF.Kit.FixedLayout.Resources.Transparency;

    using Path = Apitron.PDF.Kit.FixedLayout.Content.Path;

	// This sample shows how to draw using transparency groups ( semi-transparent colors ).
    internal class Program
    {
        private static void Main( string[] args )
        {
            using (FileStream stream = new FileStream(@"..\..\..\..\OutputDocuments\TransparencyGroups.pdf", FileMode.Create))
            {
                FixedDocument document = new FixedDocument();
                Page page = new Page();

                // Isolated Knockout
                TransparencyGroup IKGroup = CreateGroup("IK", document.ResourceManager, new Boundary(0, 0, 200, 200), 1, true, true, BlendMode.Multiply);
                document.ResourceManager.RegisterResource(IKGroup);

                // Isolated Non-Knockout
                TransparencyGroup InKGroup = CreateGroup("InK", document.ResourceManager, new Boundary(0, 0, 200, 200), 1, true, false, BlendMode.Multiply);
                document.ResourceManager.RegisterResource(InKGroup);

                // Non-Isolated Knockout
                TransparencyGroup nIKGroup = CreateGroup("nIK", document.ResourceManager, new Boundary(0, 0, 200, 200), 1, false, true, BlendMode.Multiply);
                document.ResourceManager.RegisterResource(nIKGroup );

                // Non-Isolated Non-Knockout
                TransparencyGroup nInKGroup = CreateGroup("nInK", document.ResourceManager, new Boundary(0, 0, 200, 200), 1, false, false, BlendMode.Multiply);
                document.ResourceManager.RegisterResource(nInKGroup );

                Path background = new Path();
                background.AppendRectangle(0, -200, page.Boundary.MediaBox.Width, page.Boundary.MediaBox.Height);

                Path background1 = new Path();
                background1.AppendRectangle(150, 150, 350, 350);                


                Content pageContent = page.Content;
                pageContent.Translate(0, 200);

                pageContent.SaveGraphicsState();

                pageContent.SetDeviceNonStroking(0.5);
                pageContent.FillPath(background);

                pageContent.SetDeviceNonStroking(1);
                pageContent.FillPath(background1);


                pageContent.Translate(100, 100);
                pageContent.AppendXObject("nIK");

                pageContent.Translate( 250, 0 );
                pageContent.AppendXObject("nInK");

                pageContent.Translate(-250, 250);
                pageContent.AppendXObject("IK");

                pageContent.Translate(250, 0);
                pageContent.AppendXObject("InK");

                pageContent.RestoreGraphicsState();

                ShowLabels(pageContent);

                document.Pages.Add(page);
                document.Save(stream);
            }
            Process.Start(@"..\..\..\..\OutputDocuments\TransparencyGroups.pdf");
        }

        private static TransparencyGroup CreateGroup(string resourceID, ResourceManager resourceManager, Boundary boundary, double objectOpacity, bool isIsolated, bool isKnockout, BlendMode blendMode)
        {
            TransparencyGroup group = new TransparencyGroup(resourceID, boundary);
            group.Attributes.IsIsolated = isIsolated;
            group.Attributes.IsKnockout = isKnockout;

            string gsResourceID = resourceID + "gs01";
            GraphicsState gs = new GraphicsState(gsResourceID);
            gs.CurrentNonStrokingAlpha = gs.CurrentStrokingAlpha = objectOpacity;
            gs.BlendMode = blendMode;
            gs.LineWidth = 10;
            resourceManager.RegisterResource(gs);

            double width = boundary.Width - boundary.Width / 3;
            double height = boundary.Height - boundary.Height / 3;
            Path path = new Path();
            path.AppendRectangle(0, 0, width, height);

            group.Content.SetGraphicsState(gsResourceID);

            group.Content.SetDeviceNonStroking(0, 0, 1);
            group.Content.FillAndStrokePath(path);

            group.Content.Translate(width / 2, 0);
            group.Content.SetDeviceNonStroking(0, 1, 0);
            group.Content.FillAndStrokePath(path);

            group.Content.Translate(-width / 4, height / 2);
            group.Content.SetDeviceNonStroking(1, 0, 0);
            group.Content.FillAndStrokePath(path);

            return group;
        }

        private static void ShowLabels( Content pageContent )
        {
            TextObject isolatedText = new TextObject( StandardFonts.Helvetica, 14 );
            isolatedText.SetTextMatrix( 1, 0, 0, 1, 10, 450 );
            isolatedText.AppendText( "Isolated" );

            TextObject nonIsolatedText = new TextObject( StandardFonts.Helvetica, 14 );
            isolatedText.SetTextMatrix( 1, 0, 0, 1, 10, 200 );
            isolatedText.AppendText( "Non-Isolated" );

            TextObject knockputText = new TextObject( StandardFonts.Helvetica, 14 );
            isolatedText.SetTextMatrix( 1, 0, 0, 1, 120, 70 );
            isolatedText.AppendText( "Knockout" );

            TextObject nonKnockputText = new TextObject( StandardFonts.Helvetica, 14 );
            isolatedText.SetTextMatrix( 1, 0, 0, 1, 370, 70 );
            isolatedText.AppendText( "Non-Knockout" );

            pageContent.SetDeviceNonStroking( 0 );
            pageContent.AppendText( isolatedText );
            pageContent.AppendText( nonIsolatedText );
            pageContent.AppendText( knockputText );
            pageContent.AppendText( nonKnockputText );
        }
    }
}
