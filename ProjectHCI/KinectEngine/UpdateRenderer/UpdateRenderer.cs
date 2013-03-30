using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Diagnostics;


namespace ProjectHCI.KinectEngine
{
    public class UpdateRenderer : IUpdateRenderer
    {

        private Dictionary<String, List<IGameObject>> prevFrameGameObjectListMapByTag;


        /// <summary>
        /// 
        /// </summary>
        public UpdateRenderer()
        {
            this.prevFrameGameObjectListMapByTag = new Dictionary<String, List<IGameObject>>();
        }



        /// <summary>
        /// 
        /// </summary>
        public void drawObject()
        {

            Dictionary<String, List<IGameObject>> gameObjectListMapByTag = GameLoop.getSceneManager().getGameObjectListMapByTag();
            Debug.Assert(gameObjectListMapByTag != null, "expected gameObjectListMapByType != null");


            foreach (KeyValuePair<String, List<IGameObject>> gameObjectListMapByTag0 in gameObjectListMapByTag)
            {


                String gameObjectTag0 = gameObjectListMapByTag0.Key;
                List<IGameObject> gameObjectList0 = gameObjectListMapByTag0.Value;

                if (this.prevFrameGameObjectListMapByTag.ContainsKey(gameObjectTag0))
                {
                    
                    List<IGameObject> prevGameObjectList0 = this.prevFrameGameObjectListMapByTag[gameObjectTag0];


                    //we use List and not directly the Enumerable type because changing the list, witch their are based on, raise an exception.
                    List<IGameObject> newGameObjectEnumerable0 = gameObjectList0.Except(prevGameObjectList0).ToList<IGameObject>();
                    List<IGameObject> deadGameObjecEnumerable0 = prevGameObjectList0.Except(gameObjectList0).ToList<IGameObject>();
                    List<IGameObject> updatableGameObjectEnumerable0 = prevGameObjectList0.Intersect(gameObjectList0).ToList<IGameObject>();

                  

                    //display new gameObject to GUI
                    foreach (IGameObject newGameObject00 in newGameObjectEnumerable0)
                    {
                        newGameObject00.onRendererDisplayDelegate();

                        //prepare the structure for the next frame
                        this.prevFrameGameObjectListMapByTag[gameObjectTag0].Add(newGameObject00);
                    }

                    //remove deadGameObject from GUI
                    foreach (IGameObject deadGameObject00 in deadGameObjecEnumerable0)
                    {
                        deadGameObject00.onRendererRemoveDelegate();

                        //prepare the structure for the next frame
                        this.prevFrameGameObjectListMapByTag[gameObjectTag0].Remove(deadGameObject00);
                    }

                    //update gameObject in the GUI
                    foreach (IGameObject updatableGameObject00 in updatableGameObjectEnumerable0)
                    {
                        updatableGameObject00.onRendererUpdateDelegate();

                    }



                }
                else
                {

                    this.prevFrameGameObjectListMapByTag.Add(gameObjectTag0, new List<IGameObject>(20));

                    //display new gameObject to GUI
                    foreach (IGameObject gameObject00 in gameObjectList0)
                    {
                        gameObject00.onRendererDisplayDelegate();

                        //prepare the structure for the next frame
                        this.prevFrameGameObjectListMapByTag[gameObjectTag0].Add(gameObject00);
                    }
                }
            }



        }

    }
}
