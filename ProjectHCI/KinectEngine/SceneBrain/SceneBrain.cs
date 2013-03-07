using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;


namespace ProjectHCI.KinectEngine
{
    public class SceneBrain : ISceneBrain
    {

        private SceneBrain.SceneManager sceneManager;

        private int maxNumberOfChopAllowed;
        private int maxNumberOfUserFriendlyGameObjectAllowed;
        private float bonusPercentiege;

        private int gameTotalTimeMillis;


        //private int objectLiveCount;

        /// <summary>
        /// 
        /// </summary>
        public SceneBrain()
        {
            this.sceneManager = new SceneBrain.SceneManager();

            this.maxNumberOfChopAllowed = 0;
            this.maxNumberOfUserFriendlyGameObjectAllowed = 0;
            this.bonusPercentiege = 0.0f;

            this.gameTotalTimeMillis = 0;

            //this.objectLiveCount = 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        public void addGameObject(IGameObject gameObject)
        {

            //System.Diagnostics.Debug.WriteLine("************ add gameObj:" 
            //                                                + " geometryX=" + gameObject.getGeometry().Bounds.X 
            //                                                + " geometryY=" + gameObject.getGeometry().Bounds.Y
            //                                                + " type=" + gameObject.GetType());

            //this.objectLiveCount++;
            this.sceneManager.addGameObject(gameObject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collidableGameObject"></param>
        public void addCollidableGameObject(IGameObject collidableGameObject)
        {
            this.sceneManager.addCollidableGameObject(collidableGameObject);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObjectType"></param>
        /// <returns></returns>
        public List<IGameObject> getCollaidableGameObjectList(GameObjectTypeEnum gameObjectType)
        {
            Dictionary<GameObjectTypeEnum, List<IGameObject>> collidableGameObjectListMapByType = this.sceneManager.getCollidableGameObjectListMapByType();
            if (collidableGameObjectListMapByType.ContainsKey(gameObjectType))
            {
                return this.sceneManager.getCollidableGameObjectListMapByType()[gameObjectType];
            }
            else
            {
                return new List<IGameObject>();
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public void cleanDeadGameObject()
        {
            List<IGameObject> deadGameObjectList = new List<IGameObject>(20);
            foreach (KeyValuePair<GameObjectTypeEnum, List<IGameObject>> gameObjectListMapByTypeEntry0 in this.sceneManager.getGameObjectListMapByType())
            {
                foreach (IGameObject gameObject00 in gameObjectListMapByTypeEntry0.Value)
                {
                    if (gameObject00.isDead())
                    {
                        deadGameObjectList.Add(gameObject00);
                    }
                }
            }


            foreach (IGameObject gameObject0 in deadGameObjectList)
            {
                this.sceneManager.removeGameObject(gameObject0);
                //this.objectLiveCount--;
            }

            //System.Diagnostics.Debug.WriteLine("************ cleaned " + deadGameObjectList.Count + " gameObject");
        }


        /// <summary>
        /// 
        /// </summary>
        public void think(int deltaTimeMillis, List<KeyValuePair<IGameObject, IGameObject>> collidedGameObjectPairList)
        {
            this.gameTotalTimeMillis += deltaTimeMillis;

            

            //***** Fake implementation, these parameters should be bound to the totaltime...
            Random random = new Random();
            this.maxNumberOfChopAllowed = random.Next(1, 5);


            Dictionary<GameObjectTypeEnum, List<IGameObject>> allGameObjectListMapByType = this.getAllGameObjectListMapByType();

            if (allGameObjectListMapByType.ContainsKey(GameObjectTypeEnum.FriendlyObject))
            {
                if (allGameObjectListMapByType[GameObjectTypeEnum.FriendlyObject].Count < 5)
                {
                    this.maxNumberOfUserFriendlyGameObjectAllowed = 5 - allGameObjectListMapByType[GameObjectTypeEnum.FriendlyObject].Count;
                }
                else
                {
                    this.maxNumberOfUserFriendlyGameObjectAllowed = 0;
                }
                
            }
            else
            {
                this.maxNumberOfUserFriendlyGameObjectAllowed = 5;
            }

            this.bonusPercentiege = 0.0f;
            //******


            this.cleanDeadGameObject();
            

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<GameObjectTypeEnum, List<IGameObject>> getAllGameObjectListMapByType()
        {
            return this.sceneManager.getGameObjectListMapByType();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<GameObjectTypeEnum, List<IGameObject>> getCollidableGameObjectListMapByType()
        {
            return this.sceneManager.getCollidableGameObjectListMapByType();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getMaxNumberOfChopAllowed()
        {
            return this.maxNumberOfChopAllowed;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int getMaxNumberOfUserFriendlyGameObjectAllowed()
        {
            return this.maxNumberOfUserFriendlyGameObjectAllowed;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float getBonusPercentiege()
        {
            return this.bonusPercentiege;
        }





        private class SceneManager
        {

            private Dictionary<GameObjectTypeEnum, List<IGameObject>> gameObjectListMapByType;
            private Dictionary<GameObjectTypeEnum, List<IGameObject>> collidableGameObjectListMapByType;


            public SceneManager()
            {
                this.gameObjectListMapByType = new Dictionary<GameObjectTypeEnum, List<IGameObject>>(5);
                this.collidableGameObjectListMapByType = new Dictionary<GameObjectTypeEnum, List<IGameObject>>(5);
            }


            private void registerGameObject(IGameObject gameObject, Dictionary<GameObjectTypeEnum, List<IGameObject>> targetGameObjectListMapByType)
            {
                Debug.Assert(gameObject != null, "expected gameObject not null.");
                Debug.Assert(targetGameObjectListMapByType != null, "expected targetGameObjectListMapByType not null");

                //Type gameObjectType = gameObject.GetType();
                //switched to enum
                GameObjectTypeEnum gameObjectType = gameObject.objectType;


                if (!targetGameObjectListMapByType.ContainsKey(gameObjectType))
                {
                    targetGameObjectListMapByType.Add(gameObjectType, new List<IGameObject>(20));
                }
                targetGameObjectListMapByType[gameObjectType].Add(gameObject);

            }


            private void deleteGameObject(IGameObject gameObject, Dictionary<GameObjectTypeEnum, List<IGameObject>> targetGameObjectListMapByType)
            {
                Debug.Assert(gameObject != null, "expected gameObject not null.");
                Debug.Assert(targetGameObjectListMapByType != null, "expected targetGameObjectListMapByType not null");



                //Type gameObjectType = gameObject.GetType();
                GameObjectTypeEnum gameObjectType = gameObject.objectType;  //switched to enum value

                if (targetGameObjectListMapByType.ContainsKey(gameObjectType)
                        && targetGameObjectListMapByType[gameObjectType].Contains(gameObject))
                {
                    targetGameObjectListMapByType[gameObjectType].Remove(gameObject);
                }
 
            }



            public void addGameObject(IGameObject gameObject)
            {
                this.registerGameObject(gameObject, this.gameObjectListMapByType);
            }

            public void addCollidableGameObject(IGameObject collidableGameObject)
            {
                this.registerGameObject(collidableGameObject, this.collidableGameObjectListMapByType);
            }



            public void removeGameObject(IGameObject gameObject)
            {
                this.deleteGameObject(gameObject, this.gameObjectListMapByType);
                this.deleteGameObject(gameObject, this.collidableGameObjectListMapByType);
            }



            public Dictionary<GameObjectTypeEnum, List<IGameObject>> getGameObjectListMapByType()
            {
                return this.gameObjectListMapByType;
            }

            public Dictionary<GameObjectTypeEnum, List<IGameObject>> getCollidableGameObjectListMapByType()
            {
                return this.collidableGameObjectListMapByType;
            }

        }


    }
}
