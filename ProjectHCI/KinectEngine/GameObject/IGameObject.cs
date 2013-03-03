using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Controls;


namespace ProjectHCI.KinectEngine
{
    public interface IGameObject
    {

        int getTimeToLiveMillis();

        int getCurrentTimeToLiveMillis();
        
        Geometry getGeometry();

        ImageSource getImageSource();

        String getUid();



        void updateTimeToLive(int deltaTimeMillis);

        bool isCollidable();

        bool isDead();



        void onRendererUpdateDelegate(Canvas mainWindowCanvas);

        void onRendererDisplayDelegate(Canvas mainWindowCanvas);

        void onRendererRemoveDelegate(Canvas mainWindowCanvas);

        void onCollisionEnterDelegate(IGameObject otherGameObject);

        void onCollisionExitDelegate(IGameObject otherGameObject);
        
        

    }
}
