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
using ProjectHCI.Utility;

namespace ProjectHCI.ReverseFruitNinja
{
    public class MainMenuSpawnerManager : SpawnerManager
    {

        public delegate List<KeyValuePair<IGameObject, IGameObject>> SpawnStateFunction();


        private Stack<SpawnStateFunction> stateStackTrace;

        private bool stateChanged;
        private SpawnStateFunction spawnerStateFunction;
        private CenteredScreenAreaGameObject menuAreaGameObject;
        private Configuration configuration;


        public MainMenuSpawnerManager()
            : base()
        {
            this.stateChanged = true;
            this.spawnerStateFunction = null;
            this.stateStackTrace = new Stack<SpawnStateFunction>();
            this.configuration = Configuration.getCurrentConfiguration();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override List<KeyValuePair<IGameObject, IGameObject>> spawnGameObjectsOnStart()
        {
            return this.spawnBaseMenuInitialObject();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override List<KeyValuePair<IGameObject, IGameObject>> spawnGameObjectsPerFrame()
        {
            List<KeyValuePair<IGameObject, IGameObject>> gameObjectParentGameObjectPairList;

            if (this.stateChanged)
            {
                gameObjectParentGameObjectPairList = this.spawnerStateFunction();
                this.stateChanged = false;
            }
            else
            {
                gameObjectParentGameObjectPairList = new List<KeyValuePair<IGameObject, IGameObject>>();
            }

            return gameObjectParentGameObjectPairList;
        }




        private IGameObject createBackButton()
        {

            ISceneManager sceneManager = GameLoop.getSceneManager();


            Image buttonImage = new Image();
            buttonImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"back.png")));
            buttonImage.Height = 100;
            buttonImage.Width = 250;
            buttonImage.Stretch = Stretch.Fill;
            buttonImage.StretchDirection = StretchDirection.Both;

            Geometry boundingBoxGeometry = new RectangleGeometry(new Rect(new Point(0, 0), new Point(250, 100)));
            ButtonGameObject.ActivationDelegate buttonDelegate = new ButtonGameObject.ActivationDelegate(this.backButtonActivationDelegate);

            Point menuAreaButtomRightPoint = this.menuAreaGameObject.getBoundingBoxGeometry().Bounds.BottomRight;
            double xPos = sceneManager.getCanvasWidth() - buttonImage.Width;
            double yPos = sceneManager.getCanvasHeight() - buttonImage.Height;

            ButtonGameObject buttonGameObject = new ButtonGameObject(xPos, yPos, boundingBoxGeometry, buttonImage, true, buttonDelegate);

            return buttonGameObject;

        }




