using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectHCI.KinectEngine;
using System.Windows.Media;

namespace ProjectHCI.ReverseFruitNinja
{
    public class GameScoreLabelObject : FormattedTextGameObject
    {
        protected const string LABEL_HEADER_DEFAULT = "Score  ";

        protected string labelHeader;
        protected IGameStateTracker gameStateTracker;

        #region ctors and dtors

        public GameScoreLabelObject(double xPosition, double yPosition, string headerText, IGameStateTracker gameStateTracker)
            : base(xPosition, yPosition, headerText, -1)
        {
            this.labelHeader = headerText;
            this.gameStateTracker = gameStateTracker;
        }

        public GameScoreLabelObject(double xPosition, double yPosition, IGameStateTracker gameStateTracker)
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
            base.update(deltaTimeMillis);
            if (this.labelHeader != LABEL_HEADER_DEFAULT)
            {
                int i = 0;
            }
            //also update game score text
            int currentScore = this.gameStateTracker.getGameScore();

            this.setText(this.labelHeader + currentScore);
        }
    }
}
