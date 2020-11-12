namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Content;
    using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;

	// This sample shows how to increase or decrease the size of a text line using horizontal scaling.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\HorizontalScaling.pdf";

            // create new PDF file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();

                // add new page
                Page page = new Page(new PageBoundary(Boundaries.A4));
                document.Pages.Add(page);

                // add text object
                TextObject text = new TextObject("Helvetica", 14);
                text.SetFont(StandardFonts.TimesItalic, 14);
                text.SetTextMatrix(1, 0, 0, 1, 100, 600);

                // set horizontal scaling
                text.SetHorizontalScaling(200);
                text.AppendText("Hello world!");

                document.Pages[0].Content.AppendText(text);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
