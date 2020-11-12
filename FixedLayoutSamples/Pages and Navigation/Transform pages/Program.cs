#region

using System.Diagnostics;
using System.IO;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.FixedLayout.Content;
using Apitron.PDF.Kit.FixedLayout.PageProperties;
using Apitron.PDF.Kit.FixedLayout.Resources;

#endregion

namespace Apitron.PDF.Kit.Samples
{
    // This sample shows how to transform page orientation in a PDF document.
    internal class Program
    {
        private static void Main(string[] args)
        {
            // open and load the file
            using (FileStream inPdf = new FileStream(@"..\..\..\..\OutputDocuments\vaf1a.pdf", FileMode.Open, FileAccess.Read))
            using (FileStream outPdf = new FileStream(@"..\..\..\..\OutputDocuments\TransformPages.pdf", FileMode.Create, FileAccess.ReadWrite))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument(inPdf, new ResourceManager());

                for (int i = 0; i < document.Pages.Count - 1; i++)
                {
                    // left page in our booklet
                    Page page1 = document.Pages[i];
                    // right page in our booklet
                    Page page2 = document.Pages[i + 1];
                    
                    // we'll transform original mediabox 
                    Boundary mediaBox = page1.Boundary.MediaBox;
                    Boundary newMediaBox = new Boundary(mediaBox.Left, mediaBox.Bottom,
                                                        mediaBox.Right + page2.Boundary.MediaBox.Width, mediaBox.Top);
                    
                    // we'll transform original cropbox 
                    Boundary cropBox = page1.Boundary.CropBox;
                    Boundary newCropBox = new Boundary(cropBox.Left, cropBox.Bottom,
                                                       cropBox.Right + page2.Boundary.CropBox.Width, cropBox.Top);

                    // apply transformation for the page size and content
                    page1.Resize(new PageBoundary(newMediaBox, newCropBox));
                    page1.Content.SetTranslation(mediaBox.Width, 0);

                    // move page's content
                    page1.Content.AppendContent(page2.Content);

                    // remove outdated 2nd page
                    document.Pages.Remove(page2);
                }
                // save document
                document.Save(outPdf);
            }

            Process.Start(@"..\..\..\..\OutputDocuments\TransformPages.pdf");
        }
    }
}
