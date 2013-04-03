using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectHCI.KinectEngine;
using System.Windows.Media;
using System.Windows;

namespace ProjectHCI.ReverseFruitNinja
{
    public class GameScoreLabelObject : GameLabelObject
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

        #region refactored method
        //protected override FormattedText formatText(string stringText)
        //{
        //    Typeface typeFace = new Typeface(new FontFamily(new Uri("pack://application:,,,/"), "./Resources/#Made in China"),
        //                                     FontStyles.Normal,
        //                                     FontWeights.Medium,
        //                                     FontStretches.Normal); 
        //    return new FormattedText(stringText,
        //                                    System.Globalization.CultureInfo.CurrentCulture,
        //                                    System.Windows.FlowDirection.LeftToRight,
        //                                    typeFace,          // the custom embedded font
        //                                    70,                // font size in em
        //                                    Brushes.Black);    //unused param, we create a geometry from this FormattedText
        //}
        #endregion

        public override void update(int deltaTimeMillis)
        {
            base.update(deltaTimeMillis);
            //also update game score text
            int currentScore = this.gameStateTracker.getGameScore();

            this.setText(this.labelHeader + currentScore);
        }
    }
}
