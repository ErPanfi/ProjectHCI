using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace ProjectHCI.KinectEngine
{
    class SpawnerTestForCollisionManager:ISpawnerManager
    {
        private bool userSpawned;
        private Random random;
        private const double TRY_TO_CUT_PLAYERS_PROBABILITY = 0.6;

        public SpawnerTestForCollisionManager()
        {
            this.userSpawned = false;
            this.random = new Random();
        }

        void ISpawnerManager.awaken()
        {
            ISceneManager sceneManager = GameLoop.getSceneManager();
            ISceneBrain sceneBrain = GameLoop.getSceneBrain();


            if (!this.userSpawned)
            {
                sceneManager.addGameObject(this.spawnUserGameObject());
                this.userSpawned = true;
            }


            //obtain generation parameters and object lists from scene brain
            Dictionary<GameObjectTypeEnum, List<IGameObject>> gameObjListMapByType = sceneManager.getGameObjectListMapByTypeEnum(); //.getAllGameObjectListMapByTypeEnum();

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

            int maxNumberOfChopAllowed = sceneBrain.getMaxNumberOfChopAllowed();
            int maxNumberOfUserFriendlyGameObjectAllowed = sceneBrain.getMaxNumberOfUserFriendlyGameObjectAllowed();
            float bonusPercentiage = sceneBrain.getBonusPercentiege();

            sceneManager.addGameObject(this.spawnNewFriendlyObject(friendlyObjs));

            sceneManager.addGameObject(this.spawnNewUnfriendlyObject(userGameObjs, friendlyObjs));

        }

        protected IGameObject spawnNewUnfriendlyObject(List<IGameObject> userObjs, List<IGameObject> friendlyObjs)
        {
           

            //decide if vertical or horizontal cut
            Geometry cutGeometry;
            ImageSource cutImage;

            if (random.NextDouble() < 0.5)  //horizontal
            {
                cutImage = new BitmapImage(new Uri(@"pack://application:,,,/Resources/slash_horiz.png"));
            }
            else
            {
                cutImage = new BitmapImage(new Uri(@"pack://application:,,,/Resources/slash_vert.png"));
            }
            cutGeometry = new RectangleGeometry(new Rect(random.Next(200, 500),random.Next(200, 500), cutImage.Width, cutImage.Height ));
            cutGeometry.Freeze();
            cutImage.Freeze();

            return new NotUserFriendlyGameObject(cutGeometry, cutImage, random.Next(100, 10000));
        }

        protected IGameObject spawnUserGameObject()
        {

            ImageSource imageSource = new BitmapImage(new Uri(@"pack://application:,,,/Resources/shark.png"));
            Geometry geometry = new EllipseGeometry(new Point(300, 300), 100, 100);
            imageSource.Freeze();

            return new UserGameObject(geometry, imageSource, SkeletonSmoothingFilter.MEDIUM_SMOOTHING_LEVEL);
        }

        /// <summary>
        /// Builds a new friendly object
        /// </summary>
        /// <param name="presentObjs">The currently existing non-dead friendly objects (users objects excluded)</param>
        /// <returns>The newly created friendly object</returns>
        protected IGameObject spawnNewFriendlyObject(List<IGameObject> presentObjs)
        {
            ImageSource imageSource = new BitmapImage(new Uri(@"pack://application:,,,/Resources/skype.png"));

            int canvasWidth = (int)GameLoop.getSceneManager().getTargetCanvas().RenderSize.Width;
            int canvasHeight = (int)GameLoop.getSceneManager().getTargetCanvas().RenderSize.Height;

            Geometry geometry = new EllipseGeometry(new Point(random.Next(200, 500), random.Next(200, 500)), 50, 50);
            geometry.Freeze();
            imageSource.Freeze();

            return new UserFriendlyGameObject(geometry, imageSource, random.Next(100, 100000));
        }
    }
}
