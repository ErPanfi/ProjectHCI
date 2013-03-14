using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

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
                while (listEnumerator.MoveNext())
                {
                    IGameObject currObj = listEnumerator.Current;
                    if (currObj.GetType().IsSubclassOf(typeof(UIGameObjectBase)))
                    {
                        ((UIGameObjectBase)currObj).setRendered(false);
                    }
                    else if (currObj.GetType() == typeof(UserGameObject))
                    {
                        ((UserGameObject)currObj).setDead(true);
                    }
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

                UIGameObjectBase gameObject = new UIGameObj_NewGameButton();
                
                yTotal = gameObject.getImageSource().Height + MENU_ITEM_SPACING;

                spawnedObjects.Add(gameObject);
                GameLoop.getSceneManager().addGameObject(gameObject);

                gameObject = new UIGameObj_OptionsButton();

                yTotal += gameObject.getImageSource().Height + MENU_ITEM_SPACING;

                spawnedObjects.Add(gameObject);
                GameLoop.getSceneManager().addGameObject(gameObject);

                gameObject = new UIGameObj_ExitButton();
                
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
    }

}
