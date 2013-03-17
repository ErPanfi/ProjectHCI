using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;


namespace ProjectHCI.KinectEngine
{
    public abstract class GameObject : IGameObject
    {


        protected double _xPosition;
        protected double _yPosition;
        protected Geometry _boundingBoxGeometry;
        protected object _extraData;
        protected String _uid;
        protected GameObjectTypeEnum _gameObjectTypeEnum;
        protected Image _image;






        public void setPosition(double xPosition, double yPosition)
        {
            _xPosition = xPosition;
            _yPosition = yPosition;
        }

        public double getXPosition()
        {
            return _xPosition;
        }

        public double getYPosition()
        {
            return _yPosition;
        }





        public void setBoundingBoxGeometry(Geometry boundingBoxGeometry)
        {
            //for convention the boundingBox position is relative to the gameObject position,
            //so we need to convert this position into game-Object space.
            boundingBoxGeometry.Transform = new TranslateTransform(this._xPosition, this._yPosition);
            _boundingBoxGeometry = boundingBoxGeometry;
        }

        public Geometry getBoundingBoxGeometry()
        {
            return _boundingBoxGeometry;
        }




        public void setImage(Image image)
        {
            _image = image;
        }

        public Image getImage()
        {
            return _image;
        }






        public void setExtraData(object extraData)
        {
            _extraData = extraData;
        }

        public object getExtraData()
        {
            return _extraData;
        }




        public void setUid(String uid)
        {
            _uid = uid;
        }

        public String getUid()
        {
            return _uid;
        }




        public void setGameObjectTypeEnum(GameObjectTypeEnum gameObjectTypeEnum)
        {
            _gameObjectTypeEnum = gameObjectTypeEnum;
        }

        public GameObjectTypeEnum getGameObjectTypeEnum()
        {
            return _gameObjectTypeEnum;
        }




        public abstract void update(int deltaTimeMillis);




        public abstract bool isCollidable();

        public abstract bool isDead();




        public abstract void onRendererDisplayDelegate();

        public abstract void onRendererUpdateDelegate();

        public abstract void onRendererRemoveDelegate();




        public abstract void onCollisionEnterDelegate(IGameObject otherGameObject);

        public abstract void onCollisionExitDelegate(IGameObject otherGameObject);



//        /// <summary>
//        /// 
//        /// </summary>
//        public virtual void onRendererDisplayDelegate()
//        {

//            if (Application.Current == null)
//            {
//                return;
//            }



//            ISceneManager sceneManager = GameLoop.getSceneManager();
//            Geometry boundingBoxAsFrozen = (Geometry) _geometry.GetAsFrozen();


//            Application.Current.Dispatcher.Invoke(new Action(
//                delegate()
//                {
                    

//                    Image image = new Image();
//                    image.Source = (ImageSource) this.getExtraData();
//                    image.Uid = _uid;
//                    image.Height = boundingBoxAsFrozen.Bounds.Height;
//                    image.Width = boundingBoxAsFrozen.Bounds.Width;

//                    sceneManager.boundUiElementToGameObject(image, this);
//                    sceneManager.getTargetCanvas().Children.Add(image);
                    

//                    Canvas.SetTop(image, boundingBoxAsFrozen.Bounds.Y);
//                    Canvas.SetLeft(image, boundingBoxAsFrozen.Bounds.X);


//#if DEBUG
//                    //********************* display boundingBox


//                    Brush boundingBoxBrush = this.isCollidable() ? Brushes.Red : Brushes.Yellow;
//                    GeometryDrawing geometryDrawing = new GeometryDrawing(null, new Pen(boundingBoxBrush, 1.0), boundingBoxAsFrozen);

//                    DrawingImage boundingBoxDrawingImage = new DrawingImage(geometryDrawing);

//                    Image boundingBoxImage = new Image();
//                    boundingBoxImage.Source = boundingBoxDrawingImage;
//                    boundingBoxImage.Uid = "BB_" + _uid;

//                    sceneManager.boundUiElementToGameObject(boundingBoxImage, this);
//                    sceneManager.getTargetCanvas().Children.Add(boundingBoxImage);
                    

//                    Canvas.SetTop(boundingBoxImage, boundingBoxAsFrozen.Bounds.Y);
//                    Canvas.SetLeft(boundingBoxImage, boundingBoxAsFrozen.Bounds.X);
//                    Canvas.SetZIndex(boundingBoxImage, 100);
//                    //*********************
//#endif

//                }
//            )); 

            
//        }




//        /// <summary>
//        /// 
//        /// </summary>
//        public virtual void onRendererRemoveDelegate()
//        {
           

//            ISceneManager sceneManager = GameLoop.getSceneManager();

//            if (Application.Current != null)
//            {
//                Application.Current.Dispatcher.Invoke(new Action(
//                    delegate()
//                    {

//                        foreach (UIElement uiElement in sceneManager.getUiElementListBoundToGameObject(this))
//                        {
//                            sceneManager.getTargetCanvas().Children.Remove(uiElement);
//                        }
//                        sceneManager.unboundAllUiElementFromGameObject(this);

//                    }
//                ));
//            }

//        }





//        /// <summary>
//        /// 
//        /// </summary>
//        public virtual void onRendererUpdateDelegate()
//        {

//#if DEBUG
//            //********************* update boundingbox 
//            if (Application.Current == null)
//            {
//                return;
//            }


//            ISceneManager sceneManager = GameLoop.getSceneManager();
//            Geometry boundingBoxAsFrozen = (Geometry)_geometry.GetAsFrozen();


            
//            Application.Current.Dispatcher.Invoke(new Action(
//                delegate()
//                {

//                    Brush boundingBoxBrush = this.isCollidable() ? Brushes.Red : Brushes.Yellow;
//                    GeometryDrawing geometryDrawing = new GeometryDrawing(null, new Pen(boundingBoxBrush, 1.0), boundingBoxAsFrozen);
//                    DrawingImage boundingBoxDrawingImage = new DrawingImage(geometryDrawing);


//                    foreach (UIElement uiElement0 in sceneManager.getUiElementListBoundToGameObject(this))
//                    {

//                        if (uiElement0.Uid.Equals("BB_" + _uid))
//                        {
//                            UIElement boundingBoxUiElement = uiElement0;

//                            Debug.Assert(boundingBoxUiElement.GetType() == typeof(Image), "expected boundingBoxUiElement typeof Image");

//                            Image boundingBoxImage = (Image)boundingBoxUiElement;

//                            //if gameObject change its "isCollidable" state then change its bounding-box color
//                            if (!boundingBoxImage.Source.Equals(boundingBoxDrawingImage))
//                            {

//                                sceneManager.getTargetCanvas().Children.Remove(boundingBoxUiElement);
//                                sceneManager.unboundUiElementFromGameObject(this, boundingBoxUiElement);


//                                Image newBoundingBoxImage = new Image();
//                                newBoundingBoxImage.Source = boundingBoxDrawingImage;
//                                newBoundingBoxImage.Uid = "BB_" + _uid;

//                                sceneManager.getTargetCanvas().Children.Add(newBoundingBoxImage);
//                                sceneManager.boundUiElementToGameObject(newBoundingBoxImage, this);

//                                Canvas.SetTop(newBoundingBoxImage, boundingBoxAsFrozen.Bounds.Y);
//                                Canvas.SetLeft(newBoundingBoxImage, boundingBoxAsFrozen.Bounds.X);
//                                Canvas.SetZIndex(newBoundingBoxImage, 100);

//                            }

//                        }

//                    }
//                }
//            ));
//            //********************* 
//#endif
//        }


    }
}
