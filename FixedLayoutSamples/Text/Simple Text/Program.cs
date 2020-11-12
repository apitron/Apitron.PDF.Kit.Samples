namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Content;
    using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;

	// This sample shows how to use TextObject and append some text. 
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\SimpleText.pdf";

            // create new PDF file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                Page page = new Page(new PageBoundary(Boundaries.A4));
                document.Pages.Add(page);
                
                // create text object based on standard fonts
                TextObject text = new TextObject(StandardFonts.Helvetica, 16);
                text.SetTextMatrix(1, 0, 0, 1, 80, 700);
                text.AppendTextLine("celebrate");
                page.Content.AppendText(text);
                text.SetTextMatrix(1, 0, 0, 1, 80, 670);
                text.SetFont(StandardFonts.TimesBoldItalic, 16);
                text.AppendTextLine("fanciful");
                page.Content.AppendText(text);
                text.SetTextMatrix(1, 0, 0, 1, 80, 640);
                text.SetFont(StandardFonts.CourierBoldOblique, 16);
                text.AppendTextLine("groovy");


                page.Content.AppendText(text);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
