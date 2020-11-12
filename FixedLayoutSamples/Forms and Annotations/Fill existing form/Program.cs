namespace Apitron.PDF.Kit.Samples
{
    using System;
    using System.IO;
    using Apitron.PDF.Kit.Interactive.Forms;

    // This sample shows how to fill forms in existing PDF documents.
    // When you open a PDF document with a form, you can access and modify document widgets.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\FillExistingForm.pdf";

            // create new file
            using (FileStream inPdf = new FileStream(@"..\..\..\..\OutputDocuments\fw4.pdf", FileMode.Open, FileAccess.Read))
            using (FileStream outPdf = new FileStream(out_path, FileMode.Create, FileAccess.Write))
            {
                // load PDF form
                FixedDocument document = new FixedDocument(inPdf);
                PrintFields(document.AcroForm);
                Console.ReadKey();
                FillFields(document.AcroForm);

                // add page and save document
                document.Save(outPdf);
            }

            System.Diagnostics.Process.Start(out_path);
        }

        // print all document fields
        private static void PrintFields(AcroForm fields)
        {
            foreach (string name in fields.FieldNames)
            {
                Field field = fields[name];
                if (field != null)
                {
                    Console.WriteLine(name + " is " + field.GetType());
                }
                else
                {
                    Console.WriteLine(name + " is NULL");
                }
            }
        }

        // fill the PDF document fields
        private static void FillFields(AcroForm acroForm)
        {
            foreach (string name in acroForm.FieldNames)
            {
                Field field = acroForm[name];
                if (field is CheckBoxField)
                {
                    ((CheckBoxField) field).IsChecked = true;
                }
                if (field is TextField)
                {
                    ((TextField) field).Text = "test info";
                }
            }
        }
    }
}