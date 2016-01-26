using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.Configuration;
using Apitron.PDF.Rasterizer;
using Page = Apitron.PDF.Kit.FixedLayout.Page;
using RenderingSettings = Apitron.PDF.Rasterizer.Configuration.RenderingSettings;
using Resolution = Apitron.PDF.Rasterizer.Configuration.Resolution;

namespace EngineSettingsUsage
{
    class Program
    {
        static void Main(string[] args)
        {            
            /* GLOBAL SETTINGS usage, same for 
               Apitron PDF Kit  and Apitron PDF Rasterizer rasterizer */ 

            // controls whether we have to unload resources whenever possible based on size limit setting,
            // if the resource takes more than allowed it will be unloaded.
            EngineSettings.GlobalSettings.MemoryAllocationMode = MemoryAllocationMode.ResourcesLowMemory;
            // sets the resource size limit
            EngineSettings.GlobalSettings.ResourceSizeLimit = 1048676;

            // system font paths used to find external fonts [readonly].
            ICollection<string> systemFontPaths = EngineSettings.SystemFontPaths;

            // a collection used to specifiy additional font search paths
            ICollection<string> userFontPaths = EngineSettings.UserFontPaths;
            // example:
            userFontPaths.Add(@"c:\\myfonts");

            // set font fallbacks
            // map Arial and Calibri to Helvetica if they are not embedded in document 
            // and not found in system and user font folders
            EngineSettings.UserFontMappings.Add(new KeyValuePair<string, string[]>("Helvetica",new string[]{"Arial","Calibri"}));
            // map all not found fonts to TimesNewRoman using special name "*"
            EngineSettings.UserFontMappings.Add(new KeyValuePair<string, string[]>("TimesNewRoman",new string[]{"*"}));

            // registers additional font in library's font cache if don't have a user font folder
            // or can't create one. Font name and parameters will be read from font file.
            using (Stream fontStream = File.Open("c:\\fonts\\Consolas.ttf", FileMode.Open))
            {
                EngineSettings.RegisterUserFont(fontStream);
            }

            // unregister all fonts registered via RegisterUserFonts()
            EngineSettings.UnregisterUserFonts();

            // create document
            using (Stream outputDocument = File.Create("document.pdf"))
            {
                FixedDocument document = new FixedDocument();
                document.Pages.Add(new Page());
                document.Save(outputDocument);
            }            

            /* Apitron PDF Rasterizer only usage*/
            using (Stream inputDocument = File.Open("document.pdf", FileMode.Open))
            {
                // create engine settings instance and set memory usage limit
                Apitron.PDF.Rasterizer.Configuration.EngineSettings settings = new Apitron.PDF.Rasterizer.Configuration.EngineSettings();
                settings.MemoryAllocationMode = Apitron.PDF.Rasterizer.Configuration.MemoryAllocationMode.ResourcesLowMemory;
                settings.ResourceSizeLimit = 2000000;

                // open PDF document using specific engine settings,
                // these settings will be applied to this document only
                using (Document doc = new Document(inputDocument, settings))
                {
                    // create rendering settings instance
                    RenderingSettings renderingSettings = new RenderingSettings();
                    // turn off annotations drawing for example
                    renderingSettings.DrawAnotations = false;                    

                    // render page
                    Bitmap bitmap = doc.Pages[0].Render(new Resolution(96, 96), renderingSettings);
                    bitmap.Save("page0.png");
                }
            }
        }
    }
}
