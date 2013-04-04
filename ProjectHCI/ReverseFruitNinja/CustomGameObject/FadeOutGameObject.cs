using ProjectHCI.KinectEngine;
using ProjectHCI.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ProjectHCI.ReverseFruitNinja
{
    public class FadeOutGameObject : GameObject
    {

        private const int Z_INDEX = 0;


        protected int timeToLiveMillis;
        protected int currentTimeToLiveMillis;
        protected int fadeOutDurationMillis;

        private byte prevAlphaChannelInterpolated;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xPosition"></param>
        /// <param name="yPosition"></param>
        /// <param name="image"></param>
        /// <param name="timeToLiveMillis"></param>
        /// <param name="fadeOutDurationMillis"></param>
        public FadeOutGameObject(double xPosition,
                                 double yPosition,
                                 Image image,
                                 int timeToLiveMillis,
                                 int fadeOutDurationMillis)
        {
            Debug.Assert(fadeOutDurationMillis <= timeToLiveMillis, "expected fadeOutDurationMillis <= timeToLiveMillis");

            base._xPosition = xPosition;
            base._yPosition = yPosition;
            base._image = image;
            base._gameObjectTag = Tags.UI_TAG;

            this.timeToLiveMillis = timeToLiveMillis;
            this.currentTimeToLiveMillis = timeToLiveMillis;
            this.fadeOutDurationMillis = fadeOutDurationMillis;
            this.prevAlphaChannelInterpolated = 0;
            
        }




        public override void update(int deltaTimeMillis)
        {
            this.currentTimeToLiveMillis -= deltaTimeMillis;
        }

        public override bool isCollidable()
        {
            return false;
        }

        public override bool isDead()
        {
            return this.currentTimeToLiveMillis <= 0;
        }
        



        public override void onRendererDisplayDelegate()
        {
            GameLoop.getSceneManager().canvasDisplayImage(this, Z_INDEX);
        }

        public override void onRendererUpdateDelegate()
        {
            
            const int alphaChannelIndex = 3;

            if (this.currentTimeToLiveMillis <= this.fadeOutDurationMillis)
            {

                RgbData rgbData = BitmapUtility.getRgbData((BitmapSource)this.getImage().Source);
                byte alphaValueInterpolated = (byte)StandardUtility.mapValueToNewRange(this.fadeOutDurationMillis - this.currentTimeToLiveMillis, 0, this.fadeOutDurationMillis, 0, 255);

                for (int i = 0; i < rgbData.dataLength; i += 4)
                {
                    if (rgbData.rawRgbByteArray[i + alphaChannelIndex] > 0)
                    {

                        byte deltaAlphaChannel = (byte)(alphaValueInterpolated - this.prevAlphaChannelInterpolated);
                        byte currentAlphaValue = rgbData.rawRgbByteArray[i + alphaChannelIndex];

                        rgbData.rawRgbByteArray[i + alphaChannelIndex] = (currentAlphaValue - deltaAlphaChannel) <= 0 ? (byte)0 : (byte)(currentAlphaValue - deltaAlphaChannel);

                    }

                }

                this.prevAlphaChannelInterpolated = alphaValueInterpolated;

                this._image.Source = BitmapUtility.createBitmapSource(rgbData);
                GameLoop.getSceneManager().canvasUpdateImage(this);
            }
            

            
            
        }

        public override void onRendererRemoveDelegate()
        {
            GameLoop.getSceneManager().canvasRemoveImage(this);
        }





        public override void onCollisionEnterDelegate(IGameObject otherGameObject)
        {
            //do nothing
        }

        public override void onCollisionExitDelegate(IGameObject otherGameObject)
        {
            //do nothing
        }
    }
}
