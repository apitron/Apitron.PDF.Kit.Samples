namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Content;
    using Apitron.PDF.Kit.FixedLayout.Resources;
    using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;

	// This sample shows how to use fonts.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\Fonts.pdf";

            // create new PDF file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // register font in Resource Manager and add font resources
                ResourceManager rm = new ResourceManager();
                Font font = new Font("S1", StandardFonts.Courier);
                rm.RegisterResource(font);
                Font font2 = new Font("S2", "TimesNewRoman");
                rm.RegisterResource(font2);
                
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument(rm);

                // add new page
                Page page = new Page(new PageBoundary(Boundaries.A4));
                document.Pages.Add(page);
                
                // create text object
                TextObject text = new TextObject("S1", 18);
                text.SetTextMatrix(1, 0, 0, 1, 10, 450);
                text.AppendText("Hello world!");
                text.SetFont(StandardFonts.TimesItalic, 18);
                text.SetTextMatrix(1, 0, 0, 1, 10, 500);
                text.AppendText("Hello world!");
                text.SetFont("S2", 18);
                text.SetTextMatrix(1, 0, 0, 1, 10, 550);
                text.AppendText("Hello world!");
                
                // add text
                document.Pages[0].Content.AppendText(text);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
