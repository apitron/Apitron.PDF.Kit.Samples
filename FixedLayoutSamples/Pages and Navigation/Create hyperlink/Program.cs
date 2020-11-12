namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.Interactive.Annotations;

    // This sample shows how to add hyperlink on a page of your PDF document.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\CreateHyperlink.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                
                // AddPage method adds a new page to the end of the PDF document (by default)
                document.Pages.Add(new Page());

                // Create link annotation
                LinkAnnotation link = new LinkAnnotation(
                    new Boundary(10, 170, 100, 190),
                    AnnotationFlags.Locked,
                    new AnnotationBorderStyle(5, AnnotationBorderType.Underline));

                link.Color = new double[] { 0.4, 0.4, 0.9 };
                link.InteriorColor = new double[] { 0.6, 0.6, 0.9 };
                link.Contents = "Apitron.PDF.Kit link";

                // add annotation onto the 1st page 
                document.Pages[0].Annotations.Add(link);

                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
