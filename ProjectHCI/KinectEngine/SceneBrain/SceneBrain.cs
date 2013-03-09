using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;


namespace ProjectHCI.KinectEngine
{
    public class SceneBrain : ISceneBrain
    {


        private int maxNumberOfChopAllowed;
        private int maxNumberOfUserFriendlyGameObjectAllowed;
        private float bonusPercentiege;

        

        /// <summary>
        /// 
        /// </summary>
        public SceneBrain()
        {

            this.maxNumberOfChopAllowed = 0;
            this.maxNumberOfUserFriendlyGameObjectAllowed = 0;
            this.bonusPercentiege = 0.0f;


        }


        

        /// <summary>
        /// 
        /// </summary>
        public void cleanDeadGameObject()
        {

            ISceneManager sceneManager = GameLoop.getSceneManager();

            List<IGameObject> deadGameObjectList = new List<IGameObject>(20);
            foreach (KeyValuePair<GameObjectTypeEnum, List<IGameObject>> gameObjectListMapByTypeEntry0 in sceneManager.getGameObjectListMapByTypeEnum())
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
                sceneManager.removeGameObject(gameObject0);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        public void think(List<KeyValuePair<IGameObject, IGameObject>> collidedGameObjectPairList)
        {
            ISceneManager sceneManager = GameLoop.getSceneManager();



            //Time.getDeltaTimeMillis();
            //Time.getTotalTimeMillis();

            

            //***** Fake implementation, these parameters should be bound to the totaltime...
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

            this.bonusPercentiege = 0.0f;
            //******


            this.cleanDeadGameObject();
            

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


    }
}
