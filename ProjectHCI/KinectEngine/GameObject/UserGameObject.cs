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
            //this.kinectSensorHelper.initializeKinect();

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
            sceneManager.canvasDisplayImage(this, 10);

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainWindowCanvas"></param>
        public override void onRendererUpdateDelegate()
        {


            ISceneManager sceneManager = GameLoop.getSceneManager();


            Skeleton skeleton = this.kinectSensorHelper.getTrackedSkeleton();

//             if (skeleton != null)
//             {
// 
//                 foreach (UIElement uiElement0 in sceneManager.getUiElementListBoundToGameObject(this))
//                 {
// 
//                     //UIElement userUiElement = sceneManager.getUiElementByUid(base._uid);
// 
// 
//                     Joint headJoint = skeleton.Joints[JointType.Head];
//                     Joint shoulderCenterJoint = skeleton.Joints[JointType.ShoulderCenter];
// 
// 
//                     if (headJoint.TrackingState == JointTrackingState.Tracked)
//                     {
// 
//                         double canvasWidth = sceneManager.getTargetCanvas().RenderSize.Width;
//                         double canvasHeight = sceneManager.getTargetCanvas().RenderSize.Height;
// 
//                         double xScreenPosition = this.mapValueToNewRange(headJoint.Position.X, -1.0, 1.0, 0, canvasWidth);
//                         double yScreenPosition = this.mapValueToNewRange(headJoint.Position.Y, 1.0, -1.0, 0, canvasHeight);
// 
// 
//                         TransformGroup transformGroup = new TransformGroup();
//                         //translate component
//                         transformGroup.Children.Add(new TranslateTransform(xScreenPosition, yScreenPosition));
// 
// 
//                         if (shoulderCenterJoint.TrackingState == JointTrackingState.Tracked)
//                         {
// 
//                             Vector shoulderCenterVector = new Vector(shoulderCenterJoint.Position.X, shoulderCenterJoint.Position.Y);
//                             Vector headVector = new Vector(headJoint.Position.X, headJoint.Position.Y);
// 
//                             Vector shoulderCenterToHeadVector = Vector.Subtract(headVector, shoulderCenterVector);
//                             shoulderCenterToHeadVector.Normalize();
// 
//                             Vector upVector = new Vector(0, 1.0);
// 
//                             double rotationAngle = Vector.AngleBetween(upVector, shoulderCenterToHeadVector);
// 
//                             double xRotationCenter = base._boundingBoxGeometry.Bounds.X + (base._boundingBoxGeometry.Bounds.Width * 0.5);
//                             double yRotationCenter = base._boundingBoxGeometry.Bounds.Y + (base._boundingBoxGeometry.Bounds.Height * 0.5);
// 
//                             //rotation component
//                             transformGroup.Children.Add(new RotateTransform(-1 * rotationAngle, xRotationCenter, yRotationCenter));
// 
//                         }
// 
// 
//                         transformGroup.Freeze();
// 
// 
//                         base._boundingBoxGeometry.Transform = transformGroup;
// 
// 
//                         Application.Current.Dispatcher.Invoke(new Action(
//                             delegate()
//                             {
//                                 uiElement0.RenderTransform = transformGroup;
//                                 //#if DEBUG
//                                 //                                //********************* translateBoundingBox 
//                                 //                                UIElement boundingBoxUiElement = sceneManager.getUiElementByUid("BB_" + base._uid);
//                                 //                                boundingBoxUiElement.RenderTransform = transformGroup;
//                                 //                                //*********************
//                                 //#endif
// 
//                             }
//                         ));
// 
// 
// 
// 
// 
//                     }
//                 }
//             }

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
