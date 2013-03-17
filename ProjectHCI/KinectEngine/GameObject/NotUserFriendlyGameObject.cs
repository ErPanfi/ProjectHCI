using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows;

namespace ProjectHCI.KinectEngine
{
    public class NotUserFriendlyGameObject : GameObject
    {

        private int collidableTimeMillis;
        private int timeToLiveMillis;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="imageSource"></param>
        /// <param name="timeToLiveMillis"></param>
        /// <param name="chopDurationMillis"></param>
        public NotUserFriendlyGameObject(double xPosition,
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
            base._gameObjectTypeEnum = GameObjectTypeEnum.UnfriendlyObject;
            base._image = image;

            this.timeToLiveMillis = timeToLiveMillis;
            this.collidableTimeMillis = this.timeToLiveMillis - notCollidableTimeMillis;
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
            return this.timeToLiveMillis <= collidableTimeMillis;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool isDead()
        {
            return this.timeToLiveMillis <= 0;
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
            sceneManager.canvasDisplayImage(this, 0);


        }


        /// <summary>
        /// 
        /// </summary>
        public override void onRendererUpdateDelegate()
        {
            //do nothing... maybe change color
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
            throw new NotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherGameObject"></param>
        public override void onCollisionExitDelegate(IGameObject otherGameObject)
        {
            throw new NotSupportedException();
        }
    }
}
