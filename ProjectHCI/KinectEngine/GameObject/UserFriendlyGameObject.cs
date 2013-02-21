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
        private String uid;

        private Delegate onTimeElapsedDelegate;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="geometry"></param>
        /// <param name="imageSource"></param>
        /// <param name="timeToLiveMillis"></param>
        /// <param name="onTimeElapsedDelegate"></param>
        public UserFriendlyGameObject(String uid,
                                      Geometry geometry, 
                                      ImageSource imageSource, 
                                      int timeToLiveMillis,
                                      Delegate onTimeElapsedDelegate)
        {
            this.timeToLiveMillis = timeToLiveMillis;
            this.currentTimeToLiveMillis = timeToLiveMillis;
            this.geometry = geometry;
            this.imageSource = imageSource;
            this.uid = uid;
            this.onTimeElapsedDelegate = onTimeElapsedDelegate;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String getUid()
        {
            return this.uid;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTimeMillis"></param>
        public void updateTimeToLive(int deltaTimeMillis)
        {
            if (this.currentTimeToLiveMillis >= 0)
            {
                this.currentTimeToLiveMillis -= deltaTimeMillis;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool isCollidable()
        {
            return this.currentTimeToLiveMillis >= 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool isDead()
        {
            return this.currentTimeToLiveMillis <= 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getTimeToLiveMillis()
        {
            return this.timeToLiveMillis;
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getCurrentTimeToLiveMillis()
        {
            return this.currentTimeToLiveMillis;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Geometry getGeometry()
        {
            return this.geometry;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ImageSource getImageSource()
        {
            return this.imageSource;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Delegate getOnTimeToLiveUpdateDelegate()
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Delegate getOnTimeElapsedDelegate()
        {
            return this.onTimeElapsedDelegate;
        }

    }
}
