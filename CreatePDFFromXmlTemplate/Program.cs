using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout.Resources;
using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;
using Apitron.PDF.Kit.FlowLayout.Content;
using Apitron.PDF.Kit.Styles;
using Apitron.PDF.Kit.Styles.Appearance;
using Font = Apitron.PDF.Kit.Styles.Text.Font;

namespace CreatePDFFromXmlTemplate
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length==0)
            {
                CreateTemplate();
            }
            else
            {
                LoadTemplate(args[0]);
            }
        }

        private static void LoadTemplate(string templatePath)
        {
            using (Stream stream = File.OpenRead(templatePath), outputStream = File.Create("fromTemplate.pdf"))
            {
                ResourceManager resourceManager = new ResourceManager();

                FlowDocument doc = FlowDocument.LoadFromXml(stream, resourceManager );

                doc.Write(outputStream, resourceManager);
            }
        }

        private static void CreateTemplate()
        {
            // create resource manager and register image resource
            ResourceManager resourceManager = new ResourceManager();
            resourceManager.RegisterResource(new Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image("logo",
                "../../images/logo.png", true));

            // create document object and register styles
            FlowDocument doc = new FlowDocument(){Margin = new Thickness(30, 20, 30, 20) };       
            // style for image logo
            doc.StyleManager.RegisterStyle(".logo", new Style() {Margin = new Thickness(0, 0, 10, 10)});
            // style for note below the header
            doc.StyleManager.RegisterStyle(".headerNote", new Style() { Display = Display.InlineBlock, Align = Align.Justify });
            // style for header
            doc.StyleManager.RegisterStyle(".header",
               new Style()
               {
                   Font = new Font(StandardFonts.HelveticaBold, 18),
                   Display = Display.InlineBlock,
                   Align = Align.Right,
                   VerticalAlign = VerticalAlign.Bottom,
                   Margin = new Thickness(0, 0, 0, 10)
               });
            // style for unordered list
            doc.StyleManager.RegisterStyle(".ul", new Style()
                {
                    ListStyle = ListStyle.Unordered,
                    ListMarker = ListMarker.Circle,
                    ListMarkerPadding = new Thickness(0, 0, 10, 0),
                    Margin = new Thickness(0, 20, 0, 0)
                });
            //style for list items
            doc.StyleManager.RegisterStyle(".ul > *", new Style()
                {
                    ListStyle = ListStyle.ListItem,
                    Margin = new Thickness(0, 10, 0, 10)
                });
    
            doc.Add(new Image("logo") {Class = "logo"});

            doc.Add(new TextBlock("Sample Interview Questions for Candidates") {Class = "header"});
            doc.Add(new Br());
            doc.Add(new TextBlock(
                    "To help facilitate the interview process the Human Resources Department " +
                    "has compiled a list of questions that might be used during the phone " +
                    "and/or on-campus interviews. " +
                    "Some of the questions deal with the same content, " +
                    "but are phrased differently while other questions may not pertain " +
                    "to a specific discipline; however all of the questions" +
                    " are unbiased and appropriate to ask. We hope you'll find " +
                    "this helpful.") {Class = "headerNote"});

            // add questions list
            Section list = new Section() {Class = "ul"};

            list.Add(new TextBlock("What do you consider to be one of your greatest achievements? Why?"));
            list.Add(new TextBlock("What is the latest technology innovation you're aware of?"));
            list.Add(new TextBlock("What motivates you to do your best?"));
            list.Add(new TextBlock("Why did you choose to apply to this position?"));
            list.Add(new TextBlock("What do you consider to be your particular strength(s)?"));
            list.Add(new TextBlock("What areas would you like to improve during the next couple of years?"));
            list.Add(new TextBlock("Why are you interested in working in the COMPANY?"));
            list.Add(new TextBlock("What do you consider to be your particular weakness(es)?"));
            list.Add(new TextBlock("What is the most exciting thing you've been working on recently?"));
            list.Add(new TextBlock("Do you like to be a part of the team or prefer to work separately?"));
            list.Add(new TextBlock("Are you able to work remotely?"));
            list.Add(new TextBlock("Why should we hire you?"));
            list.Add(new TextBlock("Where do you see yourself in a two years?"));
            list.Add(new TextBlock("Why are you leaving your current job?"));
            list.Add(new TextBlock("What type of work environment do you prefer?"));

            doc.Add(list);

            // export as PDF
            using (Stream stream = File.Create("out.pdf"))
            {
                doc.Write(stream, resourceManager);
            }
    
            // create xml template
            using (Stream stream = File.Create("template.xml"))
            {
                doc.SaveToXml(stream, resourceManager);
            }

            Process.Start("out.pdf");
        }
    }
}
