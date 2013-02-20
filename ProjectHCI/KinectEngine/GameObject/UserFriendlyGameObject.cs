using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Controls;

namespace ProjectHCI.KinectEngine
{
    public class UserFriendlyGameObject : IGameObject
    {
        
        private int timeToLiveMillis;
        private int currentTimeToLiveMillis;
        private Geometry geometry;
        private ImageSource imageSource;

        private Delegate onTimeElapsedDelegate;



        public UserFriendlyGameObject(Geometry geometry, 
                                      ImageSource imageSource, 
                                      int timeToLiveMillis,
                                      Delegate onTimeElapsedDelegate)
        {
            this.timeToLiveMillis = timeToLiveMillis;
            this.currentTimeToLiveMillis = timeToLiveMillis;
            this.geometry = geometry;
            this.imageSource = imageSource;
            this.onTimeElapsedDelegate = onTimeElapsedDelegate;
        }




        
        public void updateTimeToLive(int deltaTimeMillis)
        {
            if (this.currentTimeToLiveMillis >= 0)
            {
                this.currentTimeToLiveMillis -= deltaTimeMillis;
            }
        }


        public bool isCollidable()
        {
            return this.currentTimeToLiveMillis >= 0;
        }


        public bool isDead()
        {
            return this.currentTimeToLiveMillis <= 0;
        }


        public int getTimeToLiveMillis()
        {
            return this.timeToLiveMillis;
        }

        public int getCurrentTimeToLiveMillis()
        {
            return this.currentTimeToLiveMillis;
        }

        public Geometry getGeometry()
        {
            return this.geometry;
        }

        public ImageSource getImageSource()
        {
            return this.imageSource;
        }


        public Delegate getOnTimeToLiveUpdateDelegate()
        {
            return null;
        }

        public Delegate getOnTimeElapsedDelegate()
        {
            return this.onTimeElapsedDelegate;
        }

    }
}
