namespace Apitron.PDF.Kit.Samples
{
    using System.IO;

    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.Security;

    /// <summary>
    /// This sample shows how to change security parameters for an existing PDF document.    
    /// </summary>
    class Program
    {
        static string inFileName = @"..\..\..\..\OutputDocuments\protected.pdf";
        static string outFileName = @"..\..\..\..\OutputDocuments\protected_changed.pdf";


        static void Main( string[] args )
        {
            CreateProtectedDocument(inFileName, Permissions.AllowAllPermissions, EncryptionLevel.AES_256bit, "user", "owner");

            // We have a protected document. Let's change some parameters.
            using (FileStream inPdf = new FileStream(inFileName, FileMode.Open))
            using (FileStream outPdf = new FileStream(outFileName, FileMode.Create))
            {
                // Load document using user password
                FixedDocument document = new FixedDocument(inPdf, "user");

                IDocumentSecurity securitySettings = new StandardSecurity("changedOwner", "changedUser", Permissions.AllowAllPermissions, EncryptionSpecialization.AllDocument);

                // Change paramerers.
                securitySettings.Permissions = Permissions.DisallowAllPermissions;
                securitySettings.EncryptionLevel = EncryptionLevel.RC4_128bit;

                // Save changes into a new document.
                document.Save(outPdf);
            }

            System.Diagnostics.Process.Start(outFileName);
        }

        /// <summary>
        /// Creates the protected document.
        /// </summary>
        /// <param name="outFileName">Name of the out file.</param>
        /// <param name="permissions">The permissions.</param>
        /// <param name="encryptionLevel">The encryption level.</param>
        /// <param name="userPassword">The user password.</param>
        /// <param name="ownerPassword">The owner password.</param>
        private static void CreateProtectedDocument(string outFileName, Permissions permissions, EncryptionLevel encryptionLevel, string userPassword, string ownerPassword)
        {
            // open and load the file
            using (FileStream fs = new FileStream(outFileName, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                document.SecuritySettings = new StandardSecurity(ownerPassword, userPassword, permissions);
                document.SecuritySettings.EncryptionLevel = encryptionLevel;

                document.Pages.Add(new Page(new PageBoundary(new Boundary(0, 0, 210, 297))));
                document.Save(fs);
            }
        }
    }
}
