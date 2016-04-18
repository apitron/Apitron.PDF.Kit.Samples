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
    /// This program demonstrates how to create custom appearance for FreeText annotation 
    /// using generated <see cref="FixedContent"/>.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            using (Stream inputStream = File.Open("../../docs/input.pdf", FileMode.Open, FileAccess.ReadWrite),
                outputStream = new FileStream("out.pdf", FileMode.Create, FileAccess.ReadWrite))
            {
                // create new document
                using (FixedDocument document = new FixedDocument(inputStream))
                {
                    Page page = document.Pages[0];

                    // register image resource to be used as background for custom cloud annotation
                    document.ResourceManager.RegisterResource(
                        new Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image("cloud", "../../images/cloud.png", true));

                    // add two identical annotations to demonstrate normal and rollover appearance
                    page.Annotations.Add(
                        CreateRectangularFreeTextAnnotation(
                            "Free text annotation created using Apitron PDF Kit", 50, 550, 200, 40));

                    page.Annotations.Add(
                        CreateRectangularFreeTextAnnotation(
                            "Free text annotation created using Apitron PDF Kit", 50, 490, 200, 40));

                    // add image based annotation looking like a cloud
                    page.Annotations.Add(CreateCloudFreeTextAnnotation("Hmm...I'm also a FreeText annotation...",
                        350, 550, 200, 200));

                    // save changed copy of the document
                    document.Save(outputStream);                   
                }
            }

            Process.Start("out.pdf");
        }

        private static Annotation CreateRectangularFreeTextAnnotation(string text, double x, double y, double width, double height)
        {                      
            // create PDF annotation object
            FreeTextAnnotation annotation =
                new FreeTextAnnotation(
                    new Boundary(x, y, x + width, y + height),
                    AnnotationFlags.ReadOnly);

            annotation.Title = "Apitron";
            annotation.Intent = IntentStyle.FreeText;

            // set custom appearance for normal and rollover states.
            // if you remove the read-only flag from the annotation, 
            // Adobe reader will change this appearance to new when clicked . 
            // If custom appearance is set, it's used instead of default.            
            annotation.Appearance.Normal = CreateNormalAppearance(text, width, height);
            annotation.Appearance.Rollover = CreateRolloverAppearance(text, width, height);

            // set properties affecting default appearance to be used as fallback
            annotation.FontSize = 12;
            annotation.BorderEffect = new AnnotationBorderEffect(AnnotationBorderEffectStyle.NoEffect, 0);
            annotation.Contents = string.Format("{0} - default",text);
            // text and border color
            annotation.TextColor = RgbColors.Red.Components;
            // set  background here if needed
            annotation.Color = RgbColors.Green.Components;                             

            return annotation;            
        }       
                      
        /// <summary>
        /// Creates a fixed content object that contains 
        /// drawing instructions for normal annotation state.        
        /// </summary>        
        private static FixedContent CreateNormalAppearance(string text, double width, double height)
        {
            // create fixed content object, set its unique ID using guid.
            // this object will be implicitly added to page resources using this ID.
            FixedContent fixedContent = new FixedContent(Guid.NewGuid().ToString("N"), new Boundary(0, 0, width, height));

            // use text block from flow layout API subset, 
            // to quickly draw text in a fixed content container.
            TextBlock textBlock = new TextBlock(string.Format("{0} - normal", text));
            textBlock.Border = Border.Solid;
            textBlock.BorderColor = RgbColors.Blue;
            textBlock.Display = Display.Block;
            textBlock.Color = RgbColors.White;
            textBlock.Width = width;
            textBlock.Height = height;
            textBlock.Background = RgbColors.Green;

            fixedContent.Content.AppendContentElement(textBlock, width, height);
            return fixedContent;
        }

        /// <summary>
        /// Creates a fixed content object that contains 
        /// drawing instructions for rollover annotation state.        
        /// </summary>           
        private static FixedContent CreateRolloverAppearance(string text, double width, double height)
        {
            // create fixed content object, set its unique ID using guid.
            // this object will be implicitly added to page resources using this ID.
            FixedContent fixedContent = new FixedContent(Guid.NewGuid().ToString("N"), new Boundary(0, 0, width, height));

            // use text block from flow layout API subset, 
            // to quickly draw text in a fixed content container.
            TextBlock textBlock = new TextBlock(string.Format("{0} - rollover", text));
            textBlock.Border = Border.Solid;
            textBlock.BorderColor = RgbColors.Blue;
            textBlock.Display = Display.Block;
            textBlock.Color = RgbColors.Green;
            textBlock.Width = width;
            textBlock.Height = height;
            textBlock.Background = RgbColors.Yellow;

            fixedContent.Content.AppendContentElement(textBlock, width, height);
            return fixedContent;
        }

        /// <summary>
        /// Creates a fixed content object that contains 
        /// drawing instructions cloud-shaped annotation based on image.    
        /// </summary> 
        private static Annotation CreateCloudFreeTextAnnotation(string text, double x, double y, double width, double height)
        {
            FreeTextAnnotation annotation =
               new FreeTextAnnotation(
                   new Boundary(x, y, x + width, y + height),
                   AnnotationFlags.ReadOnly);

            annotation.Title = "Apitron";
            annotation.Intent = IntentStyle.FreeText;

            // set custom normal appearance,
            // if you change the annotation to be editable, 
            // then Adobe reader will change this appearance to new when clicked               
            annotation.Appearance.Normal = CreateNormalCloudAppearance(text, width, height);

            return annotation;
        }

        /// <summary>
        /// Creates a fixed content object that contains 
        /// drawing instructions for normal annotation state.
        /// </summary>        
        private static FixedContent CreateNormalCloudAppearance(string text, double width, double height)
        {
            // create fixed content object, set its unique ID using guid.
            // this object will be implicitly added to page resources using this ID.
            FixedContent fixedContent = new FixedContent(Guid.NewGuid().ToString("N"), new Boundary(0, 0, width, height));

            // append image using its resource id defined in program entry fn.
            fixedContent.Content.AppendImage("cloud", 0, 0, width, height);
            TextBlock textBlock = new TextBlock(text);
            textBlock.Color = RgbColors.Black;

            fixedContent.Content.Translate(40, -65);
            fixedContent.Content.AppendContentElement(textBlock, width, height);

            return fixedContent;
        }
    }
}
