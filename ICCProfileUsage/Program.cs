using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.ErrorHandling;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.FixedLayout.Resources.ColorSpaces.CIEBased;
using Path = Apitron.PDF.Kit.FixedLayout.Content.Path;

namespace ICCProfileUsage
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Stream outputStream = File.Create("icccolors.pdf"))
            {
                using (FixedDocument doc = new FixedDocument())
                {
                    // register CMYK profile "US Web Uncoated v2"
                    // you can get it from Adobe website
                    string profileName = "US Web Uncoated v2";
                    doc.ResourceManager.RegisterResource(new ICCBasedColorSpace(profileName, File.ReadAllBytes("../../data/USWebUncoated.icc")));

                    // create and add new page
                    Page page = new Page();
                    doc.Pages.Add(page);

                    // create rectangular shape
                    Path rectangle = new Path();
                    rectangle.AppendRectangle(10,700,200,100);
                    
                    // RECT 1
                    // select CMYK colorspace for drawing using loaded color profile
                    page.Content.SetNonStrokingColorSpace(profileName);
                    page.Content.SetStrokingColorSpace(profileName);

                    // select fill and stroke colors
                    page.Content.SetNonStrokingColor(new double[]{0,1,0,0});
                    page.Content.SetStrokingColor(new double[]{0,0,0,1});

                    // fill and stroke the path
                    page.Content.FillAndStrokePath(rectangle);
                    
                    //RECT 2
                    // select colors using device CMYK colorspace, the viewer will
                    // use default CMYK profile for representing these colors
                    page.Content.SetDeviceNonStrokingColor(new double[] { 0, 1, 0, 0 });
                    page.Content.SetDeviceStrokingColor(new double[] { 0, 0, 0, 1 });

                    // fill and stroke the path again
                    page.Content.Translate(0,-120);
                    page.Content.FillAndStrokePath(rectangle);
                    
                    //save document
                    doc.Save(outputStream);
                }
            }
        }
    }
}
