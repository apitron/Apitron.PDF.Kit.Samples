namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.Styles;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.Interactive.Navigation.DocumentLevel;

    // This sample shows how to build PDF document's bookmarks with colored, bold and italic items.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\BookmarksWithStyle.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create, FileAccess.Write))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();

                Boundary mediaBox = Boundaries.A4;
                PageBoundary pageBoundary = new PageBoundary(mediaBox);
                Bookmark main = new Bookmark("Root");
                main.TextColor = RgbColors.Green;
                Bookmark bookmark1 = new Bookmark("Sub");
                bookmark1.TextColor = RgbColors.Yellow;
                main.AddFirst(bookmark1);
                document.Bookmarks.AddFirst(main);
                for (int i = 0; i < 4; i++)
                {
                    Page page = new Page(pageBoundary);
                    Bookmark bookmark = new Bookmark(page, string.Format("Author {0}", i), i % 2 == 0);
                    bookmark.TextColor = RgbColors.Green;
                    bookmark1.AddLast(bookmark);
                    document.Pages.Add(page);

                    for (int j = 0; j < 5; j++)
                    {
                        Bookmark bookmark_l1 = new Bookmark(page, string.Format("Book {0}", j), (i + j) % 2 == 0);
                        bookmark_l1.IsTextBold = true;
                        for (int k = 0; k < 5; k++)
                        {
                            Bookmark bookmark_level2 = new Bookmark(page, $"Chapter {k}");
                            bookmark_level2.IsTextItalic = true;
                            bookmark_level2.IsTextBold = true;
                            bookmark_level2.TextColor = new Color(new double[] { 0.6, 0.8, 0.9 });
                            bookmark_l1.AddLast(bookmark_level2);
                        }
                        bookmark.AddLast(bookmark_l1);
                    }
                }

                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
