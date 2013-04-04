using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectHCI.KinectEngine;

namespace ProjectHCI.ReverseFruitNinja
{
    public class GameSceneBrain : SceneBrain, IGameStateTracker
    {
        private Configuration currentConfiguration;

        #region protected int currentScore {public get; set;}

        protected int currentScore;

        public int getGameScore()
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

        #region protected int gameStartCountdownMillis {public get; set;}

        protected int gameStartCountdownMillis;

        public int getGameStartCountdownMillis()
        {
            return gameStartCountdownMillis;
        }
        #endregion

        #region protected int rageLevel {public get; set;}

        public const int MAX_RAGE_LEVEL = 3;

        protected int ragePoints;

        public int getRagePoints()
        {
            return ragePoints;
        }

        public void incRage()
        {
            this.ragePoints++;
        }

        public void decRage()
        {
            this.ragePoints--;
        }

        public int getRageLevel()
        {
            int ret = this.ragePoints / currentConfiguration.rageLevelIncrement;

            if (ret < 0)
            {
                return 0;
            }
            else if (ret > MAX_RAGE_LEVEL)
            {
                return MAX_RAGE_LEVEL;
            }
            else
            {
                return ret;
            }
        
        }

        #endregion

        private bool userTracked;

        public GameSceneBrain()
        {
            this.currentConfiguration = Configuration.getCurrentConfiguration();
            this.gameStartCountdownMillis = currentConfiguration.gameStartCountdownMillis;
            this.userTracked = false;
        }

        /// <summary>
        /// This returns the maximum number of chops
        /// </summary>
        /// <returns> The maximum number of chops that can exist simultaneously into the scene </returns>
        /// <remarks> this can be adapted by the rage level and/or the difficulty level</remarks>
        public int getMaxNumberOfChopAllowed()
        {
            double gameDiffFactor = 1;
            double rageLevelFactor = 1;

            switch (currentConfiguration.gameDifficulty)
            {
                case Configuration.GameDifficultyEnum.Easy:
                    gameDiffFactor = 0.5;
                    break;
                case Configuration.GameDifficultyEnum.Medium:
                    gameDiffFactor = 0.8;
                    break;
            }

            switch (this.getRageLevel())
            {
                case 0: 
                    rageLevelFactor = 0.4; 
                    break;
                case 1:  
                    rageLevelFactor = 0.6; 
                    break;
                case 2:  
                    rageLevelFactor = 0.8; 
                    break;
            }

            return (int)(currentConfiguration.maxNumOfChopsAllowed * (gameDiffFactor * rageLevelFactor));
        }


        public int getMinCutLifetime()
        {
            double gameDiffFactor = 1;
            double rageLevelFactor = 1;

            switch (currentConfiguration.gameDifficulty)
            {
                case Configuration.GameDifficultyEnum.Hard:
                    gameDiffFactor = 0.5;
                    break;
                case Configuration.GameDifficultyEnum.Medium:
                    gameDiffFactor = 0.8;
                    break;
            }

            switch (this.getRageLevel())
            {
                case 2:
                    rageLevelFactor = 0.75;
                    break;
                case 3:
                    rageLevelFactor = 0.5;
                    break;
            }

            return (int)(currentConfiguration.minChopLifetimeMillis * (gameDiffFactor * rageLevelFactor));
        }

        public int getMaxCutLifetime()
        {
            double gameDiffFactor = 1;
            double rageLevelFactor = 1;

            switch (currentConfiguration.gameDifficulty)
            {
                case Configuration.GameDifficultyEnum.Hard:
                    gameDiffFactor = 0.5;
                    break;
                case Configuration.GameDifficultyEnum.Medium:
                    gameDiffFactor = 0.8;
                    break;
            }

            switch (this.getRageLevel())
            {
                case 2:
                    rageLevelFactor = 0.75;
                    break;
                case 3:
                    rageLevelFactor = 0.5;
                    break;
            }

            return (int)(currentConfiguration.maxChopLifetimeMillis * (gameDiffFactor * rageLevelFactor));
        }

        public int getMinCutCooldown()
        {
            double gameDiffFactor = 1;
            double rageLevelFactor = 1;

            switch (currentConfiguration.gameDifficulty)
            {
                case Configuration.GameDifficultyEnum.Hard:
                    gameDiffFactor = 0.5;
                    break;
                case Configuration.GameDifficultyEnum.Medium:
                    gameDiffFactor = 0.8;
                    break;
            }

            switch (this.getRageLevel())
            {
                case 2:
                    rageLevelFactor = 0.75;
                    break;
                case 3:
                    rageLevelFactor = 0.5;
                    break;
            }

            return (int)(currentConfiguration.minChopSpawnCooldownTimeMillis * (gameDiffFactor * rageLevelFactor));
        }

