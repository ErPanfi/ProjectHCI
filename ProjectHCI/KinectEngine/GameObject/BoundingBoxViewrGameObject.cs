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
    public class BoundingBoxViewrGameObject : GameObject
    {

        private IGameObject gameObject;
        private bool previousCollidableState;

        public BoundingBoxViewrGameObject(IGameObject gameObject)
        {

            Debug.Assert(gameObject.getBoundingBoxGeometry() != null, "expected gameObject.getBoundingBoxGeometry() != null");

            base._xPosition = gameObject.getBoundingBoxGeometry().Bounds.X;
            base._yPosition = gameObject.getBoundingBoxGeometry().Bounds.Y;
            base._boundingBoxGeometry = gameObject.getBoundingBoxGeometry();
            base._extraData = null;
            base._uid = Guid.NewGuid().ToString();
            base._gameObjectTypeEnum = GameObjectTypeEnum.DebugObject;


            Brush boundingBoxBrush = gameObject.isCollidable() ? Brushes.Red : Brushes.Yellow;
            GeometryDrawing geometryDrawing = new GeometryDrawing(null, new Pen(boundingBoxBrush, 1.0), gameObject.getBoundingBoxGeometry());
            DrawingImage boundingBoxDrawingImage = new DrawingImage(geometryDrawing);

            Image boundingBoxImage = new Image();
            boundingBoxImage.Source = boundingBoxDrawingImage;

            base._image = boundingBoxImage;

            this.gameObject = gameObject;
            this.previousCollidableState = gameObject.isCollidable();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTimeMillis"></param>
        public override void update(int deltaTimeMillis)
        {
            // do nothing
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool isCollidable()
        {
            return false;
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
            sceneManager.canvasDisplayImage(this, 100);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void onRendererUpdateDelegate()
        {
            if (this.previousCollidableState != this.gameObject.isCollidable())
            {
                Brush boundingBoxBrush = gameObject.isCollidable() ? Brushes.Red : Brushes.Yellow;
                GeometryDrawing geometryDrawing = new GeometryDrawing(null, new Pen(boundingBoxBrush, 1.0), gameObject.getBoundingBoxGeometry());
                DrawingImage boundingBoxDrawingImage = new DrawingImage(geometryDrawing);

                Image boundingBoxImage = new Image();
                boundingBoxImage.Source = boundingBoxDrawingImage;

                base._image = boundingBoxImage;

                ISceneManager sceneManager = GameLoop.getSceneManager();
                sceneManager.canvasUpdateImage(this, 100);

                this.previousCollidableState = gameObject.isCollidable();

            }
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherGameObject"></param>
        public override void onCollisionExitDelegate(IGameObject otherGameObject)
        {
            throw new NotImplementedException();
        }
    }
}
