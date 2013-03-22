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
    public class MainMenuSpawnerManager : ISpawnerManager
    {
        //private List<IGameObject> spawnedObjects;
        private bool menuSpawned = false;

        private const double MENU_ITEM_SPACING = 30;


        public MainMenuSpawnerManager()
        {
            //this.spawnedObjects = new List<IGameObject>();
        }




        /// <summary>
        /// 
        /// </summary>
        public void awaken()
        {
            if (!menuSpawned)
            {

                ISceneManager sceneManager = GameLoop.getSceneManager();


                { //user gameObject

                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri(@"pack://application:,,,/Resources/crosshair.gif"));
                    image.Height = 200;
                    image.Width = 200;

                    Geometry boundingBoxGeometry = new EllipseGeometry(new Point(100, 100), 80, 80);

                    double halfCanvasWidth = sceneManager.getCanvasWidth() * 0.5;
                    double halfCanvasHeight = sceneManager.getCanvasHeight() * 0.5;

                    HandUserGameObject userGameObject = new HandUserGameObject(200, 100, boundingBoxGeometry, image, SkeletonSmoothingFilter.MEDIUM_SMOOTHING_LEVEL);
                    sceneManager.addGameObject(userGameObject, null);
#if DEBUG
                    sceneManager.addGameObject(new BoundingBoxViewrGameObject(userGameObject), userGameObject);
#endif

                }





                // used as centered layout
                CenteredScreenAreaGameObject centeredScreenAreaGameObject = new CenteredScreenAreaGameObject(sceneManager.getCanvasWidth(),
                                                                                                             sceneManager.getCanvasHeight(),
                                                                                                             1024,
                                                                                                             320);
                sceneManager.addGameObject(centeredScreenAreaGameObject, null);
#if DEBUG
                sceneManager.addGameObject(new BoundingBoxViewrGameObject(centeredScreenAreaGameObject), centeredScreenAreaGameObject);
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

                    sceneManager.addGameObject(buttonGameObject, centeredScreenAreaGameObject);
#if DEBUG
                    sceneManager.addGameObject(new BoundingBoxViewrGameObject(buttonGameObject), buttonGameObject);
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

                    sceneManager.addGameObject(buttonGameObject, centeredScreenAreaGameObject);
#if DEBUG
                    sceneManager.addGameObject(new BoundingBoxViewrGameObject(buttonGameObject), buttonGameObject);
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

                    sceneManager.addGameObject(buttonGameObject, centeredScreenAreaGameObject);
#if DEBUG
                    sceneManager.addGameObject(new BoundingBoxViewrGameObject(buttonGameObject), buttonGameObject);
#endif
                }
                

                menuSpawned = true;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<IGameObject> getSpawnedObjects()
        {
            throw new NotImplementedException();
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
