using ProjectHCI.KinectEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ProjectHCI.ReverseFruitNinja
{
    public class StainGameObject : FadeOutGameObject
    {
        public StainGameObject(double xPosition,
                                 double yPosition,
                                 Image image,
                                 int timeToLiveMillis,
                                 int fadeOutDurationMillis)
            : base(xPosition, yPosition, image, timeToLiveMillis, fadeOutDurationMillis)
        {

        }


        public override void onRendererDisplayDelegate()
        {
            Random random = new Random();
            GameLoop.getSceneManager().applyRotation(this, random.Next(0, 360), this._image.Width * 0.5, this._image.Height * 0.5, false, false);
            GameLoop.getSceneManager().canvasDisplayImage(this, 0);
        }



    }
}
