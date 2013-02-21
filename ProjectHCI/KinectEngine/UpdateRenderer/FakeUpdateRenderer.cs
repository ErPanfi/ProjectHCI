﻿using System;
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


                    //we use List and not direclty the Enumerable type because changing the list, wich their are based on, raise an exception.
                    List<IGameObject> newGameObjectEnumerable0 = gameObjectList0.Except(prevGameObjectList0).ToList<IGameObject>();
                    List<IGameObject> deadGameObjecEnumerable0 = prevGameObjectList0.Except(gameObjectList0).ToList<IGameObject>();
                    List<IGameObject> updatableGameObjectEnumerable0 = prevGameObjectList0.Intersect(gameObjectList0).ToList<IGameObject>();

                  

                    //diplay new gameObject to GUI
                    foreach (IGameObject newGameObject00 in newGameObjectEnumerable0)
                    {
                        if (displayGameObjectEventHandler != null)
                        {
                            displayGameObjectEventHandler(this, new GameObjectEventArgs(newGameObject00));
                        }
                        //prepare the structure for the next frame
                        this.prevFrameGameObjectListMapByType[gameObjectType0].Add(newGameObject00);
                    }

                    //remove deadGameObject from GUI
                    foreach (IGameObject deadGameObject00 in deadGameObjecEnumerable0)
                    {
                        if (removeGameObjectEventHandler != null)
                        {
                            removeGameObjectEventHandler(this, new GameObjectEventArgs(deadGameObject00));
                        }
                        //prepare the structure for the next frame
                        this.prevFrameGameObjectListMapByType[gameObjectType0].Remove(deadGameObject00);
                    }

                    //update gameObject in the GUI
                    foreach (IGameObject updatableGameObject00 in updatableGameObjectEnumerable0)
                    {
                        if (updateGameObjectEventHandler != null)
                        {
                            updateGameObjectEventHandler(this, new GameObjectEventArgs(updatableGameObject00));
                        }
                    }



                }
                else
                {

                    this.prevFrameGameObjectListMapByType.Add(gameObjectType0, new List<IGameObject>(20));

                    //diplay new gameObject to GUI
                    foreach (IGameObject gameObject00 in gameObjectList0)
                    {
                        if (displayGameObjectEventHandler != null)
                        {
                            displayGameObjectEventHandler(this, new GameObjectEventArgs(gameObject00));
                        }
                        //prepare the structure for the next frame
                        this.prevFrameGameObjectListMapByType[gameObjectType0].Add(gameObject00);
                    }
                }
            }



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
