namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.Interactive.Actions;
    using Apitron.PDF.Kit.Interactive.Annotations;
    using Apitron.PDF.Kit.Interactive.Forms;

    // This sample shows how to use Hide actions.
	// Using hide action (see HideAction class) you change visibility of controls.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\HideAction.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                FixedDocument document = new FixedDocument();
                document.Pages.Add(new Page());
                PushbuttonField pushbuttonField = new PushbuttonField("hide_btn", "Hide field");
                PushbuttonFieldView pushbuttonFieldView = new PushbuttonFieldView(pushbuttonField, new Boundary(10, 500, 110, 530));
                TextField textField = new TextField("hide_txt", "Will be hidden");
                TextFieldView textFieldView = new TextFieldView(textField, new Boundary(120, 500, 220, 530));
                document.AcroForm.Fields.Add(pushbuttonField);
                document.AcroForm.Fields.Add(textField);

                pushbuttonFieldView.Action = new HideAction(true, textFieldView);

                textFieldView.BorderColor = new double[] { 0.3, 0.1, 0.4 };
                textFieldView.BorderStyle = new AnnotationBorderStyle(2, AnnotationBorderType.Inset);
               
                document.Pages[0].Annotations.Add(pushbuttonFieldView);
                document.Pages[0].Annotations.Add(textFieldView);

                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
