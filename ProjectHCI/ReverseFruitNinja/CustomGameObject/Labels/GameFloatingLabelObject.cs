using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectHCI.KinectEngine;
using System.Windows.Media;
using System.Windows;

namespace ProjectHCI.ReverseFruitNinja
{
    public class GameFloatingLabelObject : FormattedTextGameObject
    {
        protected const int FLOATING_TIME_MILLIS = 2000;
        protected const double VERTICAL_TRANSACTION_OFFSET_PER_MILLIS = 30 / FLOATING_TIME_MILLIS;

        public static string points2string(int points)
        {
            string ret = (points >= 0 ? "+" : "-");

            return ret + points;
        }

        public GameFloatingLabelObject(IGameObject generatingObject, string text)
            : base(generatingObject.getXPosition(), generatingObject.getYPosition(), text, FLOATING_TIME_MILLIS)
        {
        }

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
                                            30,                // font size in em
                                            Brushes.Black);    //unused param, we create a geometry from this FormattedText
        }

        public override void update(int deltaTimeMillis)
        {
            base.update(deltaTimeMillis);
            //also translate label higher
            GameLoop.getSceneManager().applyTranslation(this, 0, VERTICAL_TRANSACTION_OFFSET_PER_MILLIS * deltaTimeMillis, true);
        }
    }
}
