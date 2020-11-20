using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.FixedLayout.Content;
using Apitron.PDF.Kit.FixedLayout.OptionalContent;
using Apitron.PDF.Kit.FlowLayout;
using Apitron.PDF.Kit.FlowLayout.Content;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CreateContentLayersOnPDFPage
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Stream stream = File.Create("manual.pdf"))
            {
                // create our PDF document
                using (FixedDocument doc = new FixedDocument())
                {
                    // turn on the layers panel when opened
                    doc.PageMode = PageMode.UseOC;
    
                    // register image resource
                    doc.ResourceManager.RegisterResource(new Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image("chair","../../data/chair.jpg"));
    
                    // FIRST STEP: create layer definitions, they should be registered as document resources
                    OptionalContentGroup group0 = new OptionalContentGroup("group0", "Page layers", IntentName.View);                    
                    doc.ResourceManager.RegisterResource(group0);
    
                    OptionalContentGroup group1 = new OptionalContentGroup("group1", "Chair image", IntentName.View);
                    doc.ResourceManager.RegisterResource(group1);                    
    
                    OptionalContentGroup group2 = new OptionalContentGroup("English", "English", IntentName.View);
                    doc.ResourceManager.RegisterResource(group2);
    
                    OptionalContentGroup group3 = new OptionalContentGroup("Dansk", "Dansk", IntentName.View);
                    doc.ResourceManager.RegisterResource(group3);
    
                    OptionalContentGroup group4 = new OptionalContentGroup("Deutch", "Deutch", IntentName.View);
                    doc.ResourceManager.RegisterResource(group4);
    
                    OptionalContentGroup group5 = new OptionalContentGroup("Русский", "Русский", IntentName.View);
                    doc.ResourceManager.RegisterResource(group5);
    
                    OptionalContentGroup group6 = new OptionalContentGroup("Nederlands", "Nederlands", IntentName.View);
                    doc.ResourceManager.RegisterResource(group6);
    
                    OptionalContentGroup group7 = new OptionalContentGroup("Français", "Français", IntentName.View);
                    doc.ResourceManager.RegisterResource(group7);
    
                    OptionalContentGroup group8 = new OptionalContentGroup("Italiano", "Italiano", IntentName.View);
                    doc.ResourceManager.RegisterResource(group8);
    
                    // SECOND STEP:
                    // create the configurations, it allows to combine the layers together in any order      
                        
                    // Default configuration:             
                    OptionalContentConfiguration config = new OptionalContentConfiguration("configuration");
                    // add groups to lists which define the rules controlling their visibility                   
                    // ON groups
                    config.OnGroups.Add(group0);
                    config.OnGroups.Add(group1);
                    config.OnGroups.Add(group2);
                             
                    // OFF groups               
                    config.OffGroups.Add(group3);
                    config.OffGroups.Add(group4);
                    config.OffGroups.Add(group5);
                    config.OffGroups.Add(group6);
                    config.OffGroups.Add(group7);
                    config.OffGroups.Add(group8);
    
                    // lock the image layer
                    config.LockedGroups.Add(group1);
    
                    // make other layers working as radio buttons
                    // only one translation will be visible at time
                    config.RadioButtonGroups.Add(new[] { group2, group3, group4, group5, group6, group7, group8 });                    
    
                    // show only groups referenced by visible pages
                    config.ListMode = ListMode.VisiblePages;
                    // initialize the states for all content groups
                    // for the default configuration it should be on
                    config.BaseState = OptionalContentGroupState.On;
                    // set the name of the presentation tree
                    config.Order.Name = "Default config";       
                    // create a root node + sub elements             
                    config.Order.Entries.Add(group0);
                    config.Order.Entries.Add(new OptionalContentGroupTree(group1, group2, group3, group4, group5, group6, group7, group8));                                       
    
                    // FINAL step: 
                    // assign the configuration properties to document
                    // all configurations and groups should be specified
                    doc.OCProperties = new OptionalContentProperties(config, new OptionalContentConfiguration[] {}, new[] { group0, group1, group2, group3, group4, group5, group6, group7, group8 });
    
                    // create page and assing top layer id to its content
                    // it will allow you to completely hide page's 
                    // content using the configuration we have created                    
                    Page page = new Page();
                    page.Content.OptionalContentID = "group0";
    
                    // create image layer 
                    ClippedContent imageBlock = new ClippedContent(0, 0, 245, 300);
                    // set the layer id
                    imageBlock.OptionalContentID = "group1";
                    imageBlock.AppendImage("chair", 0, 0, 245, 300);
    
                    // put the layer on page
                    page.Content.SaveGraphicsState();
                    page.Content.SetTranslation(0, 530);
                    page.Content.AppendContent(imageBlock);
                    page.Content.RestoreGraphicsState();
    
                    // append text layers
                    AppendTextLayers(page);
    
                    // add the page to the document and save it
                    doc.Pages.Add(page);
    
                    doc.Save(stream);
                }
            }
    
            Process.Start("manual.pdf");
        }
    
        static void AppendTextLayers(Page page)
        {
            page.Content.SaveGraphicsState();
            page.Content.SetTranslation(250, 325);
    
            // evaluate each property of a resource dictionary and add text to the PDF page
            foreach (PropertyInfo info in typeof(strings).GetRuntimeProperties())
            {
                if (info.PropertyType == typeof(string))
                {
                    ClippedContent textContent = new ClippedContent(0, 0, 300, 500);
                    // assign layer id
                    textContent.OptionalContentID = info.Name;
                    textContent.SetTranslation(0, 0);
    
                    // preprocess parsed elements and set additional properties
                    // for better visual appearance
                    IEnumerable<ContentElement> elements =  ContentElement.FromMarkup((string)info.GetValue(null));
                        
                    foreach (Br lineBreak in elements.OfType<Br>())
                    {
                        lineBreak.Height = 10;   
                    }
    
                    foreach (Section subSection in elements.OfType<Section>())
                    {
                        subSection.Font = new Apitron.PDF.Kit.Styles.Text.Font("HelveticaBold", 14);
                    }
    
                    // draw text
                    textContent.AppendContentElement(new Section(elements), 300, 500);
                    // put the text layer on page
                    page.Content.AppendContent(textContent);                 
                }
            }
    
            page.Content.RestoreGraphicsState();
        }
    }
}
