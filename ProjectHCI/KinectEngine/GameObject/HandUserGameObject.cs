using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectHCI.KinectEngine
{
    class HandUserGameObject : HeadUserGameObject
    {

        private const double PIXEL_PER_CENTIMETER = 5;

        
        public HandUserGameObject(double xPosition,
                                  double yPosition,
                                  Geometry boundingBoxGeometry,
                                  Image image,
                                  SkeletonSmoothingFilter skeletonSmoothingFilter)
            : base (xPosition, yPosition, boundingBoxGeometry, image, skeletonSmoothingFilter)
        {
        }


        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainWindowCanvas"></param>
        public override void onRendererUpdateDelegate()
        {


            ISceneManager sceneManager = GameLoop.getSceneManager();

            
            Skeleton skeleton = base.kinectSensorHelper.getTrackedSkeleton();

            if (skeleton != null)
            {

                Joint handJoint = skeleton.Joints[JointType.HandRight];

                if (handJoint.TrackingState == JointTrackingState.Tracked || handJoint.TrackingState == JointTrackingState.Inferred)
                {

                    double xOffsetScreenSpace;

                    { // calculate xOffsetScreenSpace 

                        double xCurrentJointPosition = handJoint.Position.X;
                        double xOffsetScreenCandidate = xCurrentJointPosition * PIXEL_PER_CENTIMETER;
                        bool xOutOfCanvasBound = this.getXPosition() + xOffsetScreenCandidate > sceneManager.getCanvasWidth() - this.getImage().Width || this.getXPosition() + xOffsetScreenCandidate < 0.0;

                        xOffsetScreenSpace = xOutOfCanvasBound ? 0.0 : xOffsetScreenCandidate;

                    }



                    double yOffsetScreenSpace;

                    { // calculate yOffsetScreenSpace 

                        double yCurrentJointPosition = handJoint.Position.Y;
                        double yOffsetScreenCandidate = -1 * (yCurrentJointPosition * PIXEL_PER_CENTIMETER);
                        bool yOutOfCanvasBound = this.getYPosition() + yOffsetScreenCandidate > sceneManager.getCanvasHeight() - this.getImage().Height || this.getYPosition() + yOffsetScreenCandidate < 0.0;

                        yOffsetScreenSpace = yOutOfCanvasBound ? 0.0 : yOffsetScreenCandidate;
                        
                    }


                    sceneManager.applyTranslation(this, xOffsetScreenSpace, yOffsetScreenSpace);



                    //Debug.WriteLine("***********************************");

                    //double xOffsetScreenSpace;

                    //{ // calculate xOffsetScreenSpace 

                    //    double xCurrentJointPosition = handJoint.Position.X;
                    //    double xOffsetScreenCandidate = xCurrentJointPosition * PIXEL_PER_CENTIMETER;

                    //    bool xSignificantShiftRegistered = Math.Abs(this.xPreviousJointPosition - xCurrentJointPosition) > SENSIBILITY;
                    //    bool xOutOfCanvasBound = this.getXPosition() + xOffsetScreenCandidate > sceneManager.getCanvasWidth() - this.getImage().Width || this.getXPosition() + xOffsetScreenCandidate < 0.0;


                    //    if (!xSignificantShiftRegistered)
                    //    {
                    //        this.xStableTimeMillis += Time.getDeltaTimeMillis();
                    //    }
                    //    else
                    //    {
                    //        this.xStableTimeMillis = 0;
                    //        this.xPreviousJointPosition = xCurrentJointPosition;
                    //    }


                    //    if (this.xStableTimeMillis >= STABLE_POSITION_MIN_TIME_MILLIS || xOutOfCanvasBound)
                    //    {
                    //        xOffsetScreenSpace = 0.0;
                    //    }
                    //    else
                    //    {
                    //        xOffsetScreenSpace = xOffsetScreenCandidate;
                    //    }


                    //}



                    //double yOffsetScreenSpace;

                    //{ // calculate yOffsetScreenSpace 

                    //    double yCurrentJointPosition = handJoint.Position.Y;
                    //    double yOffsetScreenCandidate = -1 * (yCurrentJointPosition * PIXEL_PER_CENTIMETER);

                    //    bool ySignificantShiftRegistered = Math.Abs(this.yPreviousJointPosition - yCurrentJointPosition) > SENSIBILITY;
                    //    bool yOutOfCanvasBound = this.getYPosition() + yOffsetScreenCandidate > sceneManager.getCanvasHeight() - this.getImage().Height || this.getYPosition() + yOffsetScreenCandidate < 0.0;


                    //    if (!ySignificantShiftRegistered)
                    //    {
                    //        this.yStableTimeMillis += Time.getDeltaTimeMillis();
                    //    }
                    //    else
                    //    {
                    //        this.yStableTimeMillis = 0;
                    //        this.yPreviousJointPosition = yCurrentJointPosition;
                    //    }


                    //    if (this.yStableTimeMillis >= STABLE_POSITION_MIN_TIME_MILLIS || yOutOfCanvasBound)
                    //    {
                    //        yOffsetScreenSpace = 0.0;
                    //    }
                    //    else
                    //    {
                    //        yOffsetScreenSpace = yOffsetScreenCandidate;
                    //    }


                    //}
                    
                    
                    //sceneManager.applyTranslation(this, xOffsetScreenSpace, yOffsetScreenSpace);

                    
                }
            }
        }


    }
}
