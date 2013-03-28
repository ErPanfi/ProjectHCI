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
        protected String _gameObjectTag = "no_tag";
        protected Image _image;






        public virtual void setPosition(double xPosition, double yPosition)
        {
            _xPosition = xPosition;
            _yPosition = yPosition;
        }

        public virtual double getXPosition()
        {
            return _xPosition;
        }

        public virtual double getYPosition()
        {
            return _yPosition;
        }





        public virtual void setBoundingBoxGeometry(Geometry boundingBoxGeometry)
        {
            //for convention the boundingBox position is relative to the gameObject position,
            //so we need to convert this position into game-Object space.
            boundingBoxGeometry.Transform = new TranslateTransform(this._xPosition, this._yPosition);
            _boundingBoxGeometry = boundingBoxGeometry;
        }

        public virtual Geometry getBoundingBoxGeometry()
        {
            return _boundingBoxGeometry;
        }




        public virtual void setImage(Image image)
        {
            _image = image;
        }

        public Image getImage()
        {
            return _image;
        }






        public virtual void setExtraData(object extraData)
        {
            _extraData = extraData;
        }

        public virtual object getExtraData()
        {
            return _extraData;
        }




        public virtual void setUid(String uid)
        {
            _uid = uid;
        }

        public virtual String getUid()
        {
            return _uid;
        }




        public virtual void setGameObjectTag(String gameObjectTag)
        {
            _gameObjectTag = gameObjectTag;
        }

        public virtual String getGameObjectTag()
        {
            return _gameObjectTag;
        }




        public abstract void update(int deltaTimeMillis);




        public abstract bool isCollidable();

        public abstract bool isDead();




        public abstract void onRendererDisplayDelegate();

        public abstract void onRendererUpdateDelegate();

        public abstract void onRendererRemoveDelegate();




        public abstract void onCollisionEnterDelegate(IGameObject otherGameObject);

        public abstract void onCollisionExitDelegate(IGameObject otherGameObject);



    }
}