        #region spawnBaseMenuInitialObject

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<KeyValuePair<IGameObject, IGameObject>> spawnBaseMenuInitialObject()
        {
            List<KeyValuePair<IGameObject, IGameObject>> gameObjectParentGameObjectPairList = new List<KeyValuePair<IGameObject, IGameObject>>();


            #region test code
            //{ // test

            //    FormattedTextGameObject fgo = new FormattedTextGameObject(300, 300, "Font di merda", -1);
            //    gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(fgo, null));


            //    //foreach (FontFamily fontFamily in Fonts.GetFontFamilies(new Uri("pack://application:,,,/"), "./Resources/"))
            //    //{
            //    //    System.Diagnostics.Debug.WriteLine("font: " + fontFamily);
            //    //}

            //}
            #endregion


            { //userObject

                Image image = new Image();
                image.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"hand.png")));
                image.Height = 150;
                image.Width = 150;

                Image notAlreadyTracketImage = new Image();
                notAlreadyTracketImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"wave.png")));
                notAlreadyTracketImage.Height = 129;
                notAlreadyTracketImage.Width = 600;

                Geometry boundingBoxGeometry = new EllipseGeometry(new Point(75, 75), 10, 10);

                HandUserGameObject userGameObject = new HandUserGameObject(0, 0, boundingBoxGeometry, notAlreadyTracketImage, image, SkeletonSmoothingFilter.HIGH_SMOOTHING_LEVEL);
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(userGameObject, null));
#if DEBUG
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(userGameObject), userGameObject));
#endif
            }


            { //logo

                Image logoImage = new Image();
                logoImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"logo.png")));
                logoImage.Height = 221;
                logoImage.Width = 400;

                ImageGuiGameObject logoGameObject = new ImageGuiGameObject(600, 20, logoImage);
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(logoGameObject, null));

            }


            { //menu area

                ISceneManager sceneManager = GameLoop.getSceneManager();

                CenteredScreenAreaGameObject areaGameObject = new CenteredScreenAreaGameObject(sceneManager.getCanvasWidth(),
                                                                                                   sceneManager.getCanvasHeight(),
                                                                                                   1366,
                                                                                                   700);
                this.menuAreaGameObject = areaGameObject;
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(areaGameObject, null));
#if DEBUG
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(areaGameObject), areaGameObject));
#endif
            }



            this.spawnerStateFunction = this.spawnMainMenu;
            this.stateChanged = true;
            return gameObjectParentGameObjectPairList;

        }


        #endregion




        #region spawnMainMenu

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<KeyValuePair<IGameObject, IGameObject>> spawnMainMenu()
        {
            List<KeyValuePair<IGameObject, IGameObject>> gameObjectParentGameObjectPairList = new List<KeyValuePair<IGameObject, IGameObject>>();



            { //new-game button

                Image buttonImage = new Image();
                buttonImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"new_game.png")));
                buttonImage.Height = 350;
                buttonImage.Width = 420;
                buttonImage.Stretch = Stretch.Fill;
                buttonImage.StretchDirection = StretchDirection.Both;

                Geometry boundingBoxGeometry = new RectangleGeometry(new Rect(new Point(0, 0), new Point(420, 350)));
                ButtonGameObject.ActivationDelegate buttonDelegate = new ButtonGameObject.ActivationDelegate(this.newGameButtonActivationDelegate);
                ButtonGameObject buttonGameObject = new ButtonGameObject(0, 350 - buttonImage.Height * 0.5, boundingBoxGeometry, buttonImage, true, buttonDelegate);

                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(buttonGameObject, this.menuAreaGameObject));
#if DEBUG
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(buttonGameObject), buttonGameObject));
#endif
            }



            { //option button

                Image buttonImage = new Image();
                buttonImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"settings.png")));
                buttonImage.Height = 350;
                buttonImage.Width = 420;
                buttonImage.Stretch = Stretch.Fill;
                buttonImage.StretchDirection = StretchDirection.Both;

                Geometry boundingBoxGeometry = new RectangleGeometry(new Rect(new Point(0, 0), new Point(420, 350)));
                ButtonGameObject.ActivationDelegate buttonDelegate = new ButtonGameObject.ActivationDelegate(this.settingsButtonActivationDelegate);
                ButtonGameObject buttonGameObject = new ButtonGameObject(470, 350 - buttonImage.Height * 0.5, boundingBoxGeometry, buttonImage, true, buttonDelegate);

                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(buttonGameObject, this.menuAreaGameObject));
#if DEBUG
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(buttonGameObject), buttonGameObject));
#endif
            }



            { //exit button

                Image buttonImage = new Image();
                buttonImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"quit.png")));
                buttonImage.Height = 350;
                buttonImage.Width = 420;
                buttonImage.Stretch = Stretch.Fill;
                buttonImage.StretchDirection = StretchDirection.Both;

                Geometry boundingBoxGeometry = new RectangleGeometry(new Rect(new Point(0, 0), new Point(420, 350)));
                ButtonGameObject.ActivationDelegate buttonDelegate = new ButtonGameObject.ActivationDelegate(this.quitButtonActivationDelegate);
                ButtonGameObject buttonGameObject = new ButtonGameObject(940, 350 - buttonImage.Height * 0.5, boundingBoxGeometry, buttonImage, true, buttonDelegate);

                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(buttonGameObject, this.menuAreaGameObject));
