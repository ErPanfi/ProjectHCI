using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace ProjectHCI.KinectEngine
{
    public class TimerManager : ITimerManager
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sceneBrain"></param>
        public TimerManager()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTimeMillis"></param>
        public void tick()
        {

            ISceneManager sceneManager = GameLoop.getSceneManager();


            Dictionary<GameObjectTypeEnum, List<IGameObject>> allGameObjectListMapByType = sceneManager.getGameObjectListMapByTypeEnum(); 
            Debug.Assert(allGameObjectListMapByType != null, "expected allGameObjectListMapByType != null");

            foreach (KeyValuePair<GameObjectTypeEnum, List<IGameObject>> gameObjectListMapByTypeEntry0 in allGameObjectListMapByType)
            {
                foreach (IGameObject gameObject00 in gameObjectListMapByTypeEntry0.Value)
                {
                    gameObject00.updateTimeToLive(Time.getDeltaTimeMillis());


                    Dictionary<GameObjectTypeEnum, List<IGameObject>> collidableGameObjectListMapByType00 = sceneManager.getCollidableGameObjectListMapByTypeEnum();

                    

                    if (gameObject00.isCollidable()
                        && !sceneManager.getCollaidableGameObjectList(gameObject00.getObjectTypeEnum()).Contains(gameObject00)){
    
                        sceneManager.registerAsCollidableGameObject(gameObject00);

                    }

                }
            }
        }




    }
}
