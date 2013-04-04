using Microsoft.Kinect;
using ProjectHCI.KinectEngine;
using ProjectHCI.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjectHCI.ReverseFruitNinja
{
    class HandUserGameObject : HeadUserGameObject
    {

        private const double PIXEL_PER_CENTIMETER = 5;

        public HandUserGameObject(double xPosition,
                                  double yPosition,
                                  Geometry boundingBoxGeometry,
                                  Image notAlreadyTrackedImage,
                                  Image image,
                                  SkeletonSmoothingFilter skeletonSmoothingFilter)
            : base (xPosition, yPosition, boundingBoxGeometry, notAlreadyTrackedImage, image, skeletonSmoothingFilter)
        {
        }

        

        public override void onRendererUpdateDelegate()
        {
            ISceneManager sceneManager = GameLoop.getSceneManager();

            
            Skeleton skeleton = base.kinectSensorHelper.getTrackedSkeleton();

            if (skeleton != null)
            {

                Joint handJoint = skeleton.Joints[JointType.HandRight];

                if (handJoint.TrackingState == JointTrackingState.Tracked)
                {
                    this._image = cursorImage;
                    this.firstTimeTracked = true;

                    double xHandJointPosition = Math.Abs(handJoint.Position.X) > 0.4 ? 0.4 * Math.Sign(handJoint.Position.X) : handJoint.Position.X;
                    double yHandJointPosition = Math.Abs(handJoint.Position.Y) > 0.4 ? 0.4 * Math.Sign(handJoint.Position.Y) : handJoint.Position.Y;


                    double xScreenPosition = StandardUtility.mapValueToNewRange(xHandJointPosition, -0.4, 0.4, 0, sceneManager.getCanvasWidth() - this.getImage().Width);
                    double yScreenPosition = StandardUtility.mapValueToNewRange(yHandJointPosition, 0.4, -0.4, 0, sceneManager.getCanvasHeight() - this.getImage().Height);

           
                    sceneManager.applyTranslation(this, xScreenPosition - this.getXPosition(), yScreenPosition - this.getYPosition(), true);

                }

                sceneManager.canvasUpdateImage(this);
            }

            
        }

        

        public override void onCollisionEnterDelegate(IGameObject otherGameObject)
        {
        }

        public override void onCollisionExitDelegate(IGameObject otherGameObject)
        {
        }

    }
}
