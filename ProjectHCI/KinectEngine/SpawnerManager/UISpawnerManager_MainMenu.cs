using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Diagnostics;

namespace ProjectHCI.KinectEngine
{
    public class UISpawnerManager_MainMenu : ISpawnerManager
    {
        private List<IGameObject> spawnedObjects;
        private bool menuSpawned = false;

        private const double MENU_ITEM_SPACING = 30;

        #region ctors and dtors

        ~UISpawnerManager_MainMenu()
        {
            if (spawnedObjects != null && spawnedObjects.Count > 0)
            {
                List<IGameObject>.Enumerator listEnumerator = spawnedObjects.GetEnumerator();
                ISceneManager sceneManager = GameLoop.getSceneManager();
                while (listEnumerator.MoveNext())
                {
                    /*
                    Debug.Assert(listEnumerator.Current != null, "Expected menu item game object to be not null");
                    sceneManager.removeGameObject(listEnumerator.Current);
                     */ 
                    
                    IGameObject currObj = listEnumerator.Current;
                    if (currObj.GetType().IsSubclassOf(typeof(UIGameObjectBase)))
                    {
                        ((UIGameObjectBase)currObj).setRendered(false);
                    }
                    else if (currObj.GetType() == typeof(UserGameObject))
                    {
                        ((UserGameObject)currObj).setDead(true);
                    }
                     // 
                }
            }
        }

        #endregion

        #region ISpawnerManager members

        public void awaken()
        {
            if (!menuSpawned)
            {
                double yTotal = 0;

                spawnedObjects = new List<IGameObject>();

                UIGameObjectBase gameObject = new UIGameObj_NewGameButton(null, new BitmapImage(new Uri(@"pack://application:,,,/Resources/NewGameButton.png")), new UIGameObjectBase.ActivationDelegate(this.newGameButtonActivationDelegate));
                
                yTotal = gameObject.getImageSource().Height + MENU_ITEM_SPACING;

                spawnedObjects.Add(gameObject);
                GameLoop.getSceneManager().addGameObject(gameObject);

                gameObject = new UIGameObjectBase(null, new BitmapImage(new Uri(@"pack://application:,,,/Resources/OptionsButton.png")), new UIGameObjectBase.ActivationDelegate(this.optionsButtonActivationDelegate));

                yTotal += gameObject.getImageSource().Height + MENU_ITEM_SPACING;

                spawnedObjects.Add(gameObject);
                GameLoop.getSceneManager().addGameObject(gameObject);

                gameObject = new UIGameObjectBase(null, new BitmapImage(new Uri(@"pack://application:,,,/Resources/ExitButton.png")), new UIGameObjectBase.ActivationDelegate(this.exitButtonActivationDelegate));
                
                yTotal += gameObject.getImageSource().Height;

                spawnedObjects.Add(gameObject);
                GameLoop.getSceneManager().addGameObject(gameObject);

                double xOffset = GameLoop.getSceneManager().getTargetCanvas().RenderSize.Width / 2.0;
                double yOffset = (GameLoop.getSceneManager().getTargetCanvas().RenderSize.Height - yTotal) / 2.0;

                List<IGameObject>.Enumerator listEnumerator = spawnedObjects.GetEnumerator();
                while (listEnumerator.MoveNext())
                {
                    gameObject = (UIGameObjectBase) listEnumerator.Current;
                    gameObject.setGeometry(new RectangleGeometry(new System.Windows.Rect(
                                            new System.Windows.Point(xOffset - (gameObject.getImageSource().Width / 2.0), yOffset),
                                            new System.Windows.Point(xOffset + (gameObject.getImageSource().Width / 2.0), yOffset + gameObject.getImageSource().Height)
                                            )));

                    yOffset += (gameObject.getImageSource().Height + MENU_ITEM_SPACING);
                }

                ImageSource imageSource = new BitmapImage(new Uri(@"pack://application:,,,/Resources/crosshair.gif"));
                UserGameObject userGameObject = new UserGameObject(new EllipseGeometry(new Point(100, 100), 100, 100), imageSource, SkeletonSmoothingFilter.MEDIUM_SMOOTHING_LEVEL);
                imageSource.Freeze();

                spawnedObjects.Add(userGameObject);
                GameLoop.getSceneManager().addGameObject(userGameObject);

                menuSpawned = true;
            }            
        }

        public List<IGameObject> getSpawnedObjects()
        {
            return spawnedObjects.ToList<IGameObject>();
        }

        #endregion

        #region Buttons activation delegates

        public void exitButtonActivationDelegate()
        {
            //stop game loop
            GameLoop.getGameLoopSingleton().stop();
        }

        public void newGameButtonActivationDelegate()
        {
            //menu cleaning accomplished with spawner switching
            //fetch User Interface Game Objects enumerator
            List<IGameObject>.Enumerator listEnumerator = GameLoop.getSceneManager().getGameObjectListMapByTypeEnum()[GameObjectTypeEnum.UIObject].ToList<IGameObject>().GetEnumerator(); //TODO improve object fetching method: fetch all and only UIGameObjects of current menu
            ISceneManager sceneManager = GameLoop.getSceneManager();

            //mark all objects as not-renderized, so that they'll be removed
            while (listEnumerator.MoveNext())
            {
                if (listEnumerator.Current.GetType() == typeof(UIGameObjectBase) || listEnumerator.Current.GetType().IsSubclassOf(typeof(UIGameObjectBase)))
                {
                    UIGameObjectBase currObj = (UIGameObjectBase)listEnumerator.Current;
                    currObj.setRendered(false);
                }

                /*
                Debug.Assert(listEnumerator.Current != null, "Expected game object to remove to be not null");
                sceneManager.removeGameObject(listEnumerator.Current);
                */
            }

            listEnumerator = GameLoop.getSceneManager().getGameObjectListMapByTypeEnum()[GameObjectTypeEnum.UserObject].ToList<IGameObject>().GetEnumerator(); //TODO improve object fetching method: fetch all and only UIGameObjects of current menu

            //mark all objects as not-renderized, so that they'll be removed
            while (listEnumerator.MoveNext())
            {
                if (listEnumerator.Current.GetType() == typeof(UserGameObject))
                {
                    UserGameObject currObj = (UserGameObject)listEnumerator.Current;
                    currObj.setDead(true);
                }

                /*
                Debug.Assert(listEnumerator.Current != null, "Expected game object to remove to be not null");
                sceneManager.removeGameObject(listEnumerator.Current);
                */
            }

            //set the game spawner on the game loop object
            GameLoop.getGameLoopSingleton().setSpawnerManager(new GameSpawnerManager());

            //let the madness begin!!!
        }

        public void optionsButtonActivationDelegate()
        {
            //do nothing
        }

        #endregion
    }

}
