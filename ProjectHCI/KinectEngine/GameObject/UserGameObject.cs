using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Controls;
using System.Diagnostics;
using Microsoft.Kinect;
using System.Windows;

namespace ProjectHCI.KinectEngine
{
    class UserGameObject : GameObject
    {


        private KinectSensorHelper kinectSensorHelper;
        private int Z_INDEX = 2;

        private bool inverter = true;
        private int coolTime = 0;

        public UserGameObject(double xPosition,
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

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTimeMillis"></param>
        public override void update(int deltaTimeMillis)
        {
            //do nothing
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
            return false;
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



            //coolTime += Time.getDeltaTimeMillis();

            //if (coolTime > 10000)
            //{
            //    if (inverter)
            //    {
            //        sceneManager.applyTranslation(this, 200, 200);
            //    }
            //    else
            //    {
            //        sceneManager.applyTranslation(this, -200, -200);
            //    }

            //    inverter = !inverter;
            //    coolTime = 0;
            //}

            //return;




            Skeleton skeleton = this.kinectSensorHelper.getTrackedSkeleton();

            if (skeleton != null)
            {

                Joint headJoint = skeleton.Joints[JointType.Head];
                Joint shoulderCenterJoint = skeleton.Joints[JointType.ShoulderCenter];


                if (headJoint.TrackingState == JointTrackingState.Tracked)
                {

                    double xScreenPosition = this.mapValueToNewRange(headJoint.Position.X, -1.0, 1.0, 0, sceneManager.getCanvasWidth());
                    double yScreenPosition = this.mapValueToNewRange(headJoint.Position.Y, 1.0, -1.0, 0, sceneManager.getCanvasHeight());


                    


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
            Debug.WriteLine("user colpito");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherGameObject"></param>
        public override void onCollisionExitDelegate(IGameObject otherGameObject)
        {
            //throw new NotSupportedException();
        }



        private double mapValueToNewRange(double value,
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
}