#if DEBUG
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(buttonGameObject), buttonGameObject));
#endif
            }


            return gameObjectParentGameObjectPairList;
        }


        #endregion




        #region spawnSettingsMenu


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<KeyValuePair<IGameObject, IGameObject>> spawnSettingsMenu()
        {

            List<KeyValuePair<IGameObject, IGameObject>> gameObjectParentGameObjectPairList = new List<KeyValuePair<IGameObject, IGameObject>>();



            { //difficulty button

                Image buttonImage = new Image();
                buttonImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"difficulty.png")));
                buttonImage.Height = 350;
                buttonImage.Width = 420;
                buttonImage.Stretch = Stretch.Fill;
                buttonImage.StretchDirection = StretchDirection.Both;

                Geometry boundingBoxGeometry = new RectangleGeometry(new Rect(new Point(0, 0), new Point(420, 350)));
                ButtonGameObject.ActivationDelegate buttonDelegate = new ButtonGameObject.ActivationDelegate(this.difficultyMainButtonActivationDelegate);
                ButtonGameObject buttonGameObject = new ButtonGameObject(0, 350 - buttonImage.Height * 0.5, boundingBoxGeometry, buttonImage, true, buttonDelegate);

                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(buttonGameObject, this.menuAreaGameObject));
#if DEBUG
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(buttonGameObject), buttonGameObject));
#endif
            }



            { //controls button

                Image buttonImage = new Image();
                buttonImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"controls.png")));
                buttonImage.Height = 350;
                buttonImage.Width = 420;
                buttonImage.Stretch = Stretch.Fill;
                buttonImage.StretchDirection = StretchDirection.Both;

                Geometry boundingBoxGeometry = new RectangleGeometry(new Rect(new Point(0, 0), new Point(420, 350)));
                ButtonGameObject.ActivationDelegate buttonDelegate = new ButtonGameObject.ActivationDelegate(this.controlsMainButtonActivationDelegate);
                ButtonGameObject buttonGameObject = new ButtonGameObject(470, 350 - buttonImage.Height * 0.5, boundingBoxGeometry, buttonImage, true, buttonDelegate);

                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(buttonGameObject, this.menuAreaGameObject));
#if DEBUG
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(buttonGameObject), buttonGameObject));
#endif
            }



            { //graphic button

                Image buttonImage = new Image();
                buttonImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"graphics.png")));
                buttonImage.Height = 350;
                buttonImage.Width = 420;
                buttonImage.Stretch = Stretch.Fill;
                buttonImage.StretchDirection = StretchDirection.Both;

                Geometry boundingBoxGeometry = new RectangleGeometry(new Rect(new Point(0, 0), new Point(420, 350)));
                ButtonGameObject.ActivationDelegate buttonDelegate = new ButtonGameObject.ActivationDelegate(this.graphicsMainButtonActivationDelegate);
                ButtonGameObject buttonGameObject = new ButtonGameObject(940, 350 - buttonImage.Height * 0.5, boundingBoxGeometry, buttonImage, true, buttonDelegate);

                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(buttonGameObject, this.menuAreaGameObject));
#if DEBUG
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(buttonGameObject), buttonGameObject));
#endif
            }


            IGameObject backButton = this.createBackButton();
            gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(backButton, null));
#if DEBUG
            gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(backButton), backButton));
#endif



            return gameObjectParentGameObjectPairList;
        }


        #endregion




        #region spawnDifficultyMenu

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<KeyValuePair<IGameObject, IGameObject>> spawnDifficultyMenu()
        {

            List<KeyValuePair<IGameObject, IGameObject>> gameObjectParentGameObjectPairList = new List<KeyValuePair<IGameObject, IGameObject>>();



            { //difficulty easy

                String imageName = this.configuration.gameDifficulty == Configuration.GameDifficultyEnum.Easy ? "easy_selected.png" : "easy.png";

                Image buttonImage = new Image();
                buttonImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(imageName)));
                buttonImage.Height = 350;
                buttonImage.Width = 420;
                buttonImage.Stretch = Stretch.Fill;
                buttonImage.StretchDirection = StretchDirection.Both;

                Geometry boundingBoxGeometry = new RectangleGeometry(new Rect(new Point(0, 0), new Point(420, 350)));
                ButtonGameObject.ActivationDelegate buttonDelegate = new ButtonGameObject.ActivationDelegate(this.easyLevelButtonActivationDelegate);
                ButtonGameObject buttonGameObject = new ButtonGameObject(0, 350 - buttonImage.Height * 0.5, boundingBoxGeometry, buttonImage, true, buttonDelegate);

                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(buttonGameObject, this.menuAreaGameObject));
