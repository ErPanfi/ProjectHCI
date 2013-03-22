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
                foreach (IGameObject gameObject00 in gameObjectListMapByTypeEntry0.Value.ToList()) //ToList used as a copy 
                {

                    gameObject00.update(Time.getDeltaTimeMillis());

                    //an object can be removed by another gameObject00.update e.g. an object decides to remove all children in its update step.
                    bool gameObjectIsStillPresent = sceneManager.getGameObjectListMapByTypeEnum()[gameObject00.getGameObjectTypeEnum()].Contains(gameObject00);

                    if (gameObject00.isCollidable()
                        && gameObjectIsStillPresent
                        && !sceneManager.getCollaidableGameObjectList(gameObject00.getGameObjectTypeEnum()).Contains(gameObject00)){
    

                        sceneManager.promoteToCollidableGameObject(gameObject00);

                    }

                }
            }
        }




    }
}
