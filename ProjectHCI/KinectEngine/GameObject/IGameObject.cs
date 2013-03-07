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
        Unknown,
        UserObject,
        FriendlyObject,
        UnfriendlyObject
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



        void onRendererUpdateDelegate(Canvas mainWindowCanvas, MainWindow currentMainWindow);

        void onRendererDisplayDelegate(Canvas mainWindowCanvas, MainWindow currentMainWindow);

        void onRendererRemoveDelegate(Canvas mainWindowCanvas, MainWindow currentMainWindow);

        void onCollisionEnterDelegate(IGameObject otherGameObject);

        void onCollisionExitDelegate(IGameObject otherGameObject);
        
        

    }
}
