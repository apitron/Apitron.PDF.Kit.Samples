using Apitron.PDF.Kit;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.FixedLayout.Content;
using Apitron.PDF.Kit.FlowLayout;
using Apitron.PDF.Kit.FlowLayout.Content;
using Apitron.PDF.Kit.Styles;
using Apitron.PDF.Kit.Styles.Appearance;
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
                    // page boundary, otherwise it should coincide with the parent content's boundary.
                    // See the examples below:
                    // The first converts the entire page using pre-rendered image,
                    // the second replaces the page and
                    // uses external renderer (Apitron PDF Rasterizer).
                    // Another, completely different example
                    // shows how to convert the newly generated content to an image and
                    // avoid saving it to PDF in original form - a kind of flattening.

                    // 1. Page boundary and predefined image
                    doc.Pages[0].Content.SetProcessingInstruction(ProcessingInstructions.ClippedContent.PdfToImageCropBox, doc.Pages[0].Boundary.MediaBox);
                    doc.Pages[0].Content.SetProcessingInstruction(ProcessingInstructions.ClippedContent.PdfToImageConverter, ConvertToImage);

                    // 2. Page boundary and external renderer
                    doc.Pages[1].Content.SetProcessingInstruction(ProcessingInstructions.ClippedContent.PdfToImageCropBox, doc.Pages[1].Boundary.MediaBox);
                    doc.Pages[1].Content.SetProcessingInstruction(ProcessingInstructions.ClippedContent.PdfToImageConverter, ConvertToImageUsingExternalRenderer);

                    // 3. Original generated content + "flattened" generated content, custom boundary and external renderer
                    Apitron.PDF.Kit.FixedLayout.Page page2 = new Apitron.PDF.Kit.FixedLayout.Page();
                    double blockWidth = page2.Boundary.MediaBox.Width - 20;
                    double blockHeight = (page2.Boundary.MediaBox.Height - 20)/3.0;
                    // Add the first block of text, it will be saved as is, 
                    // blocks are added starting from the bottom of the page to the top
                    page2.Content.SetTranslate(10,10);
                    page2.Content.AppendContentElement(new Section(ContentElement.FromMarkup(resources.LoremIpsumText)) {Align = Align.Justify}, blockWidth, blockHeight  );

                    // Add the second block, it will be converted to image using the chosen
                    // renderer when saving, red color is used for visibility.
                    page2.Content.SetTranslate(0, blockHeight);
                    // Create new clipped content object with desired width and height
                    // and add our text block into it.
                    // After that we add neccessary processing instructions which will 
                    // affect the content generation during the PDF saving process.
                    ClippedContent toBeFlattenedContent = new ClippedContent(0,0,blockWidth,blockHeight);
                    toBeFlattenedContent.AppendContentElement(new Section(ContentElement.FromMarkup(resources.LoremIpsumText))
                    {
                        Color = RgbColors.Red,
                        Align = Align.Justify
                    }, 
                    blockWidth, blockHeight);

                    toBeFlattenedContent.SetProcessingInstruction(ProcessingInstructions.ClippedContent.PdfToImageCropBox, new Boundary(0,0, blockWidth, blockHeight));
                    toBeFlattenedContent.SetProcessingInstruction(ProcessingInstructions.ClippedContent.PdfToImageConverter, ConvertToImageUsingExternalRenderer);

                    page2.Content.AppendContent(toBeFlattenedContent);

                    // add the third block using the same approach as for the first block
                    page2.Content.SetTranslate(0, blockHeight);
                    page2.Content.AppendContentElement(new Section(ContentElement.FromMarkup(resources.LoremIpsumText)) {Align = Align.Justify}, blockWidth, blockHeight);

                    // add the new page
                    doc.Pages.Add(page2);

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
            // we'll use the 144 DPI resolution for rendering,
            // so the difference between the first and second page will be clearly seen
            MemoryStream imageStream = new MemoryStream();
            using(Bitmap bitmap = doc.Pages[0].Render(new Resolution(144, 144), new RenderingSettings()))
            {
                bitmap.Save(imageStream, ImageFormat.Bmp);
            }
            return imageStream;
        }
    }
}
