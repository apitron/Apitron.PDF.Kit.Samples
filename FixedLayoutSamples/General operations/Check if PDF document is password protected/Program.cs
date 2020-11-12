namespace Apitron.PDF.Kit.Samples
{
    using System;
    using System.IO;

	//This sample shows how to check if an existing PDF document is password protected.
    internal class Program
    {
        private static void Main(string[] args)
        {
            // create new file
            using (FileStream inPdf = new FileStream(@"..\..\..\..\OutputDocuments\password.pdf", FileMode.Open, FileAccess.ReadWrite))
            {
                FixedDocument document;
                string password1 = "1";
                string password2 = "2";

                // load password protected file
                try
                {
                    document = new FixedDocument(inPdf);
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Specified password is invalid")
                    {
                        document = new FixedDocument(inPdf, password1);
                        Console.WriteLine("Document is password protected");
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
