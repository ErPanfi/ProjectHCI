﻿using Microsoft.Kinect;
using ProjectHCI.KinectEngine;
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
        private IGameObject hourglassChildGameObject;

        
        public HandUserGameObject(double xPosition,
                                  double yPosition,
                                  Geometry boundingBoxGeometry,
                                  Image image,
                                  SkeletonSmoothingFilter skeletonSmoothingFilter)
            : base (xPosition, yPosition, boundingBoxGeometry, image, skeletonSmoothingFilter)
        {
            this.hourglassChildGameObject = null;
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

                if (handJoint.TrackingState == JointTrackingState.Tracked)
                {

                    double xHandJointPosition = Math.Abs(handJoint.Position.X) > 0.4 ? 0.4 * Math.Sign(handJoint.Position.X) : handJoint.Position.X;
                    double yHandJointPosition = Math.Abs(handJoint.Position.Y) > 0.4 ? 0.4 * Math.Sign(handJoint.Position.Y) : handJoint.Position.Y;


                    double xScreenPosition = this.mapValueToNewRange(xHandJointPosition, -0.4, 0.4, 0, sceneManager.getCanvasWidth() - this.getImage().Width);
                    double yScreenPosition = this.mapValueToNewRange(yHandJointPosition, 0.4, -0.4, 0, sceneManager.getCanvasHeight() - this.getImage().Height);

           
                    sceneManager.applyTranslation(this, xScreenPosition - this.getXPosition(), yScreenPosition - this.getYPosition());

                }
            }
        }



        public override void onCollisionEnterDelegate(IGameObject otherGameObject)
        {
            //ISpawnerManager spawnerManager = GameLoop.getSpawnerManager();


            //// hourglass creation

            //Image image = new Image();
            //image.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/clock.png"));
            //image.Height = 100;
            //image.Width = 100;

            //Geometry boundingBoxGeometry = new EllipseGeometry(new Rect(new Point(0, 0), new Point(100, 100)));
            //IGameObject hourglassGameObject = new UserFriendlyGameObject(150, 150, boundingBoxGeometry, image, 4000);


            //spawnerManager.specialRequestToSpawn(new GameObjectSpawnRequest(hourglassGameObject, this));
            //this.hourglassChildGameObject = hourglassGameObject;
        }



        public override void onCollisionExitDelegate(IGameObject otherGameObject)
        {
            //ISpawnerManager spawnerManager = GameLoop.getSpawnerManager();
            //spawnerManager.specialRequestToKillGameObject(this.hourglassChildGameObject);
        }


    }
}
