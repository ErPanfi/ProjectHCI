using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectHCI.KinectEngine
{
    public class TimerManager : ITimerManager
    {
        private ISceneBrain sceneBrain;



        public TimerManager(ISceneBrain sceneBrain)
        {
            this.sceneBrain = sceneBrain;
        }


        public void tick(int deltaTimeMillis)
        {

            foreach (KeyValuePair<Type, List<IGameObject>> gameObjectListMapByTypeEntry0 in this.sceneBrain.getAllGameObjectListMapByType())
            {
                foreach (IGameObject gameObject00 in gameObjectListMapByTypeEntry0.Value)
                {

                    gameObject00.updateTimeToLive(deltaTimeMillis);

                    if (gameObject00.isCollidable())
                    {
                        this.sceneBrain.addCollidableGameObject(gameObject00);
                    }

                }
            }
        }




    }
}
