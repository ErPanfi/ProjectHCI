using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectHCI.KinectEngine;
using System.Windows.Media;

namespace ProjectHCI.ReverseFruitNinja
{
    public class DEBUG_LabelObject : FormattedTextGameObject
    {
        public DEBUG_LabelObject(double xPosition,
                                       double yPosition)
            : base(xPosition, yPosition, "Cooldown", -1)
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
            //also update cooldown text
            System.Diagnostics.Debug.Assert(typeof(GameSceneBrain).IsAssignableFrom(GameLoop.getSceneBrain().GetType()), "Expected a GameSceneBrain object");
            GameSceneBrain sceneBrain = (GameSceneBrain)GameLoop.getSceneBrain();

            this.setText("Rage level  = " + sceneBrain.getRageLevel() + " (" + sceneBrain.getRagePoints() + ")");
        }
    }
}
