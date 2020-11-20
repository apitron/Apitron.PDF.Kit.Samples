using System.Diagnostics;
using System.IO;
using System.Linq;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout.Content;
using Apitron.PDF.Kit.FixedLayout.ContentElements;
using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;
using Apitron.PDF.Kit.FixedLayout.Resources.XObjects;


namespace ReplaceTextAndImages
{
    class Program
    {
        private static string outputFileName = "out.pdf";

        static void Main(string[] args)
        {
            ReplaceTextAndImages("../../../data/advertisement.pdf", "$","Price: contact us", "../../../data/replacement.png");
        }

        private static void ReplaceTextAndImages(string inputFilePath, string oldText, string newText, string replacementImagePath)
        {
            using (Stream inputStream = File.Open(inputFilePath, FileMode.Open, FileAccess.Read))
            {
                using (FixedDocument doc = new FixedDocument(inputStream))
                {
                    // add the replacement image to document's resources
                    doc.ResourceManager.RegisterResource(new Image("replacement_image", replacementImagePath, true));

                    // enumerate content elements found on document's first page
                    foreach (IContentElement element in doc.Pages[0].Elements)
                    {
                        // handle the text element case
                        if (element.ElementType == ElementType.Text)
                        {
                            TextContentElement textElement = element as TextContentElement;
                            if (textElement != null)
                            {
                                // go thought all the text segments and replace 
                                // the segment that contains the sample text
                                foreach (TextSegment textSegment in textElement.Segments)
                                {
                                    if (textSegment.Text.Contains(oldText))
                                    {
                                        TextObject newTextObject = new TextObject(textSegment.FontName,textSegment.FontSize);
                                        newTextObject.AppendText(newText);
                                        textSegment.ReplaceText(0, textSegment.Text.Length, newTextObject);
                                    }
                                }
                            }
                        } // handle image case
                        else if (element.ElementType == ElementType.Image)
                        {
                            ImageContentElement imageElement = element as ImageContentElement;

                            if (imageElement != null)
                            {
                                // just replace the image with new one using
                                // registered resource, removing old one
                                imageElement.Replace("replacement_image", true);
                            }
                        }
                    }

                    // save modified file
                    using (Stream outputStream = File.Create(outputFileName))
                    {
                        doc.Save(outputStream);
                    }
                }
            }

            Process.Start(outputFileName);
        }
    }
}
