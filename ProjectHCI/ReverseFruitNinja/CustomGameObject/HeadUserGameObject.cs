﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Controls;
using System.Diagnostics;
using Microsoft.Kinect;
using System.Windows;
using ProjectHCI.KinectEngine;
using System.Threading;
using ProjectHCI.Utility;


namespace ProjectHCI.ReverseFruitNinja
{
    class HeadUserGameObject : GameObject
    {

        public const int USER_DEAD_STOP_MILLIS = 3000;

        protected KinectSensorHelper kinectSensorHelper;
        protected int Z_INDEX = 2;

        protected Image notAlreadyTrackedImage;
        protected Image cursorImage;
        protected bool firstTimeTracked;


        public bool hasBeenTracked()
        {
            return this.firstTimeTracked;
        }

        

        public HeadUserGameObject(double xPosition,
                              double yPosition,
                              Geometry boundingBoxGeometry,
                              Image notAlreadyTrackedImage,
                              Image image,
                              SkeletonSmoothingFilter skeletonSmoothingFilter)
        {
            this.firstTimeTracked = false;
            this.notAlreadyTrackedImage = notAlreadyTrackedImage;
            this.cursorImage = image;
            this._image = notAlreadyTrackedImage;

            base._xPosition = xPosition;
            base._yPosition = yPosition;
            base._boundingBoxGeometry = boundingBoxGeometry;
            base._extraData = null;
            base._uid = Guid.NewGuid().ToString();
            base._gameObjectTag = Tags.USER_TAG;

            this.kinectSensorHelper = new KinectSensorHelper(skeletonSmoothingFilter);
            this.kinectSensorHelper.initializeKinect();
            //this.calibrateCamera();


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
            return this.firstTimeTracked;
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
            sceneManager.clearImageTransformation(this);

            Skeleton skeleton = this.kinectSensorHelper.getTrackedSkeleton();

            if (skeleton != null)
            {
                Joint headJoint = skeleton.Joints[JointType.Head];
                Joint shoulderCenterJoint = skeleton.Joints[JointType.ShoulderCenter];

                if (headJoint.TrackingState == JointTrackingState.Tracked)
                {
                    this.firstTimeTracked = true;
                    this._image = cursorImage;

                    double xScreenPosition = this.mapValueToNewRange(headJoint.Position.X, -1.0, 1.0, 0, sceneManager.getCanvasWidth());
                    double yScreenPosition = this.mapValueToNewRange(headJoint.Position.Y, 1.0, -1.0, 0, sceneManager.getCanvasHeight());


                    sceneManager.applyTranslation(this, xScreenPosition - this.getXPosition(), yScreenPosition - this.getYPosition(), true);


                    //calculate rotation
                    if (shoulderCenterJoint.TrackingState == JointTrackingState.Tracked || shoulderCenterJoint.TrackingState == JointTrackingState.Inferred)
                    {

                        Vector shoulderCenterVector = new Vector(shoulderCenterJoint.Position.X, shoulderCenterJoint.Position.Y);
                        Vector headVector = new Vector(headJoint.Position.X, headJoint.Position.Y);

                        Vector shoulderCenterToHeadVector = Vector.Subtract(headVector, shoulderCenterVector);
                        shoulderCenterToHeadVector.Normalize();

                        //we are in mirror mode so the object need to rotate in the opposite direction.
                        double rotationAngle = -1 * Vector.AngleBetween(new Vector(0, 1), shoulderCenterToHeadVector);

                        double xRotationCenter = this._image.Width * 0.5;
                        double yRotationCenter = this._image.Height * 0.5;


                        sceneManager.applyRotation(this, rotationAngle, xRotationCenter, yRotationCenter, false, false);

                    }

                    
                }

            }

            sceneManager.canvasUpdateImage(this);
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

        ///// <summary>
        ///// prototype for camera calibration. HEEELP!!
        ///// </summary>
        //public void calibrateCamera()
        //{
        //    Skeleton skeleton;
        //    int deltaAngle = 3;

        //    int elevationAngle0 = 0;
        //    float headJointPositionY0;
        //    double screenPositionY0 = 0.0;

        //    int elevationAngle1 = 0;
        //    float headJointPositionY1;
        //    double screenPositionY1 = 0.0;


        //    KinectSensor sensor = kinectSensorHelper.getKinectSensor();
        //    double canvasHeight = GameLoop.getSceneManager().getCanvasHeight();

        //    /*
        //     * a partire dalla posizione iniziale in mezzo allo schermo
        //     * ottengo l'angolo iniziale del kinect
        //     */
        //    do
        //    {
        //        skeleton = this.kinectSensorHelper.getTrackedSkeleton();
        //    } while (skeleton == null);

        //    if (skeleton != null)
        //    {
        //        Joint headJoint = skeleton.Joints[JointType.Head];
        //        elevationAngle0 = sensor.ElevationAngle;
        //        headJointPositionY0 = headJoint.Position.Y;
        //        screenPositionY0 = (headJointPositionY0 + 1) * 0.5 * canvasHeight;
        //    }

        //    /*sposto il kinect*/
        //    if (elevationAngle0 >= 0)
        //    {
        //        sensor.ElevationAngle = elevationAngle0 - deltaAngle;
        //    }
        //    else
        //    {
        //        sensor.ElevationAngle = elevationAngle0 + deltaAngle;
        //    }
        //    /*cerco la nuova posizione in base al nuovo angolo*/
        //    do
        //    {
        //        skeleton = this.kinectSensorHelper.getTrackedSkeleton();
        //    } while (skeleton == null);
        //    if (skeleton != null)
        //    {
        //        Joint headJoint = skeleton.Joints[JointType.Head];
        //        elevationAngle1 = sensor.ElevationAngle;
        //        headJointPositionY1 = headJoint.Position.Y;
        //        screenPositionY1 = (headJointPositionY1 + 1) * 0.5 * canvasHeight;
        //    }
        //    Thread.Sleep(1000);
        //    sensor.ElevationAngle = elevationAngle0 + (int)((0.3 * canvasHeight - screenPositionY0) * (elevationAngle1 - elevationAngle0) / (screenPositionY1 - screenPositionY0));
        //}
    }

}

