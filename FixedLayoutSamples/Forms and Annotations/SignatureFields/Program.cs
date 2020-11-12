namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.Interactive.Annotations;
    using Apitron.PDF.Kit.Interactive.Forms;

    // This sample shows how to add signature fields to your PDF document.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\SignatureFields.pdf";

            // create new file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                Page page = new Page();
				
                // add signature field
				SignatureField signatureField = new SignatureField("siga", "This is simple signature field");
				document.AcroForm.Fields.Add(signatureField);
	
                // add view
				SignatureFieldView signatureFieldView = new SignatureFieldView(signatureField, new Boundary(100, 500, 200, 530));
				page.Annotations.Add(signatureFieldView);
	
				signatureField.TextColor   = new double[] { 1, 0, 0 };
				signatureField.ViewColor   = new double[] { 1, 0, 1 };
				signatureField.BorderColor = new double[] { 0, 1, 1 };
				signatureField.BorderStyle = new AnnotationBorderStyle(2, AnnotationBorderType.Inset);

                // set locked fields
				signatureField.LockAction = LockAction.All;
				signatureField.LockFields.Add("btn");


                // locked by signature field
                PushbuttonField pushbuttonField = new PushbuttonField("btn", "Submit");
                document.AcroForm.Fields.Add(pushbuttonField);
                PushbuttonFieldView pushbuttonFieldView = new PushbuttonFieldView(pushbuttonField, new Boundary(100, 460, 200, 490));
                page.Annotations.Add(pushbuttonFieldView);
							
                // add page and save document
                document.Pages.Add(page);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}