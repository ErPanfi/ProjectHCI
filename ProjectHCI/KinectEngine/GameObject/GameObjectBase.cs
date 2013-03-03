﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Controls;
using System.Diagnostics;


namespace ProjectHCI.KinectEngine
{
    public abstract class GameObjectBase : IGameObject
    {

        protected int _timeToLiveMillis = 0;
        protected int _currentTimeToLiveMillis = 0;
        protected Geometry _geometry = null;
        protected ImageSource _imageSource = null;
        protected String _uid = null;



        public int getTimeToLiveMillis()
        {
            return _timeToLiveMillis;
        }

        public int getCurrentTimeToLiveMillis()
        {
            return _currentTimeToLiveMillis;
        }

        public Geometry getGeometry()
        {
            return _geometry;
        }

        public ImageSource getImageSource()
        {
            return _imageSource;
        }

        public String getUid()
        {
            return _uid;
        }





        protected String generateUid()
        {
            return Guid.NewGuid().ToString();
        }




        public virtual void updateTimeToLive(int deltaTimeMillis)
        {
            if (_currentTimeToLiveMillis >= 0)
            {
                _currentTimeToLiveMillis -= deltaTimeMillis;
            }
        }



        public abstract bool isCollidable();

        public abstract bool isDead();





        public virtual void onRendererDisplayDelegate(Canvas mainWindowCanvas)
        {
            Debug.Assert(mainWindowCanvas != null, "expected mainWindowCanvas != null");

            Image image = new Image();
            image.Source = _imageSource;
            image.Uid = _uid;
            image.Height = _geometry.Bounds.Height;
            image.Width = _geometry.Bounds.Width;

            mainWindowCanvas.Children.Add(image);
            
            Canvas.SetTop(image, _geometry.Bounds.Y);
            Canvas.SetLeft(image, _geometry.Bounds.X);

#if DEBUG
            //********************* display boundingBox
            GeometryDrawing geometryDrawing = new GeometryDrawing(null, new Pen(Brushes.Red, 1.0), _geometry);
            DrawingImage boundingBoxDrawingImage = new DrawingImage(geometryDrawing);

            Image boundingBoxImage = new Image();
            boundingBoxImage.Source = boundingBoxDrawingImage;
            boundingBoxImage.Uid = "BB_" + _uid;

            mainWindowCanvas.Children.Add(boundingBoxImage);
            Canvas.SetTop(boundingBoxImage, _geometry.Bounds.Y);
            Canvas.SetLeft(boundingBoxImage, _geometry.Bounds.X);
            Canvas.SetZIndex(boundingBoxImage, 100);
            //*********************
#endif
            
        }





        public virtual void onRendererRemoveDelegate(Canvas mainWindowCanvas)
        {
            Debug.Assert(mainWindowCanvas != null, "expected mainWindowCanvas != null");

            foreach (System.Windows.UIElement childUiElement0 in mainWindowCanvas.Children)
            {
                if (childUiElement0.Uid.Equals(_uid))
                {
                    mainWindowCanvas.Children.Remove(childUiElement0);
                    break;
                }
            }

#if DEBUG
            //********************* hide boundingbox 
            String boundingBoxUid = "BB_" + _uid;
            foreach (System.Windows.UIElement childUiElement0 in mainWindowCanvas.Children)
            {
                if (childUiElement0.Uid.Equals(boundingBoxUid))
                {
                    mainWindowCanvas.Children.Remove(childUiElement0);
                    break;
                }
            }
            //*********************
#endif

        }




        public abstract void onRendererUpdateDelegate(Canvas mainWindowCanvas);

        public abstract void onCollisionEnterDelegate(IGameObject otherGameObject);

        public abstract void onCollisionExitDelegate(IGameObject otherGameObject);

    }
}
