using System;

// The Ink namespace, which contains the Tablet PC Platform API
// Note: must add a ref to:
// Microsoft Tablet PC Platform SDK\Include\Microsoft.Ink.dll
using Microsoft.Ink;


namespace MSR.LST.ConferenceXP
{
    /// <summary>
    /// The ScaledInk class allow to have Ink with a pageWidth and pagHeigth.
    /// This is used in CXP Presentation to store Ink with the size of the picture
    /// box. So if the picture box size has changed when we retrieve the Ink, we can
    /// resacle the Ink accordingly using pageWidth and pageHeight.  
    /// </summary>
    public class ScaledInk
    {
        public Ink PageInk;
        public float PageWidth;
        public float PageHeight;

        /// <summary>
        /// Ctor of ScaledInk class.
        /// </summary>
        /// <param name="pageInk">Ink of the page</param>
        /// <param name="pageWidth">Page width</param>
        /// <param name="pageHeight">Page height</param>
        public ScaledInk(Ink pageInk, float pageWidth, float pageHeight)
        {
            PageInk = pageInk;
            PageWidth = pageWidth;
            PageHeight = pageHeight;
        }
    }
}
