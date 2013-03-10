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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GameObjectTypeEnum getObjectTypeEnum()
        {
            return _objectType;
        }        
        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getTimeToLiveMillis()
        {
            return _timeToLiveMillis;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getCurrentTimeToLiveMillis()
        {
            return _currentTimeToLiveMillis;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Geometry getGeometry()
        {
            return _geometry;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ImageSource getImageSource()
        {
            return _imageSource;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String getUid()
        {
            return _uid;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected String generateUid()
        {
            return Guid.NewGuid().ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTimeMillis"></param>
        public virtual void updateTimeToLive(int deltaTimeMillis)
        {
            if (_currentTimeToLiveMillis >= 0)
            {
                _currentTimeToLiveMillis -= deltaTimeMillis;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract bool isCollidable();


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract bool isDead();




        /// <summary>
        /// 
        /// </summary>
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


                    Brush boundingBoxBrush = this.isCollidable() ? Brushes.Red : Brushes.Yellow;
                    GeometryDrawing geometryDrawing = new GeometryDrawing(null, new Pen(boundingBoxBrush, 1.0), boundingBoxAsFrozen);

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




        /// <summary>
        /// 
        /// </summary>
        public virtual void onRendererRemoveDelegate()
        {
            if (Application.Current == null)
            {
                return;
            }


            ISceneManager sceneManager = GameLoop.getSceneManager();


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





        /// <summary>
        /// 
        /// </summary>
        public virtual void onRendererUpdateDelegate()
        {

#if DEBUG
            //********************* update boundingbox 
            if (Application.Current == null)
            {
                return;
            }


            ISceneManager sceneManager = GameLoop.getSceneManager();
            Geometry boundingBoxAsFrozen = (Geometry)_geometry.GetAsFrozen();


            
            Application.Current.Dispatcher.Invoke(new Action(
                delegate()
                {

                    Brush boundingBoxBrush = this.isCollidable() ? Brushes.Red : Brushes.Yellow;
                    GeometryDrawing geometryDrawing = new GeometryDrawing(null, new Pen(boundingBoxBrush, 1.0), boundingBoxAsFrozen);
                    DrawingImage boundingBoxDrawingImage = new DrawingImage(geometryDrawing);

                    String boundingBoxUid = "BB_" + _uid;
                    UIElement boundingBoxUiElement = sceneManager.getUiElementByUid(boundingBoxUid);

                    Debug.Assert(boundingBoxUiElement.GetType() == typeof(Image), "expected boundingBoxUiElement typeof Image");

                    Image boundingBoxImage = (Image)boundingBoxUiElement;

                    //if gameObject change its "isCollidable" state then change its bounding-box color
                    if (!boundingBoxImage.Source.Equals(boundingBoxDrawingImage))
                    {

                        sceneManager.getTargetCanvas().Children.Remove(boundingBoxUiElement);
                        sceneManager.unregisterUiElement(boundingBoxUiElement);


                        Image newBoundingBoxImage = new Image();
                        newBoundingBoxImage.Source = boundingBoxDrawingImage;
                        newBoundingBoxImage.Uid = "BB_" + _uid;

                        sceneManager.getTargetCanvas().Children.Add(newBoundingBoxImage);
                        sceneManager.registerUiElement(newBoundingBoxImage);

                        Canvas.SetTop(newBoundingBoxImage, boundingBoxAsFrozen.Bounds.Y);
                        Canvas.SetLeft(newBoundingBoxImage, boundingBoxAsFrozen.Bounds.X);
                        Canvas.SetZIndex(newBoundingBoxImage, 100);

                    }




                }
            ));
            //********************* 
#endif
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherGameObject"></param>
        public abstract void onCollisionEnterDelegate(IGameObject otherGameObject);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherGameObject"></param>
        public abstract void onCollisionExitDelegate(IGameObject otherGameObject);

    }
}
