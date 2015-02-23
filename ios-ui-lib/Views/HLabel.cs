using System;
using UIKit;
using CoreGraphics;

namespace ios_ui_lib
{
    public class HLabel : UILabel
    {
        //Culled from http://blog.manniat.net/post/2010/01/17/How-to-build-a-UILabel-with-vertical-alignment.aspx
        public enum VerticalAlignments
        {
            Middle = 0,
            //the default (what standard UILabels do)
            Top,
            Bottom

        }

        #region VerticalAlignment

        private VerticalAlignments m_eVerticalAlignment;

        public VerticalAlignments VerticalAlignment
        { 
            get { return m_eVerticalAlignment; } 
            set
            { 
                if (m_eVerticalAlignment != value)
                { 
                    m_eVerticalAlignment = value; 
                    SetNeedsDisplay();    //redraw if value changed 
                } 
            } 
        }

        #endregion

        #region construction

        public HLabel()
        {
        }

        public HLabel(CGRect rF)
            : base(rF)
        {
        }
        //add other constructors if needed

        #endregion

        #region overrides (DrawText, TextRectForBounds)

        //normally it uses full size of the control - we change this
        public override void DrawText(CGRect rect)
        { 
            CGRect rErg = TextRectForBounds(rect, Lines); 
            base.DrawText(rErg); 
        }
        //calculate the rect for text output - depending on VerticalAlignment
        public override CGRect TextRectForBounds(CGRect bounds, nint numberOfLines)
        {

            CGRect rCalculated = base.TextRectForBounds(bounds, numberOfLines); 
            if (m_eVerticalAlignment != VerticalAlignments.Top)
            {    //no special handling for top
                if (m_eVerticalAlignment == VerticalAlignments.Bottom)
                { 
                    bounds.Y += bounds.Height - rCalculated.Height;    //move down by difference
                }
                else
                {    //middle == nothing set == somenthing strange ==> act like standard UILabel
                    bounds.Y += (bounds.Height - rCalculated.Height) / 2; 
                } 
            } 
            bounds.Height = rCalculated.Height;    //always the calculated height 
            return (bounds); 
        }

        #endregion
    }
}

