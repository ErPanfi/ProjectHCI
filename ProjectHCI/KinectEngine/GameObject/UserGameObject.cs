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
    class UserGameObject : GameObjectBase
    {


        private KinectSensorHelper kinectSensorHelper;


        public UserGameObject(Geometry geometry,
                              ImageSource imageSource,
                              SkeletonSmoothingFilter skeletonSmoothingFilter)
        {
            Debug.Assert(geometry != null, "expected geometry != null");
            Debug.Assert(imageSource != null, "expected imageSource != null");

            base._timeToLiveMillis = -1;
            base._currentTimeToLiveMillis = -1;
            base._geometry = geometry;
            base._imageSource = imageSource;
            base._uid = base.generateUid();
            base._objectType = GameObjectTypeEnum.UserObject;

            this.kinectSensorHelper = new KinectSensorHelper(skeletonSmoothingFilter);
            this.kinectSensorHelper.initializeKinect();

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
        /// <param name="mainWindowCanvas"></param>
        public override void onRendererUpdateDelegate(Canvas mainWindowCanvas, MainWindow currentMainWindow)
        {


            Skeleton skeleton = this.kinectSensorHelper.getTrackedSkeleton();

            if (skeleton != null)
            {

                UIElement userUiElement = currentMainWindow.getUiElementByUid(base._uid);

                Joint headJoint = skeleton.Joints[JointType.Head];
                Joint shoulderCenterJoint = skeleton.Joints[JointType.ShoulderCenter];

                if (headJoint.TrackingState == JointTrackingState.Tracked)
                {

                    double xScreenPosition = this.mapValueToNewRange(headJoint.Position.X, -1.0,  1.0, 0, mainWindowCanvas.RenderSize.Width);
                    double yScreenPosition = this.mapValueToNewRange(headJoint.Position.Y,  1.0, -1.0, 0, mainWindowCanvas.RenderSize.Height);

                    
                    TransformGroup transformGroup = new TransformGroup();
                    transformGroup.Children.Add(new TranslateTransform(xScreenPosition, yScreenPosition));


                    if (shoulderCenterJoint.TrackingState == JointTrackingState.Tracked)
                    {

                        Vector shoulderCenterVector = new Vector(shoulderCenterJoint.Position.X, shoulderCenterJoint.Position.Y);
                        Vector headVector = new Vector(headJoint.Position.X, headJoint.Position.Y);

                        Vector shoulderCenterToHeadVector = Vector.Subtract(headVector, shoulderCenterVector);
                        shoulderCenterToHeadVector.Normalize();

                        Vector upVector = new Vector(0, 1.0);

                        double rotationAngle = Vector.AngleBetween(upVector, shoulderCenterToHeadVector);

                        double xRotationCenter = base._geometry.Bounds.X + (base._geometry.Bounds.Width * 0.5);
                        double yRotationCenter = base._geometry.Bounds.Y + (base._geometry.Bounds.Height * 0.5);

                        transformGroup.Children.Add(new RotateTransform(-1 * rotationAngle, xRotationCenter, yRotationCenter));

                    }


                    base._geometry.Transform = transformGroup;
                    userUiElement.RenderTransform = transformGroup;

                    


#if DEBUG
                    //********************* translateBoundingBox 
                    UIElement boundingBoxUiElement = currentMainWindow.getUiElementByUid("BB_" + base._uid);
                    boundingBoxUiElement.RenderTransform = transformGroup;
                    //*********************
#endif
                    
                }
            }

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
