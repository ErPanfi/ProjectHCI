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
    public class FruitGameObject : GameObject
    {

        public const int FRUIT_COLLECTION_POINTS = 10;
        public const int FRUIT_DEATH_POINTS = -3;

        #region protected int timeToLiveMillis {public get; public set;}
        
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

        protected bool isCut;
        protected bool isCollected;
        protected bool displayed;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xPosition"></param>
        /// <param name="yPosition"></param>
        /// <param name="boundingBoxGeometry"></param>
        /// <param name="image"></param>
        /// <param name="timeToLiveMillis"></param>
        public FruitGameObject(double xPosition,
                                      double yPosition,
                                      Geometry boundingBoxGeometry,
                                      Image image,
                                      int timeToLiveMillis)
        {

            Debug.Assert(timeToLiveMillis > 0, "expected timeToLiveMillis > 0");

            base._xPosition = xPosition;
            base._yPosition = yPosition;
            base._boundingBoxGeometry = boundingBoxGeometry;
            base._extraData = null;
            base._uid = Guid.NewGuid().ToString();
            base._gameObjectTag = Tags.FRUIT_TAG;
            base._image = image;

            this.timeToLiveMillis = timeToLiveMillis;
            this.isCut = false;
            this.isCollected = false;
            this.displayed = false;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTimeMillis"></param>
        public override void update(int deltaTimeMillis)
        {
            this.timeToLiveMillis -= deltaTimeMillis;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool isCollidable()
        {
            return this.timeToLiveMillis >= 0 && !isCollected && !isCut;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool isDead()
        {
            return isCollected || isCut || this.timeToLiveMillis < 0;
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

            GameLoop.getSceneManager().canvasDisplayImage(this, 0);
            displayed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void onRendererUpdateDelegate()
        {
            GameLoop.getSceneManager().canvasUpdateImage(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void onRendererRemoveDelegate()
        {
            GameLoop.getSceneManager().canvasRemoveImage(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherGameObject"></param>
        public override void onCollisionEnterDelegate(IGameObject otherGameObject)
        {
            switch (otherGameObject.getGameObjectTag())
            {
                case Tags.CUT_TAG:
                    this.isCut = true;
                    //create floating death label
                    GameLoop.getSpawnerManager().specialRequestToSpawn(new GameObjectSpawnRequest(new GameFloatingLabelObject(this, GameFloatingLabelObject.points2string(FRUIT_DEATH_POINTS)), null));
                    break;
                case Tags.USER_TAG:
                    this.isCollected = true;
                    //create floating death label
                    GameLoop.getSpawnerManager().specialRequestToSpawn(new GameObjectSpawnRequest(new GameFloatingLabelObject(this, GameFloatingLabelObject.points2string(FRUIT_COLLECTION_POINTS)), null));
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherGameObject"></param>
        public override void onCollisionExitDelegate(IGameObject otherGameObject)
        {
            //throw new NotSupportedException();
        }

        #region useless method
        
        //protected void fadeImage()
        //{
        //    //Make current image transparent
        //    RgbData rgbData = BitmapUtility.getRgbData((BitmapSource)this._image.Source);

        //    for (int i = 0; i < rgbData.dataLength; i += 4)
        //    {
        //        /*
        //        rgbData.rawRgbByteArray[i + 0] = 0;	    //blue
        //        rgbData.rawRgbByteArray[i + 1] = 0;     //green
        //        rgbData.rawRgbByteArray[i + 2] = 1;     //red
        //         */
        //        rgbData.rawRgbByteArray[i + 3] = 0;     //alpha

        //    }

        //    BitmapSource bitmapSource = BitmapUtility.createBitmapSource(rgbData);
        //    this._image.Source = bitmapSource;
        //    if (displayed)
        //    {
        //        GameLoop.getSceneManager().canvasUpdateImage(this);
        //    }
        //}
        #endregion

        public int getFruitDeathPoints()
        {
            return FRUIT_DEATH_POINTS;
        }

        public int getFruitCollectionPoints()
        {
            return FRUIT_COLLECTION_POINTS;
        }
    }
}
