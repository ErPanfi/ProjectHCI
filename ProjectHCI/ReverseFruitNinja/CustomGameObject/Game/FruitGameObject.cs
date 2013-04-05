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
    public class FruitGameObject : FadeOutGameObject
    {

        public const int FRUIT_COLLECTION_POINTS = 10;
        public const int FRUIT_DEATH_POINTS = -3;

        private bool isCut;
        private bool isCollected;


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
            : base(xPosition, yPosition, image, timeToLiveMillis, (int)(timeToLiveMillis * 0.3))
        {

            Debug.Assert(timeToLiveMillis > 0, "expected timeToLiveMillis > 0");

            base._boundingBoxGeometry = boundingBoxGeometry;
            base._gameObjectTag = Tags.FRUIT_TAG;

            this.isCut = false;
            this.isCollected = false;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool isCollidable()
        {
            return base.currentTimeToLiveMillis >= 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool isDead()
        {
            return isCollected || isCut || base.currentTimeToLiveMillis < 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void onRendererDisplayDelegate()
        {
            GameLoop.getSceneManager().canvasDisplayImage(this, 1);

            { //fruit appear sound
                SoundGameObject soundGameObject = new SoundGameObject(new Uri(@"Resources\Sounds\fruit_appear.wav", UriKind.Relative), 1, false);
                GameLoop.getSpawnerManager().specialRequestToSpawn(new GameObjectSpawnRequest(soundGameObject, null));
            }
        }

        public override void onRendererRemoveDelegate()
        {
            base.onRendererRemoveDelegate();

            if (!this.isCollected && !this.isCut)
            {
                //vanish sound
                SoundGameObject soundGameObject = new SoundGameObject(new Uri(@"Resources\Sounds\fruit_vanish.wav", UriKind.Relative), 0.2, false);
                GameLoop.getSpawnerManager().specialRequestToSpawn(new GameObjectSpawnRequest(soundGameObject, null));
            }
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
                    Random random = new Random();

                    //create floating death label
                    GameLoop.getSpawnerManager().specialRequestToSpawn(new GameObjectSpawnRequest(new GameFloatingLabelObject(this, GameFloatingLabelObject.points2string(FRUIT_DEATH_POINTS)), null));

                    Image stainImage = new Image();
                    stainImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath("stain" + random.Next(1, 21) + ".png")));
                    stainImage.Height = 260;
                    stainImage.Width = 390;

                    StainGameObject stainFadeOutGameObject = new StainGameObject(this._xPosition - stainImage.Width * 0.5, this._yPosition - stainImage.Height * 0.5, stainImage, 4500, 2000);
                    GameLoop.getSpawnerManager().specialRequestToSpawn(new GameObjectSpawnRequest(stainFadeOutGameObject, null));
                    
                    break;



                case Tags.USER_TAG:
                    this.isCollected = true;
                    //create floating death label
                    GameLoop.getSpawnerManager().specialRequestToSpawn(new GameObjectSpawnRequest(new GameFloatingLabelObject(this, GameFloatingLabelObject.points2string(FRUIT_COLLECTION_POINTS)), null));
                    
                    {// fruit collected sound
                        SoundGameObject soundGameObject = new SoundGameObject(new Uri(@"Resources\Sounds\fruit_collected.wav", UriKind.Relative), false);
                        GameLoop.getSpawnerManager().specialRequestToSpawn(new GameObjectSpawnRequest(soundGameObject, null));
                    }

                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherGameObject"></param>
        public override void onCollisionExitDelegate(IGameObject otherGameObject)
        {
            //do nothing
        }

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
