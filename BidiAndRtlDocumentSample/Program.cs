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

namespace BidiAndRtlDocumentSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // create flow document
            FlowDocument doc = new FlowDocument()
                               {
                                   Margin = new Thickness(10)
                               };

            // register style for grid rows
            doc.StyleManager.RegisterStyle("gridrow", new Style()
                {
                    Font = new Font("Traditional Arabic", 20), Align = Align.Center
                });

            // register style for header row
            doc.StyleManager.RegisterStyle("gridrow.header",new Style()
                {
                    Background = RgbColors.Gray, 
                    Color = RgbColors.White,
                    Font = new Font(StandardFonts.Helvetica, 20)
                });

            // create grid
            Grid grid = new Grid(150,Length.Auto, 150);
            
            // add header row
            grid.Add(new GridRow(new TextBlock("LTR"),new TextBlock("BIDI"), new TextBlock("RTL")){Class = "header"});

            // add content row
            grid.Add(new GridRow(
                    new TextBlock("Apitron PDF Kit"),
                    new TextBlock("PDF يخلق وثائق Apitron PDF KIT"),
                    new TextBlock("مرحبا بالعالم") // hello world
                ));

            // add grid to the document object
            doc.Add(grid);

            // generate PDF document
            using (Stream fs = File.Create("output.pdf"))
            {
                doc.Write(fs, new ResourceManager());
            }

            Process.Start("output.pdf");
        }
    }
}
