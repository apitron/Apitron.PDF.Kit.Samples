using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout.Resources;
using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;
using Apitron.PDF.Kit.FlowLayout.Content;
using Apitron.PDF.Kit.Styles;
using Apitron.PDF.Kit.Styles.Appearance;
using Font = Apitron.PDF.Kit.Styles.Text.Font;

namespace PageHeaderAndFooterGeneration
{
    class Program
    {
        static void Main(string[] args)
        {
            // register doc's resources first
            ResourceManager resourceManager = new ResourceManager();
            resourceManager.RegisterResource(new Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image("logo","../../data/logo.png"));

            // create document
            FlowDocument doc = new FlowDocument(){Margin = new Thickness(10)};

            // register styles
            doc.StyleManager.RegisterStyle(".pageHeader",new Style(){Font = new Font(StandardFonts.TimesBold, 20)});
            doc.StyleManager.RegisterStyle(".pageFooter",new Style(){Align = Align.Right});
            doc.StyleManager.RegisterStyle("hr",new Style(){Height = 2, Margin = new Thickness(0,5,0,5)});
            doc.StyleManager.RegisterStyle(".content",new Style(){Align = Align.Justify, Display = Display.InlineBlock});
            
            // fill the header section
            doc.PageHeader.Class = "pageHeader";
            doc.PageHeader.Add(new Image("logo"){Width = 100, Height = 50});
            doc.PageHeader.Add(new TextBlock("This document is intended for internal use only"){TextIndent = 20}); 
            doc.PageHeader.Add(new Hr());
            
            // fill the footer section
            doc.PageFooter.Class = "pageFooter";
            doc.PageFooter.Add(new Hr());
            doc.PageFooter.Add(new TextBlock((ctx)=>string.Format("Page {0} from&nbsp;",ctx.CurrentPage+1)));
            doc.PageFooter.Add(new PageCount(3){Display = Display.Inline});

            // add pages
            for (int i = 0; i < 2; ++i)
            {
                doc.Add(new TextBlock(strings.LoremIpsum) {Class = "content"});
                doc.Add(new PageBreak());
            }            

            // generate PDF
            using (Stream stream = File.Create("out.pdf"))
            {
                doc.Write(stream, resourceManager);
            }

            Process.Start("out.pdf");
        }
    }
}
