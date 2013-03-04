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

                UIElement userUiElement = currentMainWindow.getUiElementByUid(_uid);

                Joint headJoint = skeleton.Joints[JointType.Head];
                if (headJoint.TrackingState == JointTrackingState.Tracked)
                {

                    double xScreenPosition = this.mapValueToNewRange(headJoint.Position.X, -1.0,  1.0, 0, mainWindowCanvas.RenderSize.Width);
                    double yScreenPosition = this.mapValueToNewRange(headJoint.Position.Y,  1.0, -1.0, 0, mainWindowCanvas.RenderSize.Height);

                    TranslateTransform translateTransform = new TranslateTransform(xScreenPosition, yScreenPosition);
                    userUiElement.RenderTransform = translateTransform;

#if DEBUG
                    //********************* translateBoundingBox 
                    UIElement boundingBoxUiElement = currentMainWindow.getUiElementByUid("BB_" + _uid);
                    boundingBoxUiElement.RenderTransform = translateTransform;
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
