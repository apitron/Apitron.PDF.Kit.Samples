using System;
using System.IO;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.FixedLayout.PageProperties;
using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;
using Apitron.PDF.Kit.FlowLayout.Content;
using Apitron.PDF.Kit.Styles;
using Apitron.PDF.Kit.Styles.Appearance;
using Font = Apitron.PDF.Kit.Styles.Text.Font;

namespace ResizeAndStampPDF
{
    /// <summary>
    /// Resizes PDF page and adds custom stamp at the bottom.
    /// </summary>
    static class PageStamper 
    {
        #region fields

        private const int stampHeight = 36;
        private  const int sectionMargin = 40;

        #endregion

        public static void Stamp(string sourcePath, string destinationPath, string branchNo, string incomingNo, string documentStatus)
        {
            // open file
            using (Stream stream = File.Open(sourcePath, FileMode.Open, FileAccess.ReadWrite))
            {
                // load pdf document from stream
                using (FixedDocument originalDoc = new FixedDocument(stream))
                {
                    int pageNumber = 1;

                    // process pages
                    foreach (Page page in originalDoc.Pages)
                    {
                        // append the required stamp
                        Section stampSection = CreateStampSection(page, branchNo, incomingNo, documentStatus, pageNumber++);

                        // resize the page
                        ResizePage(page);

                        // save state to avoid any problems with possible graphics state change
                        page.Content.SaveGraphicsState();

                        // apply transformations to properly position the stamp
                        ApplyTransformations(page, stampSection);

                        // add section object to the page
                        page.Content.AppendContentElement(stampSection, stampSection.Width.Value, stampSection.Height.Value);

                        // restore the original state
                        page.Content.RestoreGraphicsState();
                    }

                    // save changes to PDF
                    using (Stream outputStream = File.Create(destinationPath))
                    {
                        originalDoc.Save(outputStream);
                    }
                }
            }
        }

        private static Section CreateStampSection(Page page, string branchNo, string incomingNo, string documentStatus, int pageNumber)
        {
            // create section content element and set its size
            Section stampSection = new Section();
            stampSection.Height = stampHeight;

            if(page.Rotate == PageRotate.Rotate0 || page.Rotate == PageRotate.Rotate180)
            {
                stampSection.Width = page.Boundary.MediaBox.Width - sectionMargin;                
            }
            else
            {
                stampSection.Width = page.Boundary.MediaBox.Height - sectionMargin;
            }            

            // create grid element
            Grid grid = new Grid(Length.Auto, Length.Auto, Length.Auto, Length.Auto, Length.Auto);
            grid.Font = new Font(StandardFonts.HelveticaBold, 12);
            grid.Color = RgbColors.Red;
            grid.InnerBorderColor = RgbColors.Red;
            grid.InnerBorder = new Border(1);
            grid.Width = Length.FromPercentage(100);

            // add header row
            grid.Add(new GridRow( new TextBlock("Page #"),
                new TextBlock("Branch #"),
                new TextBlock("Incoming Doc #"),
                new TextBlock("Document Status#"),
                new TextBlock("Date")) 
                { Align = Align.Center });

            // add data row
            grid.Add(new GridRow( new TextBlock(pageNumber.ToString()),
                new TextBlock(branchNo),
                new TextBlock(incomingNo),
                new TextBlock(documentStatus),
                new TextBlock(DateTime.Now.ToString("dd/MM/yyyy")))
                { Align = Align.Center });

            stampSection.Add(grid);
            return stampSection;
        }

        /// <summary>
        /// Resizes page taking into account its rotation property.
        /// </summary>
        /// <param name="page"></param>
        private static void ResizePage(Page page)
        {
            Boundary mediaBox = ResizeBoundary(page.Boundary.MediaBox, page.Rotate);
            page.Resize(new PageBoundary(mediaBox));
        }

        #region resizing and transformations

        /// <summary>
        /// Applies transformations to generated content if the page has initial rotation.
        /// </summary>
        /// <param name="page">The page to transform.</param>
        /// <param name="section">Section used </param>
        private static void ApplyTransformations(Page page, Section section)
        {
            switch (page.Rotate)
            {
                case PageRotate.Rotate90:
                {
                    page.Content.SetRotation(Math.PI/2.0f);
                    // set current position on page
                    page.Content.SetTranslation((page.Boundary.MediaBox.Height - section.Width.Value)/2.0,
                        -page.Boundary.MediaBox.Right);
                    break;
                }
                case PageRotate.Rotate180:
                {
                    page.Content.SetRotation(Math.PI);
                    // set current position on page
                    page.Content.SetTranslation(
                        -page.Boundary.MediaBox.Width + ((page.Boundary.MediaBox.Width - section.Width.Value)/2.0),
                        -page.Boundary.MediaBox.Height);
                    break;
                }
                case PageRotate.Rotate270:
                {
                    page.Content.SetRotation(-Math.PI/2.0f);
                    // set current position on page
                    page.Content.SetTranslation(
                        -page.Boundary.MediaBox.Height + (page.Boundary.MediaBox.Height - section.Width.Value)/2.0,
                        page.Boundary.MediaBox.Left);

                    break;
                }
                case PageRotate.Rotate0:
                default:
                {
                    // set current position on page
                    page.Content.SetTranslation((page.Boundary.MediaBox.Width - section.Width.Value)/2.0,
                        page.Boundary.MediaBox.Bottom);
                    break;
                }
            }
        }


        /// <summary>
        /// Resizes passed boundary if it's not null, otherwise returns null.
        /// </summary>
        /// <param name="boundary">Boundary to resize.</param>
        /// <param name="rotation">Page rotation.</param>
        /// <param name="heightDelta">The height delta, defaut is 36.</param>
        /// <returns>New boundary the <paramref name="boundary"/> is not null, otherwise null.</returns>
        private static Boundary ResizeBoundary(Boundary boundary, PageRotate rotation, double heightDelta = stampHeight)
        {
            if (boundary == null)
            {
                return null;
            }

            switch (rotation)
            {
                case PageRotate.Rotate90:
                    return new Boundary(boundary.Left, boundary.Bottom, boundary.Right + heightDelta, boundary.Top);
                case PageRotate.Rotate180:
                    return new Boundary(boundary.Left, boundary.Bottom, boundary.Right, boundary.Top + heightDelta);
                case PageRotate.Rotate270:
                    return new Boundary(boundary.Left - heightDelta, boundary.Bottom, boundary.Right + heightDelta,
                        boundary.Top);
                case PageRotate.Rotate0:
                default:
                    return new Boundary(boundary.Left, boundary.Bottom - heightDelta, boundary.Right, boundary.Top);
            }
        }

        #endregion
    }
}