#if DEBUG
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(buttonGameObject), buttonGameObject));
#endif
            }



            { //difficulty medium

                String imageName = this.configuration.gameDifficulty == Configuration.GameDifficultyEnum.Medium ? "normal_selected.png" : "normal.png";

                Image buttonImage = new Image();
                buttonImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(imageName)));
                buttonImage.Height = 350;
                buttonImage.Width = 420;
                buttonImage.Stretch = Stretch.Fill;
                buttonImage.StretchDirection = StretchDirection.Both;

                Geometry boundingBoxGeometry = new RectangleGeometry(new Rect(new Point(0, 0), new Point(420, 350)));
                ButtonGameObject.ActivationDelegate buttonDelegate = new ButtonGameObject.ActivationDelegate(this.mediumLevelButtonActivationDelegate);
                ButtonGameObject buttonGameObject = new ButtonGameObject(470, 350 - buttonImage.Height * 0.5, boundingBoxGeometry, buttonImage, true, buttonDelegate);

                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(buttonGameObject, this.menuAreaGameObject));
#if DEBUG
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(buttonGameObject), buttonGameObject));
#endif
            }



            { //difficulty hard

                String imageName = this.configuration.gameDifficulty == Configuration.GameDifficultyEnum.Hard ? "hard_selected.png" : "hard.png";

                Image buttonImage = new Image();
                buttonImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(imageName)));
                buttonImage.Height = 350;
                buttonImage.Width = 420;
                buttonImage.Stretch = Stretch.Fill;
                buttonImage.StretchDirection = StretchDirection.Both;

                Geometry boundingBoxGeometry = new RectangleGeometry(new Rect(new Point(0, 0), new Point(420, 350)));
                ButtonGameObject.ActivationDelegate buttonDelegate = new ButtonGameObject.ActivationDelegate(this.hardLevelButtonActivationDelegate);
                ButtonGameObject buttonGameObject = new ButtonGameObject(940, 350 - buttonImage.Height * 0.5, boundingBoxGeometry, buttonImage, true, buttonDelegate);

                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(buttonGameObject, this.menuAreaGameObject));
#if DEBUG
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(buttonGameObject), buttonGameObject));
#endif
            }





            IGameObject backButton = this.createBackButton();
            gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(backButton, null));
#if DEBUG
            gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(backButton), backButton));
#endif


            return gameObjectParentGameObjectPairList;
        }


        #endregion




        #region spawnControlsMenu

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<KeyValuePair<IGameObject, IGameObject>> spawnControlsMenu()
        {

            List<KeyValuePair<IGameObject, IGameObject>> gameObjectParentGameObjectPairList = new List<KeyValuePair<IGameObject, IGameObject>>();



            { //hand control

                String imageName = this.configuration.userControlMethod == Configuration.UserControlMethodEnum.Hand ? "hand_control_selected.png" : "hand_control.png";

                Image buttonImage = new Image();
                buttonImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(imageName)));
                buttonImage.Height = 350;
                buttonImage.Width = 420;
                buttonImage.Stretch = Stretch.Fill;
                buttonImage.StretchDirection = StretchDirection.Both;

                Geometry boundingBoxGeometry = new RectangleGeometry(new Rect(new Point(0, 0), new Point(420, 350)));
                ButtonGameObject.ActivationDelegate buttonDelegate = new ButtonGameObject.ActivationDelegate(this.handControlButtonActivationDelegate);
                ButtonGameObject buttonGameObject = new ButtonGameObject(0, 350 - buttonImage.Height * 0.5, boundingBoxGeometry, buttonImage, true, buttonDelegate);

                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(buttonGameObject, this.menuAreaGameObject));
#if DEBUG
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(buttonGameObject), buttonGameObject));
#endif
            }



            { //head control

                String imageName = this.configuration.userControlMethod == Configuration.UserControlMethodEnum.Head ? "head_control_selected.png" : "head_control.png";

                Image buttonImage = new Image();
                buttonImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(imageName)));
                buttonImage.Height = 350;
                buttonImage.Width = 420;
                buttonImage.Stretch = Stretch.Fill;
                buttonImage.StretchDirection = StretchDirection.Both;

                Geometry boundingBoxGeometry = new RectangleGeometry(new Rect(new Point(0, 0), new Point(420, 350)));
                ButtonGameObject.ActivationDelegate buttonDelegate = new ButtonGameObject.ActivationDelegate(this.headControlButtonActivationDelegate);
                ButtonGameObject buttonGameObject = new ButtonGameObject(470, 350 - buttonImage.Height * 0.5, boundingBoxGeometry, buttonImage, true, buttonDelegate);

                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(buttonGameObject, this.menuAreaGameObject));
