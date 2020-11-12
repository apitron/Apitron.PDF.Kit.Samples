namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.Interactive.Annotations;
    using Apitron.PDF.Kit.Interactive.Forms;

    // This sample shows how to use checkboxes in your PDF document.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\Checkboxes.pdf";

            // create new file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                Page page = new Page();

                // add checkBox field and view
                CheckBoxField checkBoxField = new CheckBoxField("btn[0]", "Yes, I agree!", "Checkbox tool tip");
                CheckBoxFieldView checkBoxFieldView = new CheckBoxFieldView(checkBoxField, new Boundary(10, 400, 310, 450));
                document.AcroForm.Fields.Add(checkBoxField);
                page.Annotations.Add(checkBoxFieldView);

                // add checkBox field and view 
                CheckBoxField checkBox = new CheckBoxField("btn[1]", "No, I disagree", "Checkbox tool tip");
                CheckBoxFieldView boxFieldView = new CheckBoxFieldView(checkBox, new Boundary(10, 460, 310, 500));
                boxFieldView.TextColor   = new double[]{0.1,0.9,0.7};
                boxFieldView.BorderColor = new double[]{0.3,0.6,0.7};
                boxFieldView.Flags = AnnotationFlags.Print;
                
                document.AcroForm.Fields.Add(checkBox);
                page.Annotations.Add(boxFieldView);

                // add page
                document.Pages.Add(page);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}