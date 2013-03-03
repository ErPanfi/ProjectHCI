using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Controls;
using System.Diagnostics;
using Microsoft.Kinect;

namespace ProjectHCI.KinectEngine
{
    class UserGameObject : GameObjectBase
    {

        //TODO populate this variable by kinect messages
        private Skeleton skeleton;

        public UserGameObject(Geometry geometry,
                                         ImageSource imageSource)
        {
            Debug.Assert(geometry != null, "expected geometry != null");
            Debug.Assert(imageSource != null, "expected imageSource != null");

            base._timeToLiveMillis = -1;
            base._currentTimeToLiveMillis = -1;
            base._geometry = geometry;
            base._imageSource = imageSource;
            base._uid = base.generateUid();

            this.skeleton = null;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool isCollidable()
        {
            return true;
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
        /// <param name="mainWindowCanvas"></param>
        public override void onRendererUpdateDelegate(Canvas mainWindowCanvas)
        {
            //TODO rotate and\or translate player image...
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherGameObject"></param>
        public override void onCollisionEnterDelegate(IGameObject otherGameObject)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherGameObject"></param>
        public override void onCollisionExitDelegate(IGameObject otherGameObject)
        {
            throw new NotSupportedException();
        }

    }
}
