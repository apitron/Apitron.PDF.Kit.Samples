namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.Resources.XObjects;

    // This sample shows how to load an existing PDF document using ctor of FixedDocument.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\LoadAnExistingPDFDocument.pdf"; 

            using (FileStream inPDF = new FileStream(@"..\..\..\..\OutputDocuments\testfile.pdf", FileMode.Open, FileAccess.Read))
            using (FileStream outPDF = new FileStream(out_path, FileMode.Create, FileAccess.ReadWrite))
            {
                // this object represents a PDF fixed document, open existing PDF file
                FixedDocument document = new FixedDocument(inPDF);

                // create  fixed content
                FixedContent stamp = new FixedContent("Stamp", new Boundary(0, 0, 100, 100));
                stamp.Content.SetDeviceNonStrokingColor(new double[] { 0.5, 0.8, 0.1 });
                FixedLayout.Content.Path path = new FixedLayout.Content.Path();
                
                // add rect
                path.AppendRectangle(10, 10, 80, 80);
                stamp.Content.FillPath(path);

                document.ResourceManager.RegisterResource(stamp);
                
                foreach (Page page in document.Pages)
                {
                    page.Content.ModifyCurrentTransformationMatrix(1, 0, 0, 1, 100, 100);
                    page.Content.AppendXObject("Stamp");
                }

                // save current document with modifycations into the new document
                document.Save(outPDF);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
