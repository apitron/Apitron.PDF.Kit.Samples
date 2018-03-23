using System;
using System.IO;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.FixedLayout.Content;
using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            // create output PDF file stream
            using (FileStream outputStream = new FileStream("outfile.pdf", FileMode.Create, FileAccess.Write))
            {
                // create new document
                using(FixedDocument document = new FixedDocument())
                {
                    // add blank first page
                    document.Pages.Add(new Page(Boundaries.A4));

                    // create text object and append text to it
                    TextObject textObject = new TextObject(StandardFonts.Helvetica,16);                

                    // apply identity matrix, that doesn't change default appearance
                    textObject.SetTextMatrix(1,0,0,1,0,0);
                    textObject.AppendText("Hello world using Apitron PDF Kit and Visual Studio Code on Mac!");

                    // set current transformation matrix so text will be added to the top of the page,
                    // PDF coordinate system has Y-axis directed from bottom to top.
                    document.Pages[0].Content.SetTranslate(10, 820);

                    // add text object to page content, it will automatically create text showing operators                                
                    document.Pages[0].Content.AppendText(textObject);

                    // save to output stream
                    document.Save(outputStream);
                }
            }
        }
    }
}
