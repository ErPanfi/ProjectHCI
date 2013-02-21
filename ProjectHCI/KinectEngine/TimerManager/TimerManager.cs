using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            foreach (KeyValuePair<Type, List<IGameObject>> gameObjectListMapByTypeEntry0 in this.sceneBrain.getAllGameObjectListMapByType())
            {
                foreach (IGameObject gameObject00 in gameObjectListMapByTypeEntry0.Value)
                {
                    gameObject00.updateTimeToLive(deltaTimeMillis);

                    //System.Diagnostics.Debug.WriteLine("************ updateTimer:"
                    //                                        + " geometryX=" + gameObject00.getGeometry().Bounds.X
                    //                                        + " geometryY=" + gameObject00.getGeometry().Bounds.Y
                    //                                        + " currTimeToLive=" + gameObject00.getCurrentTimeToLiveMillis()
                    //                                        + " type=" + gameObject00.GetType());

                    

                    if (gameObject00.isCollidable())
                    {
                        this.sceneBrain.addCollidableGameObject(gameObject00);
                    }

                }
            }
        }




    }
}
