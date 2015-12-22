using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apitron.PDF.Kit.FixedLayout.Content;

namespace ResizeAndStampPDF
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                string destinationPath = System.IO.Path.GetFileNameWithoutExtension(args[0]) + "_stamped.pdf";
                PageStamper.Stamp(args[0],destinationPath,"FF6173","949124","Approved");

                Process.Start(destinationPath);
            }
        }
    }
}
