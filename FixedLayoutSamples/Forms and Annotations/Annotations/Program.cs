namespace Apitron.PDF.Kit.Samples
{
    using System.IO;
    using Apitron.PDF.Kit;
    using Apitron.PDF.Kit.FixedLayout;
    using Apitron.PDF.Kit.Interactive.Annotations;
	using Apitron.PDF.Kit.Interactive.Annotations.RichText;


    // This sample shows how to add text annotations (sticky notes) to your PDF document.
    internal class Program
    {
        private static void Main(string[] args)
        {
            string out_path =@"..\..\..\..\OutputDocuments\Annotations.pdf";
            
			RichTextBuilder rtb = new RichTextBuilder(new RichTextStyle() { Color = RichTextRgb.FromString("rgb(100,100,100)"), FontStretch = FontStretch.Expanded });
            rtb.AppendText("Hello world !!!\r\n", new RichTextStyle() { FontStyle = FontStyle.Italic });
            rtb.AppendText("This is underligned text.\r", new RichTextStyle() { TextDecoration = TextDecoration.Underline });
            rtb.AppendText("This is red text\r", new RichTextStyle() { Color = new RichTextRgb(255) });
            rtb.AppendText("And this is default ");
            rtb.AppendText("TEXT");
            rtb.AppendText("\rAnd this is big one", new RichTextStyle() { FontWeight = FontWeight.FontWeight900, FontSize = 30 });

			
            // create new file
            using (FileStream fs = new FileStream(out_path, FileMode.Create))
            {
                // this object represents a PDF fixed document
                FixedDocument document = new FixedDocument();
                Page page = new Page();
          
		      // add text annotation with Richtext style
				TextAnnotation annotation = new TextAnnotation(100, 100);
                annotation.Icon = AnnotationIconNames.Key;
                annotation.StrokingOpacity = 0.4;
                annotation.BlendMode = FixedLayout.Resources.GraphicsStates.BlendMode.ColorBurn;
                annotation.RichTextString = rtb.ToString();
                annotation.Contents = rtb.ToTextString();
                annotation.Popup = new PopupAnnotation(new Boundary(110, 110, 200, 200));

                page.Annotations.Add(annotation);
                page.Annotations.Add(annotation.Popup);


                // add text annotations
                TextAnnotation textAnnotation = new TextAnnotation(100, 10);
                textAnnotation.Contents = "Hello Apitron";

                TextAnnotation replyAnnotation = new TextAnnotation(100, 50);
                replyAnnotation.ReplyTo = textAnnotation;
                replyAnnotation.Contents = "Reply me.";

                TextAnnotation replyAnnotation1 = new TextAnnotation(100, 100);
                replyAnnotation1.ReplyTo = textAnnotation;
                replyAnnotation1.Contents = "Reply to you.";

                TextAnnotation replyAnnotation2 = new TextAnnotation(100, 150);
                replyAnnotation2.ReplyTo = textAnnotation;
                replyAnnotation2.Contents = "Reply to you.";

                // popup
                replyAnnotation1.Popup = new PopupAnnotation(new Boundary(200, 200, 300, 300));
                replyAnnotation1.Popup.Color = new double[] { 1, 0, 0 };

                // different annotation states
                StateAnnotation setAceptToReply     = new StateAnnotation(replyAnnotation1, AnnotationReplyType.Single, AnnotationState.Accepted);
                StateAnnotation setMarkedToReply    = new StateAnnotation(replyAnnotation,  AnnotationReplyType.Single, AnnotationState.Marked);
                StateAnnotation setCompletedToReply = new StateAnnotation(replyAnnotation2, AnnotationReplyType.Single, AnnotationState.Completed);


                // add all annotations into the page collection
                page.Annotations.Add(textAnnotation);
                page.Annotations.Add(replyAnnotation);
                page.Annotations.Add(replyAnnotation1);
                page.Annotations.Add(replyAnnotation2);
                page.Annotations.Add(replyAnnotation1.Popup);
                page.Annotations.Add(setAceptToReply);
                page.Annotations.Add(setMarkedToReply);
                page.Annotations.Add(setCompletedToReply);

                // add page and save document
                document.Pages.Add(page);
                document.Save(fs);
            }

            System.Diagnostics.Process.Start(out_path);
        }
    }
}