namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.Interactive.Annotations;
    using Apitron.PDF.Kit.Interactive.Forms;

    // This sample shows how to use push buttons in your PDF document.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\Buttons.pdf";

            // create new file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                Page page = new Page();

				// add push button field
                PushbuttonField pushbutton = new PushbuttonField("btn", "Push me", "This is simple button");
                pushbutton.BorderColor = new double[]{0.1, 0.8, 0.7};
				document.AcroForm.Fields.Add(pushbutton);

                // add button view
				PushbuttonFieldView pushbuttonFieldView  = new PushbuttonFieldView(pushbutton, new Boundary(100, 400, 200, 430));
				pushbuttonFieldView.Color = new double[]{0.2, 0.8, 0.7};
                page.Annotations.Add(pushbuttonFieldView);
							
                // add page and save document
                document.Pages.Add(page);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}