using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.FixedLayout.Content;
using Apitron.PDF.Kit.FixedLayout.ContentElements;
using Apitron.PDF.Kit.FixedLayout.Resources.GraphicsStates;
using Apitron.PDF.Kit.FixedLayout.Resources.XObjects;
using Apitron.PDF.Kit.FlowLayout.Content;
using Apitron.PDF.Kit.Styles;
using Apitron.PDF.Kit.Styles.Appearance;
using Path = Apitron.PDF.Kit.FixedLayout.Content.Path;


namespace ContentDeletion
{
    class Program
    {
        private static string outputFileName = "out.pdf";

        static void Main(string[] args)
        {
            RemoveContentInRect("../../../data/apitron_pdf_kit_in_action_excerpt.pdf", new Boundary(70, 200, 330, 450));
        }

        private static void RemoveContentInRect(string inputFilePath, Boundary redactionRect)
        {
            using (Stream inputStream = File.Open(inputFilePath, FileMode.Open, FileAccess.Read))
            {
                using (FixedDocument doc = new FixedDocument(inputStream))
                {
                    doc.ResourceManager.RegisterResource(new GraphicsState("myGraphicsState") {CurrentNonStrokingAlpha = 0.3});

                    // enumerate content elements found on document's first page
                    Page firstPage = doc.Pages[0];

                    firstPage.Content.SaveGraphicsState();
                    firstPage.Content.SetDeviceStrokingColor(new []{1.0,0,0});

                    foreach (IContentElement element in firstPage.Elements)
                    {
                        // remove elements falling into the deletion region
                        // even if they just overlap

                        if (element.ElementType == ElementType.Text)
                        {
                            TextContentElement textElement = (TextContentElement) element;

                            foreach (TextSegment segment in textElement.Segments)
                            {
                                if (RectsOverlap(redactionRect, segment.Boundary))
                                {
                                   firstPage.Content.StrokePath(Path.CreateRect(segment.Boundary));
                                   segment.Remove();
                                }
                            }
                        }
                        else if (!RectsOverlap(redactionRect, element.Boundary))
                        {
                            firstPage.Content.StrokePath(Path.CreateRect(element.Boundary));
                            element.Remove();
                        }
                    }
                
                    // highlight deletetion region
                    firstPage.Content.SetGraphicsState("myGraphicsState");
                    firstPage.Content.SetDeviceStrokingColor(new []{0.0});
                    firstPage.Content.SetDeviceNonStrokingColor(new []{0.0});
                    firstPage.Content.FillAndStrokePath(Path.CreateRect(redactionRect));
                    firstPage.Content.RestoreGraphicsState();

                    // save modified file
                    using (Stream outputStream = File.Create(outputFileName))
                    {
                        doc.Save(outputStream);
                    }
                }
            }

            Process.Start(outputFileName);
        }

        public static bool RectsOverlap(Boundary a, Boundary b)
        {
            return (a.Left < b.Right && a.Right> b.Left && a.Bottom<b.Top && a.Top>b.Bottom);
        }
    }
}
