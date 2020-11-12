namespace Apitron.PDF.Kit.Samples
{
    using System;
    using System.IO;
    using Apitron.PDF.Kit.Interactive.Forms;

    // This sample shows how flatten fields in existing PDF document.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\FlattenFields.pdf";
            using (FileStream stream = new FileStream(@"..\..\..\..\OutputDocuments\vaf1a.pdf", FileMode.Open, FileAccess.Read))
            using (FileStream outPDF = new FileStream(out_path, FileMode.Create, FileAccess.ReadWrite))
            {
                FixedDocument result = new FixedDocument(stream);
                int count = 0;
                foreach (string name in result.AcroForm.FieldNames)
                {
                    Apitron.PDF.Kit.Interactive.Forms.Field field = result.AcroForm[name];
                    Apitron.PDF.Kit.Interactive.Forms.TerminalField terminalField = field as Apitron.PDF.Kit.Interactive.Forms.TerminalField;
                    if (terminalField != null)
                    {
                        terminalField.Flatten();
                        count++;
                    }
                }

                result.Save(outPDF);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}