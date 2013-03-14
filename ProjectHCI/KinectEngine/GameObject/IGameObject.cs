using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Controls;


namespace ProjectHCI.KinectEngine
{
    public enum GameObjectTypeEnum
    {
        DebugObject,
        UserObject,
        FriendlyObject,
        UnfriendlyObject,
        UIObject
    }

    public interface IGameObject
    {

        int getTimeToLiveMillis();

        int getCurrentTimeToLiveMillis();
        
        Geometry getGeometry();

        ImageSource getImageSource();

        String getUid();

        GameObjectTypeEnum getObjectTypeEnum();


        void updateTimeToLive(int deltaTimeMillis);

        bool isCollidable();

        bool isDead();


        void onRendererUpdateDelegate();

        void onRendererDisplayDelegate();

        void onRendererRemoveDelegate();

        void onCollisionEnterDelegate(IGameObject otherGameObject);

        void onCollisionExitDelegate(IGameObject otherGameObject);
        
        

    }
}
