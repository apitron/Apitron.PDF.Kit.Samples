using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.FixedLayout.PageProperties;
using Apitron.PDF.Kit.FixedLayout.Resources.XObjects;
using Apitron.PDF.Kit.FlowLayout.Content;
using Apitron.PDF.Kit.Interactive.Annotations;
using Apitron.PDF.Kit.Styles;
using Apitron.PDF.Kit.Styles.Appearance;

namespace FreeTextAnnotationWithCustomAppearance
{
    /// <summary>
    /// This programs demonstrates how to create custom appearance for FreeText annotation 
    /// using generated <see cref="FixedContent"/>.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {           
            // create new document
            using (FixedDocument document = new FixedDocument())
            {
                Page page = new Page();
                
                FreeTextAnnotation annotation = new FreeTextAnnotation(new Boundary(50, 440, 250, 540), AnnotationFlags.ReadOnly);                
                annotation.Title = "Apitron";
                annotation.Intent = IntentStyle.FreeText;                
               
                // set custom appearance normal and rollover states.
                // if you change the annotation to be editable, 
                // then Adobe reader will change this appearance to new when clicked               
                annotation.Appearance.Normal = CreateNormalAppearance();
                annotation.Appearance.Rollover = CreateRolloverAppearance();

                // create default appearance, to be used as fallback
                annotation.FontSize = 12;
                annotation.BorderEffect = new AnnotationBorderEffect(AnnotationBorderEffectStyle.NoEffect, 0);

                annotation.Contents = "Free text annotation created using Apitron PDF Kit - default";
                // text and border color
                annotation.TextColor = RgbColors.Red.Components;
                
                // set  background here if needed
                // annotation.Color = RgbColors.Green.Components;                             

                page.Annotations.Add(annotation);
                document.Pages.Add(page);

                using (FileStream stream = new FileStream("out.pdf", FileMode.Create, FileAccess.ReadWrite))
                {
                    document.Save(stream);
                }

                Process.Start("out.pdf");
            }
        }

        private static FixedContent CreateNormalAppearance()
        {
            FixedContent fixedContent = new FixedContent(Guid.NewGuid().ToString("N"), new Boundary(0, 0, 200, 100));

            TextBlock textBlock = new TextBlock("Free text annotation created using Apitron PDF Kit - normal");
            textBlock.Border = Border.Solid;
            textBlock.BorderColor = RgbColors.Blue;
            textBlock.Display = Display.Block;
            textBlock.Color = RgbColors.Red;

            fixedContent.Content.AppendContentElement(textBlock, 200, 100);
            return fixedContent;
        }

        private static FixedContent CreateRolloverAppearance()
        {
            FixedContent fixedContent = new FixedContent(Guid.NewGuid().ToString("N"), new Boundary(0, 0, 200, 100));

            TextBlock textBlock = new TextBlock("Free text annotation created using Apitron PDF Kit - rollover");
            textBlock.Border = Border.Solid;
            textBlock.BorderColor = RgbColors.Blue;
            textBlock.Display = Display.Block;
            textBlock.Color = RgbColors.Green;

            fixedContent.Content.AppendContentElement(textBlock, 200, 100);
            return fixedContent;
        }
    }
}
