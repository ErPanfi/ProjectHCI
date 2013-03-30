using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows;
using ProjectHCI.KinectEngine;
using ProjectHCI.Utility;
using System.Windows.Media.Imaging;

namespace ProjectHCI.ReverseFruitNinja
{
    public class CutGameObject : GameObject
    {

        private static int nextZIndex = 5;

        private int collidableTimeMillis;
        private int currentTimeToLiveMillis;
        private ImageSource originalImageSource;

        #region protected timeToLiveMillis {public get; public set;}

        protected int timeToLiveMillis;

        public int getTimeToLiveMillis()
        {
            return timeToLiveMillis;
        }

        public void setTimeToLiveMillis(int timeToLiveMillis)
        {
            this.timeToLiveMillis = timeToLiveMillis;
        }

        #endregion
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="imageSource"></param>
        /// <param name="timeToLiveMillis"></param>
        /// <param name="chopDurationMillis"></param>
        public CutGameObject(double xPosition,
                                         double yPosition,
                                         Geometry boundingBoxGeometry,
                                         Image image,
                                         int timeToLiveMillis,
                                         int notCollidableTimeMillis)
        {
            Debug.Assert(timeToLiveMillis > 0, "expected timeToLiveMillis > 0");
            Debug.Assert(notCollidableTimeMillis > 0, "expected notCollidableTimeMillis > 0");
            Debug.Assert(notCollidableTimeMillis <= timeToLiveMillis, "expected notCollidableTimeMillis <= timeToLiveMillis");

           

            base._xPosition = xPosition;
            base._yPosition = yPosition;
            base._boundingBoxGeometry = boundingBoxGeometry;
            base._extraData = null;
            base._uid = Guid.NewGuid().ToString();
            base._gameObjectTag = Tags.CUT_TAG;
            base._image = image;

            this.originalImageSource = image.Source.Clone();

            this.timeToLiveMillis = timeToLiveMillis;
            this.currentTimeToLiveMillis = timeToLiveMillis;
            this.collidableTimeMillis = this.timeToLiveMillis - notCollidableTimeMillis;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTimeMillis"></param>
        public override void update(int deltaTimeMillis)
        {
            this.currentTimeToLiveMillis -= deltaTimeMillis;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool isCollidable()
        {
            return this.currentTimeToLiveMillis <= collidableTimeMillis;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool isDead()
        {
            return this.currentTimeToLiveMillis <= 0;
        }


        /// <summary>
        /// 
        /// </summary>
        public override void onRendererDisplayDelegate()
        {

            if (this.getImage() == null)
            {
                return;
            }


            ISceneManager sceneManager = GameLoop.getSceneManager();
            sceneManager.applyRotation(this, new Random().Next(0, 360), this.getImage().Width * 0.5, this.getImage().Height * 0.5, true, true);

            this.getImage().Source = this.animateImageColor();

            sceneManager.canvasDisplayImage(this, nextZIndex++);



        }


        /// <summary>
        /// 
        /// </summary>
        public override void onRendererUpdateDelegate()
        {

            this.getImage().Source = this.animateImageColor();
            GameLoop.getSceneManager().canvasUpdateImage(this);
        }


        /// <summary>
        /// 
        /// </summary>
        public override void onRendererRemoveDelegate()
        {

            ISceneManager sceneManager = GameLoop.getSceneManager();
            sceneManager.canvasRemoveImage(this);

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherGameObject"></param>
        public override void onCollisionEnterDelegate(IGameObject otherGameObject)
        {
            //throw new NotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherGameObject"></param>
        public override void onCollisionExitDelegate(IGameObject otherGameObject)
        {
            //throw new NotSupportedException();
        }
        }







        private ImageSource animateImageColor()
        {
            if (this.currentTimeToLiveMillis > collidableTimeMillis)
            {


                const int BlueChannelIndex = 0;
                const int GreenChannelIndex = 1;
                const int RedChannelIndex = 2;

                RgbData rgbData = BitmapUtility.getRgbData((BitmapSource)this.getImage().Source);

                for (int i = 0; i < rgbData.dataLength; i += 4)
                {
                    //blue decrease cause currentTimeToLiveMillis is decreased
                    //rgbData.rawRgbByteArray[i + BlueChannelIndex] = (byte)this.mapValueToNewRange(this.currentTimeToLiveMillis - this.collidableTimeMillis,
                    //                                                                             0,
                    //                                                                             this.timeToLiveMillis - this.collidableTimeMillis,
                    //                                                                             0,
                    //                                                                             255);

                    rgbData.rawRgbByteArray[i + BlueChannelIndex] = 0;
                    rgbData.rawRgbByteArray[i + GreenChannelIndex] = 0;

                    //red increase cause this.timeToLiveMillis - this.currentTimeToLiveMillis
                    rgbData.rawRgbByteArray[i + RedChannelIndex] = (byte)this.mapValueToNewRange(this.timeToLiveMillis - this.currentTimeToLiveMillis,
                                                                                                 0,
                                                                                                 this.timeToLiveMillis - this.collidableTimeMillis,
                                                                                                 0,
                                                                                                 255);
                }
                return BitmapUtility.createBitmapSource(rgbData);

            }
            else
            {
                return originalImageSource;
            }
        }







        protected double mapValueToNewRange(double value,
                                            double oldLowerLimit,
                                            double oldHigherLimit,
                                            double newLowerLimit,
                                            double newHigherLimit)
        {

            double oldRange = oldHigherLimit - oldLowerLimit;
            double newRange = newHigherLimit - newLowerLimit;

            return (((value - oldLowerLimit) * newRange) / oldRange) + newLowerLimit;
    }
}
