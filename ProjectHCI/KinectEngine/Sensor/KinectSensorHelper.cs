using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Kinect;
using System.IO;
using System.Diagnostics;
using System.Windows;

namespace ProjectHCI.KinectEngine
{

    public enum SkeletonSmoothingFilter
    {
        DEFAULT_SMOOTHING_LEVEL,
        MEDIUM_SMOOTHING_LEVEL,
        HIGH_SMOOTHING_LEVEL
    }


    public class KinectSensorHelper
    {

        private KinectSensor kinectSensor;
        private Skeleton trackedSkeleton;
        private TransformSmoothParameters transformSmoothParameters;


        public KinectSensorHelper(SkeletonSmoothingFilter skeletonSmoothingFilter)
        {

            this.kinectSensor = null;
            this.trackedSkeleton = null;

            //values keep from MSDN Joint Filtering at http://msdn.microsoft.com/en-us/library/jj131024.aspx
            switch (skeletonSmoothingFilter)
            {

                case SkeletonSmoothingFilter.DEFAULT_SMOOTHING_LEVEL:
                    this.transformSmoothParameters = new TransformSmoothParameters();
                    this.transformSmoothParameters.Smoothing = 0.5f;
                    this.transformSmoothParameters.Correction = 0.5f;
                    this.transformSmoothParameters.Prediction = 0.5f;
                    this.transformSmoothParameters.JitterRadius = 0.05f;
                    this.transformSmoothParameters.MaxDeviationRadius = 0.04f;

                    break;

                case SkeletonSmoothingFilter.MEDIUM_SMOOTHING_LEVEL:
                    this.transformSmoothParameters = new TransformSmoothParameters();
                    this.transformSmoothParameters.Smoothing = 0.5f;
                    this.transformSmoothParameters.Correction = 0.1f;
                    this.transformSmoothParameters.Prediction = 0.5f;
                    this.transformSmoothParameters.JitterRadius = 0.1f;
                    this.transformSmoothParameters.MaxDeviationRadius = 0.1f;

                    break;

                case SkeletonSmoothingFilter.HIGH_SMOOTHING_LEVEL:
                    this.transformSmoothParameters = new TransformSmoothParameters();
                    this.transformSmoothParameters.Smoothing = 0.7f;
                    this.transformSmoothParameters.Correction = 0.3f;
                    this.transformSmoothParameters.Prediction = 1.0f;
                    this.transformSmoothParameters.JitterRadius = 1.0f;
                    this.transformSmoothParameters.MaxDeviationRadius = 1.0f;

                    break;

                default:
                    throw new Exception("Unexpected SkeletonSmoothingFilter");
            }

        }


        /// <summary>
        /// 
        /// </summary>
        public void initializeKinect()
        {

            KinectSensor choosenKinectSensor = null;

            foreach (KinectSensor kinectSensor0 in KinectSensor.KinectSensors)
            {
                if (kinectSensor0.Status == KinectStatus.Connected)
                {
                    choosenKinectSensor = kinectSensor0;
                    break;
                }
            }

            if (choosenKinectSensor != null)
            {
                choosenKinectSensor.SkeletonStream.Enable(transformSmoothParameters);
                choosenKinectSensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(this.onSensorSkeletonFrameReady);

                choosenKinectSensor.Start();
                this.kinectSensor = choosenKinectSensor;

            }
            else
            {
                MessageBox.Show("No Kinect sensor found, please connect your Kinect and restart the application",
                                "Kinect Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                GameLoop.getGameLoopSingleton().stop();
            }
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventSender"></param>
        /// <param name="skeletonFrameReadyEventArgs"></param>
        private void onSensorSkeletonFrameReady(object eventSender, SkeletonFrameReadyEventArgs skeletonFrameReadyEventArgs)
        {
            Skeleton[] skeletonArray = null;

            using (SkeletonFrame skeletonFrame = skeletonFrameReadyEventArgs.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletonArray = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletonArray);

                    Skeleton firstTrackedSkeleton = skeletonArray.FirstOrDefault<Skeleton>(skeleton0 => skeleton0.TrackingState == SkeletonTrackingState.Tracked);

                    if (firstTrackedSkeleton != null)
                    {
                        //use this setter for thread safety
                        this.setTrackedSkeleton(firstTrackedSkeleton);
                    }
                    
                }
            }

            
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="skeleton"></param>
        private void setTrackedSkeleton(Skeleton skeleton)
        {
            lock (this)
            {
                this.trackedSkeleton = skeleton;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Skeleton getTrackedSkeleton()
        {
            lock (this)
            {
                return this.trackedSkeleton;
            }
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public KinectSensor getKinectSensor()
        //{
        //    return this.kinectSensor;
        //}


        //private void TrackedSkeletonJoints(JointCollection jointCollection)
        //{
        //    UpdatePositionHeadAnanas(jointCollection[JointType.Head], jointCollection[JointType.ShoulderCenter], jointCollection[JointType.Spine]);
        //}

        

        //private void UpdatePositionHeadAnanas(Joint jointHead, Joint jointShoulderCenter, Joint jointSpine)
        //{

        //    double degree = DetectRollHeadAngle(jointHead.Position.X, jointHead.Position.Y, jointSpine.Position.X, jointSpine.Position.Y);
        //    LabelColloTesta.Content = "testa X,Y=(" + jointHead.Position.X + " , " + jointHead.Position.Y + ")" + "\ncollo X,Y=(" + jointSpine.Position.X + " , " + jointSpine.Position.Y + ")" + "\n" + "Angolo: " + degree;
        //    RotateTransform rotateTransform1 = new RotateTransform(degree, 50, 50);
        //    //le posizioni degli oggetti sono sempre aggiornate in base al proprio contenitore (coordinate relative)
        //    Canvas.SetLeft(AnanasImage, 200 + (jointHead.Position.X * 500));
        //    Canvas.SetTop(AnanasImage, 400 - (jointHead.Position.Y * 500));
        //    AnanasImage.RenderTransform = rotateTransform1;

        //}



    }
}
