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
        protected const string LABEL_HEADER_DEFAULT = "Time  ";

        protected string labelHeader;
        protected IGameStateTracker gameStateTracker;


        #region ctors and dtors

        public GameLengthLabelObject(double xPosition, double yPosition, string headerText, IGameStateTracker gameStateTracker)
            : base(xPosition, yPosition, headerText, -1)
        {
            this.labelHeader = headerText;
            this.gameStateTracker = gameStateTracker;
        }

        public GameLengthLabelObject(double xPosition, double yPosition, IGameStateTracker gameStateTracker)
            : this(xPosition, yPosition, LABEL_HEADER_DEFAULT, gameStateTracker)
        {
        }
        
        #endregion
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
            if (this.labelHeader != LABEL_HEADER_DEFAULT)
            {
                int i = 0;
            }
            base.update(deltaTimeMillis);
            //also update game length text
            int currentMillis = this.gameStateTracker.getGameLengthMillis();
            int hh = currentMillis / 3600000;
            currentMillis %= 3600000;
            int mm = currentMillis / 60000;
            currentMillis %= 60000;
            int ss = currentMillis / 1000;

            this.setText(this.labelHeader + hh + ":" + mm + ":" + ss);
        }
    }
}
