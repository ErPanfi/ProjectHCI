using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Collections;
using System.Collections.Concurrent;
using ProjectHCI.KinectEngine;
using ProjectHCI.Utility;

namespace ProjectHCI.ReverseFruitNinja
{
    /// <summary>
    /// This class implements the spawner mechanism of the game.
    /// In order to generate a constant object flow the spawner must check the objects currently present in the scene and, if needed, create & add a new object to it.
    /// </summary>
    public class GameSpawnerManager : SpawnerManager
    {

        private Random random;

        private const double VERTICAL_LABEL_SPACE = 10;
        private const double HORIZ_RAGE_IMG_SPACE = 10;

        private int lastFriendlyObjSpawned, friendlyObjCooldown;
        private int lastUnfriendlyObjSpawned, unfriendlyObjCooldown;
        private Configuration currentConfiguration;

        private GameSceneBrain gameSceneBrain;

        #region accelerating chop spawning rate code

        private const double MIN_CHOP_SPAWN_OFFSET_INC_PER_MILLIS = 0.3;
        public double minChopSpawnCooldownMillis;

        private const double MAX_CHOP_SPAWN_OFFSET_INC_PER_MILLIS = 0.6;
        public double maxChopSpawnCooldownMillis;

        protected void updateSpawnOffsets(int deltaTimeMillis)
        {
            if (this.minChopSpawnCooldownMillis > 0)
            {
                this.minChopSpawnCooldownMillis -= deltaTimeMillis * MIN_CHOP_SPAWN_OFFSET_INC_PER_MILLIS;
                if (this.minChopSpawnCooldownMillis < 0)
                {
                    this.minChopSpawnCooldownMillis = 0;
                }
            }

            if (this.maxChopSpawnCooldownMillis > this.minChopSpawnCooldownMillis)
            {
                this.maxChopSpawnCooldownMillis -= deltaTimeMillis * MAX_CHOP_SPAWN_OFFSET_INC_PER_MILLIS;
                if (this.maxChopSpawnCooldownMillis < this.minChopSpawnCooldownMillis)
                {
                    this.maxChopSpawnCooldownMillis = this.minChopSpawnCooldownMillis;
                }
            }
        }

        protected int generateNewChopCooldown()
        {
            if(this.maxChopSpawnCooldownMillis == 0)
                return 0;

            int min = (int)Math.Round(this.minChopSpawnCooldownMillis);
            int max = (int)Math.Round(this.maxChopSpawnCooldownMillis);

            return random.Next(min, max);
        }

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="sceneBrain">The active scene brain must be passed,in order to allow the spawner to reference it</param>
        public GameSpawnerManager(GameSceneBrain gameSceneBrain) : base()
        {
            this.random = new Random();

            currentConfiguration = Configuration.getCurrentConfiguration();

            this.gameSceneBrain = gameSceneBrain;
            this.minChopSpawnCooldownMillis = this.gameSceneBrain.getMinCutCooldown();
            this.maxChopSpawnCooldownMillis = this.gameSceneBrain.getMaxCutCooldown();
            this.unfriendlyObjCooldown = this.generateNewChopCooldown();
            this.lastUnfriendlyObjSpawned = unfriendlyObjCooldown;
            this.friendlyObjCooldown = random.Next(currentConfiguration.minFriendlyObjectSpawnCooldownTimeMillis, currentConfiguration.maxFriendlyObjectSpawnCoooldownTimeMillis);
            this.lastFriendlyObjSpawned = 0;
        }

        #region commented code
        //private void obtainGameSceneBrain()
        //{
        //    if (this.gameSceneBrain == null)
        //    {
        //        Debug.Assert(typeof(GameSceneBrain).IsAssignableFrom(GameLoop.getSceneBrain().GetType()), "Expected a GameSceneBrain object");
        //        this.gameSceneBrain = (GameSceneBrain)GameLoop.getSceneBrain();
        //    }
        //}        
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override List<KeyValuePair<IGameObject, IGameObject>> spawnGameObjectsOnStart()
        {

            //userGameObject spawn

            List<KeyValuePair<IGameObject, IGameObject>> gameObjectParentGameObjectPairList = new List<KeyValuePair<IGameObject, IGameObject>>();



            IGameObject gameObject = this.spawnUserGameObject();

            gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, null));

#if DEBUG
            gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(gameObject), gameObject));            
