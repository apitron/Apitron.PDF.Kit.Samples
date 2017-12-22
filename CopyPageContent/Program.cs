using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.FixedLayout.PageProperties;
using Apitron.PDF.Kit.FlowLayout;

namespace CopyPageContent
{
    class Program
    {
        static void Main(string[] args)
        {
            // open source and destination docs
            using (FixedDocument sourceDocument = new FixedDocument(File.OpenRead("../../data/source.pdf")), 
                destinationDocument = new FixedDocument(File.OpenRead("../../data/destination.pdf")))
            {
                // just insert the desired source page to the
                // destination doc
                // TIP: you can use the same doc as source and destination
                destinationDocument.Pages.Add(sourceDocument.Pages[0]);

                // another nice trick is the ability to create page
                // thumbnails", e.g. as below.
                Page page = new Page();

                // we'll use for calculating transforms below
                double scaleFactor = 0.5;
                // number of thumbnails in a row and column
                int numberOfThumbnails = (int)Math.Round(1.0/scaleFactor);

                // generate the thumnails, basically fit the source page
                // as many times as we can based on scalefactor
                for (int j = 0; j <numberOfThumbnails; j++)
                {
                    double xOffset = page.Boundary.MediaBox.Width*(scaleFactor*j);
                    for (int i = 1; i <= numberOfThumbnails; ++i)
                    {
                        page.Content.SaveGraphicsState();
                        page.Content.SetTranslate(xOffset, page.Boundary.MediaBox.Height*(1 - scaleFactor*i));
                        page.Content.SetScale(scaleFactor, scaleFactor);
                        page.Content.AppendContent(sourceDocument.Pages[0].Content);
                        page.Content.RestoreGraphicsState();
                    }
                }
                // add new page
                destinationDocument.Pages.Add(page);

                // save and open for preview
                using (Stream outputStream = File.Create("combined.pdf"))
                {
                    destinationDocument.Save(outputStream);
                }

                Process.Start("combined.pdf");
            }
        }
    }
}
