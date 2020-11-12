namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.Security;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;

	// This sample shows how to setup user acess permissions for a PDF document.
    // There are two passwords types: a "user" and an "owner" password. 
    //
    // FixedDocument.SecuritySettings.OwnerPassword property is for "owner" password and 
    // FixedDocument.SecuritySettings.UserPassword property is for "user" password.
    // Opening a PDF document with an "owner" password allows the reader to do everything with it. 
    // Opening a PDF document with a "user" password allows the reader to perform only operations allowed by specified user access permissions.
    //
    // User access permissions may be used to disallow printing of document, 
    // filling of form fields or other operations.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\Permissions.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                document.SecuritySettings = new StandardSecurity("user", "owner", Permissions.DisallowAllPermissions);

                document.Pages.Add(new Page(new PageBoundary(new Boundary(0, 0, 210, 297))));
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
