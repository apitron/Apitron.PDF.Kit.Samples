using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.Configuration;
using Apitron.PDF.Kit.Extraction;

namespace ConvertPdfToHtml
{
    class Program
    {
        static void Main(string[] args)
        {
            // open pdf document
            using (Stream inputStream = File.OpenRead("../../data/notification.pdf"))
            {
                using (FixedDocument doc = new FixedDocument(inputStream))
                {               
                    // create output file
                    using (TextWriter writer = new StreamWriter(File.Create("out.html"),Encoding.UTF8))
                    {
                        // write returned html string to file
                        writer.Write(doc.Pages[0].ConvertToHtml(TextExtractionOptions.HtmlPage));
                    }
                }
            }

            Process.Start("out.html");
        }
    }
}
