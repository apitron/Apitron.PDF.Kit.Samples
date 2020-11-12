namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Resources;
    using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;
    using Apitron.PDF.Kit.Interactive.Actions;
    using Apitron.PDF.Kit.Interactive.Annotations;

    // This sample shows how to use JavaScript actions.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\JavaScriptAction.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();

                // register font in Resource Manager
                ResourceManager rm = new ResourceManager();
                Font font = new Font("font", StandardFonts.Courier);
                rm.RegisterResource(font);
                
                // add new page
                Page page = new Page(new PageBoundary(Boundaries.A7));
                document.Pages.Add(page);

                // add link annotation with JS action
                LinkAnnotation link = new LinkAnnotation(
                   new Boundary(10, 170, 100, 190),
                   AnnotationFlags.Locked,
                   new AnnotationBorderStyle(5, AnnotationBorderType.Beveled));
                link.Color = new double[] { 0.4, 0.4, 0.9 };
                link.InteriorColor = new double[] { 0.6, 0.6, 0.9 };
                link.Contents = "Apitron.PDF.Kit link";
                link.FontSize = 12;
                link.FontResourceID = "font";
                
                // add annotation onto the 1st page 
                document.Pages[0].Annotations.Add(link);

                link.Action = new JavaScriptAction("app.alert('This is JS alert!');");
                link.HighlightingMode = AnnotationHighlightingMode.Invert;
                Boundary boundary = link.Boundary;
                Boundary boundary1 = new Boundary(boundary.Left + 100, boundary.Bottom + 100, boundary.Right + 100, boundary.Top + 100);
                link.Boundary = new Boundary(link.Boundary.Left, link.Boundary.Bottom, boundary1.Right, boundary1.Top);
                link.QuadPoints = new double[]
                    {
                        boundary.Left - 10, boundary.Bottom - 10, boundary.Right + 10,
                        boundary.Bottom - 10, boundary.Right + 10, boundary.Top + 10,
                        boundary.Left - 10, boundary.Top + 10,
                    };

                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
