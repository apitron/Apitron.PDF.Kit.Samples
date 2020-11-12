namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Content;
    using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;

	// This sample shows how to change character spacing using CharacterSpacing property
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\CharacterSpacing.pdf";

            // create new PDF file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();

                // add pages
                Page page = new Page(new PageBoundary(Boundaries.A4));
                document.Pages.Add(page);

                // create text object with character spacing
                TextObject text = new TextObject(StandardFonts.TimesItalic, 18);
                text.SetTextMatrix(1, 0, 0, 1, 10, 450);
                text.SetTextRenderingMode(RenderingMode.FillText);
                text.SetCharacterSpacing(10);
                text.AppendText("Hello world!");

                document.Pages[0].Content.AppendText(text);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
