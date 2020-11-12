namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.Interactive.Annotations;
    using Apitron.PDF.Kit.Interactive.Actions;
    using Apitron.PDF.Kit.Interactive.Forms;

    // This sample shows how to add text fields to your PDF document.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\TextFields.pdf";

            // create new file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                Page page = new Page();

                // add text field
                TextField textField = new TextField("tx_field", "Text field", "This is text field");
                textField.BorderColor = new double[]{0.1, 0.1, 0.1};
                textField.BorderStyle = new AnnotationBorderStyle(1.0, AnnotationBorderType.Beveled);
                document.AcroForm.Fields.Add(textField);

                // add text field view
                TextFieldView textFieldView  = new TextFieldView(textField, new Boundary(10, 500, 110, 530));

                // add another text field view
                TextFieldView textFieldView1 = new TextFieldView(textField, new Boundary(10, 460, 110, 490));
                textFieldView1.FontResourceID = "TimesNewRoman";
                textFieldView1.FontSize = 10;
                textFieldView1.HighlightingMode = AnnotationHighlightingMode.Push;
                textFieldView1.BorderStyle = new AnnotationBorderStyle(1.0, AnnotationBorderType.Underline);
                textFieldView1.BorderColor = new double[]{0.2, 0.3, 0.2};
                
                textField.AdditionalActions.OnValueChanged = new JavaScriptAction("app.alert('please check the value...');");
                page.Annotations.Add(textFieldView);
                page.Annotations.Add(textFieldView1);
							
                // add page and save document
                document.Pages.Add(page);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}