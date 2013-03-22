using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;

namespace ProjectHCI.KinectEngine
{
    /// <summary>
    /// This class implements the spawner mechanism of the game.
    /// In order to generate a constant object flow the spawner must check the objects currently present in the scene and, if needed, create & add a new object to it.
    /// </summary>
    public class GameSpawnerManager : ISpawnerManager
    {

        private Random random;

        private const double TRY_TO_CUT_PLAYERS_PROBABILITY = 0.6;
        private bool userSpawned;
        private List<IGameObject> spawnedGameObjectList;


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="sceneBrain">The active scene brain must be passed,in order to allow the spawner to reference it</param>
        public GameSpawnerManager()
        {
            this.userSpawned = false;

            this.random = new Random();
        }

        public List<IGameObject> getSpawnedObjects()
        {
            return new List<IGameObject>(spawnedGameObjectList);
        }

        /// <summary>
        /// This method, when called, awake the spawner.
        /// The spawner, then, check if a new object (user friendly or not) must be added to the scene and, if necessary, create the appropriate object and submit it to the scene manager.
        /// </summary>
        public void awaken()
        {
            ISceneManager sceneManager = GameLoop.getSceneManager();
            ISceneBrain sceneBrain = GameLoop.getSceneBrain();

            spawnedGameObjectList = new List<IGameObject>();

            if(!this.userSpawned)
            {
                IGameObject gameObject = this.spawnUserGameObject();
                sceneManager.addGameObject(gameObject, null);
#if DEBUG
                sceneManager.addGameObject(new BoundingBoxViewrGameObject(gameObject), gameObject);
#endif

                spawnedGameObjectList.Add(gameObject);
                this.userSpawned = true;
            }





            
            Dictionary<GameObjectTypeEnum, List<IGameObject>> gameObjectListMapByType = sceneManager.getGameObjectListMapByTypeEnum();



            //userGameObjectList
            Debug.Assert(gameObjectListMapByType.ContainsKey(GameObjectTypeEnum.UserObject), "the userGameObject must be created.");
            List<IGameObject> userGameObjectList = userGameObjectList = gameObjectListMapByType[GameObjectTypeEnum.UserObject];
            Debug.Assert(userGameObjectList.Count > 0, "expected userGameObjectList.Count > 0");




            //friendlyObjectList
            List<IGameObject> friendlyObjectList;
            if (gameObjectListMapByType.ContainsKey(GameObjectTypeEnum.FriendlyObject))
            {
                friendlyObjectList = gameObjectListMapByType[GameObjectTypeEnum.FriendlyObject];
            }
            else
            {
                friendlyObjectList = new List<IGameObject>();
            }



            //unfriendlyObjectList
            List<IGameObject> unfriendlyObjectList;
            if (gameObjectListMapByType.ContainsKey(GameObjectTypeEnum.UnfriendlyObject))
            {
                unfriendlyObjectList = gameObjectListMapByType[GameObjectTypeEnum.UnfriendlyObject];
            }
            else
            {
                unfriendlyObjectList = new List<IGameObject>();
            }



            //obtain generation parameters from scene brain
            int maxNumberOfChopAllowed = sceneBrain.getMaxNumberOfChopAllowed();
            int maxNumberOfUserFriendlyGameObjectAllowed = sceneBrain.getMaxNumberOfUserFriendlyGameObjectAllowed();
            float bonusPercentiage = sceneBrain.getBonusPercentiege();




            //spawn a new unfriendly obj
            if (this.shouldSpawnNewGameObject(unfriendlyObjectList.Count, maxNumberOfChopAllowed))
            {
                IGameObject gameObject = this.spawnNewUnfriendlyObject(userGameObjectList, friendlyObjectList);
                spawnedGameObjectList.Add(gameObject);
                sceneManager.addGameObject(gameObject, null);
#if DEBUG
                sceneManager.addGameObject(new BoundingBoxViewrGameObject(gameObject), gameObject);
#endif
                //sceneManager.applyRotation(gameObject,
                //                            10,
                //                            gameObject.getXPosition() - gameObject.getImage().Width * 0.5,
                //                            gameObject.getYPosition() - gameObject.getImage().Height * 0.5);
            }

            //spawn new friendly obj
            if (this.shouldSpawnNewGameObject(friendlyObjectList.Count, maxNumberOfUserFriendlyGameObjectAllowed))
            {
                IGameObject gameObject = this.spawnNewFriendlyObject(friendlyObjectList);
                spawnedGameObjectList.Add(gameObject);
                sceneManager.addGameObject(gameObject, null);
#if DEBUG
                sceneManager.addGameObject(new BoundingBoxViewrGameObject(gameObject), gameObject);
#endif
            }
        }



        ///// <summary> 
        ///// This method evaluate if a new friendly object should be created
        ///// </summary>
        ///// <param name="presentObjectsNum">The number of current non-dead friendly objects in the scene.</param>
        ///// <param name="maxObjectsNum">The maximum number of non-dead friendly objects allowed in the scene.</param>
        ///// <returns>Returns true iff a new friendly object must be spawned.</returns>
        ///// <remarks>Only one object per cycle should spawn</remarks>
        //protected bool shouldSpawnNewFriendlyObject(int presentObjectsNum, int maxObjectsNum)
        //{
        //    //currently use the same probability function of unfriendly objs
        //    return this.shouldSpawnNewGameObject(presentObjectsNum, maxObjectsNum);            
        //}



