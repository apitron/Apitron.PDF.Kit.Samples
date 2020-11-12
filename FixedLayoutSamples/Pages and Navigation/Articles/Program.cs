namespace Articles
{
    using System;
    using System.IO;

    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;
    using Apitron.PDF.Kit.FlowLayout.Content;
    using Apitron.PDF.Kit.Styles.Appearance;
    using Apitron.PDF.Kit.Interactive.Navigation.PageLevel;
    using Font = Apitron.PDF.Kit.Styles.Text.Font;

	// This sample shows how to use navigation by the articles.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string text1 = "Some types of documents may contain sequences of content items that are logically connected but not physically sequential.";
            string text2 = "EXAMPLE 1&nbsp;&nbsp; A news story may begin on the first page of a newsletter and run over onto one or more nonconsecutive interior pages.";
            string text3 = "To represent such sequences of physically discontiguous but logically related items, a PDF document may define one or more articles (PDF 1.1). The sequential flow of an article shall be defined by an article thread; the individual content items that make up the article are called beads on the thread. Conforming readers may provide navigation facilities to allow the user to follow a thread from one bead to the next.";
            string text4 = "The optional Threads entry in the document catalogue (see 7.7.2, “Document Catalog”) holds an array of thread dictionaries (Table 160) defining the document’s articles. Each individual bead within a thread shall be represented by a bead dictionary (Table 161). The thread dictionary’s F entry shall refer to the first bead in the thread; the beads shall be chained together sequentially in a doubly linked list through their N (next) and V(previous) entries. In addition, for each page on which article beads appear, the page object (see 7.7.3, “Page Tree”) shall contain a B entry whose value is an array of indirect references to the beads on the page, in drawing order.";

            using (FileStream fs = new FileStream(@"..\..\..\..\OutputDocuments\Articles.pdf", FileMode.Create))
            {
                // This objects represents a PDF fixed document
                FixedDocument document = new FixedDocument();

                // Creates an article
                ArticleThread article = new ArticleThread("12.4.3Articles", "John Doe", "Sample", "Navigation, Articles, Apitron", new DateTime(2004, 1, 1));

                // Creates and fill flow content
                Section firstPart = new Section();
                firstPart.Font = new Font(StandardFonts.Helvetica, 12);
                firstPart.Add(new TextBlock(text1));

                Section example = new Section() {Padding = new Thickness(0, 10, 0, 10)};
                example.Add(new TextBlock(text2) { Font = new Font(StandardFonts.HelveticaOblique, 10) });

                firstPart.Add(example);
                firstPart.Add(new TextBlock(text3));

                Section secondPart = new Section();                
                secondPart.Add(new TextBlock(text4));

                // Creates the page
                Page page = new Page(new Boundary(0, 0, 630, 400));

                // Add flow content into the page
                page.Content.SaveGraphicsState();
                page.Content.Translate(10, 50);
                page.Content.AppendContentElement(firstPart, 300, 300);
                page.Content.RestoreGraphicsState();
                article.AddBeadLast(new ArticleBead(page, new Boundary(10, 50, 310, 350)));

                page.Content.SaveGraphicsState();
                page.Content.Translate(320, 50);
                page.Content.AppendContentElement(secondPart, 300, 300);
                page.Content.RestoreGraphicsState();
                article.AddBeadLast(new ArticleBead(page, new Boundary(320, 50, 620, 350)));

                document.Pages.Add(page);
                document.ArticleThreads.Add(article);
                document.Save(fs);
            }
        }
    }
}
