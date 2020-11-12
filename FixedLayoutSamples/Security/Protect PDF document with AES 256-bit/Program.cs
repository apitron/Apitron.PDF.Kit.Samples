namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.Security;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;

    /// <summary>
	/// This sample shows how to protect your PDF document with a password using AES 256-bit encryption algorithm.    
    /// Specify <see cref="EncryptionLevel.AES_256bit"/> for FixedDocument.SecuritySettings.EncryptionLevel property and setup owner password, 
    /// user password or both to protect output PDF file with advanced protection offered by AES 256-bit encryption.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\AESProtection.pdf";
            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                document.SecuritySettings = new StandardSecurity("","",Permissions.AllowAllPermissions);
                document.SecuritySettings.EncryptionLevel = EncryptionLevel.AES_256bit;
                
                document.Pages.Add(new Page(new PageBoundary(new Boundary(0,0,210,297))));
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
