﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Controls;


namespace ProjectHCI.KinectEngine
{
    //public enum GameObjectTypeEnum
    //{
    //    DebugObject,
    //    UserObject,
    //    FriendlyObject,
    //    UnfriendlyObject,
    //    UIObject
    //}

    public interface IGameObject
    {

 
        void setPosition(double xPosition, double yPosition);

        double getXPosition();

        double getYPosition();





        void setBoundingBoxGeometry(Geometry boundingBoxGeometry);

        Geometry getBoundingBoxGeometry();




        void setImage(Image image);

        Image getImage();

       



        void setExtraData(object extraData);

        object getExtraData();




        void setUid(String uid);

        String getUid();




        void setGameObjectTag(String gameObjectTag);

        String getGameObjectTag();




        void update(int deltaTimeMillis);




        bool isCollidable();

        bool isDead();





        void onRendererUpdateDelegate();

        void onRendererDisplayDelegate();

        void onRendererRemoveDelegate();




        void onCollisionEnterDelegate(IGameObject otherGameObject);

        void onCollisionExitDelegate(IGameObject otherGameObject);
        
        

    }
}
