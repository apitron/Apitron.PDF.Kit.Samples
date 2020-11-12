namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Content;
    using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;

	// This sample shows how to draw text in different languages.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\Internationalization.pdf";
            // create new pdf file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                 // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                Page page = new Page(new PageBoundary(Boundaries.A4));
                document.Pages.Add(page);
                
                Font font = new Font("S1", StandardFonts.CourierBold);
                Font simSun = new Font("S2", "PMingLiU");
      
                document.ResourceManager.RegisterResource(simSun);
                document.ResourceManager.RegisterResource(font);
                TextObject text = new TextObject("S1", 14);

                text.SetFont("SimSun", 18);
                text.SetTextMatrix(1, 0, 0, 1, 100, 800);
                text.SetTextRenderingMode(RenderingMode.FillText);
                text.SetWordSpacing(10);
                text.SetTextLeading(18);
                text.AppendText("Chinese(traditional): 世界您好");
                text.MoveToNextLine(0,-18);

                text.SetFont("Arial", 18);
                text.AppendText("Russian: Привет, мир");
                text.MoveToNextLine(0, -18);

				text.SetFont("S1", 18);
                text.AppendText("Portugal: Olá mund");

                document.Pages[0].Content.AppendText(text);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
