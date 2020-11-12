namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.Interactive.Annotations;
    using Apitron.PDF.Kit.Interactive.Forms;

    // This sample shows how to use radio buttons in your PDF document.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\RadioButtons.pdf";

            // create new file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                Page page = new Page();

				// add radiobutton field
                RadioButtonField radioButtonField = new RadioButtonField("radio", "Selected", "This is radio button field");
                radioButtonField.FontSize = 8;
                document.AcroForm.Fields.Add(radioButtonField);

                // add field views
                RadioButtonFieldView radioButtonFieldView1 = new RadioButtonFieldView("First radio button", "UnSelected", radioButtonField, new Boundary(100, 500, 400, 530));
                RadioButtonFieldView radioButtonFieldView2 = new RadioButtonFieldView("Second radio button", "Selected",  radioButtonField, new Boundary(100, 460, 400, 490), CheckBoxMark.Star);
                RadioButtonFieldView radioButtonFieldView3 = new RadioButtonFieldView("Third radio button", "Selected",   radioButtonField, new Boundary(100, 420, 400, 450), CheckBoxMark.Cross);
                RadioButtonFieldView radioButtonFieldView4 = new RadioButtonFieldView("Fourth radio button", "Selected",  radioButtonField, new Boundary(100, 380, 400, 410), CheckBoxMark.Diamond);
                RadioButtonFieldView radioButtonFieldView5 = new RadioButtonFieldView("Fifth radio button", "Selected",   radioButtonField, new Boundary(100, 340, 400, 370), CheckBoxMark.Check);
                page.Annotations.Add(radioButtonFieldView1);
                page.Annotations.Add(radioButtonFieldView2);
                page.Annotations.Add(radioButtonFieldView3);
                page.Annotations.Add(radioButtonFieldView4);
                page.Annotations.Add(radioButtonFieldView5);

                // add page and save document
                document.Pages.Add(page);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}