namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.Interactive.Navigation.PageLevel;

    // This sample shows how add Page Labels to PDF document.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\PageLabels.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();

                // Set different page labels
                document.PageLabels.Add(new PageLabel(0, PageNumberingStyle.UppercaseLetters));
                int size = 1;
                document.PageLabels.Add(new PageLabel(size, PageNumberingStyle.LowercaseLetters));
                document.PageLabels.Add(new PageLabel(2, PageNumberingStyle.UppercaseRoman));
                document.PageLabels.Add(new PageLabel(3, PageNumberingStyle.LowercaseRoman));
                document.PageLabels.Add(new PageLabel(4, PageNumberingStyle.None));
                document.PageLabels.Add(new PageLabel(5, PageNumberingStyle.DecimalArabic, 7, "Page #"));

                // Add pages with different page size
                Page page1 = new Page(new PageBoundary(new Boundary(0, 0, 210, 297)));
                Page page2 = new Page(new PageBoundary(new Boundary(0, 0, 297, 210)));
                Page page3 = new Page(new PageBoundary(new Boundary(0, 0, 40, 80)));
                Page page4 = new Page(new PageBoundary(new Boundary(0, 0, 210, 297)));
                Page page5 = new Page(new PageBoundary(new Boundary(10, 20, 210, 297)));
                Page page6 = new Page(new PageBoundary(Boundaries.A10));
                Page page7 = new Page(new PageBoundary(Boundaries.A2));
                Page page8 = new Page(new PageBoundary(Boundaries.Ledger));
                Page page9 = new Page(new PageBoundary(Boundaries.Letter));


                // Add pages into the final document
                document.Pages.Add(page1);
                document.Pages.Add(page2);
                document.Pages.Add(page3);
                document.Pages.Add(page4);
                document.Pages.Add(page5);
                document.Pages.Add(page6);
                document.Pages.Add(page7);
                document.Pages.Add(page8);
                document.Pages.Add(page9);

                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
