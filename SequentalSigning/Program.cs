using System;
using System.IO;
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

namespace SequentalSigning
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
                        signatureField.Signature = Signature.Create(new Pkcs12Store(signatureDataStream,password));
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
        }

        static void Main(string[] args)
        {
            string fileName = "signedTwice.pdf";

            CreatePDFDocument(fileName);

            // save once and save
            Sign(fileName, "../../data/certs/JohnDoe.pfx", "password", "../../data/images/signatureImage.png", new Boundary(10, 750, 110, 800));                       

            // add second signature to the already signed doc and save
            Sign(fileName, "../../data/certs/JaneDoe.pfx", "password","../../data/images/signatureImageJane.png",new Boundary(120, 750, 220, 800));                       
        }

        private static void CreatePDFDocument(string fileName)
        {
            using (Stream stream = File.Create(fileName))
            {
                FlowDocument doc = new FlowDocument(){Margin = new Thickness(10)};
                doc.Add(new TextBlock("Signed using Apitron PDF Kit for .NET"));
                doc.Write(stream,new ResourceManager());
            }
        }                
    }
}