#if DEBUG
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(buttonGameObject), buttonGameObject));
#endif
            }



            IGameObject backButton = this.createBackButton();
            gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(backButton, null));
#if DEBUG
            gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(backButton), backButton));
#endif


            return gameObjectParentGameObjectPairList;
        }

        #endregion




        #region spawnGraphicsMenu

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<KeyValuePair<IGameObject, IGameObject>> spawnGraphicsMenu()
        {

            List<KeyValuePair<IGameObject, IGameObject>> gameObjectParentGameObjectPairList = new List<KeyValuePair<IGameObject, IGameObject>>();



            { //fruit 1

                String imageName = this.configuration.userFruitImage == Configuration.USER_FRUIT_1_FILENAME ? "user_fruit_button1_selected.png" : "user_fruit_button1.png";

                Image buttonImage = new Image();
                buttonImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(imageName)));
                buttonImage.Height = 350;
                buttonImage.Width = 420;
                buttonImage.Stretch = Stretch.Fill;
                buttonImage.StretchDirection = StretchDirection.Both;

                Geometry boundingBoxGeometry = new RectangleGeometry(new Rect(new Point(0, 0), new Point(420, 350)));
                ButtonGameObject.ActivationDelegate buttonDelegate = new ButtonGameObject.ActivationDelegate(this.fruit1ButtonActivationDelegate);
                ButtonGameObject buttonGameObject = new ButtonGameObject(0, 350 - buttonImage.Height * 0.5, boundingBoxGeometry, buttonImage, true, buttonDelegate);

                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(buttonGameObject, this.menuAreaGameObject));
#if DEBUG
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(buttonGameObject), buttonGameObject));
#endif
            }



            { //fruit 2

                String imageName = this.configuration.userFruitImage == Configuration.USER_FRUIT_2_FILENAME ? "user_fruit_button2_selected.png" : "user_fruit_button2.png";

                Image buttonImage = new Image();
                buttonImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(imageName)));
                buttonImage.Height = 350;
                buttonImage.Width = 420;
                buttonImage.Stretch = Stretch.Fill;
                buttonImage.StretchDirection = StretchDirection.Both;

                Geometry boundingBoxGeometry = new RectangleGeometry(new Rect(new Point(0, 0), new Point(420, 350)));
                ButtonGameObject.ActivationDelegate buttonDelegate = new ButtonGameObject.ActivationDelegate(this.fruit2ButtonActivationDelegate);
                ButtonGameObject buttonGameObject = new ButtonGameObject(470, 350 - buttonImage.Height * 0.5, boundingBoxGeometry, buttonImage, true, buttonDelegate);

                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(buttonGameObject, this.menuAreaGameObject));
#if DEBUG
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(buttonGameObject), buttonGameObject));
#endif
            }


            { //fruit 3

                String imageName = this.configuration.userFruitImage == Configuration.USER_FRUIT_3_FILENAME ? "user_fruit_button3_selected.png" : "user_fruit_button3.png";

                Image buttonImage = new Image();
                buttonImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(imageName)));
                buttonImage.Height = 350;
                buttonImage.Width = 420;
                buttonImage.Stretch = Stretch.Fill;
                buttonImage.StretchDirection = StretchDirection.Both;

                Geometry boundingBoxGeometry = new RectangleGeometry(new Rect(new Point(0, 0), new Point(420, 350)));
                ButtonGameObject.ActivationDelegate buttonDelegate = new ButtonGameObject.ActivationDelegate(this.fruit3ButtonActivationDelegate);
                ButtonGameObject buttonGameObject = new ButtonGameObject(940, 350 - buttonImage.Height * 0.5, boundingBoxGeometry, buttonImage, true, buttonDelegate);

                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(buttonGameObject, this.menuAreaGameObject));
