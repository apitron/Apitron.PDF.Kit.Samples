#region

using System.Diagnostics;
using System.IO;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.FixedLayout.Content;
using Apitron.PDF.Kit.FixedLayout.OptionalContent;
using Apitron.PDF.Kit.FixedLayout.PageProperties;
using Apitron.PDF.Kit.FixedLayout.Resources;
using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;

#endregion

namespace Apitron.PDF.Kit.Samples
{
    // This sample shows how to create optional content in a PDF document.
    internal class Program
    {
        private static void Main(string[] args)
        {
            // create PDF documennt stream
            using (FileStream stream = new FileStream(@"..\..\..\..\OutputDocuments\OCG.pdf", FileMode.Create, FileAccess.ReadWrite))
            {
                FixedDocument document = new FixedDocument();

                // create four groups (or more)
                OptionalContentGroup group1 = new OptionalContentGroup("group1", "Page OCG", IntentName.View);
                document.ResourceManager.RegisterResource(group1);

                OptionalContentGroup group2 = new OptionalContentGroup("group2", "English", IntentName.View);
                document.ResourceManager.RegisterResource(group2);

                OptionalContentGroup group3 = new OptionalContentGroup("group3", "Russian", IntentName.View);
                document.ResourceManager.RegisterResource(group3);

                OptionalContentGroup group4 = new OptionalContentGroup("group4", "Chinees", IntentName.View);
                document.ResourceManager.RegisterResource(group4);

                // create configuration
                OptionalContentConfiguration config = new OptionalContentConfiguration("The first config");
                config.OnGroups.Add(group1);
                config.OffGroups.Add(group2);
                config.OffGroups.Add(group3);
                config.OffGroups.Add(group4);
                config.ListMode = ListMode.VisiblePages;
                config.BaseState = OptionalContentGroupState.On;
                config.Order.Name = "Config 0";
                config.Order.Entries.Add(group1);
                config.Order.Entries.Add(new OptionalContentGroupTree(group2, group3, group4));

                // create alternative configuration
                OptionalContentConfiguration config1 = new OptionalContentConfiguration("The second config");
                config1.OffGroups.Add(group1);
                config1.OffGroups.Add(group2);
                config1.OffGroups.Add(group3);
                config1.OffGroups.Add(group4);
                config1.ListMode = ListMode.VisiblePages;
                config1.BaseState = OptionalContentGroupState.Off;
                config1.Order.Name = "Config 1";
                config1.Order.Entries.Add(group1);
                config1.Order.Entries.Add(new OptionalContentGroupTree("Sub Layer", group2, group3, group4));


                // only one 'll be switched on
                config.RadioButtonGroups.Add(new [] {group2, group3, group4});

                // add all configuration properties
                document.OCProperties = new OptionalContentProperties(config, new OptionalContentConfiguration[] { config1 }, new[] { group1, group2, group3, group4 });

                ClippedContent content = new ClippedContent(new Boundary(200, 200));
                TextObject text = new TextObject(StandardFonts.CourierBold, 20);
                text.AppendText("Hello!");
                content.AppendText(text);

                ClippedContent content2 = new ClippedContent(new Boundary(200, 200));
                TextObject text2 = new TextObject(StandardFonts.TimesBold, 20);
                text2.AppendText("Привет!");
                content2.AppendText(text2);

                ClippedContent content3 = new ClippedContent(new Boundary(200, 200));
                TextObject text3 = new TextObject(StandardFonts.CourierBold, 20);
                text3.AppendText("您好");
                content3.AppendText(text3);

                content.OptionalContentID  = "group2";
                content2.OptionalContentID = "group3";
                content3.OptionalContentID = "group4";

                // add page
                Page page = new Page();
                page.Content.OptionalContentID = "group1";
                page.Content.SaveGraphicsState();
                page.Content.SetTranslation(100, 500);

                // add content
                page.Content.AppendContent(content);
                page.Content.AppendContent(content2);
                page.Content.AppendContent(content3);
                page.Content.RestoreGraphicsState();

                document.Pages.Add(page);

                document.Save(stream);
            }

            Process.Start(@"..\..\..\..\OutputDocuments\OCG.pdf");
        }
    }
}
