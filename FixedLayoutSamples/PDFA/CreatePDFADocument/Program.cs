using System.Diagnostics;
using System.IO;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.FixedLayout.Content;
using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;

namespace CreatePDFADocument
{
    class Program
    {
        static void Main(string[] args)
        {
            // create document object and specify the output format
            FixedDocument doc = new FixedDocument(PdfStandard.PDFA);
            
            // create page 
            Page page = new Page();

            // create text object and append it to the page
            TextObject text = new TextObject(StandardFonts.TimesRoman, 18);
            text.AppendText("Hello world with Apitron PDF Kit (PDFA version)");

            page.Content.SaveGraphicsState();
            page.Content.SetTranslation(10,800);
            page.Content.AppendText(text);
            page.Content.RestoreGraphicsState();

            doc.Pages.Add(page);

            // save document
            using (Stream stream = File.Create(@"..\..\..\..\OutputDocuments\pdfa_document.pdf"))
            {
                doc.Save(stream);
            }

            Process.Start(@"..\..\..\..\OutputDocuments\pdfa_document.pdf");
        }
    }
}
