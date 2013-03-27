using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Controls;
using System.Diagnostics;
using Microsoft.Kinect;
using System.Windows;
using ProjectHCI.KinectEngine;

namespace ProjectHCI.ReverseFruitNinja
{
    class HeadUserGameObject : GameObject
    {

        public const int USER_DEAD_STOP_MILLIS = 3000;

        protected KinectSensorHelper kinectSensorHelper;
        private int Z_INDEX = 2;

        protected bool isCut;

        #region protected int timeToLiveMillis {public get; public set;}

        protected int timeToLiveMillis;

        public int getTimeToLiveMillis()
        {
            return this.timeToLiveMillis;
        }

        public void setTimeToLiveMillis(int timeToLiveMillis)
        {
            this.timeToLiveMillis = timeToLiveMillis;
        }

        #endregion

        public HeadUserGameObject(double xPosition,
                              double yPosition,
                              Geometry boundingBoxGeometry,
                              Image image,
                              SkeletonSmoothingFilter skeletonSmoothingFilter)
        {
            
            base._xPosition = xPosition;
            base._yPosition = yPosition;
            base._boundingBoxGeometry = boundingBoxGeometry;
            base._extraData = null;
            base._uid = Guid.NewGuid().ToString();
            base._gameObjectTypeEnum = GameObjectTypeEnum.UserObject;
            base._image = image;

            this.kinectSensorHelper = new KinectSensorHelper(skeletonSmoothingFilter);
            this.kinectSensorHelper.initializeKinect();
            this.timeToLiveMillis = 0;
            this.isCut = false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTimeMillis"></param>
        public override void update(int deltaTimeMillis)
        {
            if (isCut && timeToLiveMillis >= 0)
            {
                timeToLiveMillis -= deltaTimeMillis;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool isCollidable()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool isDead()
        {
            return (isCut && timeToLiveMillis < 0);
        }


        /// <summary>
        /// 
        /// </summary>
        public override void onRendererDisplayDelegate()
        {
            ISceneManager sceneManager = GameLoop.getSceneManager();
            sceneManager.canvasDisplayImage(this, Z_INDEX);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainWindowCanvas"></param>
        public override void onRendererUpdateDelegate()
        {
            ISceneManager sceneManager = GameLoop.getSceneManager();

            Skeleton skeleton = this.kinectSensorHelper.getTrackedSkeleton();

            if (skeleton != null)
            {
                Joint headJoint = skeleton.Joints[JointType.Head];
                Joint shoulderCenterJoint = skeleton.Joints[JointType.ShoulderCenter];

                if (headJoint.TrackingState == JointTrackingState.Tracked)
                {
                    double xScreenPosition = this.mapValueToNewRange(headJoint.Position.X, -1.0, 1.0, 0, sceneManager.getCanvasWidth());
                    double yScreenPosition = this.mapValueToNewRange(headJoint.Position.Y, 1.0, -1.0, 0, sceneManager.getCanvasHeight());

                    #region old commented code
                    //TransformGroup transformGroup = new TransformGroup();

                    ////translate component
                    //transformGroup.Children.Add(new TranslateTransform( this._xPosition - xScreenPosition, this._yPosition - yScreenPosition));


                    ////calculate rotation
                    //if (shoulderCenterJoint.TrackingState == JointTrackingState.Tracked)
                    //{

                    //    Vector shoulderCenterVector = new Vector(shoulderCenterJoint.Position.X, shoulderCenterJoint.Position.Y);
                    //    Vector headVector = new Vector(headJoint.Position.X, headJoint.Position.Y);

                    //    Vector shoulderCenterToHeadVector = Vector.Subtract(headVector, shoulderCenterVector);
                    //    shoulderCenterToHeadVector.Normalize();

                    //    Vector skeletonUpVector = new Vector(0, 1.0);

                    //    double rotationAngle = Vector.AngleBetween(skeletonUpVector, shoulderCenterToHeadVector);

                    //    double xRotationCenter = this._xPosition + this._image.Width * 0.5;
                    //    double yRotationCenter = this._yPosition + this._image.Height * 0.5;


                    //    transformGroup.Children.Add(new RotateTransform(-1 * rotationAngle, xRotationCenter, yRotationCenter));

                    //}
                    #endregion

                    sceneManager.applyTranslation(this, xScreenPosition - this.getXPosition(), yScreenPosition - this.getYPosition());
                }

            }           
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
            //Debug.WriteLine("user colpito");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherGameObject"></param>
        public override void onCollisionExitDelegate(IGameObject otherGameObject)
        {
            //throw new NotSupportedException();
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

        public void cutTrigger()
        {
            GameLoop.getSpawnerManager().specialRequestToSpawn(new GameObjectSpawnRequest(new GameFloatingLabelObject(this, "Dead!!!"), null));
        }
    }
}
