using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace ProjectHCI.KinectEngine
{
    /// <summary>
    /// This class implements the spawner mechanism of the game.
    /// In order to generate a constant object flow the spawner must check the objects currently present in the scene and, if needed, create & add a new object to it.
    /// </summary>
    public class SpawnerManager : ISpawnerManager
    {

        /// <summary>
        /// reference to the scene brain, in order to fetch the current scene data
        /// </summary>
        private ISceneBrain sceneBrain;

        private Random random;

        private const double TRY_TO_CUT_PLAYERS_PROBABILITY = 0.6;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="sceneBrain">The active scene brain must be passed,in order to allow the spawner to reference it</param>
        public SpawnerManager(ISceneBrain sceneBrain)
        {
            this.sceneBrain = sceneBrain;
            random = new Random();
            this.sceneBrain.addGameObject(this.spawnUserGameObject());
        }

        /// <summary>
        /// This method, when called, awake the spawner.
        /// The spawner, then, check if a new object (user friendly or not) must be added to the scene and, if necessary, create the appropriate object and submit it to the scene manager.
        /// </summary>
        public void awaken()
        {

            //obtain generation parameters and object lists from scene brain
            Dictionary<GameObjectTypeEnum, List<IGameObject>> gameObjListMapByType = sceneBrain.getAllGameObjectListMapByTypeEnum();

            List<IGameObject> userGameObjs = null;
            if (gameObjListMapByType.ContainsKey(GameObjectTypeEnum.UserObject))
            {
                userGameObjs = gameObjListMapByType[GameObjectTypeEnum.UserObject];
            }
                

            List<IGameObject> friendlyObjs;
            if (gameObjListMapByType.ContainsKey(GameObjectTypeEnum.FriendlyObject))       //if already present use it
            {
                friendlyObjs = gameObjListMapByType[GameObjectTypeEnum.FriendlyObject];
            }
            else //otherwise create an empty copy
            {
                friendlyObjs = new List<IGameObject>();
            }


            List<IGameObject> unfriendlyObjs;
            if (gameObjListMapByType.ContainsKey(GameObjectTypeEnum.UnfriendlyObject))
            {
                unfriendlyObjs = gameObjListMapByType[GameObjectTypeEnum.UnfriendlyObject];
            }
            else
            {
                unfriendlyObjs = new List<IGameObject>();
            }

            int maxNumberOfChopAllowed = this.sceneBrain.getMaxNumberOfChopAllowed();
            int maxNumberOfUserFriendlyGameObjectAllowed = this.sceneBrain.getMaxNumberOfUserFriendlyGameObjectAllowed();
            float bonusPercentiage = this.sceneBrain.getBonusPercentiege();

            //spawn a new unfriendly obj
            if (this.shouldSpawnNewUnfriendlyObject(unfriendlyObjs.Count, maxNumberOfChopAllowed))
            {
                sceneBrain.addGameObject(this.spawnNewUnfriendlyObject(userGameObjs, friendlyObjs));
            }

            //spawn new friendly obj
            if (this.shouldSpawnNewFriendlyObject(friendlyObjs.Count, maxNumberOfUserFriendlyGameObjectAllowed))
            {
                sceneBrain.addGameObject(this.spawnNewFriendlyObject(friendlyObjs));
            }
        }

        /// <summary> 
        /// This method evaluate if a new friendly object should be created
        /// </summary>
        /// <param name="presentObjectsNum">The number of current non-dead friendly objects in the scene.</param>
        /// <param name="maxObjectsNum">The maximum number of non-dead friendly objects allowed in the scene.</param>
        /// <returns>Returns true iff a new friendly object must be spawned.</returns>
        /// <remarks>Only one object per cycle should spawn</remarks>
        protected bool shouldSpawnNewFriendlyObject(int presentObjectsNum, int maxObjectsNum)
        {
            //currently use the same probability function of unfriendly objs
            return this.shouldSpawnNewUnfriendlyObject(presentObjectsNum, maxObjectsNum);            
        }

        /// <summary> 
        /// This method evaluate if a new unfriendly object should be created
        /// </summary>
        /// <param name="presentObjectsNum">The number of current non-dead unfriendly objects in the scene.</param>
        /// <param name="maxObjectsNum">The maximum number of non-dead unfriendly objects allowed in the scene.</param>
        /// <returns>Returns true iff a new unfriendly object must be spawned.</returns>
        /// <remarks>Only one object per cycle should spawn</remarks>
        protected bool shouldSpawnNewUnfriendlyObject(int presentObjectsNum, int maxObjectsNum)
        {
            //it's more unlikely to spawn something if there's already many objs in the screen
            return (
                        (presentObjectsNum < maxObjectsNum)                                   //if there's already the maximum obj num short-circuit next condition
                    &&  (random.Next(maxObjectsNum) < (maxObjectsNum - presentObjectsNum))   //note that this lead to certain object spawning if presentObjectsNum == 0
                   );
        }


        /// <summary>
        /// Builds a new friendly object
        /// </summary>
        /// <param name="presentObjs">The currently existing non-dead friendly objects (users objects excluded)</param>
        /// <returns>The newly created friendly object</returns>
        protected IGameObject spawnNewFriendlyObject(List<IGameObject> presentObjs)
        {
            ImageSource imageSource = new BitmapImage(new Uri(@"pack://application:,,,/Resources/skype.png"));

            int xPosition = random.Next(0, 800);
            int yPosition = random.Next(0, 800);
            
            Geometry geometry = new EllipseGeometry(new Point(xPosition, yPosition), 50, 50);
            geometry.Freeze();

            imageSource.Freeze();

            return new UserFriendlyGameObject(geometry, imageSource, random.Next(100, 5000));
        }

        /// <summary>
        /// Builds a new unfriendly object
        /// </summary>
        /// <param name="friendlyObjs">The non-dead friendly objects currently existing in the scene</param>
        /// <param name="userObjs">The non-dead user objects currently existing in the scene</param>
        /// <returns>The newly created unfriendly object</returns>
        protected IGameObject spawnNewUnfriendlyObject(List<IGameObject> userObjs, List<IGameObject> friendlyObjs)
        {
            //choose the target of the cut
            IGameObject targetObject = this.extractRandomObjFromList((friendlyObjs.Count == 0 || random.NextDouble() < TRY_TO_CUT_PLAYERS_PROBABILITY) ? userObjs : friendlyObjs);

            //locate the cut center point
            Rect bounds = new Rect();

            switch(targetObject.getObjectTypeEnum())
            {
                case GameObjectTypeEnum.FriendlyObject :
                    if (targetObject.getGeometry().IsFrozen)    //freely accessible without dispatcher
                    {
                        bounds = targetObject.getGeometry().Bounds;
                    }
                    else //otherwise fall through next case
                    {
                        goto case GameObjectTypeEnum.UserObject;
                    }

                    break;

                case GameObjectTypeEnum.UserObject :
                    //use dispatcher to access object geometry
                    targetObject.getGeometry().Dispatcher.Invoke((Action)(() =>
                    {
                        bounds = targetObject.getGeometry().Bounds;
                    }));

                break;

                default:
                    throw new Exception("Unexpected objectType");
            }
            Point targetCenter = new Point(bounds.TopLeft.X + (bounds.Width / 2), bounds.TopLeft.Y + (bounds.Height / 2));

            //decide if vertical or horizontal cut
            Geometry cutGeometry;
            ImageSource cutImage;
            //TODO refine and implement randomly inclined and scaled cuts
            if (random.NextDouble() < 0.5)  //horizontal
            {
                cutImage = new BitmapImage(new Uri(@"pack://application:,,,/Resources/slash_horiz.png"));
            }
            else
            {
                cutImage = new BitmapImage(new Uri(@"pack://application:,,,/Resources/slash_vert.png"));
            }

            cutGeometry = new RectangleGeometry(new Rect(
                                                            new Point(targetCenter.X - (cutImage.Width / 2), targetCenter.Y - (cutImage.Height / 2)), //bottom left corner
                                                            new Point(targetCenter.X + (cutImage.Width / 2), targetCenter.Y + (cutImage.Height / 2))  //top right corner
                                                )       );
            cutGeometry.Freeze();
            cutImage.Freeze();

            return new NotUserFriendlyGameObject(cutGeometry, cutImage, random.Next(100, 5000));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected IGameObject spawnUserGameObject()
        {

            ImageSource imageSource = new BitmapImage(new Uri(@"pack://application:,,,/Resources/shark.png"));
            Geometry geometry = new EllipseGeometry(new Point(100, 100), 100, 100);
            imageSource.Freeze();

            return new UserGameObject(geometry, imageSource, SkeletonSmoothingFilter.MEDIUM_SMOOTHING_LEVEL);
        }
        
        /// <summary>
        /// Extract a random game object from a given list of game objects
        /// </summary>
        /// <param name="targetList">The list of game objects</param>
        /// <returns>One of the items of the list or null if the random extraction has failed</returns>
        protected IGameObject extractRandomObjFromList(List<IGameObject> targetList)
        {
            int x = random.Next(targetList.Count) + 1;
            List<IGameObject>.Enumerator enumerator = targetList.GetEnumerator();
            bool hasNext = false;
            for (int y = 0; y < x; y++)
            {
                hasNext = enumerator.MoveNext();
            }

            if (hasNext)
            {
                return enumerator.Current;
            }
            else
            {
                return null;
            }
        }

    }
}
