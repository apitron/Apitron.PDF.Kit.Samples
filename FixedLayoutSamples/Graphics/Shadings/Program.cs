namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.FixedLayout.Resources;
    using Apitron.PDF.Kit.FixedLayout.Resources.Functions;
    using Apitron.PDF.Kit.FixedLayout.Resources.Patterns;
    using Apitron.PDF.Kit.FixedLayout.Resources.Shadings;
    using Apitron.PDF.Kit.Styles;

    // This sample shows how to use graphics shading and shading functions.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path = @"..\..\..\..\OutputDocuments\Shadings.pdf";

            // open and load the file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                Page page = new Page();

                string functionShadingID = "functionShading";
                CreateFunctionShading(document.ResourceManager, functionShadingID);
                page.Content.PaintShading(functionShadingID, Apitron.PDF.Kit.FixedLayout.Content.Path.CreateRoundRect(50, 50, 250, 250, 30));

                string axialShadingID = "axialShading";
                CreateAxialShading(document.ResourceManager, axialShadingID);
                page.Content.PaintShading(axialShadingID, Apitron.PDF.Kit.FixedLayout.Content.Path.CreateRoundRect(50, 300, 250, 250, 30));

                string radialShadingID = "radialShading";
                CreateRadialShading(document.ResourceManager, radialShadingID);
                page.Content.PaintShading(radialShadingID, Apitron.PDF.Kit.FixedLayout.Content.Path.CreateRoundRect(50, 550, 250, 250, 30));

                page.Content.SetTranslation(250, 0);
                page.Content.SetNonStrokingColorSpace(PredefinedColorSpaces.Pattern);

                ShadingPattern sh01 = new ShadingPattern("sh0", functionShadingID);
                document.ResourceManager.RegisterResource(sh01);
                page.Content.SetNonStrokingColor("sh0");
                page.Content.FillPath(Apitron.PDF.Kit.FixedLayout.Content.Path.CreateRoundRect(50, 50, 250, 250, 30));

                ShadingPattern sh02 = new ShadingPattern("sh1", axialShadingID);
                document.ResourceManager.RegisterResource(sh02);
                page.Content.SetNonStrokingColor("sh1");
                page.Content.FillPath(Apitron.PDF.Kit.FixedLayout.Content.Path.CreateRoundRect(50, 300, 250, 250, 30));

                ShadingPattern sh03 = new ShadingPattern("sh2", radialShadingID);
                document.ResourceManager.RegisterResource(sh03);
                page.Content.SetNonStrokingColor("sh2");
                page.Content.FillPath(Apitron.PDF.Kit.FixedLayout.Content.Path.CreateRoundRect(50, 550, 250, 250, 30));


                document.Pages.Add(page);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }

        public static void CreateFunctionShading(ResourceManager rm, string resourceID)
        {
            FunctionShading shading = new FunctionShading(resourceID, PredefinedColorSpaces.RGB, Boundaries.A4, new double[] { 0.3, 0.2, 0.7 }, new string[] { resourceID + "fn0", resourceID + "fn1", resourceID + "fn2" }, new double[] { 0, 1, 0, 1 }, new double[] { 400, 0, 0, 250, 75, 50 });
            rm.RegisterResource(shading);

            // Create 2-in 1-out postscript function
            PostScriptFunction fn0 = new PostScriptFunction(resourceID + "fn0", new double[] { 0, 1, 0, 1 }, new double[] { 0, 1 }, "{ 360 mul sin 2 div exch 360 mul sin 2 div add }");
            PostScriptFunction fn1 = new PostScriptFunction(resourceID + "fn1", new double[] { 0, 1, 0, 1 }, new double[] { 0, 1 }, "{ 360 mul sin 2 div exch 360 mul sin 2 div add dup mul }");
            PostScriptFunction fn2 = new PostScriptFunction(resourceID + "fn2", new double[] { 0, 1, 0, 1 }, new double[] { 0, 1 }, "{ 360 mul sin 2 div exch 360 mul sin 2 div add dup add 2 div }");

            rm.RegisterResource(fn0);
            rm.RegisterResource(fn1);
            rm.RegisterResource(fn2);
        }

        public static void CreateAxialShading(ResourceManager rm, string resourceID)
        {
            AxialShading shading = new AxialShading(resourceID, PredefinedColorSpaces.RGB, Boundaries.A4, new double[] { 0.3, 0.2, 0.7 }, new double[] { 100, 300, 550, 400 }, new string[] { resourceID + "fn0" }, new bool[] { true, true });
            rm.RegisterResource(shading);

            // Create 1-in 3-out sampled function
            SampledFunction fn0 = new SampledFunction(resourceID + "fn0", sampler, new double[] {0,1}, new double[] {0,1,0,1,0,1}, new int[] {100}, BitsPerSample.TwoBytes );
            rm.RegisterResource(fn0);
        }

        public static void CreateRadialShading(ResourceManager rm, string resourceID)
        {
            RadialShading shading = new RadialShading(resourceID, PredefinedColorSpaces.RGB, Boundaries.A4, new double[] {0.3, 0.2, 0.7}, new double[] {250, 600, 0, 335, 675, 200}, new string[] {resourceID + "fn0"});
            rm.RegisterResource(shading);

            // Create 1-in 3-out sampled function
            ExponentialFunction fn0 = new ExponentialFunction(resourceID + "fn0", new double[] {0.1, 0.3, 0.2}, new double[] {0.8, 0.5, 0.7}, 3, new double[] {0, 1});
            rm.RegisterResource(fn0);
        }

        private static double[] sampler(double[] input)
        {
            return new double[] {input[0], input[0] * input[0], input[0]/input[0]};
        }
    }
}
