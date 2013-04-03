using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectHCI.KinectEngine;
using System.Windows.Media;
using System.Windows;

namespace ProjectHCI.ReverseFruitNinja
{
    public class GameStartCountdownLabelObject : GameLabelObject
    {

        private GameSceneBrain gameSceneBrain;
        private int currSec;

        public static int MIN_FONT_SIZE = 100;
        public static int MAX_FONT_SIZE = 400;
        public static int FONT_SIZE_INC_PER_SEC = 50;

        #region ctors and dtors

        public GameStartCountdownLabelObject(double xPosition, double yPosition, GameSceneBrain sceneBrain)
            : base(xPosition, yPosition, "" + (Configuration.getCurrentConfiguration().gameStartCountdownMillis / 1000), Configuration.getCurrentConfiguration().gameStartCountdownMillis)
        {
            this.gameSceneBrain = sceneBrain;
        }

        #endregion

        protected override FormattedText formatText(string stringText)
        {
            Typeface typeFace = new Typeface(new FontFamily(new Uri("pack://application:,,,/"), "./Resources/#Made in China"),
                                             FontStyles.Normal,
                                             FontWeights.Medium,
                                             FontStretches.Normal);
            int fontSize = MAX_FONT_SIZE - FONT_SIZE_INC_PER_SEC * (currSec - 1);
            if (fontSize < MIN_FONT_SIZE)
            {
                fontSize = MIN_FONT_SIZE;
            }
            return new FormattedText(stringText,
                                            System.Globalization.CultureInfo.CurrentCulture,
                                            System.Windows.FlowDirection.LeftToRight,
                                            typeFace,          // the custom embedded font
                                            fontSize,          // font size in em
                                            Brushes.Black);    //unused param, we create a geometry from this FormattedText
        }

        public override void update(int deltaTimeMillis)
        {
            this.timeToLive = this.gameSceneBrain.getGameStartCountdownMillis();
            currSec = (int) Math.Ceiling(this.timeToLive / 1000.0);
            string labelTailor;
            if (currSec > 5)
            {
                labelTailor = "...";
            }
            else
            {
                labelTailor = "";
            }

            this.setText(currSec + labelTailor);
        }
    }
}
