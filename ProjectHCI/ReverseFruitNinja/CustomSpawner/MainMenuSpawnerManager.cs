using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Collections.Concurrent;
using ProjectHCI.KinectEngine;

namespace ProjectHCI.ReverseFruitNinja
{
    public class MainMenuSpawnerManager : SpawnerManager
    {


        public MainMenuSpawnerManager() : base()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override List<KeyValuePair<IGameObject, IGameObject>> spawnGameObjectsOnStart()
        {

            ISceneManager sceneManager = GameLoop.getSceneManager();

            List<KeyValuePair<IGameObject, IGameObject>> gameObjectParentGameObjectPairList = new List<KeyValuePair<IGameObject, IGameObject>>();


            { //user gameObject

                Image image = new Image();
                image.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/crosshair.gif"));
                image.Height = 200;
                image.Width = 200;

                Geometry boundingBoxGeometry = new EllipseGeometry(new Point(100, 100), 80, 80);

                double halfCanvasWidth = sceneManager.getCanvasWidth() * 0.5;
                double halfCanvasHeight = sceneManager.getCanvasHeight() * 0.5;

                HandUserGameObject userGameObject = new HandUserGameObject(0, 0, boundingBoxGeometry, image, SkeletonSmoothingFilter.MEDIUM_SMOOTHING_LEVEL);
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(userGameObject, null));
#if DEBUG
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewrGameObject(userGameObject), userGameObject));
#endif

            }





            // used as centered layout
            CenteredScreenAreaGameObject centeredScreenAreaGameObject = new CenteredScreenAreaGameObject(sceneManager.getCanvasWidth(),
                                                                                                         sceneManager.getCanvasHeight(),
                                                                                                         1024,
                                                                                                         320);
            gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(centeredScreenAreaGameObject, null));
#if DEBUG
            gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewrGameObject(centeredScreenAreaGameObject), centeredScreenAreaGameObject));
#endif




            { //new-game button

                Image buttonImage = new Image();
                buttonImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/NewGameButton.png"));
                buttonImage.Height = 50;
                buttonImage.Width = 200;
                buttonImage.Stretch = Stretch.Fill;
                buttonImage.StretchDirection = StretchDirection.Both;

                Geometry boundingBoxGeometry = new RectangleGeometry(new Rect(new Point(0, 0), new Point(320, 320)));
                ButtonGameObject.ActivationDelegate buttonDelegate = new ButtonGameObject.ActivationDelegate(this.newGameButtonActivationDelegate);

                ButtonGameObject buttonGameObject = new ButtonGameObject(0, 0, boundingBoxGeometry, buttonImage, true, buttonDelegate);

                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(buttonGameObject, centeredScreenAreaGameObject));
#if DEBUG
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewrGameObject(buttonGameObject), buttonGameObject));
#endif
            }



            { //option button

                Image buttonImage = new Image();
                buttonImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/OptionsButton.png"));
                buttonImage.Height = 50;
                buttonImage.Width = 200;
                buttonImage.Stretch = Stretch.Fill;
                buttonImage.StretchDirection = StretchDirection.Both;

                Geometry boundingBoxGeometry = new RectangleGeometry(new Rect(new Point(0, 0), new Point(320, 320)));
                ButtonGameObject.ActivationDelegate buttonDelegate = new ButtonGameObject.ActivationDelegate(this.optionsButtonActivationDelegate);

                ButtonGameObject buttonGameObject = new ButtonGameObject(350, 0, boundingBoxGeometry, buttonImage, true, buttonDelegate);

                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(buttonGameObject, centeredScreenAreaGameObject));
#if DEBUG
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewrGameObject(buttonGameObject), buttonGameObject));
#endif
            }



            { //exit button

                Image buttonImage = new Image();
                buttonImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/ExitButton.png"));
                buttonImage.Height = 50;
                buttonImage.Width = 200;
                buttonImage.Stretch = Stretch.Fill;
                buttonImage.StretchDirection = StretchDirection.Both;

                Geometry boundingBoxGeometry = new RectangleGeometry(new Rect(new Point(0, 0), new Point(320, 320)));
                ButtonGameObject.ActivationDelegate buttonDelegate = new ButtonGameObject.ActivationDelegate(this.exitButtonActivationDelegate);

                ButtonGameObject buttonGameObject = new ButtonGameObject(700, 0, boundingBoxGeometry, buttonImage, true, buttonDelegate);

                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(buttonGameObject, centeredScreenAreaGameObject));
#if DEBUG
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewrGameObject(buttonGameObject), buttonGameObject));
#endif
            }


            return gameObjectParentGameObjectPairList;
        }


        


        /// <summary>
        /// 
        /// </summary>
        public void exitButtonActivationDelegate()
        {
            //stop game loop
            GameLoop.getGameLoopSingleton().stop();

        }



        /// <summary>
        /// 
        /// </summary>
        public void newGameButtonActivationDelegate()
        {

            ISceneManager sceneManager = GameLoop.getSceneManager();


            { //menu cleaning accomplished with spawner switching

                foreach (IGameObject gameObject in sceneManager.getGameObjectListMapByTypeEnum()[GameObjectTypeEnum.UIObject].ToList())//ToList used as a copy 
                {
                    sceneManager.removeGameObject(gameObject);
                }

                foreach (IGameObject gameObject in sceneManager.getGameObjectListMapByTypeEnum()[GameObjectTypeEnum.UserObject].ToList())//ToList used as a copy 
                {
                    sceneManager.removeGameObject(gameObject);
                }

            }



            //set the game spawner on the game loop object
            GameLoop.getGameLoopSingleton().setSpawnerManager(new GameSpawnerManager());

            //let the madness begin!!! .....WTF?!
        }



        /// <summary>
        /// 
        /// </summary>
        public void optionsButtonActivationDelegate()
        {
            //do nothing
        }
                
    }

}
