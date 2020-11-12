namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.Interactive.Actions;
    using Apitron.PDF.Kit.Interactive.Annotations;
    using Apitron.PDF.Kit.Interactive.Navigation.DocumentLevel;
    
    // This sample shows how to use Go-To actions.
	// Using methods and properties of GoToAction class you can specify a target page 
	// to be shown as the result of the action. You can also specify the upper-left corner 
	// of the target page to be positioned at the upper-left corner of the window using GoToAction.Offset property.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\GoToAction.pdf";

            // open file stream
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                Page page = new Page(new PageBoundary(new Boundary(0,0,1210,1297)));
                Page page2 = new Page(new PageBoundary(new Boundary(0, 0, 1210, 1297)));

                LinkAnnotation link = new LinkAnnotation(new Boundary(50, 695, 450, 715), AnnotationFlags.Default, new AnnotationBorderStyle(2, AnnotationBorderType.Dashed, new BoxStyleDashPattern(new int[] { 2, 2 })));
                link.Action = new GoToAction(new Destination("Page #1"));
                link.HighlightingMode = AnnotationHighlightingMode.Invert;
                link.Color = new double[]{0.5,0.6,0.8};
                page.Annotations.Add(link);
                
                // AddPage method adds a new page to the end of the PDF document (by default)
                document.Pages.Add(page);
                document.Pages.Add(page2);

                document.Names.Destinations.Add("Page #1", new Destination(page2));

                document.Save(fs);
			}

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
