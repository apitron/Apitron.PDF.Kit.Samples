namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Content;
    using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;

	// The text rendering mode specifies whether glyph outlines are to be 
	// stroked, filled, used as a clipping boundary, or some combination of the three.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\TextRenderingMode.pdf";
            // create new PDF file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                // add new page
                Page page = new Page(new PageBoundary(Boundaries.A4));
                document.Pages.Add(page);

                // create text object
                TextObject text = new TextObject(StandardFonts.TimesBold, 14);
                // set new text position
                text.SetTextMatrix(1, 0, 0, 1, 100, 550);

                // add text with different rendering modes
                text.SetTextRenderingMode(RenderingMode.FillText);
                text.AppendText("Hello world! ");
                text.SetTextRenderingMode(RenderingMode.FillAndStrokeText);
                text.AppendText("Hello world! ");
                text.SetTextRenderingMode(RenderingMode.StrokeText);
                text.AppendText("Hello world! ");
                text.SetTextRenderingMode(RenderingMode.Invisible);
                text.AppendText("Hello world!");

                document.Pages[0].Content.AppendText(text);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