#if DEBUG
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(buttonGameObject), buttonGameObject));
#endif
            }



            IGameObject backButton = this.createBackButton();
            gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(backButton, null));
#if DEBUG
            gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(backButton), backButton));
#endif


            return gameObjectParentGameObjectPairList;
        }


        #endregion




        /// <summary>
        /// 
        /// </summary>
        /// <param name="newSpawnerState"></param>
        private void changeState(SpawnStateFunction newSpawnerState)
        {
            ISceneManager sceneManager = GameLoop.getSceneManager();
            sceneManager.removeGameObjectsByTag(Tags.BUTTON_TAG);

            this.stateChanged = true;
            this.stateStackTrace.Push(this.spawnerStateFunction);
            this.spawnerStateFunction = newSpawnerState;

        }

        /// <summary>
        /// 
        /// </summary>
        private void reloadState()
        {
            ISceneManager sceneManager = GameLoop.getSceneManager();
            sceneManager.removeGameObjectsByTag(Tags.BUTTON_TAG);
            this.stateChanged = true;
        }




        /// <summary>
        /// 
        /// </summary>
        public void newGameButtonActivationDelegate()
        {

            ISceneManager sceneManager = GameLoop.getSceneManager();

            sceneManager.removeGameObjectsByTag(Tags.UI_TAG);
            sceneManager.removeGameObjectsByTag(Tags.USER_TAG);
            sceneManager.removeGameObjectsByTag(Tags.BUTTON_TAG);



            //set the game spawner on the game loop object
            GameSceneBrain gameSceneBrain = new GameSceneBrain();
            GameLoop.getGameLoopSingleton().setSceneBrain(gameSceneBrain);
            GameLoop.getGameLoopSingleton().setSpawnerManager(new GameSpawnerManager(gameSceneBrain));

            //let the madness begin!!! .....WTF?!
        }

        public void settingsButtonActivationDelegate()
        {
            this.changeState(this.spawnSettingsMenu);
        }

        public void quitButtonActivationDelegate()
        {
            GameLoop.getGameLoopSingleton().stop();
        }







        public void difficultyMainButtonActivationDelegate()
        {
            this.changeState(this.spawnDifficultyMenu);
        }


        public void controlsMainButtonActivationDelegate()
        {
            this.changeState(this.spawnControlsMenu);
        }

        public void graphicsMainButtonActivationDelegate()
        {
            this.changeState(this.spawnGraphicsMenu);
        }



        public void easyLevelButtonActivationDelegate()
        {
            this.configuration.gameDifficulty = Configuration.GameDifficultyEnum.Easy;
            this.reloadState();
        }

        public void mediumLevelButtonActivationDelegate()
        {
            this.configuration.gameDifficulty = Configuration.GameDifficultyEnum.Medium;
            this.reloadState();
        }

        public void hardLevelButtonActivationDelegate()
        {
            this.configuration.gameDifficulty = Configuration.GameDifficultyEnum.Hard;
            this.reloadState();
        }





        public void headControlButtonActivationDelegate()
        {
            this.configuration.userControlMethod = Configuration.UserControlMethodEnum.Head;
            this.reloadState();
        }

        public void handControlButtonActivationDelegate()
        {
            this.configuration.userControlMethod = Configuration.UserControlMethodEnum.Hand;
            this.reloadState();
        }



        public void fruit1ButtonActivationDelegate()
        {
            this.configuration.userFruitImage = Configuration.USER_FRUIT_1_FILENAME;
            this.reloadState();
        }

        public void fruit2ButtonActivationDelegate()
        {
            this.configuration.userFruitImage = Configuration.USER_FRUIT_2_FILENAME;
            this.reloadState();
        }

        public void fruit3ButtonActivationDelegate()
        {
            this.configuration.userFruitImage = Configuration.USER_FRUIT_2_FILENAME;
            this.reloadState();
        }





        public void backButtonActivationDelegate()
        {
            ISceneManager sceneManager = GameLoop.getSceneManager();
            sceneManager.removeGameObjectsByTag(Tags.BUTTON_TAG);

            this.stateChanged = true;
            this.spawnerStateFunction = this.stateStackTrace.Pop();
        }









    }
}
