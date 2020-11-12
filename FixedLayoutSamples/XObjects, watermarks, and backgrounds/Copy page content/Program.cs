namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.Resources.ColorSpaces.Device;

	//This sample shows how to save page content to another page.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\CopyPageContent.pdf";
            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this objects represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                Page page = new Page();
                document.Pages.Add(page);

                // register RGB color spaces. 
                document.ResourceManager.RegisterResource(new RgbColorSpace("CS_RGB"));
                
                // use RGB color space
                page.Content.SetNonStrokingColorSpace("CS_RGB");
                page.Content.SetNonStrokingColor(0.33, 0.66, 0.33);

                // draw path or add some page content
                FixedLayout.Content.Path path = new FixedLayout.Content.Path();
                path.AppendRectangle(10, 10, page.Boundary.MediaBox.Width - 20, page.Boundary.MediaBox.Height - 20);
                page.Content.FillAndStrokePath(path);

                // add new page
                Page page1 = new Page();
                document.Pages.Add(page1);

                // re-save page content from  the 1st to another page.
                document.Pages[1].Content.AppendContent(document.Pages[0].Content);
                document.Pages.Add(page);
				document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
