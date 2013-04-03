using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectHCI.KinectEngine;
using System.Windows.Media;
using System.Windows;


namespace ProjectHCI.ReverseFruitNinja
{
    public class GameLabelObject : FormattedTextGameObject
    {
        #region ctors and dtors

        public GameLabelObject(double xPosition, double yPosition, String text, int timeToLive)
            : base(xPosition, yPosition, text, timeToLive)
        {
        }

        #endregion

        //only override formatting style
        protected override FormattedText formatText(string stringText)
        {
            Typeface typeFace = new Typeface(new FontFamily(new Uri("pack://application:,,,/"), "./Resources/#Made in China"),
                                             FontStyles.Normal,
                                             FontWeights.Medium,
                                             FontStretches.Normal);
            return new FormattedText(stringText,
                                            System.Globalization.CultureInfo.CurrentCulture,
                                            System.Windows.FlowDirection.LeftToRight,
                                            typeFace,          // the custom embedded font
                                            70,                // font size in em
                                            Brushes.Black);    //unused param, we create a geometry from this FormattedText
        }
    }
}
