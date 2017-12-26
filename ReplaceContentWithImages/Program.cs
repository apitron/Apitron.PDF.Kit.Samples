using Apitron.PDF.Kit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.FixedLayout.Content;
using Apitron.PDF.Rasterizer;
using Apitron.PDF.Rasterizer.Configuration;

namespace ReplaceContentWithImages
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Stream inputStream = new FileStream("../../data/source.pdf", FileMode.Open))
            {
                using (FixedDocument doc = new FixedDocument(inputStream))
                {
                    // CropBox instruction is used to mark the content cut from the source
                    // and create specialized PDF doc based on it for custom conversion to image.
                    // In the simplest case, when we replace the entire page, it coincides with the
                    // page boundary. Otherwise you should mark the desired part of the content 
                    // based on its exact location within the containing source.
                    // See the examples below:
                    // The first converts the entire page using pre-rendered image,
                    // the second replaces only the part of the page and
                    // uses external renderer (Apitron PDF Rasterizer).

                    // 1. page boundary and predefined image
                    doc.Pages[0].Content.SetProcessingInstruction(ProcessingInstructions.ClippedContent.PdfToImageCropBox, new Boundary(doc.Pages[0].Boundary.MediaBox) );
                    doc.Pages[0].Content.SetProcessingInstruction(ProcessingInstructions.ClippedContent.PdfToImageConverter, ConvertToImage);

                    // 2. custom cropbox and external renderer
                    doc.Pages[1].Content.SetProcessingInstruction(ProcessingInstructions.ClippedContent.PdfToImageCropBox, new Boundary(350,600,550,750) );
                    doc.Pages[1].Content.SetProcessingInstruction(ProcessingInstructions.ClippedContent.PdfToImageConverter, ConvertToImageUsingExternalRenderer);

                    doc.Save(File.Create("out.pdf"));
                }

                Process.Start("out.pdf");
            }
        }

        /// <summary>
        /// Sample PDF to image conversion delegate, in our case it just returns the predefined image.
        /// It can be used to permanently replace classified info, add watermarked versions of the content or
        /// use it for any other reasons. Don't forget to set the cropbox instruction.
        /// </summary>
        /// <param name="stream">PDF content stream generated based on the given cropbox instruction.</param>
        /// <returns>Rendered image, can be in bmp, jpg/jpeg, png or tiff formats.</returns>
        private static Stream ConvertToImage(Stream stream)
        {
            return new FileStream("../../data/converted_page.png",FileMode.Open); 
        }

        /// <summary>
        /// Sample PDF content to image conversion delegate using external renderer.
        /// It can be used to permanently replace classified info, add watermarked versions of the content or
        /// use it for any other reasons. Don't forget to set the cropbox instruction.
        /// </summary>
        /// <param name="stream">PDF content stream generated based on the given cropbox instruction.</param>
        /// <returns>Rendered image, can be in bmp, jpg/jpeg, png or tiff formats.</returns>
        private static Stream ConvertToImageUsingExternalRenderer(Stream stream)
        {
            // open the doc
            Apitron.PDF.Rasterizer.Document doc = new Document(stream);
            
            // render the page and return image stream here,
            // we'll use the 96 DPI resolution for rendering
            MemoryStream imageStream = new MemoryStream();
            using(Bitmap bitmap = doc.Pages[0].Render(new Resolution(96, 96), new RenderingSettings()))
            {
                // draw red rect around to mark the replaced part
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.DrawRectangle(Pens.Red,0,0,bitmap.Width-1,bitmap.Height-1);
                }
                bitmap.Save(imageStream, ImageFormat.Bmp);
            }
            return imageStream;
        }
    }
}
