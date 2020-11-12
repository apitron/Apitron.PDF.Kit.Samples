namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.Interactive.Annotations;
    using Apitron.PDF.Kit.Interactive.Forms;

    // This sample shows how to use choices in your PDF document.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\Choices.pdf";

            // create new file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                Page page = new Page();

				// add choice field
                ChoiceField choiceField1 = new ChoiceField("ch[1]", "alt_ch[1]", new string[] { "One", "Two", "Three", "Four", "Five" }, ChoiceFieldType.ComboBox);
                choiceField1.SelectName("Two");
                document.AcroForm.Fields.Add(choiceField1);

                // add choice field view
                ChoiceFieldView choiceFieldView1 = new ChoiceFieldView(choiceField1, new Boundary(10, 400, 210, 430), AnnotationFlags.ReadOnly);
                page.Annotations.Add(choiceFieldView1);
                
                // add page and save document
                document.Pages.Add(page);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}