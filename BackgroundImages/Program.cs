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
using Apitron.PDF.Kit.Styles.Text;

namespace BackgroundImagesSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // create resource manager and add image resource
            ResourceManager resourceManager = new ResourceManager();            
            resourceManager.RegisterResource(new Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image("background","../../images/image.jpg"));

            // create document and define styles
            FlowDocument pdfDocument = new FlowDocument();
           
            // define style for section with id = imageBackgroud
            pdfDocument.StyleManager.RegisterStyle("section#imageBackground", new Style()
                {
                    BackgroundImage = new BackgroundImage("background"),
                    Width = Length.FromPercentage(100),
                    Height = Length.FromPercentage(100),                                        
                });

            // define style for textblocks with class "title"
            pdfDocument.StyleManager.RegisterStyle("textblock.title", new Style()
                {
                    Color = RgbColors.White,
                    Font = new Font("Arial",40),
                    Margin = new Thickness(70,395,0,0),
                });

            // add section containing text block
            pdfDocument.Add(new Section(new TextBlock("Background image sample") {Class = "title"})
                {
                    Id = "imageBackground"
                });
             
            // save document
            using (Stream stream = File.Create("out.pdf"))
            {
                pdfDocument.Write(stream,resourceManager);
            }

            Process.Start("out.pdf");
        }
    }
}
