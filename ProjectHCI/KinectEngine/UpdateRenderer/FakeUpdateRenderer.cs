using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media.Imaging;


namespace ProjectHCI.KinectEngine
{
    public class FakeUpdateRenderer : IUpdateRenderer
    {

        private ISceneBrain sceneBrain;
        private Dictionary<Type, List<IGameObject>> prevFrameGameObjectListMapByType;



        public event EventHandler<GameObjectEventArgs> displayGameObjectEventHandler;
        public event EventHandler<GameObjectEventArgs> updateGameObjectEventHandler;
        public event EventHandler<GameObjectEventArgs> removeGameObjectEventHandler;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sceneBrain"></param>
        public FakeUpdateRenderer(ISceneBrain sceneBrain)
        {
            this.sceneBrain = sceneBrain;
            this.prevFrameGameObjectListMapByType = new Dictionary<Type, List<IGameObject>>();
        }



        /// <summary>
        /// 
        /// </summary>
        public void drawObject()
        {

            Dictionary<Type, List<IGameObject>> gameObjectListMapByType = this.sceneBrain.getAllGameObjectListMapByType();
            
         
            foreach (KeyValuePair<Type, List<IGameObject>> gameObjectListMapByTypeEntry0 in gameObjectListMapByType)
            {


                Type gameObjectType0 = gameObjectListMapByTypeEntry0.Key;
                List<IGameObject> gameObjectList0 = gameObjectListMapByTypeEntry0.Value;

                if (this.prevFrameGameObjectListMapByType.ContainsKey(gameObjectType0))
                {
                    
                    List<IGameObject> prevGameObjectList0 = this.prevFrameGameObjectListMapByType[gameObjectType0];


                    IEnumerable<IGameObject> newGameObjectEnumerable0 = gameObjectList0.Except(prevGameObjectList0);
                    IEnumerable<IGameObject> deadGameObjecEnumerable0 = prevGameObjectList0.Except(gameObjectList0);
                    IEnumerable<IGameObject> updatableGameObjectEnumerable0 = prevGameObjectList0.Intersect(gameObjectList0);


                    foreach (IGameObject newGameObject00 in newGameObjectEnumerable0)
                    {
                        //diplay
                        if (displayGameObjectEventHandler != null)
                        {
                            displayGameObjectEventHandler(this, new GameObjectEventArgs(newGameObject00));
                        }
                    }

                    foreach (IGameObject deadGameObject00 in deadGameObjecEnumerable0)
                    {
                        //remove
                        if (removeGameObjectEventHandler != null)
                        {
                            removeGameObjectEventHandler(this, new GameObjectEventArgs(deadGameObject00));
                        }
                    }

                    foreach (IGameObject updatableGameObject00 in updatableGameObjectEnumerable0)
                    {
                        //update
                        if (updateGameObjectEventHandler != null)
                        {
                            updateGameObjectEventHandler(this, new GameObjectEventArgs(updatableGameObject00));
                        }
                    }



                }
                else
                {
                    
                    //display
                    foreach (IGameObject gameObject00 in gameObjectList0)
                    {
                        if (displayGameObjectEventHandler != null)
                        {
                            displayGameObjectEventHandler(this, new GameObjectEventArgs(gameObject00));
                        }
                    }
                }
            }



            this.prevFrameGameObjectListMapByType = gameObjectListMapByType;

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventHandler"></param>
        public void setDisplayGameObjectEventHandler(EventHandler<GameObjectEventArgs> eventHandler)
        {
            this.displayGameObjectEventHandler += eventHandler;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventHandler"></param>
        public void setUpdateGameObjectEventHandler(EventHandler<GameObjectEventArgs> eventHandler)
        {
            this.updateGameObjectEventHandler += eventHandler;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventHandler"></param>
        public void setRemoveGameObjectEventHandler(EventHandler<GameObjectEventArgs> eventHandler)
        {
            this.removeGameObjectEventHandler += eventHandler;
        }





    }
}