#endif

            //label spawn
            //Debug.Assert(typeof(GameSceneBrain).IsAssignableFrom(GameLoop.getSceneBrain().GetType()), "Expected a GameSceneBrain object");
            //GameSceneBrain sceneBrain = (GameSceneBrain)GameLoop.getSceneBrain();
            
            gameObject = new GameLengthLabelObject(HORIZ_RAGE_IMG_SPACE, VERTICAL_LABEL_SPACE, gameSceneBrain);
            gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, null));

            gameObject = new GameScoreLabelObject(HORIZ_RAGE_IMG_SPACE, (2 * VERTICAL_LABEL_SPACE) + gameObject.getBoundingBoxGeometry().Bounds.Height, gameSceneBrain);
            gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, null));

            //this label will be spawned with special request
            //gameObject = new GameStartCountdownLabelObject(GameLoop.getSceneManager().getCanvasWidth() / 2 - 30, GameLoop.getSceneManager().getCanvasHeight() / 2 - 30, gameSceneBrain);
            //gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, null));

            //debug cooldown label objs
            gameObject = new DEBUG_LabelObject(10, (3 * VERTICAL_LABEL_SPACE) + (2 * gameObject.getBoundingBoxGeometry().Bounds.Height));
            gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, null));

            //rage icons spawn
            ISceneManager sceneManager = GameLoop.getSceneManager();
            gameObject = new RageItemGUIGameObject(gameSceneBrain, 1, sceneManager.getCanvasWidth() - (4 * HORIZ_RAGE_IMG_SPACE + 3 * RageItemGUIGameObject.IMAGE_WIDTH), VERTICAL_LABEL_SPACE);
            gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, null));
            gameObject = new RageItemGUIGameObject(gameSceneBrain, 2, sceneManager.getCanvasWidth() - (3 * HORIZ_RAGE_IMG_SPACE + 2 * RageItemGUIGameObject.IMAGE_WIDTH), VERTICAL_LABEL_SPACE);
            gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, null));
            gameObject = new RageItemGUIGameObject(gameSceneBrain, 3, sceneManager.getCanvasWidth() - (2 * HORIZ_RAGE_IMG_SPACE + 1 * RageItemGUIGameObject.IMAGE_WIDTH), VERTICAL_LABEL_SPACE);
            gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, null));

            return gameObjectParentGameObjectPairList;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override List<KeyValuePair<IGameObject, IGameObject>> spawnGameObjectsPerFrame()
        {
            //spawn objs only if game start countdown is expired
            //Debug.Assert(typeof(GameSceneBrain).IsAssignableFrom(GameLoop.getSceneBrain().GetType()), "Expected a GameSceneBrain object");
            //GameSceneBrain sceneBrain = (GameSceneBrain)GameLoop.getSceneBrain();
            List<KeyValuePair<IGameObject, IGameObject>> gameObjectParentGameObjectPairList = new List<KeyValuePair<IGameObject, IGameObject>>();

            if (gameSceneBrain.getGameStartCountdownMillis() <= 0)
            {

                int deltaTimeMillis = Time.getDeltaTimeMillis();

                this.updateSpawnOffsets(deltaTimeMillis);

                ISceneManager sceneManager = GameLoop.getSceneManager();

                Dictionary<String, List<IGameObject>> gameObjectListMapByType = sceneManager.getGameObjectListMapByTag();
                //userGameObjectList
                Debug.Assert(gameObjectListMapByType.ContainsKey(Tags.USER_TAG), "the userGameObject must be created.");
                List<IGameObject> userGameObjectList = userGameObjectList = gameObjectListMapByType[Tags.USER_TAG];
                Debug.Assert(userGameObjectList.Count > 0, "expected userGameObjectList.Count > 0");

                //friendlyObjectList
                List<IGameObject> friendlyObjectList;
                if (gameObjectListMapByType.ContainsKey(Tags.FRUIT_TAG))
                {
                    friendlyObjectList = gameObjectListMapByType[Tags.FRUIT_TAG];
                }
                else
                {
                    friendlyObjectList = new List<IGameObject>();
                }

                //unfriendlyObjectList
                List<IGameObject> unfriendlyObjectList;
                if (gameObjectListMapByType.ContainsKey(Tags.CUT_TAG))
                {
                    unfriendlyObjectList = gameObjectListMapByType[Tags.CUT_TAG];
                }
                else
                {
                    unfriendlyObjectList = new List<IGameObject>();
                }

                //spawn a new unfriendly obj
                if (this.shouldSpawnNewCutGameObject(unfriendlyObjectList.Count, gameSceneBrain.getMaxNumberOfChopAllowed(), this.unfriendlyObjCooldown, this.lastUnfriendlyObjSpawned))
                {
                    IGameObject gameObject = this.spawnNewCutGameObject(userGameObjectList, friendlyObjectList);

                    gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, null));

                    this.unfriendlyObjCooldown = this.generateNewChopCooldown();
                    this.lastUnfriendlyObjSpawned = 0;
#if DEBUG
                    gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(gameObject), gameObject));
