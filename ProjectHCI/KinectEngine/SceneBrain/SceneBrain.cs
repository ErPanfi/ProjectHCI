﻿using System;
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


        public SceneBrain()
        {
            this.sceneManager = new SceneBrain.SceneManager();

            this.maxNumberOfChopAllowed = 0;
            this.maxNumberOfUserFriendlyGameObjectAllowed = 0;
            this.bonusPercentiege = 0.0f;

            this.gameTotalTimeMillis = 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        public void addGameObject(IGameObject gameObject)
        {
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
        /// <param name="gameObjectClassType"></param>
        /// <returns></returns>
        public List<IGameObject> getCollaidableGameObjectList(Type gameObjectClassType)
        {
            return this.sceneManager.getCollidableGameObjectListMapByType()[gameObjectClassType];
        }



        /// <summary>
        /// 
        /// </summary>
        public void cleanDeadGameObject()
        {
            List<IGameObject> deadGameObjectList = new List<IGameObject>(20);
            foreach (KeyValuePair<Type, List<IGameObject>> gameObjectListMapByTypeEntry0 in this.sceneManager.getGameObjectListMapByType())
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
            }
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
            this.maxNumberOfUserFriendlyGameObjectAllowed = random.Next(1, 5);
            this.bonusPercentiege = this.maxNumberOfChopAllowed % this.maxNumberOfUserFriendlyGameObjectAllowed;
            //******


            this.cleanDeadGameObject();
            

        }



        public Dictionary<Type, List<IGameObject>> getAllGameObjectListMapByType()
        {
            return this.sceneManager.getGameObjectListMapByType();
        }

        public Dictionary<Type, List<IGameObject>> getCollidableGameObjectListMapByType()
        {
            return this.sceneManager.getCollidableGameObjectListMapByType();
        }

        public int getMaxNumberOfChopAllowed()
        {
            return this.maxNumberOfChopAllowed;
        }

        public int getMaxNumberOfUserFriendlyGameObjectAllowed()
        {
            return this.maxNumberOfUserFriendlyGameObjectAllowed;
        }

        public float getBonusPercentiege()
        {
            return this.bonusPercentiege;
        }





        private class SceneManager
        {

            private Dictionary<Type, List<IGameObject>> gameObjectListMapByType;
            private Dictionary<Type, List<IGameObject>> collidableGameObjectListMapByType;


            public SceneManager()
            {
                this.gameObjectListMapByType = new Dictionary<Type, List<IGameObject>>(5);
                this.collidableGameObjectListMapByType = new Dictionary<Type, List<IGameObject>>(5);
            }


            private void registerGameObject(IGameObject gameObject, Dictionary<Type, List<IGameObject>> targetGameObjectListMapByType)
            {
                Debug.Assert(gameObject != null, "expected gameObject not null.");
                Debug.Assert(targetGameObjectListMapByType != null, "expected targetGameObjectListMapByType not null");

                Type gameObjectType = gameObject.GetType();


                if (!targetGameObjectListMapByType.ContainsKey(gameObjectType))
                {
                    targetGameObjectListMapByType.Add(gameObjectType, new List<IGameObject>(20));
                }
                targetGameObjectListMapByType[gameObjectType].Add(gameObject);

            }


            private void deleteGameObject(IGameObject gameObject, Dictionary<Type, List<IGameObject>> targetGameObjectListMapByType)
            {
                Debug.Assert(gameObject != null, "expected gameObject not null.");
                Debug.Assert(targetGameObjectListMapByType != null, "expected targetGameObjectListMapByType not null");



                Type gameObjectType = gameObject.GetType();

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



            public Dictionary<Type, List<IGameObject>> getGameObjectListMapByType()
            {
                return this.gameObjectListMapByType;
            }

            public Dictionary<Type, List<IGameObject>> getCollidableGameObjectListMapByType()
            {
                return this.collidableGameObjectListMapByType;
            }

        }


    }
}
