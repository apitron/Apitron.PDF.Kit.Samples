using System;
using System.Diagnostics;
using System.IO;
using Apitron.PDF.Kit;
using Apitron.PDF.Kit.FixedLayout;
using Apitron.PDF.Kit.FixedLayout.Content;
using Apitron.PDF.Kit.FixedLayout.Resources.Fonts;
using Apitron.PDF.Kit.FixedLayout.Resources.Functions;
using Apitron.PDF.Kit.FixedLayout.Resources.Patterns;
using Apitron.PDF.Kit.FixedLayout.Resources.Shadings;
using Apitron.PDF.Kit.FixedLayout.Resources.XObjects;
using Apitron.PDF.Kit.Styles;
using Path = Apitron.PDF.Kit.FixedLayout.Content.Path;

namespace GradientFillsUsingShadingsAndShadingPatterns
{
    class Program
    {
        static void Main(string[] args)
        {                                              
            // create document object
            FixedDocument doc = new FixedDocument();

            // create shading objects
            Shading axialShadingExp = CreateAndRegisterAxialShadingBasedOnExponentialFunction(doc, RgbColors.Red, RgbColors.Black);

            Shading axialShadingLinear = CreateAndRegisterAxialShadingBasedOnLinearInterpolationFunction(doc, new object[] { RgbColors.Red, RgbColors.Black });

            Shading axialShadingLinearMultipleColors = CreateAndRegisterAxialShadingBasedOnLinearInterpolationFunction(doc, new object[] { RgbColors.Red, RgbColors.Green, RgbColors.Blue, RgbColors.Black });
            
            Shading radialShading = CreateAndRegisterRadialShading(doc);

            Shading functionShading = CreateAndRegisterFunctionBasedShading(doc);
            
            // register shading pattern based on existing function-based shading
            ShadingPattern shadingPattern = new ShadingPattern(Guid.NewGuid().ToString(),functionShading.ID);
            doc.ResourceManager.RegisterResource(shadingPattern);

            // create document page
            Page firstPage = new Page();

            // draw shadings
            DrawShading(firstPage, axialShadingExp.ID, 10, 650);
            DrawShading(firstPage, axialShadingLinear.ID, 200, 650);
            DrawShading(firstPage, axialShadingLinearMultipleColors.ID, 390, 650);
            DrawShading(firstPage, functionShading.ID, 10, 460);
            DrawShading(firstPage, radialShading.ID, 200, 460);

            // draw the sample xObject demonstrating text fill using shading pattern
            firstPage.Content.AppendXObject(CreateAndRegisterXObject(doc,shadingPattern).ID, 390, 460);
            
            // add page to document
            doc.Pages.Add(firstPage);

            // save document
            using (Stream stream = File.Create("out.pdf"))
            {
                doc.Save(stream);
            }

            Process.Start("out.pdf");
        }

        // draws shading object on page at the specified coordinates
        private static void DrawShading(Page page, string resourceName, double xOffset, double yOffset)
        {
            page.Content.SaveGraphicsState();
            page.Content.Translate(xOffset, yOffset);
            page.Content.PaintShading(resourceName, Path.CreateRoundRect(0, 0, 180, 180, 10, 10, 10, 10));
            page.Content.RestoreGraphicsState();
        }       

        // creates a reusable piece of content (FormXObject)
        private static FixedContent CreateAndRegisterXObject(FixedDocument doc, ShadingPattern fillColor)
        {
            ClippedContent content = new ClippedContent(Path.CreateRoundRect(0, 0, 180, 180, 10, 10, 10, 10));

            content.SaveGraphicsState();

            // draw gray rect as a background for our text
            content.SetDeviceNonStrokingColor(RgbColors.LightGray.Components);
            content.FillPath(Path.CreateRect(0, 0, 180, 180));
            
            // draw text using shading pattern as a fill 
            content.Translate(3, 85);
            TextObject txt = new TextObject(StandardFonts.Helvetica, 25);
            // set the fill colorspace to Pattern in order to use the pattern as a color
            txt.SetNonStrokingColorSpace(PredefinedColorSpaces.Pattern);
            // set our pattern as a fill color
            txt.SetNonStrokingColor(fillColor.ID);
            txt.AppendText("Shading pattern");
            content.AppendText(txt);

            content.RestoreGraphicsState();

            FixedContent formXObject = new FixedContent(Guid.NewGuid().ToString(), new Boundary(180, 180), content);
            doc.ResourceManager.RegisterResource(formXObject);

            return formXObject;
        }

