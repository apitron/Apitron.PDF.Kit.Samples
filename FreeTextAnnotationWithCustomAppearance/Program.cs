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
                double annotationX = 50;
                double annotationY = 440;
                double annotationWidth = 200;
                double annotationHeight = 40;

                Page page = new Page();
                
                FreeTextAnnotation annotation = new FreeTextAnnotation(new Boundary(annotationX, annotationY, annotationX +annotationWidth, annotationY+annotationHeight), AnnotationFlags.ReadOnly);                

                annotation.Title = "Apitron";
                annotation.Intent = IntentStyle.FreeText;                
               
                // set custom appearance normal and rollover states.
                // if you change the annotation to be editable, 
                // then Adobe reader will change this appearance to new when clicked               
                annotation.Appearance.Normal = CreateNormalAppearance(annotationWidth, annotationHeight);
                annotation.Appearance.Rollover = CreateRolloverAppearance(annotationWidth,annotationHeight);

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


        private static FixedContent CreateNormalAppearance(double width, double height)
        {
            FixedContent fixedContent = new FixedContent(Guid.NewGuid().ToString("N"), new Boundary(0, 0, width, height));


            TextBlock textBlock = new TextBlock("Free text annotation created using Apitron PDF Kit - normal");
            textBlock.Border = Border.Solid;
            textBlock.BorderColor = RgbColors.Blue;
            textBlock.Display = Display.Block;
            textBlock.Color = RgbColors.Red;
            textBlock.Width = width;
            textBlock.Height = height;

            fixedContent.Content.AppendContentElement(textBlock, width, height);
            return fixedContent;
        }

        private static FixedContent CreateRolloverAppearance(double width, double height)
        {
            FixedContent fixedContent = new FixedContent(Guid.NewGuid().ToString("N"), new Boundary(0, 0, width, height));
          
            TextBlock textBlock = new TextBlock("Free text annotation created using Apitron PDF Kit - rollover");
            textBlock.Border = Border.Solid;
            textBlock.BorderColor = RgbColors.Blue;
            textBlock.Display = Display.Block;
            textBlock.Color = RgbColors.Green;
            textBlock.Width = width;
            textBlock.Height = height;

            fixedContent.Content.AppendContentElement(textBlock, width, height);
            return fixedContent;
        }
    }
}
