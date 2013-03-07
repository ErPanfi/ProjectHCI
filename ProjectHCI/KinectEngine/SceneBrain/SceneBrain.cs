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
        /// <param name="gameObjectTypeEnum"></param>
        /// <returns></returns>
        public List<IGameObject> getCollaidableGameObjectList(GameObjectTypeEnum gameObjectTypeEnum)
        {
            Dictionary<GameObjectTypeEnum, List<IGameObject>> collidableGameObjectListMapByTypeEnum = this.sceneManager.getCollidableGameObjectListMapByTypeEnum();
            if (collidableGameObjectListMapByTypeEnum.ContainsKey(gameObjectTypeEnum))
            {
                return this.sceneManager.getCollidableGameObjectListMapByTypeEnum()[gameObjectTypeEnum];
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
            foreach (KeyValuePair<GameObjectTypeEnum, List<IGameObject>> gameObjectListMapByTypeEntry0 in this.sceneManager.getGameObjectListMapByTypeEnum())
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


            Dictionary<GameObjectTypeEnum, List<IGameObject>> allGameObjectListMapByType = this.getAllGameObjectListMapByTypeEnum();

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
        public Dictionary<GameObjectTypeEnum, List<IGameObject>> getAllGameObjectListMapByTypeEnum()
        {
            return this.sceneManager.getGameObjectListMapByTypeEnum();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<GameObjectTypeEnum, List<IGameObject>> getCollidableGameObjectListMapByTypeEnum()
        {
            return this.sceneManager.getCollidableGameObjectListMapByTypeEnum();
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

            private Dictionary<GameObjectTypeEnum, List<IGameObject>> gameObjectListMapByTypeEnum;
            private Dictionary<GameObjectTypeEnum, List<IGameObject>> collidableGameObjectListMapByTypeEnum;


            public SceneManager()
            {
                this.gameObjectListMapByTypeEnum = new Dictionary<GameObjectTypeEnum, List<IGameObject>>(5);
                this.collidableGameObjectListMapByTypeEnum = new Dictionary<GameObjectTypeEnum, List<IGameObject>>(5);
            }


            private void registerGameObject(IGameObject gameObject, Dictionary<GameObjectTypeEnum, List<IGameObject>> targetGameObjectListMapByTypeEnum)
            {
                Debug.Assert(gameObject != null, "expected gameObject not null.");
                Debug.Assert(targetGameObjectListMapByTypeEnum != null, "expected targetGameObjectListMapByTypeEnum not null");

                //Type gameObjectType = gameObject.GetType();
                //switched to enum
                GameObjectTypeEnum gameObjectTypeEnum = gameObject.getObjectTypeEnum();


                if (!targetGameObjectListMapByTypeEnum.ContainsKey(gameObjectTypeEnum))
                {
                    targetGameObjectListMapByTypeEnum.Add(gameObjectTypeEnum, new List<IGameObject>(20));
                }
                targetGameObjectListMapByTypeEnum[gameObjectTypeEnum].Add(gameObject);

            }


            private void deleteGameObject(IGameObject gameObject, Dictionary<GameObjectTypeEnum, List<IGameObject>> targetGameObjectListMapByTypeEnum)
            {
                Debug.Assert(gameObject != null, "expected gameObject not null.");
                Debug.Assert(targetGameObjectListMapByTypeEnum != null, "expected targetGameObjectListMapByTypeEnum not null");



                //Type gameObjectType = gameObject.GetType();
                GameObjectTypeEnum gameObjectTypeEnum = gameObject.getObjectTypeEnum();  //switched to enum value

                if (targetGameObjectListMapByTypeEnum.ContainsKey(gameObjectTypeEnum)
                        && targetGameObjectListMapByTypeEnum[gameObjectTypeEnum].Contains(gameObject))
                {
                    targetGameObjectListMapByTypeEnum[gameObjectTypeEnum].Remove(gameObject);
                }
 
            }



            public void addGameObject(IGameObject gameObject)
            {
                this.registerGameObject(gameObject, this.gameObjectListMapByTypeEnum);
            }

            public void addCollidableGameObject(IGameObject collidableGameObject)
            {
                this.registerGameObject(collidableGameObject, this.collidableGameObjectListMapByTypeEnum);
            }



            public void removeGameObject(IGameObject gameObject)
            {
                this.deleteGameObject(gameObject, this.gameObjectListMapByTypeEnum);
                this.deleteGameObject(gameObject, this.collidableGameObjectListMapByTypeEnum);
            }



            public Dictionary<GameObjectTypeEnum, List<IGameObject>> getGameObjectListMapByTypeEnum()
            {
                return this.gameObjectListMapByTypeEnum;
            }

            public Dictionary<GameObjectTypeEnum, List<IGameObject>> getCollidableGameObjectListMapByTypeEnum()
            {
                return this.collidableGameObjectListMapByTypeEnum;
            }

        }


    }
}