        // creates and registers radial shading object
        private static Shading CreateAndRegisterRadialShading(FixedDocument doc)
        {
            // create simple PS function that returns the following RGB color value: [(1-x),0,0]
            PostScriptFunction psFunction = new PostScriptFunction(Guid.NewGuid().ToString(),new double[]{0,1},new double[]{0,1,0,1,0,1}, "{1 exch sub 0 0}");
            RadialShading radialShading = new RadialShading(Guid.NewGuid().ToString(),PredefinedColorSpaces.RGB,new Boundary(180,180), RgbColors.White.Components,new double[]{120,110,0,90,90,90},new string[]{psFunction.ID});
            
            // register function and shading
            doc.ResourceManager.RegisterResource(psFunction);
            doc.ResourceManager.RegisterResource(radialShading);

            return radialShading;
        }

        // creates and registers function-based shading object
        private static Shading CreateAndRegisterFunctionBasedShading(FixedDocument doc)
        {
            // create post script functions for each color component
            // it's possible to use only for one function and calculate all components at once
            // the function used for R is 1- ((x-90)/90)^2 + ((y-90)/90)^2)
            PostScriptFunction psFunctionR = new PostScriptFunction(Guid.NewGuid().ToString(), new double[] {0, 180, 0, 180},
                new double[] {0, 1}, "{90 sub 90 div dup mul exch 90 sub 90 div dup mul add 1 exch sub}");
            // the function used for G is 1- ((x-90)/60)^2 + ((y-90)/60)^2)
            PostScriptFunction psFunctionG = new PostScriptFunction(Guid.NewGuid().ToString(), new double[] {0, 180, 0, 180},
                new double[] {0, 1}, "{90 sub 60 div dup mul exch 90 sub 60 div dup mul add 1 exch sub}");
            // the function used for G is 1- ((x-90)/30)^2 + ((y-90)/30)^2)
            PostScriptFunction psFunctionB = new PostScriptFunction(Guid.NewGuid().ToString(), new double[] {0, 180, 0, 180},
                new double[] {0, 1}, "{90 sub 30 div dup mul exch 90 sub 30 div dup mul add 1 exch sub}");

            // create shading based on three functions
            FunctionShading functionShading = new FunctionShading(Guid.NewGuid().ToString(), PredefinedColorSpaces.RGB,
                new Boundary(180, 180), RgbColors.Green.Components,
                new string[] {psFunctionR.ID, psFunctionG.ID, psFunctionB.ID});

            // register functions and shading object
            doc.ResourceManager.RegisterResource(psFunctionR);
            doc.ResourceManager.RegisterResource(psFunctionG);
            doc.ResourceManager.RegisterResource(psFunctionB);
            doc.ResourceManager.RegisterResource(functionShading);

            return functionShading;
        }

        // creates and registers axial shading object based on exponential function
        private static Shading CreateAndRegisterAxialShadingBasedOnExponentialFunction(FixedDocument doc, Color beginColor, Color endColor)
        {
            // exponential function producing the gradient
            Function expFn = new ExponentialFunction(Guid.NewGuid().ToString(), beginColor.Components, endColor.Components, 3, new double[] { 0, 1 });            

            // axial shading demonstrating exponential interpolation between two colors
            AxialShading axialShadingExp = new AxialShading(Guid.NewGuid().ToString(), PredefinedColorSpaces.RGB, new Boundary(0, 0, 180, 180), RgbColors.Green.Components, new double[] { 0, 90, 180, 90 }, new string[] { expFn.ID });

            doc.ResourceManager.RegisterResource(expFn);
            doc.ResourceManager.RegisterResource(axialShadingExp);

            return axialShadingExp;
        }

        // creates and registers axial shading object based on sampled function able to interpolate between multiple colors
        private static AxialShading CreateAndRegisterAxialShadingBasedOnLinearInterpolationFunction(FixedDocument doc, params object[] colors)
        {
            int samplesCount = colors.Length;
            double[] domain = new double[] {0, 1};
            double step = domain[1] / (samplesCount-1);

            // create the delegate returning color value for corresponding sample
            SamplerDelegate fn = (double[] input) => {
                        int k = 0;
                        double tmpStep = step;                                     
                        while (input[0] >= tmpStep)
                        {
                            tmpStep += step;
                            ++k;
                        }
                        return (colors[k] as Color).Components;
                    };

            // linear sampled function producing the gradient
            Function linearFn = new SampledFunction(Guid.NewGuid().ToString(), fn, domain , new double[] { 0, 1, 0, 1, 0, 1 }, new[] { samplesCount }, BitsPerSample.OneByte);

            // axial shading demonstrating linear interpolation between two colors
            AxialShading axialShadingLinear = new AxialShading(Guid.NewGuid().ToString(), PredefinedColorSpaces.RGB, new Boundary(0, 0, 180, 180), RgbColors.Green.Components, new double[] { 0, 90, 180, 90 }, new string[] { linearFn.ID });

            doc.ResourceManager.RegisterResource(linearFn);
            doc.ResourceManager.RegisterResource(axialShadingLinear);

            return axialShadingLinear;
        }

        
    }
}
