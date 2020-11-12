

namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Content;
    using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;

	/// <summary>
    /// This sample shows how to use TextObject and append some Bi-Directional text. 
	/// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\DrawBiDiText.pdf";

            // create new PDF file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                Page page = new Page();
                document.Pages.Add(page);

                TextObject text = new TextObject("Traditional Arabic", 12);
                text.SetTextMatrix(1, 0, 0, 1, 10, 400);
                text.SetTextLeading(20);
                text.AppendText("This is simple Bi-Directional test: Hello world ! مرحبا بالعالم! End of text.");

                text.SetFont("Miriam", 12);
                text.AppendTextLine("This is one more simple Bi-Directional test: Hello world ! שלום עולם! End of text.");
                page.Content.AppendText(text);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
