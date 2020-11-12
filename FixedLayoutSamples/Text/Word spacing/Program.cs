namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Content;
    using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;

	// This sample shows how to change word spacing using WordSpacing property
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\WordSpacing.pdf";
            // create new pdf file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                
                // add new page
                Page page = new Page(new PageBoundary(Boundaries.A4));
                document.Pages.Add(page);

                // create text object with registered font
                TextObject text = new TextObject(StandardFonts.Courier, 14);

                // set via the list of standard fonts
                text.SetFont(StandardFonts.TimesItalic, 14);

                // set font with name of registered in ResourceManager
                text.SetFont(StandardFonts.Courier, 18);

                // transform current text matrix
                text.SetTextMatrix(1, 0, 0, 1, 100, 400);
                
                // set rendering mode stroke/fill
                text.SetTextRenderingMode(RenderingMode.FillText);

                // set word spacing
                text.SetWordSpacing(10);
                text.AppendText("We are glad to see you here !");

                document.Pages[0].Content.AppendText(text);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
