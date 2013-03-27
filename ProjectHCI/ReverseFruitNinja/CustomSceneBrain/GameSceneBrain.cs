using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectHCI.KinectEngine;

namespace ProjectHCI.ReverseFruitNinja
{
    public class GameSceneBrain : SceneBrain
    {
        private Configuration currentConfiguration;

        #region protected int currentScore {public get; set;}

        protected int currentScore;

        public int getCurrentScore()
        {
            return currentScore;
        }

        #endregion

        #region protected int gameLengthMillis {public get;  set;}

        protected int gameLengthMillis;

        public int getGameLengthMillis()
        {
            return gameLengthMillis;
        }
        
        #endregion

        public GameSceneBrain()
        {
            currentConfiguration = Configuration.getCurrentConfiguration();
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

        public override void think(List<KeyValuePair<IGameObject, IGameObject>> collidedGameObjectPairList)
        {
//            SortedSet<IGameObject> collidedFriendlyObjects = new SortedSet<IGameObject>();
            bool playerCut = false;
            List<IGameObject> collidedCutsList = new List<IGameObject>();

            //update 
            gameLengthMillis += Time.getDeltaTimeMillis();


            foreach (KeyValuePair<IGameObject, IGameObject> collidedObjs0 in collidedGameObjectPairList)
            {
                switch (collidedObjs0.Key.getGameObjectTypeEnum())
                {
                    case GameObjectTypeEnum.UserObject:
                        switch (collidedObjs0.Value.getGameObjectTypeEnum())
	                        {
                                case GameObjectTypeEnum.FriendlyObject:
                                    this.bonusCollected((UserFriendlyGameObject)collidedObjs0.Value);
                                    break;

                                case GameObjectTypeEnum.UnfriendlyObject:
                                    playerCut = true;
                                    if (!collidedCutsList.Contains(collidedObjs0.Value))
                                    {
                                        collidedCutsList.Add(collidedObjs0.Value);
                                    }
                                    break;
	                        }
                        break;
                    case GameObjectTypeEnum.FriendlyObject:
                        switch (collidedObjs0.Value.getGameObjectTypeEnum())
	                    {
                            case GameObjectTypeEnum.UserObject:
                                this.bonusCollected((UserFriendlyGameObject)collidedObjs0.Key);
                                break;

                            case GameObjectTypeEnum.UnfriendlyObject:
                                this.bonusDead((UserFriendlyGameObject)collidedObjs0.Key);
                                break;
	                    }
                        break;
                    case GameObjectTypeEnum.UnfriendlyObject:
                        switch (collidedObjs0.Value.getGameObjectTypeEnum())
	                    {
                            case GameObjectTypeEnum.UserObject :
                                playerCut = true;
                                if (!collidedCutsList.Contains(collidedObjs0.Key))
                                {
                                    collidedCutsList.Add(collidedObjs0.Key);
                                }
                                break;

                            case GameObjectTypeEnum.FriendlyObject :
                                this.bonusDead((UserFriendlyGameObject)collidedObjs0.Value);
                                break;
	                    }
                        break;
                }
            }
            
            //if user has been hit invoke appropriate trigger
            if (playerCut)
            {
                this.userDeadTrigger(collidedCutsList);
            }
              

            //lastly clean dead objects
            base.think(collidedGameObjectPairList);
        }

        protected void bonusCollected(UserFriendlyGameObject collectedBonusObject)
        {
            this.currentScore += collectedBonusObject.collectionTrigger();
        }

        protected void bonusDead(UserFriendlyGameObject deadBonusObject)
        {
            this.currentScore += deadBonusObject.cutTrigger();
        }

        protected void userDeadTrigger(List<IGameObject> collidedCuts)
        {
            ISceneManager sceneManager = GameLoop.getSceneManager();
            List<IGameObject> objsToRemove = new List<IGameObject>();

            if (sceneManager.getGameObjectListMapByTypeEnum()[GameObjectTypeEnum.UserObject].Count == 1)
            {
                //remove all object from scene, except the cuts which killed the user
                foreach (KeyValuePair<GameObjectTypeEnum, List<IGameObject>> gameObjectList0 in sceneManager.getGameObjectListMapByTypeEnum())
                {
                    //we must remove all visible objects from scene
                    foreach (IGameObject gameObject00 in gameObjectList0.Value)
                    {
                        //invoke the correct trigger for each object
                        switch (gameObjectList0.Key)
                        {
                            case GameObjectTypeEnum.UserObject:
                                ((HeadUserGameObject)gameObject00).cutTrigger();
                                goto default;   //fallback in default case

                            case GameObjectTypeEnum.UnfriendlyObject:
                                if (collidedCuts.Contains(gameObject00))
                                {
                                    ((NotUserFriendlyGameObject)gameObject00).cutUserTrigger();
                                }
                                goto default;   //fallback in default case

                            default:
                                //can't remove now: collection in use
                                objsToRemove.Add(gameObject00);
                                break;
                        }
                    }       //end inner foreach
                }       //end outer foreach
            }
            else    //multiplayer! :-)
            {
                //just put user as cut, without stopping anything else
                foreach (IGameObject gameObject0 in sceneManager.getGameObjectListMapByTypeEnum()[GameObjectTypeEnum.UserObject])
                {
                    ((HeadUserGameObject)gameObject0).cutTrigger();
                }
            }

            ////remove objs
            //foreach (IGameObject gameObject0 in objsToRemove)
            //{
            //    sceneManager.removeGameObject(gameObject0);
            //}

            //TODO switch to endgame scene
        }
    }
}
