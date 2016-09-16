using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.FixedLayout.Resources.ColorSpaces.Device;
using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;
using Apitron.PDF.Kit.FixedLayout.Resources.XObjects;
using Apitron.PDF.Kit.FlowLayout.Content;
using Apitron.PDF.Kit.Interactive.Annotations;
using Apitron.PDF.Kit.Interactive.FormsData;
using Apitron.PDF.Kit.Styles;
using Apitron.PDF.Kit.Styles.Appearance;
using Apitron.PDF.Kit.Styles.Text;
using Image = Apitron.PDF.Kit.FlowLayout.Content.Image;

namespace ModifyPDFUsingFDF
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("1. For adding annotations using FDF press '1'," +
                          "\n2. To fill the sample form using loaded FDF press '2'," +
                          "\n3. To fill the original sample form using FDF created on the fly press '3'," +
                          "\n4. To fill the compact sample form using FDF created on the fly press '4'," +
                          "\n5. To add watermark using FDF created on the fly press '5'," +
                          "\n6. To generate FDF from PDF press '6'," +
                          "\n7. To insert new pages to PDF doc using FDF page templates press '7'," +
                          "\nTo exit press ESC:");

            bool exit = false;
            do
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(false);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1:
                        AddAnnotationsUsingFDF();
                        break;
                    case ConsoleKey.D2:
                        FillFormUsingLoadedFDF();
                        break;
                    case ConsoleKey.D3:
                        CreateFDFOnTheFlyAndFillForm("interactiveForm");
                        break;
                    case ConsoleKey.D4:
                        CreateFDFOnTheFlyAndFillForm("interactiveFormCompact");
                        break;
                    case ConsoleKey.D5:
                        AddWatermarkUsingFDF();
                        break;
                    case ConsoleKey.D6:
                        GenerateFDF();
                        break;
                    case ConsoleKey.D7:
                        AddPagesUsingFDF();
                        break;
                    case ConsoleKey.Escape:
                        exit = true;
                        break;
                }
            } while (!exit);
        }

        #region Add annotations using FDF generated on the fly

        private static void AddAnnotationsUsingFDF()
        {
            // open pdf document that later can be used 
            // as a target for applying FDF
            using (Stream inputStream = File.Open("../../data/interactiveForm.pdf", FileMode.Open),
                outputStream = File.Create("documentWithAnnotations.pdf"))
            {
                using (FixedDocument targetPDFDocument = new FixedDocument(inputStream))
                {
                    // prepare our forms data document
                    using (FormsDataDocument fdfDocument = new FormsDataDocument())
                    {
                        // set default file this document applies to
                        fdfDocument.File = "interactiveForm.pdf";
                        // add two annotations here, they will be added the target PDF document
                        fdfDocument.Annotations.Add(new CircleAnnotation(new Boundary(100, 300, 300, 500),
                            AnnotationFlags.Default,
                            new AnnotationBorderStyle())
                        {
                            Color = RgbColors.Red.Components,
                            InteriorColor = RgbColors.Yellow.Components,
                            Opacity = 0.5
                        });

                        fdfDocument.Annotations.Add(new TextAnnotation(190, 390)
                        {
                            Contents = "A sample annotation created usign FDF!",
                            IsOpen = true
                        });

                        // optionally save the FDF, it can be opened later and used
                        // for applying the content to PDF files
                        using (FileStream fileStream = File.Create("annotations.fdf"))
                        {
                            fdfDocument.Save(fileStream);
                        }

                        // Apply FDF and save the result. It can be also applied using Adobe PDF Reader, 
                        // just open it and it will be applied automatically using thes file name
                        // set to fdfDocument.File property.
                        targetPDFDocument.ApplyFormsDataDocument(fdfDocument);

                        targetPDFDocument.Save(outputStream);
                    }
                }
            }

            Process.Start("documentWithAnnotations.pdf");
        }

        private static void AddWatermarkUsingFDF()
        {
            // open pdf document that later can be used 
            // as a target for applying FDF
            using (Stream inputStream = File.Open("../../data/topSecretDocument.pdf", FileMode.Open),
                outputStream = File.Create("stampedAsTopSecret.pdf"))
            {
                using (FixedDocument targetPDFDocument = new FixedDocument(inputStream))
                {
                    // prepare our forms data document
                    using (FormsDataDocument fdfDocument = new FormsDataDocument())
                    {
                        // register image resource for future use
                        fdfDocument.ResourceManager.RegisterResource(new Apitron.PDF.Kit.FixedLayout.Resources.XObjects.Image("lock","../../data/lock.png",true));

                        // set default file this document applies to
                        fdfDocument.File = "topSecretDocument.pdf";

                        double xOffsetFromLeft = 10;
                        double yOffsetFromTop = 10;
                        double contentHeight = 85;
                        double contentWidth = Boundaries.Letter.Width - 20;

                        // add watermark annotation here
                        WatermarkAnnotation watermarkAnnotation =
                            new WatermarkAnnotation(
                                new Boundary(xOffsetFromLeft, Boundaries.Letter.Height-yOffsetFromTop-contentHeight, xOffsetFromLeft+contentWidth, Boundaries.Letter.Height-yOffsetFromTop ),
                                AnnotationFlags.Default,
                                new AnnotationBorderStyle());

                        // generate custom content
                        FixedContent annotationContent = new FixedContent(Guid.NewGuid().ToString("N"),new Boundary(0,0,watermarkAnnotation.Boundary.Width,watermarkAnnotation.Boundary.Height));

                        // create fixed size section
                        Section section = new Section
                        {
                            Border = new Border(1),
                            BorderColor = RgbColors.Red,
                            Width = contentWidth,
                            Height = contentHeight,
                            Padding = new Thickness(5),
                            LineHeight = 36,
                        };

                        // create text content
                        TextBlock text = new TextBlock("TOP SECRET - AUTHORIZED PERSONNEL ONLY, DESTROY THIS DOC AFTER READING.")
                        {
                            Color = RgbColors.Red,
                            Font = new Apitron.PDF.Kit.Styles.Text.Font("Arial",26),
                            TextRenderingMode = TextRenderingMode.Stroke,
                            VerticalAlign = VerticalAlign.Middle
                        };

                        // add an image to the annotation's content,
                        // set its float property to let the text flow.
                        Image image = new Image("lock")
                        {
                            Float = Float.Left
                        };

                        section.Add(image);
                        section.Add(text);

                        // set the content
                        annotationContent.Content.AppendContentElement(section,annotationContent.Boundary.Width,annotationContent.Boundary.Height);
                        watermarkAnnotation.Watermark = annotationContent;
                        fdfDocument.Annotations.Add(watermarkAnnotation);

                        // optionally save the FDF, it can be opened later and used
                        // for applying the content to PDF files
                        using (FileStream fileStream = File.Create("topSecretAnnotation.fdf"))
                        {
                            fdfDocument.Save(fileStream);
                        }

                        // Apply FDF and save the result. It can be also applied using Adobe PDF Reader, 
                        // just open it and it will be applied automatically using the the file name
                        // set to fdfDocument.File property.
                        targetPDFDocument.ApplyFormsDataDocument(fdfDocument);

                        targetPDFDocument.Save(outputStream);
                    }
                }
            }

            Process.Start("stampedAsTopSecret.pdf");
        }

        #endregion

        #region Fill form using loaded pre-generated PDF

        private static void FillFormUsingLoadedFDF()
        {
            using (Stream inputStream = File.Open("../../data/interactiveForm.pdf", FileMode.Open, FileAccess.ReadWrite),
                outputStream =File.Create("filledForm.pdf"))
            {
                // open the target PDF file
                using (FixedDocument target = new FixedDocument(inputStream))
                {
                    // open the prepared FDF doc
                    using (Stream fdfStream = File.Open("../../data/formData.fdf", FileMode.Open))
                    {
                        using (FormsDataDocument fdfDocument = new FormsDataDocument(fdfStream))
                        {
                            // fill the FDF and apply it
                            fdfDocument.Fields["Name_First"].SetValue("John");
                            fdfDocument.Fields["Name_Last"].SetValue("Doe");
                            fdfDocument.Fields["Name_Middle"].SetValue("Middle");

                            target.ApplyFormsDataDocument(fdfDocument);
                        }
                    }

                    target.Save(outputStream);
                }
            }

            Process.Start("filledForm.pdf");
        }

        #endregion

        #region Fill form using FDF created on the FLY (3)(4)

        private static void CreateFDFOnTheFlyAndFillForm(string pdfFormFile)
        {
            using (Stream inputStream = File.Open(string.Format("../../data/{0}.pdf",pdfFormFile), FileMode.Open),
                outputStream = File.Create("filledOnTheFlyForm.pdf"))
            {
                using (FixedDocument target = new FixedDocument(inputStream))
                {
                    // create the forms data document and add
                    // a few fields we know the names of along with their values.
                    using (FormsDataDocument fdfDocument = new FormsDataDocument())
                    { 
                        // add the first name field
                        FdfField firstName = new FdfField("Name_First", FdfFieldType.Text);
                        firstName.SetValue("John");

                        // add the last name field
                        FdfField lastName = new FdfField("Name_Last", FdfFieldType.Text);
                        lastName.SetValue("Doe");

                        // add the last name field
                        FdfField middleName = new FdfField("Name_Middle", FdfFieldType.Text);
                        middleName.SetValue("Alvanda");

                        fdfDocument.Fields.Add(firstName);
                        fdfDocument.Fields.Add(lastName);
                        fdfDocument.Fields.Add(middleName);

                        // apply the FDF doc to the target and save,
                        // now we should have field's values copied to the 
                        // original PDF doc
                        target.ApplyFormsDataDocument(fdfDocument);

                        target.Save(outputStream);
                    }
                }
            }

            Process.Start("filledOnTheFlyForm.pdf");
        }

        #endregion

        #region Generate FDF from PDF (5)

        private static void GenerateFDF()
        {
            using (Stream fileStream = File.Open("../../data/interactiveForm.pdf", FileMode.Open, FileAccess.ReadWrite))
            {
                using (FixedDocument target = new FixedDocument(fileStream))
                {
                    // dump PDF doc's form data to FDF file
                    using (FormsDataDocument fdfDocument = target.AcroForm.ExportFormsData())
                    {
                        using (FileStream fdfOutputStream = File.Create("interactiveForm.fdf"))
                        {
                            fdfDocument.Save(fdfOutputStream);
                        }
                    }
                }
            }
        }

        #endregion

        #region Add pages using FDF

        private static void AddPagesUsingFDF()
        {
            // create an empty PDF document (or you can an existing one)
            using (FixedDocument target = new FixedDocument())
            {
                // create and fill new FDF document
                using (FormsDataDocument fdfDoc = new FormsDataDocument())
                {
                    // create template using first template source
                    FdfNamedPageReference pageReference1 = new FdfNamedPageReference("page1",
                        new FileSpecification("../../data/templateSource1.pdf"));
                    FdfTemplate template1 = new FdfTemplate(pageReference1);

                    // create template using second template source
                    FdfNamedPageReference pageReference2 = new FdfNamedPageReference("page2",
                        new FileSpecification("../../data/templateSource2.pdf"));
                    FdfTemplate template2 = new FdfTemplate(pageReference2);

                    // add pages based on templates created above
                    fdfDoc.Pages.Add(new FdfPage(new[] {template1}));
                    fdfDoc.Pages.Add(new FdfPage(new[] {template2}));

                    // add combined page, its content is composed using several templates
                    fdfDoc.Pages.Add(new FdfPage(new[] {template1, template2}));

                    target.ApplyFormsDataDocument(fdfDoc);
                }

                using (Stream outputStream = File.Create("updatedDocumentWithAdditionalPages.pdf"))
                {
                    target.Save(outputStream);
                }
            }

            Process.Start("updatedDocumentWithAdditionalPages.pdf");
        }

        // shows how to create PDF document and define page template
        private static void CreatePDFDocumentAndSetTemplateName()
        {
            // create PDF doc
            using (FixedDocument doc = new FixedDocument())
            {
                // create new page and add some content
                Page page = new Page();

                page.Content.Translate(10, 700);
                page.Content.AppendContentElement(
                    new TextBlock(
                        @"This page is used as a template in FDF document, its name is 'page1'. You can manage templates using Tools->Document Processing->Page Templates")
                    {
                        Color = RgbColors.Red
                    }, 580, 100);

                doc.Pages.Add(page);
                 
                // define the template by providing its name and page it refers to
                doc.Names.Pages.Add("page1",page);

                using (Stream stream = File.Create("templateSource.pdf"))
                {
                    doc.Save(stream);
                }
            }
        }

        #endregion
    }
}
