using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.FixedLayout.Resources;
using Apitron.PDF.Kit.FlowLayout.Content;
using Apitron.PDF.Kit.Interactive.Annotations;
using Apitron.PDF.Kit.Interactive.Forms;
using Apitron.PDF.Kit.Interactive.Forms.Signature;
using Apitron.PDF.Kit.Interactive.Forms.SignatureSettings;
using Apitron.PDF.Kit.Styles;
using Image = Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image;

namespace CustomSignatureProperties
{
    class Program
    {
        private static void Sign(string pathToDocument, string pathToCertificate, string password, string pathToSignatureImage, Boundary signatureViewLocation)
        {            
            // open existing document andd sign once 
            using (Stream inputStream = new FileStream( pathToDocument, FileMode.Open, FileAccess.ReadWrite))
            {
                using (FixedDocument doc = new FixedDocument(inputStream))
                {
                    string imageResourceId = Guid.NewGuid().ToString("N");
                    string signatureFieldId = Guid.NewGuid().ToString("N");

                    // register signature image resource
                    doc.ResourceManager.RegisterResource(new Image(imageResourceId, pathToSignatureImage));

                    // create first signature field and initialize it using a stored certificate
                    SignatureField signatureField = new SignatureField(signatureFieldId);
                    using (Stream signatureDataStream = File.OpenRead(pathToCertificate))
                    {
                        signatureField.Signature = new Pkcs7DetachedSignature(new Pkcs12Store(signatureDataStream,password));
                        // set the software module name
                        signatureField.Signature.SoftwareModuleName = "MyApp based on Apitron PDF Kit for .NET";         
                        // set the GEO location of the place where the signature was created
                        signatureField.PropBuild.SetValue("GEOTAG", "38.8977° N, 77.0365° W");                                                            
                    }

                    // add signature fields to the document
                    doc.AcroForm.Fields.Add(signatureField);

                    // create first signature view using the image resource
                    SignatureFieldView signatureView = new SignatureFieldView(signatureField, signatureViewLocation);
                    signatureView.ViewSettings.Graphic = Graphic.Image;
                    signatureView.ViewSettings.GraphicResourceID = imageResourceId;
                    signatureView.ViewSettings.Description = Description.None;

                    // add views to page annotations collection
                    doc.Pages[0].Annotations.Add(signatureView);

                    // save as incremental update
                    doc.Save();
                }
            }

            Process.Start("signed.pdf");
        }

        /// <summary>
        /// Creates simple PDF file as an example for signing.
        /// </summary>        
        private static void CreatePDFDocument(string fileName)
        {
            using (Stream stream = File.Create(fileName))
            {
                FlowDocument doc = new FlowDocument() { Margin = new Thickness(10) };
                doc.Add(new TextBlock("Signed using Apitron PDF Kit for .NET, the signature has a custom property containing app name. " +
                                      "Click on the signature image and select \"Signature Properties...\"->\"Advanced Properties...\""));
                doc.Write(stream, new ResourceManager());
            }
        }

        static void Main(string[] args)
        {
            string fileName = "signed.pdf";

            CreatePDFDocument(fileName);

            // sign once and save
            Sign(fileName, "../../data/certs/JohnDoe.pfx", "password", "../../data/images/signatureImage.png", new Boundary(10, 750, 110, 800));                                
        }
       
    }
}
