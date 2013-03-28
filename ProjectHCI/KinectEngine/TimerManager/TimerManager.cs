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


            Dictionary<String, List<IGameObject>> allGameObjectListMapByTag = sceneManager.getGameObjectListMapByTag(); 
            Debug.Assert(allGameObjectListMapByTag != null, "expected allGameObjectListMapByType != null");

            foreach (KeyValuePair<String, List<IGameObject>> gameObjectListMapByTag0 in allGameObjectListMapByTag)
            {
                foreach (IGameObject gameObject00 in gameObjectListMapByTag0.Value.ToList()) //ToList used as a copy 
                {

                    gameObject00.update(Time.getDeltaTimeMillis());

                    //an object can be removed by another gameObject00.update (the line above) e.g. an object decides to remove all children in its update step.
                    bool gameObjectIsStillPresent = sceneManager.getGameObjectListMapByTag()[gameObject00.getGameObjectTag()].Contains(gameObject00);

                    if (gameObject00.isCollidable()
                        && gameObjectIsStillPresent
                        && sceneManager.isGameObjectDisplayed(gameObject00)
                        && !sceneManager.getCollaidableGameObjectList(gameObject00.getGameObjectTag()).Contains(gameObject00)){
    

                        sceneManager.promoteToCollidableGameObject(gameObject00);

                    }

                }
            }
        }




    }
}
