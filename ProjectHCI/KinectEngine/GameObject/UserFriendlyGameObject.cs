﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Controls;
using System.Diagnostics;

namespace ProjectHCI.KinectEngine
{
    public class UserFriendlyGameObject : GameObjectBase
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="imageSource"></param>
        /// <param name="timeToLiveMillis"></param>
        public UserFriendlyGameObject(Geometry geometry,
                                      ImageSource imageSource,
                                      int timeToLiveMillis)
        {
            Debug.Assert(geometry != null, "expected geometry != null");
            Debug.Assert(imageSource != null, "expected imageSource != null");

            base._timeToLiveMillis = timeToLiveMillis;
            base._currentTimeToLiveMillis = timeToLiveMillis;
            base._geometry = geometry;
            base._imageSource = imageSource;
            base._uid = base.generateUid();
            base._objectType = GameObjectTypeEnum.FriendlyObject;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool isCollidable()
        {
            return base._currentTimeToLiveMillis >= 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool isDead()
        {
            return base._currentTimeToLiveMillis <= 0;
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="mainWindowCanvas"></param>
        //public override void onRendererUpdateDelegate()
        //{
        //    //TODO maybe bouncing icons...
        //}

        
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
