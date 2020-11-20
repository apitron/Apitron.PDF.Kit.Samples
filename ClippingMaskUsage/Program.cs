using System.Diagnostics;
using System.IO;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.FixedLayout.Content;
using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;
using Apitron.PDF.Kit.FixedLayout.Resources.XObjects;
using Apitron.PDF.Kit.Styles;
using Path = Apitron.PDF.Kit.FixedLayout.Content.Path;

namespace ClippingMaskUsage
{
    class Program
    {
        // demonstrates how to use text string as clipping path
        public static ClippedContent DrawContentUsingTextClipping()
        {
            ClippedContent clippedContent = new ClippedContent(0,0,200,200);            

            // create text object
            TextObject clipText = new TextObject(StandardFonts.HelveticaBold, 30);            
            // set text rendering mode that applies clipping
            clipText.SetTextRenderingMode(RenderingMode.SetAsClipping);
            clipText.AppendText("Text clipping!");
            
            // set current fill color
            clippedContent.SetDeviceNonStrokingColor(RgbColors.Red.Components);
            // position the text
            clippedContent.SetTranslation(0, 20);
            clippedContent.AppendText(clipText);
            clippedContent.SetTranslation(0, -30);
            // draw image through our clipping
            clippedContent.AppendImage("gradient",0,0,200,200);

            return clippedContent;
        }

        // demonstrates how to use regular path as clipping path
        public static ClippedContent DrawContentUsingClippingPath()
        {
            // create base clipping path comprising two circles drawn 
            // in different directions one inside another
            Path clippingPath = new Path();
            clippingPath.AppendPath(Path.CreateCircle(150, 600, 100));
            clippingPath.AppendPath(Path.CreateCircle(150, 600, 50, false));

            // create clipped content object and set its clipping path,
            // we also set the clipping rule to even-odd to get the
            // donut-shaped clipping area
            ClippedContent clippedContent = new ClippedContent(clippingPath, FillRule.EvenOdd);

            // set current fill color
            clippedContent.SetDeviceNonStrokingColor(RgbColors.Red.Components);

            // draw rectanle through our clipping
            clippedContent.FillPath(Path.CreateRect(50, 500, 300, 200));

            return clippedContent;
        }

        static void Main(string[] args)
        {            
            // create document and register image resource
            FixedDocument doc = new FixedDocument();
            doc.ResourceManager.RegisterResource(new Image("gradient","../../data/gradient.jpg"));
            
            // create page and append our clipped contents to it
            Page page = new Page();
            page.Content.AppendContent(DrawContentUsingClippingPath());             
            page.Content.SetTranslation(250,700);
            page.Content.AppendContent(DrawContentUsingTextClipping());

            // append page to document and save it
            doc.Pages.Add(page);

            using (Stream stream = File.Create("clippedContent.pdf"))
            {
                doc.Save(stream);
            }

            Process.Start("clippedContent.pdf");
        }
    }
}