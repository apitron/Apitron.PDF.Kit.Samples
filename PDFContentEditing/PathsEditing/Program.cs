using System;
using System.Diagnostics;
using System.IO;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout.ContentElements;


namespace PathsEditing
{
    class Program
    {
        private static string outputFileName = "out.pdf";

        static void Main(string[] args)
        {
            ReplacePaths("../../../data/graphics.pdf");
        }

        private static void ReplacePaths(string inputFilePath)
        {
            using (Stream inputStream = File.Open(inputFilePath, FileMode.Open, FileAccess.Read))
            {
                using (FixedDocument doc = new FixedDocument(inputStream))
                {
                    double colorComponent = 0;
                    double colorDelta = 0.1;

                    // enumerate content elements found on document's first page
                    foreach (IContentElement element in doc.Pages[0].Elements)
                    {
                        // change the fill color of each found drawing
                        if (element.ElementType == ElementType.Drawing)
                        {
                            DrawingContentElement drawingElement = (DrawingContentElement) element;
                            drawingElement.SetNonStrokingColor(new double[] {Math.Min(colorComponent,1),0, 0});
                            colorComponent += colorDelta;
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
