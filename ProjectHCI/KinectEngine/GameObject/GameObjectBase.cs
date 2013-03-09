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

            if (Application.Current == null)
            {
                return;
            }



            ISceneManager sceneManager = GameLoop.getSceneManager();
                       

            Geometry boundingBoxAsFrozen = (Geometry) _geometry.GetAsFrozen();


            Application.Current.Dispatcher.Invoke(new Action(
                delegate()
                {
                    

                    Image image = new Image();
                    image.Source = _imageSource;
                    image.Uid = _uid;
                    image.Height = boundingBoxAsFrozen.Bounds.Height;
                    image.Width = boundingBoxAsFrozen.Bounds.Width;

                    sceneManager.getTargetCanvas().Children.Add(image);
                    sceneManager.registerUiElement(image);

                    Canvas.SetTop(image, boundingBoxAsFrozen.Bounds.Y);
                    Canvas.SetLeft(image, boundingBoxAsFrozen.Bounds.X);


#if DEBUG
                    //********************* display boundingBox

                    GeometryDrawing geometryDrawing = new GeometryDrawing(null, new Pen(Brushes.Red, 1.0), boundingBoxAsFrozen);
                    DrawingImage boundingBoxDrawingImage = new DrawingImage(geometryDrawing);

                    Image boundingBoxImage = new Image();
                    boundingBoxImage.Source = boundingBoxDrawingImage;
                    boundingBoxImage.Uid = "BB_" + _uid;

                    sceneManager.getTargetCanvas().Children.Add(boundingBoxImage);
                    sceneManager.registerUiElement(boundingBoxImage);

                    Canvas.SetTop(boundingBoxImage, boundingBoxAsFrozen.Bounds.Y);
                    Canvas.SetLeft(boundingBoxImage, boundingBoxAsFrozen.Bounds.X);
                    Canvas.SetZIndex(boundingBoxImage, 100);
                    //*********************
#endif

                }
            )); 

            
        }





        public virtual void onRendererRemoveDelegate()
        {

            ISceneManager sceneManager = GameLoop.getSceneManager();



            if (Application.Current == null)
            {
                return;
            }



            Application.Current.Dispatcher.Invoke(new Action(
                delegate()
                {

                    UIElement uiElement = sceneManager.getUiElementByUid(_uid);
                    sceneManager.getTargetCanvas().Children.Remove(uiElement);
                    sceneManager.unregisterUiElement(uiElement);

#if DEBUG
                    //********************* hide boundingbox 
                    String boundingBoxUid = "BB_" + _uid;

                    UIElement boundingBoxUiElement = sceneManager.getUiElementByUid(boundingBoxUid);
                    sceneManager.getTargetCanvas().Children.Remove(boundingBoxUiElement);
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
