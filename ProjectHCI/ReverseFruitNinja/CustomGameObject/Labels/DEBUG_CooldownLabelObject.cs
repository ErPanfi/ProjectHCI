using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectHCI.KinectEngine;
using System.Windows.Media;

namespace ProjectHCI.ReverseFruitNinja
{
    public class DEBUG_CooldownLabelObject : FormattedTextGameObject
    {
        public DEBUG_CooldownLabelObject(double xPosition,
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
            System.Diagnostics.Debug.Assert(typeof(GameSpawnerManager).IsAssignableFrom(GameLoop.getSpawnerManager().GetType()), "Expected a GameSpawnerManager object");
            GameSpawnerManager spawnerManager = (GameSpawnerManager)GameLoop.getSpawnerManager();

            this.setText("Cooldown : [" + spawnerManager.minChopSpawnCooldownMillis + "," + spawnerManager.maxChopSpawnCooldownMillis + "]");
        }
    }
}
