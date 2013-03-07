﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace ProjectHCI.KinectEngine
{
    public class TimerManager : ITimerManager
    {
        private ISceneBrain sceneBrain;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sceneBrain"></param>
        public TimerManager(ISceneBrain sceneBrain)
        {
            this.sceneBrain = sceneBrain;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTimeMillis"></param>
        public void tick(int deltaTimeMillis)
        {
            Dictionary<GameObjectTypeEnum, List<IGameObject>> allGameObjectListMapByType = this.sceneBrain.getAllGameObjectListMapByTypeEnum();
            Debug.Assert(allGameObjectListMapByType != null, "expected allGameObjectListMapByType != null");

            foreach (KeyValuePair<GameObjectTypeEnum, List<IGameObject>> gameObjectListMapByTypeEntry0 in allGameObjectListMapByType)
            {
                foreach (IGameObject gameObject00 in gameObjectListMapByTypeEntry0.Value)
                {
                    gameObject00.updateTimeToLive(deltaTimeMillis);
                                       

                    if (gameObject00.isCollidable() && !this.sceneBrain.getCollaidableGameObjectList(gameObject00.getObjectTypeEnum()).Contains(gameObject00))
                    {
                        this.sceneBrain.addCollidableGameObject(gameObject00);
                    }

                }
            }
        }




    }
}
