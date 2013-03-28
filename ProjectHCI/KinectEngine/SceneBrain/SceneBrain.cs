using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;


namespace ProjectHCI.KinectEngine
{
    public class SceneBrain : ISceneBrain
    {

        //private int maxNumberOfChopAllowed;
        //private int maxNumberOfUserFriendlyGameObjectAllowed;


        /**
         * Ok, that's exaperating things.
         
        #region public int chopSpawnCooldownMillis {get; private set;}

        private int minChopSpawnCooldownMillis;

        public int getMinChopSpawnCooldownMillis()
        {
            return minChopSpawnCooldownMillis;
        }

        private int maxChopSpawnCooldownMillis;

        public int getMaxChopSpawnCooldownMillis()
        {
            return maxChopSpawnCooldownMillis;
        }

        #endregion

        #region public int friendlyObjectSpawnCooldownMillis {get; private set;}

        private int minFriendlyObjectSpawnCooldownMillis;

        public int getMinFriendlyObjectSpawnCooldownMillis()
        {
            return minFriendlyObjectSpawnCooldownMillis;
        }

        private int maxFriendlyObjectSpawnCooldownMillis;

        public int getMaxFriendlyObjectSpawnCooldownMillis()
        {
            return maxFriendlyObjectSpawnCooldownMillis;
        }

        #endregion

        private float bonusPercentiege;
         */ 

        //it's better to encapsulate the current configuration directly
        private Configuration currentConfiguration;

        /// <summary>
        /// 
        /// </summary>
        public SceneBrain()
        {
            #region Old Commented Code
            /* 
            this.maxNumberOfChopAllowed = 0;
            this.maxNumberOfUserFriendlyGameObjectAllowed = 0;
            this.bonusPercentiege = 0.0f;
            */		 
	        #endregion

            currentConfiguration = Configuration.getCurrentConfiguration();

            #region code before de-exasperating refactoring
            /*
            this.maxNumberOfChopAllowed = configuration.getMaxNumOfChopsAllowed();
            this.maxNumberOfUserFriendlyGameObjectAllowed = configuration.getMaxNumOfFriendlyObjectsAllowed();
            this.chopSpawnCooldownMillis = configuration.getChopSpawnCooldownTimeMillis();
            this.friendlyObjectSpawnCooldownMillis = configuration.getFriendlyObjectSpawnCooldownTimeMillis();
            */

            /*
            this.maxNumberOfChopAllowed = configuration.maxNumOfChopsAllowed;
            this.maxNumberOfUserFriendlyGameObjectAllowed = configuration.maxNumOfFriendlyObjectsAllowed;
            this.chopSpawnCooldownMillis = configuration.chopSpawnCooldownTimeMillis;
            this.friendlyObjectSpawnCooldownMillis = configuration.friendlyObjectSpawnCooldownTimeMillis;
            */
            #endregion
        }


        

        /// <summary>
        /// 
        /// </summary>
        public void cleanDeadGameObject()
        {

            ISceneManager sceneManager = GameLoop.getSceneManager();

            List<IGameObject> deadGameObjectList = new List<IGameObject>(20);
            foreach (KeyValuePair<String, List<IGameObject>> gameObjectListMapByTag0 in sceneManager.getGameObjectListMapByTag())
            {
                foreach (IGameObject gameObject00 in gameObjectListMapByTag0.Value)
                {
                    if (gameObject00.isDead())
                    {
                        deadGameObjectList.Add(gameObject00);
                    }
                }
            }


            foreach (IGameObject gameObject0 in deadGameObjectList)
            {
                sceneManager.removeGameObject(gameObject0);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        public void think(List<KeyValuePair<IGameObject, IGameObject>> collidedGameObjectPairList)
        {
            //ISceneManager sceneManager = GameLoop.getSceneManager();



            //Time.getDeltaTimeMillis();
            //Time.getTotalTimeMillis();

            

            //***** Fake implementation, these parameters should be bound to the totaltime...
            /*
            Random random = new Random();
            this.maxNumberOfChopAllowed = random.Next(1, 5);


            Dictionary<GameObjectTypeEnum, List<IGameObject>> allGameObjectListMapByType = sceneManager.getGameObjectListMapByTypeEnum();

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

            //this.bonusPercentiege = 0.0f;
             */ 
            //******


            this.cleanDeadGameObject();
            

        }



        /// <summary>
        /// This returns the maximum number of chops
        /// </summary>
        /// <returns> The maximum number of chops that can exist simultaneously into the scene </returns>
        public int getMaxNumberOfChopAllowed()
        {
            //return this.maxNumberOfChopAllowed;
            return currentConfiguration.maxNumOfChopsAllowed;
        }


        /// <summary>
        /// This returns the maximum number of active friendly objects
        /// </summary>
        /// <returns> The maximum number of friendly objects that can exist simultaneously into the scene </returns>
        /// <remarks> This is backed by the configuration class</remarks>
        public int getMaxNumberOfUserFriendlyGameObjectAllowed()
        {
            //return this.maxNumberOfUserFriendlyGameObjectAllowed;
            return currentConfiguration.maxNumOfFriendlyObjectsAllowed;
        }


        /// <summary>
        /// This method should be removed. Seriously.
        /// </summary>
        /// <returns></returns>
        public float getBonusPercentiege()
        {
            //return this.bonusPercentiege;
            return 0;
        }


    }
}
