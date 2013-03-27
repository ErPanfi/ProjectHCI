using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectHCI.KinectEngine;
using System.Windows.Media;

namespace ProjectHCI.ReverseFruitNinja
{
    public class GameLengthLabelObject : FormattedTextGameObject
    {
        protected static string LABEL_HEADER = "Time  ";

        public GameLengthLabelObject(double xPosition,
                                       double yPosition)
            : base(xPosition, yPosition, LABEL_HEADER, -1)
        {
        }

        protected override FormattedText formatText(string stringText)
        {
            return new FormattedText(stringText,
                                            System.Globalization.CultureInfo.CurrentCulture,
                                            System.Windows.FlowDirection.LeftToRight,
                                            new Typeface("Arial"),          // the font... can be a custom font
                                            50,                             // font size in em
                                            Brushes.Black);                 //unused param, we create a geometry from this FormattedText
        }

        public override void update(int deltaTimeMillis)
        {
            base.update(deltaTimeMillis);
            //also update game length text
            System.Diagnostics.Debug.Assert(typeof(GameSceneBrain).IsAssignableFrom(GameLoop.getSceneBrain().GetType()), "Expected a GameSceneBrain object");
            int currentMillis = ((GameSceneBrain)GameLoop.getSceneBrain()).getGameLengthMillis();
            int hh = currentMillis / 3600000;
            currentMillis %= 3600000;
            int mm = currentMillis / 60000;
            currentMillis %= 60000;
            int ss = currentMillis / 1000;

            this.setText(LABEL_HEADER + hh + ":" + mm + ":" + ss);
        }
    }
}