        /// <summary> 
        /// This method evaluate if a new gameObject should be created
        /// </summary>
        /// <param name="presentGameObjectsNum">The number of current non-dead gameObjects of a specific type in the scene.</param>
        /// <param name="maxGameObjectsNum">The maximum number of non-dead gameObjects of a specific type  allowed in the scene.</param>
        /// <returns>Returns true iff a new gameObject of a specific type must be spawned.</returns>
        /// <remarks>Only one object per cycle should spawn</remarks>
        protected bool shouldSpawnNewGameObject(int presentGameObjectsNum, int maxGameObjectsNum)
        {
            //it's more unlikely to spawn something if there's already many objs in the screen
            return (
                        (presentGameObjectsNum < maxGameObjectsNum)                                   //if there's already the maximum obj num short-circuit next condition
                    &&  (random.Next(maxGameObjectsNum) < (maxGameObjectsNum - presentGameObjectsNum))   //note that this lead to certain object spawning if presentObjectsNum == 0
                   );
        }


        /// <summary>
        /// Builds a new friendly object
        /// </summary>
        /// <param name="presentObjs">The currently existing non-dead friendly objects (users objects excluded)</param>
        /// <returns>The newly created friendly object</returns>
        protected IGameObject spawnNewFriendlyObject(List<IGameObject> presentObjs)
        {


            Image image = new Image();
            image.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/skype.png"));
            image.Height = 100;
            image.Width = 100;

            Geometry boundingBoxGeometry = new EllipseGeometry(new Rect(new Point(0, 0), new Point(100, 100)));


            int xPosition = random.Next(0, (int) GameLoop.getSceneManager().getCanvasWidth());
            int yPosition = random.Next(0, (int) GameLoop.getSceneManager().getCanvasHeight());


            return new UserFriendlyGameObject(xPosition, yPosition, boundingBoxGeometry, image, random.Next(4000, 6000));
        }



        /// <summary>
        /// Builds a new unfriendly object
        /// </summary>
        /// <param name="friendlyGameObjectList">The non-dead friendly objects currently existing in the scene</param>
        /// <param name="userGameObjectList">The non-dead user objects currently existing in the scene</param>
        /// <returns>The newly created unfriendly object</returns>
        protected IGameObject spawnNewUnfriendlyObject(List<IGameObject> userGameObjectList, List<IGameObject> friendlyGameObjectList)
        {
            
            //choose the target of the cut
            bool targetTheUser = friendlyGameObjectList.Count == 0 || random.NextDouble() < TRY_TO_CUT_PLAYERS_PROBABILITY;

            IGameObject targetGameObject = this.extractRandomGameObjectFromList(targetTheUser ? userGameObjectList : friendlyGameObjectList);
            Debug.Assert(targetGameObject != null, "expected targetGameObject != null");
            

            //locate the cut center point
            Geometry targetBoundingBoxGeometry = targetGameObject.getBoundingBoxGeometry();
            Point targetBoundingBoxCenterPoint = new Point(targetBoundingBoxGeometry.Bounds.TopLeft.X + (targetBoundingBoxGeometry.Bounds.Width * 0.5), 
                                                           targetBoundingBoxGeometry.Bounds.TopLeft.Y + (targetBoundingBoxGeometry.Bounds.Height * 0.5));




            //TODO refine and implement randomly inclined cut
            bool cutVertically = random.NextDouble() < 0.5;

            Image image = new Image();
            image.Source = new BitmapImage(new Uri(cutVertically ? @"pack://application:,,,/Resources/slash_vert.png" : @"pack://application:,,,/Resources/slash_horiz.png"));
            image.Width = cutVertically ? 25 : random.Next(300, 800);
            image.Height = cutVertically ? random.Next(300, 800) : 25;
            image.Stretch = Stretch.Fill;
            image.StretchDirection = StretchDirection.Both;

            
            Geometry boundigBoxGeometry = new EllipseGeometry(new Rect(new Point(0, 0), new Point(image.Width, image.Height)));
            Point gameObjectImageUpperLeftCornerPoint = new Point(targetBoundingBoxCenterPoint.X - (image.Width * 0.5), 
                                                                  targetBoundingBoxCenterPoint.Y - (image.Height * 0.5));
           
            int timeToLive = random.Next(4000, 6000);
            return new NotUserFriendlyGameObject(gameObjectImageUpperLeftCornerPoint.X,
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
            image.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/shark.png"));
            image.Height = 200;
            image.Width = 200;

            Geometry boundingBoxGeometry = new EllipseGeometry(new Point(100,110), 50, 50);

            double halfCanvasWidth = GameLoop.getSceneManager().getCanvasWidth() * 0.5;
            double halfCanvasHeight = GameLoop.getSceneManager().getCanvasHeight() * 0.5;

            return new HeadUserGameObject(halfCanvasWidth - image.Width * 0.5,
                                      halfCanvasHeight - image.Height * 0.5, 
                                      boundingBoxGeometry, 
                                      image, 
                                      SkeletonSmoothingFilter.MEDIUM_SMOOTHING_LEVEL);
            
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
