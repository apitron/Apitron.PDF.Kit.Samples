namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.Security;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;

	// This sample shows how to protect your PDF document with a password.
    //
    // There are two password types: a "user" and an "owner" password. 
    // FixedDocument.SecuritySettings.OwnerPassword property is for "owner" password and 
    // FixedDocument.SecuritySettings.UserPassword property is for "user" password.
    // Opening a PDF document with "owner" password allows the reader 
    // to do everything it. Opening a PDF document with a "user" password allows the reader 
    // to perform only operations allowed by specified user access permissions.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\SetPassword.pdf";
            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                document.SecuritySettings = new StandardSecurity("user", "owner", new Permissions());
                Page page = new Page(new PageBoundary(Boundaries.A3));

                document.Pages.Add(page);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}