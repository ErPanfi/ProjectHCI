using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows;


namespace ProjectHCI.KinectEngine
{
    public abstract class GameObjectBase : IGameObject
    {

        protected int _timeToLiveMillis = 0;
        protected int _currentTimeToLiveMillis = 0;
        protected Geometry _geometry = null;
        protected ImageSource _imageSource = null;
        protected String _uid = null;

        protected GameObjectTypeEnum _objectType;

        public GameObjectTypeEnum getObjectTypeEnum()
        {
            return _objectType;
        }        

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





        public virtual void onRendererDisplayDelegate()
        {

            ISceneManager sceneManager = GameLoop.getSceneManager();

            Canvas mainWindowCanvas = sceneManager.getTargetCanvas();


            double boundingBoxHeight = _geometry.Bounds.Height;
            double boundingBoxWidth = _geometry.Bounds.Width;
            double yPositionBoundingBox = _geometry.Bounds.Y;
            double xPositionBoundingBox = _geometry.Bounds.X;


            if (Application.Current == null)
            {
                return;
            }


            Application.Current.Dispatcher.Invoke(new Action(
                delegate()
                {
                    

                    Image image = new Image();
                    image.Source = _imageSource;
                    image.Uid = _uid;
                    image.Height = boundingBoxHeight;
                    image.Width = boundingBoxWidth;

                    mainWindowCanvas.Children.Add(image);
                    sceneManager.registerUiElement(image);

                    Canvas.SetTop(image, yPositionBoundingBox);
                    Canvas.SetLeft(image, xPositionBoundingBox);


#if DEBUG
                    //********************* display boundingBox

                    GeometryDrawing geometryDrawing = new GeometryDrawing(null, new Pen(Brushes.Red, 1.0), _geometry);
                    DrawingImage boundingBoxDrawingImage = new DrawingImage(geometryDrawing);

                    Image boundingBoxImage = new Image();
                    boundingBoxImage.Source = boundingBoxDrawingImage;
                    boundingBoxImage.Uid = "BB_" + _uid;

                    mainWindowCanvas.Children.Add(boundingBoxImage);
                    sceneManager.registerUiElement(boundingBoxImage);

                    Canvas.SetTop(boundingBoxImage, yPositionBoundingBox);
                    Canvas.SetLeft(boundingBoxImage, xPositionBoundingBox);
                    Canvas.SetZIndex(boundingBoxImage, 100);
                    //*********************
#endif

                }
            )); 

            
        }





        public virtual void onRendererRemoveDelegate()
        {

            ISceneManager sceneManager = GameLoop.getSceneManager();

            Canvas mainWindowCanvas = sceneManager.getTargetCanvas();


            if (Application.Current == null)
            {
                return;
            }



            Application.Current.Dispatcher.Invoke(new Action(
                delegate()
                {

                    UIElement uiElement = sceneManager.getUiElementByUid(_uid);
                    mainWindowCanvas.Children.Remove(uiElement);
                    sceneManager.unregisterUiElement(uiElement);

#if DEBUG
                    //********************* hide boundingbox 
                    String boundingBoxUid = "BB_" + _uid;

                    UIElement boundingBoxUiElement = sceneManager.getUiElementByUid(boundingBoxUid);
                    mainWindowCanvas.Children.Remove(boundingBoxUiElement);
                    sceneManager.unregisterUiElement(boundingBoxUiElement);
                    //*********************
#endif

                }
            ));

        }




        public abstract void onRendererUpdateDelegate();

        public abstract void onCollisionEnterDelegate(IGameObject otherGameObject);

        public abstract void onCollisionExitDelegate(IGameObject otherGameObject);

    }
}
