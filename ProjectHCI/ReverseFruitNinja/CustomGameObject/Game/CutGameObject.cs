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

        private static int nextZIndex = 3;

        private int collidableTimeMillis;
        private int currentTimeToLiveMillis;
        private ImageSource originalImageSource;

        private bool cutSuccess;

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
            base._gameObjectTag = Tags.CUT_TAG;
            base._image = image;

            this.originalImageSource = image.Source.Clone();

            this.timeToLiveMillis = timeToLiveMillis;
            this.currentTimeToLiveMillis = timeToLiveMillis;
            this.collidableTimeMillis = this.timeToLiveMillis - notCollidableTimeMillis;
            this.cutSuccess = false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTimeMillis"></param>
        public override void update(int deltaTimeMillis)
        {
            //if object is already dead don't decrease its time to live
            if (!this.isDead())
            {
                this.currentTimeToLiveMillis -= deltaTimeMillis;

                //if it's just dead increase rage points
                if (this.isDead())
                {
                    Debug.Assert(typeof(GameSceneBrain).IsAssignableFrom(GameLoop.getSceneBrain().GetType()), "Expected GameSceneBrain object");
                    ((GameSceneBrain)GameLoop.getSceneBrain()).incRage();
                }
            }
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


            CutGameObject.nextZIndex = CutGameObject.nextZIndex + 1 >= 100 ? 3 : CutGameObject.nextZIndex + 1;
            sceneManager.canvasDisplayImage(this, nextZIndex);



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

            if (!this.cutSuccess)
            {
                SoundGameObject soundGameObject = new SoundGameObject(new Uri(@"D:\VisualStudio\GitRepositories\ProjectHCI\ProjectHCI\Resources\Sounds\air_cut.wav"), false);
                GameLoop.getSpawnerManager().specialRequestToSpawn(new GameObjectSpawnRequest(soundGameObject, null));
            }

            ISceneManager sceneManager = GameLoop.getSceneManager();
            sceneManager.canvasRemoveImage(this);

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherGameObject"></param>
        public override void onCollisionEnterDelegate(IGameObject otherGameObject)
        {
            if (otherGameObject.getGameObjectTag() == Tags.FRUIT_TAG)
            {
                this.cutSuccess = true;

                //cut success sound
                SoundGameObject soundGameObject = new SoundGameObject(new Uri(@"D:\VisualStudio\GitRepositories\ProjectHCI\ProjectHCI\Resources\Sounds\fruit_cutted.wav"), false);
                GameLoop.getSpawnerManager().specialRequestToSpawn(new GameObjectSpawnRequest(soundGameObject, null));
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






        private ImageSource animateImageColor()
        {
            if (this.currentTimeToLiveMillis > collidableTimeMillis)
            {


                const int blueChannelIndex = 0;
                const int greenChannelIndex = 1;
                const int redChannelIndex = 2;

                RgbData rgbData = BitmapUtility.getRgbData((BitmapSource)this.getImage().Source);


                byte redChannelInterpolated = (byte)StandardUtility.mapValueToNewRange(this.timeToLiveMillis - this.currentTimeToLiveMillis, 0, this.timeToLiveMillis - this.collidableTimeMillis, 0, 255);

                for (int i = 0; i < rgbData.dataLength; i += 4)
                {


                    rgbData.rawRgbByteArray[i + blueChannelIndex] = 0;
                    rgbData.rawRgbByteArray[i + greenChannelIndex] = 0;

                    //red increase cause this.timeToLiveMillis - this.currentTimeToLiveMillis
                    rgbData.rawRgbByteArray[i + redChannelIndex] = redChannelInterpolated;
                }



                return BitmapUtility.createBitmapSource(rgbData);

            }
            else
            {
                return originalImageSource;
            }
        }

    }
}
