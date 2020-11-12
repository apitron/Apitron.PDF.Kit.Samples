using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftMaskImage
{
    using System.Diagnostics;
    using System.IO;

    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;

    class Program
    {
        static void Main( string[] args )
        {
            using (FileStream stream = new FileStream( @"..\..\..\..\OutputDocuments\ImageSoftMask.pdf", FileMode.Create ))
            {
                FixedDocument document = new FixedDocument();

                string resourceID = "IMG0";                
                Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image image = new Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image( resourceID, @"..\..\..\..\OutputDocuments\image.jpg" );
                document.ResourceManager.RegisterResource( image );
               
                string softMaskID = "IMG1";
                Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image softMask = new Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image( softMaskID, @"..\..\..\..\OutputDocuments\mask.png" );
                document.ResourceManager.RegisterResource( softMask );

                image.SoftMaskResourceID = softMaskID;
                image.UseInvertedDecode = false;

                Boundary boundary = new Boundary( 0, 0, image.Width + 20, image.Height + 60 );
                Page page = new Page( new PageBoundary( boundary ) );
                page.Content.AppendImage( resourceID, 10, 50, image.Width, image.Height );

                document.Pages.Add(page);
                document.Save(stream);
            }
            Process.Start( @"..\..\..\..\OutputDocuments\ImageSoftMask.pdf" );
        }
    }
}
