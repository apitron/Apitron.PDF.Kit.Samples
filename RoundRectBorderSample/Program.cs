using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout.Resources;
using Apitron.PDF.Kit.FlowLayout.Content;
using Apitron.PDF.Kit.Styles;
using Apitron.PDF.Kit.Styles.Appearance;

namespace RoundRectBorderSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // create document and its resource manager
            ResourceManager manager = new ResourceManager();
            FlowDocument doc = new FlowDocument(){Margin = new Thickness(10)};

            // add new text block with roudn-rect border
            doc.Add(new TextBlock("The text with round-rect border")
                    {
                        BorderRadius = 5, 
                        Border = new Border(10),
                        BorderColor = RgbColors.Red,
                        Padding = new Thickness(5)
                    });

            // save file
            using (Stream stream = File.Create("round-rect.pdf"))
            {
                doc.Write(stream, manager);
            }
        }
    }
}
