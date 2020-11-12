namespace Apitron.PDF.Kit.Samples
{
    using System;
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Resources.GraphicsStates;
    using Apitron.PDF.Kit.FixedLayout.Content;
    using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;

    // This sample shows how to save and restore graphics state.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\GraphicsState.pdf";

            // create new PDF file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                Page page = new Page(new PageBoundary(Boundaries.A4));
                document.Pages.Add(page);

                Font font = new Font("S1", StandardFonts.TimesItalic);
                document.ResourceManager.RegisterResource(font);

                // register graphics state
                GraphicsState gs = new GraphicsState("gs01");
                document.ResourceManager.RegisterResource(gs);
                gs.FontResourceID = "S1";
                gs.FontSize = 14;

                Content content = document.Pages[0].Content;

                // default font will be used from the graphics state resource
                content.SetGraphicsState("gs01");

                // draw text
                for (double angle = 0; angle <= 4*3.14; angle+=0.2)
                {                    
                    content.SaveGraphicsState();
                    content.ModifyCurrentTransformationMatrix(Math.Cos(angle), -Math.Sin(angle), Math.Sin(angle), Math.Cos(angle), 300, 300);
                    TextObject text = new TextObject();
                    text.SetTextMatrix(1, 0, 0, 1, 300/(angle+1), 0);
                    text.SetTextRenderingMode(RenderingMode.FillText);
                    text.AppendText("Hello world!");
                    content.AppendText(text);
                    content.RestoreGraphicsState();
                }
                
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