#endif

                }
                else
                {
                    this.lastUnfriendlyObjSpawned += deltaTimeMillis;
                }

                //spawn new friendly obj, with potentially different probability distribution
                if (this.shouldSpawnNewFruitGameObject(friendlyObjectList.Count, gameSceneBrain.getMaxNumberOfUserFriendlyGameObjectAllowed(), this.friendlyObjCooldown, this.lastFriendlyObjSpawned))
                {
                    IGameObject gameObject = this.spawnNewFruitGameObject(friendlyObjectList);

                    gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, null));
                    this.friendlyObjCooldown = random.Next(currentConfiguration.minFriendlyObjectSpawnCooldownTimeMillis, currentConfiguration.maxFriendlyObjectSpawnCoooldownTimeMillis);
                    this.lastFriendlyObjSpawned = 0;
#if DEBUG
                    gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(gameObject), gameObject));
#endif
                }
                else
                {
                    this.lastFriendlyObjSpawned += deltaTimeMillis;
                }
            }
            return gameObjectParentGameObjectPairList;
        }


       


        /// <summary> 
        /// This method evaluate if a new friendly object should be created
        /// </summary>
        /// <param name="presentObjectsNum">The number of current non-dead friendly objects in the scene.</param>
        /// <param name="maxObjectsNum">The maximum number of non-dead friendly objects allowed in the scene.</param>
        /// <returns>Returns true iff a new friendly object must be spawned.</returns>
        /// <remarks>Only one object per cycle should spawn</remarks>
        protected bool shouldSpawnNewFruitGameObject(int presentGameObjectsNum, int maxGameObjectsNum, int objectSpawnCooldown, int elapsedCooldown)
        {
            //currently use the same probability function of unfriendly objs, left duped method for any diversion
            //it's more unlikely to spawn something if there's already many objs in the screen
            return (
                        (presentGameObjectsNum < maxGameObjectsNum)                                     //if there's already the maximum obj num short-circuit next condition
                    &&  (random.Next(maxGameObjectsNum) < (maxGameObjectsNum - presentGameObjectsNum))  //probability decreasing with increasing obj num, note that this lead to certain object spawning if presentObjectsNum == 0
                    && (objectSpawnCooldown == 0 || random.Next(objectSpawnCooldown) < elapsedCooldown) //probability increasing as cooldown is reached
                    );
        }



        /// <summary> 
        /// This method evaluate if a new unfriendly gameObject should be created
        /// </summary>
        /// <param name="presentGameObjectsNum">The number of current non-dead gameObjects of a specific type in the scene.</param>
        /// <param name="maxGameObjectsNum">The maximum number of non-dead gameObjects of a specific type  allowed in the scene.</param>
        /// <returns>Returns true iff a new gameObject of a specific type must be spawned.</returns>
        /// <remarks>Only one object per cycle should spawn</remarks>
        protected bool shouldSpawnNewCutGameObject(int presentGameObjectsNum, int maxGameObjectsNum, int objectSpawnCooldown, int elapsedCooldown)
        {
            //it's more unlikely to spawn something if there's already many objs in the screen
            
            return (
                        (presentGameObjectsNum < maxGameObjectsNum)                                     //if there's already the maximum obj num short-circuit next condition
                    &&  (random.Next(maxGameObjectsNum) < (maxGameObjectsNum - presentGameObjectsNum))  //probability decreasing with increasing obj num, note that this lead to certain object spawning if presentObjectsNum == 0
                    &&  (
                            objectSpawnCooldown == 0
                         //|| this.gameSceneBrain.getRageLevel() == GameSceneBrain.MAX_RAGE_LEVEL
                         || random.Next(objectSpawnCooldown) < elapsedCooldown
                        ) //probability increasing as cooldown is reached
                   );
            
            //explicit passages for debug
            //if (presentGameObjectsNum < maxGameObjectsNum)
            //{
            //    int ran = random.Next(maxGameObjectsNum);
            //    if (ran < (maxGameObjectsNum - presentGameObjectsNum))
            //    {
            //        ran = random.Next(objectSpawnCooldown);
            //        if (ran < elapsedCooldown)
            //        {
            //            return true;
            //        }
            //    }
            //}

            //return false;
        }


        /// <summary>
        /// Builds a new friendly object
        /// </summary>
        /// <param name="presentObjs">The currently existing non-dead friendly objects (users objects excluded)</param>
        /// <returns>The newly created friendly object</returns>
        protected IGameObject spawnNewFruitGameObject(List<IGameObject> presentObjs)
        {


            Image image = new Image();
            image.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"fruit" + random.Next(1, 15) +  @".png")));
            image.Height = 120;
            image.Width = 120;

            Geometry boundingBoxGeometry = new EllipseGeometry(new Point(60, 60), 40, 40);


            int xPosition = random.Next(0, (int)GameLoop.getSceneManager().getCanvasWidth());
            int yPosition = random.Next(0, (int)GameLoop.getSceneManager().getCanvasHeight());


            return new FruitGameObject(xPosition, yPosition, boundingBoxGeometry, image, random.Next(4000, 6000));
        }



        /// <summary>
        /// Builds a new unfriendly object
        /// </summary>
        /// <param name="friendlyGameObjectList">The non-dead friendly objects currently existing in the scene</param>
        /// <param name="userGameObjectList">The non-dead user objects currently existing in the scene</param>
        /// <returns>The newly created unfriendly object</returns>
        protected IGameObject spawnNewCutGameObject(List<IGameObject> userGameObjectList, List<IGameObject> friendlyGameObjectList)
        {

            //choose the target of the cut
            bool targetTheUser = friendlyGameObjectList.Count == 0 || random.NextDouble() < currentConfiguration.tryToCutPlayerProbability;

            IGameObject targetGameObject = this.extractRandomGameObjectFromList(targetTheUser ? userGameObjectList : friendlyGameObjectList);
            Debug.Assert(targetGameObject != null, "expected targetGameObject != null");


            //locate the cut center point
            Geometry targetBoundingBoxGeometry = targetGameObject.getBoundingBoxGeometry();
            Point targetBoundingBoxCenterPoint = new Point(targetBoundingBoxGeometry.Bounds.TopLeft.X + (targetBoundingBoxGeometry.Bounds.Width * 0.5),
                                                           targetBoundingBoxGeometry.Bounds.TopLeft.Y + (targetBoundingBoxGeometry.Bounds.Height * 0.5));



            //inclination is dicided randomly when the cut is display on screen. See NotUserFriendlyGameObject.onRendererDisplayDelegate

            #region oldCode
            //bool cutVertically = random.NextDouble() < 0.5;
            //Image image = new Image();
            //image.Source = new BitmapImage(new Uri(cutVertically ? BitmapUtility.getImgResourcePath(@"slash_vert.png") : BitmapUtility.getImgResourcePath(@"slash_horiz.png")));
            //image.Width = cutVertically ? 25 : random.Next(300, 800);
            //image.Height = cutVertically ? random.Next(300, 800) : 25;
            //image.Stretch = Stretch.Fill;
            //image.StretchDirection = StretchDirection.Both;
            #endregion

            Image image = new Image();
            image.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"cut.png")));
            image.Width = random.Next(300, 800);
            image.Height = 25;
            image.Stretch = Stretch.Fill;
            image.StretchDirection = StretchDirection.Both;


            Geometry boundigBoxGeometry = new EllipseGeometry(new Rect(new Point(0, 0), new Point(image.Width, image.Height)));
            Point gameObjectImageUpperLeftCornerPoint = new Point(targetBoundingBoxCenterPoint.X - (image.Width * 0.5),
                                                                  targetBoundingBoxCenterPoint.Y - (image.Height * 0.5));

            //int timeToLive = random.Next(currentConfiguration.minChopLifetimeMillis, currentConfiguration.maxChopLifetimeMillis);
            //lifetime is now influenced by ragelevel
            int timeToLive = random.Next(gameSceneBrain.getMinCutLifetime(), gameSceneBrain.getMaxCutLifetime());
            return new CutGameObject(gameObjectImageUpperLeftCornerPoint.X,
                                                 gameObjectImageUpperLeftCornerPoint.Y,
                                                 boundigBoxGeometry,
                                                 image,
                                                 timeToLive,
                                                 (int)(timeToLive * 0.8)
                                                 );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected IGameObject spawnUserGameObject()
        {

            Image image = new Image();
            image.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(currentConfiguration.userFruitImage)));
            image.Height = 160;
            image.Width = 160;

            Geometry boundingBoxGeometry = new EllipseGeometry(new Point(80, 80), 50, 50);

            double halfCanvasWidth = GameLoop.getSceneManager().getCanvasWidth() * 0.5;
            double halfCanvasHeight = GameLoop.getSceneManager().getCanvasHeight() * 0.5;



            IGameObject userGameObject = null;

            switch (this.currentConfiguration.userControlMethod)
            {
                case Configuration.UserControlMethodEnum.Head:
                    {
                        Image notAlreadyTracketImage = new Image();
                        notAlreadyTracketImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"move.png")));
                        notAlreadyTracketImage.Height = 129;
                        notAlreadyTracketImage.Width = 600;

                        userGameObject = new HeadUserGameObject(0, 0, boundingBoxGeometry, notAlreadyTracketImage, image, SkeletonSmoothingFilter.MEDIUM_SMOOTHING_LEVEL);
                    }
                    break;


                case Configuration.UserControlMethodEnum.Hand:
                    {
                        Image notAlreadyTracketImage = new Image();
                        notAlreadyTracketImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"wave.png")));
                        notAlreadyTracketImage.Height = 129;
                        notAlreadyTracketImage.Width = 600;

                        userGameObject = new HandUserGameObject(0, 0, boundingBoxGeometry, notAlreadyTracketImage, image, SkeletonSmoothingFilter.MEDIUM_SMOOTHING_LEVEL);
                    }
                    break;

                default:
                    throw new Exception("unexpected UserControlMethodEnum");
            }


            return userGameObject;
        }





        /// <summary>
        /// Extract a random game object from a given list of game objects
        /// </summary>
        /// <param name="targetGameObjectList">The list of game objects</param>
        /// <returns>One of the items of the list</returns>
        protected IGameObject extractRandomGameObjectFromList(List<IGameObject> targetGameObjectList)
        {
            Debug.Assert(targetGameObjectList.Count > 0, "expected targetGameObjectList.Count > 0");

            int gameObjectIndex = random.Next(targetGameObjectList.Count);
            return targetGameObjectList[gameObjectIndex];

        }

        ///// <summary>
        ///// Extract a random game object from a given list of game objects
        ///// </summary>
        ///// <param name="targetGameObjectList">The list of game objects</param>
        ///// <returns>One of the items of the list or null if the random extraction has failed</returns>
        //protected IGameObject extractRandomGameObjectFromList(List<IGameObject> targetGameObjectList)
        //{
        //    int x = random.Next(targetGameObjectList.Count) + 1;
        //    List<IGameObject>.Enumerator enumerator = targetGameObjectList.GetEnumerator();
        //    bool hasNext = false;
        //    for (int y = 0; y < x; y++)
        //    {
        //        hasNext = enumerator.MoveNext();
        //    }

        //    if (hasNext)
        //    {
        //        return enumerator.Current;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}


    }
}
