using System.Diagnostics;
using System.IO;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.Interactive.Annotations;
using Apitron.PDF.Kit.Interactive.Forms;
using Apitron.PDF.Kit.Styles;

namespace FlattenPDFFields
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateTestDocument();

            using (Stream inputStream = File.Open("documentWithField.pdf",FileMode.Open))
            {
                FixedDocument doc = new FixedDocument(inputStream);
                
                // we can either flatten all fields or enumerate the collection
                // and flatten specific field by calling its Flatten() method
                doc.AcroForm.FlattenFields();

                // save document
                using (Stream outputStream = File.Create("fieldsFlattening.pdf"))
                {
                    doc.Save(outputStream);
                }
            }

            Process.Start("fieldsFlattening.pdf");
        }

        // creates test PDF document with text field
        private static void CreateTestDocument()
        {
            FixedDocument doc = new FixedDocument();

            // create text field and add to doc's field collection
            TextField textField = new TextField("textField", "Text field content");
            doc.AcroForm.Fields.Add(textField);

            // create new page and add text view to it
            Page page = new Page();

            TextFieldView fieldView = new TextFieldView(textField, new Boundary(10, 800, 150, 820));
            fieldView.BorderColor = RgbColors.Red.Components;

            page.Annotations.Add( fieldView);
            // add page to document
            doc.Pages.Add(page);

            // save document
            using (Stream stream = File.Create("documentWithField.pdf"))
            {
                doc.Save(stream);
            }
        }
    }
}