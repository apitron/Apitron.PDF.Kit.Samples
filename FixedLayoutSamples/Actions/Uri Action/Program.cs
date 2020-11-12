namespace Apitron.PDF.Kit.Samples
{
    using System;
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.PageProperties;
    using Apitron.PDF.Kit.FixedLayout.Content;
    using Apitron.PDF.Kit.Interactive.Actions;
    using Apitron.PDF.Kit.Interactive.Annotations;
    using Apitron.PDF.Kit.Interactive.Navigation.DocumentLevel;

    // This sample shows how to create URI action.
	// A URI action causes an URI (Uniform Resource Identifier) to be resolved.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\UriAction.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();

                Boundary mediaBox = Boundaries.A4;
                PageBoundary pageBoundary = new PageBoundary(mediaBox);

                for (int i = 0; i < 5; i++)
                {
                    Page page = new Page(pageBoundary);
                    TextObject text = new TextObject("Arial", 40);
                    text.SetTextMatrix(1, 0, 0, 1, 50, 600);
                    string key = string.Format("Page #{0}", i + 1);
                    text.AppendText(key);
                    page.Content.AppendText(text);

                    LinkAnnotation link = new LinkAnnotation(new Boundary(50, 600, 150, 640));
                    URIAction uriAction = new URIAction(new Uri(string.Format("file://{0}/FileToEmbed.pdf", Environment.CurrentDirectory.Replace("\\", "/"))));
                    link.Action = uriAction;


                    page.Annotations.Add(link);

                    document.Pages.Add(page);
                    document.Names.Destinations.Add(key, new Destination(page, new DestinationTypeXYZ(50, 640, 3)));
                    document.Bookmarks.AddLast(new Bookmark(new Destination(key), key));
                }

                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}
