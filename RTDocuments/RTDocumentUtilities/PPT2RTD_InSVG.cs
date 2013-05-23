using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;

using MSR.LST.RTDocuments;


namespace MSR.LST.RTDocuments.Utilities
{
    /// <summary>
    /// Summary description for PPTConversion.
    /// </summary>
    public class PPT2SVG
    {

        private static System.CodeDom.Compiler.TempFileCollection tfc = new System.CodeDom.Compiler.TempFileCollection();

        public static RTDocument PPT2RTDwithSVG(string pptFilename)
        {
            //Pri2: Investigate where BasePath is and where we should be putting it in more depth.
            //      Concerned that we're creating directories there that never get cleaned up...
            if (!Directory.Exists(tfc.BasePath))
                Directory.CreateDirectory(tfc.BasePath);
            // Initialize PowerPoint app
            ApplicationClass ppt = new ApplicationClass();
            ppt.Visible = MsoTriState.msoTrue;

            // Open the PPT file
            Presentation presentation = ppt.Presentations.Open(pptFilename, MsoTriState.msoFalse, MsoTriState.msoTrue, MsoTriState.msoTrue);
            PrintOptions po = presentation.PrintOptions;
            po.ActivePrinter = @"SVGmaker";
            System.Threading.Thread.Sleep(6000);
            po.OutputType = PpPrintOutputType.ppPrintOutputSlides;
            po.PrintInBackground = MsoTriState.msoFalse;
            Slides slides = presentation.Slides;

            // Set up the document
            RTDocument rtDoc = new RTDocument();
            rtDoc.Identifier = Guid.NewGuid();
            rtDoc.Metadata.Title = GetPresentationProperty(ppt.ActivePresentation, "Title");
            rtDoc.Metadata.Creator = GetPresentationProperty(ppt.ActivePresentation, "Author");

            //Iterate through the pages
            foreach(Slide s in slides)
            {
                // Set the page properties
                Page p = new Page();
                p.Identifier = Guid.NewGuid();
                p.Extension = GetSlideSVG(presentation, s);
                p.MimeType = "image/svg";  // TODO: look up the real value for this
                rtDoc.Resources.Pages.Add(p.Identifier, p);

                // Set the TOCNode properties for the page
                TOCNode tn = new TOCNode();
                tn.Title = GetSlideTitle(s);
                tn.Resource = p;
                tn.ResourceIdentifier = p.Identifier;
                // TODO: Insert thumbnail? (tn.thumbnail)
                rtDoc.Organization.TableOfContents.Add(tn);
            }

            // Close PPT
            presentation.Close();
            if (ppt.Presentations.Count == 0)
            {
                ppt.Quit();
            }
            ppt = null;

            tfc.Delete();

            return rtDoc;
        }

        private static string GetPresentationProperty(Presentation pres, string propertyName)
        {
            // Get the Document Properties
            object oDocBuiltInProps = pres.BuiltInDocumentProperties;

            // Get the property object
            Type typeDocBuiltInProps = oDocBuiltInProps.GetType();
            object oDocProp = typeDocBuiltInProps.InvokeMember("Item", 
                BindingFlags.Default | 
                BindingFlags.GetProperty, 
                null,oDocBuiltInProps, 
                new object[] {propertyName}, CultureInfo.InvariantCulture );

            // Get the property value and return it
            Type typeDocProp = oDocProp.GetType();
            return typeDocProp.InvokeMember("Value", 
                BindingFlags.Default |
                BindingFlags.GetProperty,
                null,oDocProp,
                new object[] { }, CultureInfo.InvariantCulture).ToString();
        }

        private static string GetSlideTitle(Slide s)
        {
            // If the slide has a Title shape, use that text
            if (s.Shapes.HasTitle == Microsoft.Office.Core.MsoTriState.msoTrue)
                return CleanString(s.Shapes.Title.TextFrame.TextRange.Text);
            else
            {
                if (s.Shapes.Count > 0)
                {
                    Microsoft.Office.Interop.PowerPoint.Shapes shapes = s.Shapes;
                    IEnumerator iEN = shapes.GetEnumerator();
                    // Return the first shape that has a TextFrame
                    while (iEN.MoveNext())
                    {
                        Microsoft.Office.Interop.PowerPoint.Shape shape = (Microsoft.Office.Interop.PowerPoint.Shape)iEN.Current;
                        if (shape.HasTextFrame == Microsoft.Office.Core.MsoTriState.msoTrue)
                        {
                            if (shape.TextFrame.TextRange.Text != string.Empty)
                                return CleanString(shape.TextFrame.TextRange.Text);
                        }
                    }
                    // If none of the shapes has a text frame, return the Slide Name
                    return s.Name;
                }
                else
                    // If there are no Shapes in the Slide, return the Slide Name
                    return s.Name;
            }
        }

        // Make sure all characters in a string are printable - replace any out of range characters by a space
        private static string CleanString(string str)
        {
            StringBuilder sb = new StringBuilder(str);
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (! (Char.IsLetterOrDigit(c) || Char.IsPunctuation(c) || Char.IsSymbol(c)))
                    sb[i] = ' ';

            }
            return sb.ToString();
        }


        private static String GetSlideSVG(Presentation pres, Slide s)
        {
            // Pxport the slide to a temporary file using SVGmaker
            string filename = tfc.BasePath + "\\" + Guid.NewGuid().ToString() + ".svg";
            tfc.AddFile(filename, false);
            pres.PrintOut(s.SlideNumber, s.SlideNumber, filename, 1, MsoTriState.msoFalse);

            // seems to prevent any deadlocking during opening the file due to waiting for the printer
            System.Threading.Thread.Sleep(250);

            // Load data from file
            System.IO.StreamReader sr = new StreamReader(filename);
            String svg = sr.ReadToEnd();
            sr.Close();

            return svg;
        }

    }
}
