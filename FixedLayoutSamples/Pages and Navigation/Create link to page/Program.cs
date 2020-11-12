namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.Interactive.Actions;
    using Apitron.PDF.Kit.Interactive.Annotations;
    using Apitron.PDF.Kit.Interactive.Navigation.DocumentLevel;


    // This samples shows how to add a link to a document page.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\CreateLinkToPage.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();

                // add 1st page
                Page page = new Page();
                document.Pages.Add(page);

                // add 2nd page
                Page page2 = new Page();
                document.Pages.Add(page2);

                // go to page #2
                LinkAnnotation link = new LinkAnnotation(new Boundary(50, 695, 450, 715), AnnotationFlags.Default, new AnnotationBorderStyle(2, AnnotationBorderType.Dashed, new BoxStyleDashPattern(new int[] { 2, 2 })));
                link.Action = new GoToAction(new Destination(1));
                link.HighlightingMode = AnnotationHighlightingMode.Invert;
                page.Annotations.Add(link);


                // back to page #1
                LinkAnnotation link2 = new LinkAnnotation(new Boundary(50, 695, 450, 715), AnnotationFlags.Default, new AnnotationBorderStyle(2, AnnotationBorderType.Dashed, new BoxStyleDashPattern(new int[] { 2, 2 })));
                link2.Action = new GoToAction(new Destination(0));
                link2.HighlightingMode = AnnotationHighlightingMode.Invert;
                page2.Annotations.Add(link2);
                
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
