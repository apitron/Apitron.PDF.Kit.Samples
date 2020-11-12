

namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Content;
    using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;

	// This sample shows how to use Apitron.Pdf.Kit library to 
    // create Type3Font from the XObjects and draw some text. 
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\DrawText.pdf";

            // create new PDF file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                Page page = new Page(new PageBoundary(Boundaries.A4));
                document.Pages.Add(page);

                // register new type3 font 
                Type3Font font = new Type3Font("t3");
                document.ResourceManager.RegisterResource(font);

                // create text object based on new font
                TextObject text = new TextObject("t3", 1);
                text.SetTextMatrix(1, 0, 0, 1, 80, 680);
                text.AppendTextLine("abc");
                page.Content.AppendText(text);

                // add image resources
                FixedLayout.Resources.XObjects.Image a = new FixedLayout.Resources.XObjects.Image("a", @"..\..\..\..\OutputDocuments\a.png");
                FixedLayout.Resources.XObjects.Image b = new FixedLayout.Resources.XObjects.Image("b", @"..\..\..\..\OutputDocuments\b.png");
                FixedLayout.Resources.XObjects.Image c = new FixedLayout.Resources.XObjects.Image("c", @"..\..\..\..\OutputDocuments\c.png");
                
                // register images
                document.ResourceManager.RegisterResource(a);
                document.ResourceManager.RegisterResource(b);
                document.ResourceManager.RegisterResource(c);
                
                // add glyphs( XObject e.g. images ) into our new type3 font
                Type3FontGlyph aGlyph = new Type3FontGlyph('a', 10, 10);
                // transform our glyph from 1x1 to 10x10
                aGlyph.ModifyCurrentTransformationMatrix(10, 0, 0, 10, 0, 0); 
                aGlyph.AppendXObject("a");
                
                Type3FontGlyph bGlyph = new Type3FontGlyph('b', 10, 10);
                // transform our glyph from 1x1 to 10x10
                bGlyph.ModifyCurrentTransformationMatrix(10, 0, 0, 10, 0, 0);
                bGlyph.AppendXObject("b");
                
                Type3FontGlyph cGlyph = new Type3FontGlyph('c', 10, 10);
                // transform our glyph from 1x1 to 10x10
                cGlyph.ModifyCurrentTransformationMatrix(10, 0, 0, 10, 0, 0);
                cGlyph.AppendXObject("c");
                
                font['a'] = aGlyph;
                font['b'] = bGlyph;
                font['c'] = cGlyph;
                
                document.Pages[0].Content.AppendText(text);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
