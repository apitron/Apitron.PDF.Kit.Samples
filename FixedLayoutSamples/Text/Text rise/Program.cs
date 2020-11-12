namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Content;
    using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;

	// This sample shows how to adjust baseline of text (useful for drawing superscripts or subscripts) using TextRise property.
    // Text rise specifies the distance to move the baseline up or down from its default location. 
    // A positive value for TextRise property moves the baseline up, a negative value moves the baseline down.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\TextRise.pdf";
            // create new pdf file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();

                // add new page
                Page page = new Page(new PageBoundary(Boundaries.A4));
                document.Pages.Add(page);

                // add new text object
                TextObject text = new TextObject(StandardFonts.CourierBold, 18);
                text.SetTextMatrix(1, 0, 0, 1, 100, 400);

                // first formula
                text.AppendText("E(t) = m(t)c");
                text.SetTextRise(10);
                text.SetFont(StandardFonts.Courier, 10);
                text.AppendText("2");

                // set default position
                text.SetTextRise(0);

                // move to the next line
                text.MoveToNextLine(0, -30);
                
                // second formula
                text.SetFont(StandardFonts.CourierBold, 18);
                text.AppendText("k=eV");
                text.SetTextRise(-8);
                text.SetFont(StandardFonts.Courier, 10);
                text.AppendText("H");

                text.SetTextRise(0);
                text.SetFont(StandardFonts.CourierBold, 18);
                text.AppendText("/m");
                text.SetTextRise(-6);
                text.SetFont(StandardFonts.Courier, 10);
                text.AppendText("0");

                text.SetTextRise(0);
                text.SetFont(StandardFonts.CourierBold, 18);
                text.AppendText("c");
                text.SetTextRise(10);
                text.SetFont(StandardFonts.Courier, 10);
                text.AppendText("2");
               
                document.Pages[0].Content.AppendText(text);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
