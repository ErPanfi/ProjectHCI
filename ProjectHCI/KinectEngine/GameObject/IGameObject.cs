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


        void updateTimeToLive(int deltaTimeMillis);

        bool isCollidable();

        bool isDead();


        Delegate getOnTimeToLiveUpdateDelegate();

        Delegate getOnTimeElapsedDelegate();
        
        

    }
}
