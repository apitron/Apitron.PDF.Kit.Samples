namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.Content;
    using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;
    using Apitron.PDF.Kit.FixedLayout.Resources.XObjects;
    using Apitron.PDF.Kit.Interactive.Annotations;

	// This sample shows how to create and use watermarks using Apitron.PDF.Kit library.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\AddWatermarksAndBackgrounds.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();

                // add new page
                Page page = new Page();
                document.Pages.Add(page);
                
                // create watermark annotation
                WatermarkAnnotation watermark = new WatermarkAnnotation(new Boundary(page.Boundary.MediaBox.Left, page.Boundary.MediaBox.Bottom, page.Boundary.MediaBox.Left + 400, page.Boundary.MediaBox.Bottom + 20), AnnotationFlags.Locked);
                
                watermark.Watermark = new FixedContent("Watermark", new Boundary(0, 0, 400, 20));
                TextObject text = new TextObject(StandardFonts.Helvetica, 20);
                text.SetTextMatrix(1, 0, 0, 1, 5, 5);
                text.AppendText("Watermark created by Apitron Ltd.");
                watermark.Watermark.Content.SetDeviceNonStrokingColor(new double[] { 0, 1, 0 });
                watermark.Watermark.Content.AppendText(text);

                watermark.FixedPrint = null;

                // add green watermark annotation on a 1st page
                document.Pages[0].Annotations.Add(watermark);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