        public int getMaxCutCooldown()
        {
            double gameDiffFactor = 1;
            double rageLevelFactor = 1;

            switch (currentConfiguration.gameDifficulty)
            {
                case Configuration.GameDifficultyEnum.Hard:
                    gameDiffFactor = 0.5;
                    break;
                case Configuration.GameDifficultyEnum.Medium:
                    gameDiffFactor = 0.8;
                    break;
            }

            switch (this.getRageLevel())
            {
                case 2:
                    rageLevelFactor = 0.75;
                    break;
                case 3:
                    rageLevelFactor = 0.5;
                    break;
            }

            return (int)(currentConfiguration.maxChopSpawnCooldownTimeMillis * (gameDiffFactor * rageLevelFactor));
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

            //detect if user has been correctly tracked
            if(!this.userTracked)
            {
                List<IGameObject> userObjsList = GameLoop.getSceneManager().getGameObjectListMapByTag()[Tags.USER_TAG];
                //if there's no user object now wait for it to be spawned
                if(userObjsList.Count > 0)  
                {
                    bool hasBeenTracked = true;
                    foreach(IGameObject gameObject0 in userObjsList)    //all user has to be tracked correctly to start
                    {
                        System.Diagnostics.Debug.Assert(typeof(HeadUserGameObject).IsAssignableFrom(gameObject0.GetType()), "Expected a HeadUserGameObject object");
                        hasBeenTracked = hasBeenTracked && ((HeadUserGameObject)gameObject0).hasBeenTracked();
                    }

                    //tracking succedeed?
                    if(hasBeenTracked)
                    {
                        this.userTracked = true;    //storify this
                        //special request to spawn countdown label
                        GameStartCountdownLabelObject countdownLabel = new GameStartCountdownLabelObject(GameLoop.getSceneManager().getCanvasWidth() / 2 - 30, GameLoop.getSceneManager().getCanvasHeight() / 2 - 30, this);
                        GameLoop.getSpawnerManager().specialRequestToSpawn(new GameObjectSpawnRequest(countdownLabel, null));
                    }
                }
            }

            //update game length
            int delta;

            //if user has not been tracked yet gameStartCountdown won't diminish
            if (this.userTracked)
            {
                delta = Time.getDeltaTimeMillis();
            }
            else
            {
                delta = 0;
            }
            if (this.gameStartCountdownMillis > 0)
            {
                this.gameStartCountdownMillis -= delta;
            }
            else
            {
                this.gameLengthMillis += delta;

                foreach (KeyValuePair<IGameObject, IGameObject> collidedObjs0 in collidedGameObjectPairList)
                {
                    switch (collidedObjs0.Key.getGameObjectTag())
                    {
                        case Tags.USER_TAG:
                            switch (collidedObjs0.Value.getGameObjectTag())
                            {
                                case Tags.FRUIT_TAG:
                                    this.fruitCollected((FruitGameObject)collidedObjs0.Value);
                                    break;

                                case Tags.CUT_TAG:
                                    playerCut = true;
                                    if (!collidedCutsList.Contains(collidedObjs0.Value))
                                    {
                                        collidedCutsList.Add(collidedObjs0.Value);
                                    }
                                    break;
                            }
                            break;


                        case Tags.FRUIT_TAG:
                            switch (collidedObjs0.Value.getGameObjectTag())
                            {
                                case Tags.USER_TAG:
                                    this.fruitCollected((FruitGameObject)collidedObjs0.Key);
                                    break;

                                case Tags.CUT_TAG:
                                    this.fruitDead((FruitGameObject)collidedObjs0.Key);
                                    break;
                            }
                            break;


                        case Tags.CUT_TAG:
                            switch (collidedObjs0.Value.getGameObjectTag())
                            {
                                case Tags.USER_TAG:
                                    playerCut = true;
                                    if (!collidedCutsList.Contains(collidedObjs0.Key))
                                    {
                                        collidedCutsList.Add(collidedObjs0.Key);
                                    }
                                    break;

                                case Tags.FRUIT_TAG:
                                    this.fruitDead((FruitGameObject)collidedObjs0.Value);
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
            }

            //lastly clean dead objects
            base.think(collidedGameObjectPairList);
        }

        protected void fruitCollected(FruitGameObject collectedBonusObject)
        {
            this.currentScore += collectedBonusObject.getFruitCollectionPoints();
        }

        protected void fruitDead(FruitGameObject deadBonusObject)
        {
            this.decRage();
            this.currentScore += deadBonusObject.getFruitDeathPoints();
            if (this.currentScore < 0)
            {
                this.currentScore = 0;
            }
        }

        protected void userDeadTrigger(List<IGameObject> collidedCuts)
        {
            ISceneManager sceneManager = GameLoop.getSceneManager();

            this.decRage();

            //delete all except sound gameObject
            sceneManager.removeGameObjectsByTag(Tags.USER_TAG);
            sceneManager.removeGameObjectsByTag(Tags.CUT_TAG);
            sceneManager.removeGameObjectsByTag(Tags.DEBUG_TAG);
            sceneManager.removeGameObjectsByTag(Tags.FRUIT_TAG);
            sceneManager.removeGameObjectsByTag(Tags.UI_TAG);


            //switch to endgame scene
            GameLoop.getGameLoopSingleton().setSpawnerManager(new GameDebriefingSpawnerManager(this));
            GameLoop.getGameLoopSingleton().setSceneBrain(new SceneBrain());
        }
    }
}
