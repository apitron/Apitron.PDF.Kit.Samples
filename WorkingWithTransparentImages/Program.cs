using System.Diagnostics;
using System.IO;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.FixedLayout.Content;
using Apitron.PDF.Kit.FixedLayout.Resources.XObjects;
using Path = Apitron.PDF.Kit.FixedLayout.Content.Path;

namespace WorkingWithTransparentImages
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (FileStream outputStream = new FileStream("image.pdf", FileMode.Create, FileAccess.Write))
            {
                // create new PDF document 
                FixedDocument document = new FixedDocument();
                document.Pages.Add(new Page());               

                // create image XObject and set it to use embedded transparency data 
                Image maskedImage = new Image("maskedImage", "../../images/cubes.png", true);
                document.ResourceManager.RegisterResource(maskedImage);
                
                // create page content object
                ClippedContent pageContent = document.Pages[0].Content;

                // define stripe parameters
                double stripeWidth = 20.0;
                double doubleStripeWidth = stripeWidth*2;
                // create stripe rect 
                Path path = new Path();
                path.AppendRectangle(0, 0, stripeWidth, maskedImage.Height);

                // set initial translation
                pageContent.SetTranslation(20, 300);
                // save state in order to restore it later for image
                pageContent.SaveGraphicsState();                                

                // draw gray stripes with a step of 20 points
                for (int i = 0; i < (maskedImage.Width / doubleStripeWidth); ++i)
                {                    
                    pageContent.SetDeviceNonStrokingColor(new double[] {0.3});
                    pageContent.FillPath(path);
                    pageContent.SetTranslation(doubleStripeWidth, 0);
                }
                
                pageContent.RestoreGraphicsState();

                // add image
                pageContent.AppendImage(maskedImage.ID, 0, 0, maskedImage.Width, maskedImage.Height);                              
                document.Save(outputStream);
            }

            Process.Start("image.pdf");
        }

    }
}