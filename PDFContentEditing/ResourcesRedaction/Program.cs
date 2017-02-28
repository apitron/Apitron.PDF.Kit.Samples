using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.FixedLayout.ContentElements;
using Apitron.PDF.Kit.FixedLayout.Resources;
using Apitron.PDF.Kit.FixedLayout.Resources.ColorSpaces;
using Apitron.PDF.Kit.FixedLayout.Resources.ColorSpaces.Special;
using Apitron.PDF.Kit.FixedLayout.Resources.Patterns;
using Apitron.PDF.Kit.FixedLayout.Resources.Shadings;
using Apitron.PDF.Kit.Styles;
using Path = Apitron.PDF.Kit.FixedLayout.Content.Path;

namespace ResourcesRedaction
{
    class Program
    {
        private static string outputFileName = "out.pdf";

        static void Main(string[] args)
        {
            using (Stream inputStream = File.Open("../../../data/patternFill.pdf", FileMode.Open, FileAccess.Read))
            {
                using (FixedDocument doc = new FixedDocument(inputStream))
                {
                    // create a new tiling pattern
                    TilingPattern pattern = new TilingPattern("myNewPattern", new Boundary(0, 0, 20, 20), 25, 25);
                    pattern.Content.SetDeviceNonStrokingColor(new double[] { 0.1, 0.5, 0.7 });
                    pattern.Content.FillAndStrokePath(Apitron.PDF.Kit.FixedLayout.Content.Path.CreateCircle(10, 10,9));

                    // register new pattern as a resource
                    doc.ResourceManager.RegisterResource(pattern);

                    // replace the old pattern with new one
                    doc.ResourceManager.RegisterReplacement("myPattern","myNewPattern");

                    //save modified file
                    using (Stream outputStream = File.Create(outputFileName))
                    {
                        doc.Save(outputStream);
                    }
                }
            }

            Process.Start(outputFileName);
        }
    }
}
