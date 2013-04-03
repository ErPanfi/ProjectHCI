using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectHCI.KinectEngine;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using ProjectHCI.Utility;
using System.Windows;

namespace ProjectHCI.ReverseFruitNinja
{
    public class GameDebriefingSpawnerManager : SpawnerManager, IGameStateTracker
    {
        public const int VERTICAL_LABEL_SPACE = 10;

        private int gameScore;
        private int gameLength;

        #region IGameStateTracker members
        public int getGameScore()
        {
            return this.gameScore;
        }

        public int getGameLengthMillis()
        {
            return this.gameLength;
        }
        #endregion

        #region ctors and dtors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gameTracker">The tracker of the just finished game, which contains most recent tracked values</param>
        public GameDebriefingSpawnerManager(IGameStateTracker gameTracker)
            : base()
        {
            this.gameLength = gameTracker.getGameLengthMillis();
            this.gameScore = gameTracker.getGameScore();
        }

        #endregion

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

            double xPos = sceneManager.getCanvasWidth() - buttonImage.Width;
            double yPos = sceneManager.getCanvasHeight() - buttonImage.Height;

            ButtonGameObject buttonGameObject = new ButtonGameObject(xPos, yPos, boundingBoxGeometry, buttonImage, true, new ButtonGameObject.ActivationDelegate(this.backButtonActivationDelegate));

            return buttonGameObject;

        }

        protected override List<KeyValuePair<IGameObject, IGameObject>> spawnGameObjectsOnStart()
        {
            List<KeyValuePair<IGameObject, IGameObject>> ret = base.spawnGameObjectsOnStart();
            ISceneManager sceneManager = GameLoop.getSceneManager();

            //add user object
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"hand.png")));
            image.Height = 150;
            image.Width = 150;

            Image notAlreadyTracketImage = new Image();
            notAlreadyTracketImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"wave.png")));
            notAlreadyTracketImage.Height = 129;
            notAlreadyTracketImage.Width = 600;

            Geometry boundingBoxGeometry = new EllipseGeometry(new Point(75, 75), 10, 10);

            double halfCanvasWidth = sceneManager.getCanvasWidth() * 0.5;
            double halfCanvasHeight = sceneManager.getCanvasHeight() * 0.5;

            HandUserGameObject userGameObject = new HandUserGameObject(0, 0, boundingBoxGeometry, notAlreadyTracketImage, image, SkeletonSmoothingFilter.HIGH_SMOOTHING_LEVEL);
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(userGameObject, null));
#if DEBUG
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(userGameObject), userGameObject));
#endif


            //add game score and time labels
            CenteredScreenAreaGameObject centeredScreenAreaGameObject = new CenteredScreenAreaGameObject(sceneManager.getCanvasWidth(),
                                                                                                         sceneManager.getCanvasHeight(),
                                                                                                         1024,
                                                                                                         320);
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(centeredScreenAreaGameObject, null));

            //game score label
            IGameObject gameObject = new GameScoreLabelObject(0, 0, "Game score : ", this);
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, centeredScreenAreaGameObject));
#if DEBUG
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(gameObject), userGameObject));
#endif


            //game length label
            gameObject = new GameLengthLabelObject(0, VERTICAL_LABEL_SPACE + gameObject.getBoundingBoxGeometry().Bounds.Height, "Game length : ", this);
            gameObject.update(0);
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, centeredScreenAreaGameObject));
#if DEBUG
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(gameObject), userGameObject));
#endif


            //back to main button
            //Image buttonImage = new Image();
            //buttonImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"back.png")));
            //buttonImage.Height = 100;
            //buttonImage.Width = 250;
            //buttonImage.Stretch = Stretch.Fill;
            //buttonImage.StretchDirection = StretchDirection.Both;

            //gameObject = new ButtonGameObject(15,
            //                                    5 * VERTICAL_LABEL_SPACE + 2 * gameObject.getBoundingBoxGeometry().Bounds.Height,
            //                                    new RectangleGeometry(new System.Windows.Rect(0, 0, buttonImage.Width, buttonImage.Height)),
            //                                    buttonImage,
            //                                    true,
            //                                    new ButtonGameObject.ActivationDelegate(this.backButtonActivationDelegate)
            //                                 );

            gameObject = this.createBackButton();

            ret.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, null));
#if DEBUG
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(gameObject), gameObject));
#endif

            return ret;
        }

        //when the button is pressed remove all objects and go back to main menu
        public void backButtonActivationDelegate()
        {
            ISceneManager sceneManager = GameLoop.getSceneManager();
            List<IGameObject> objsToRemove = new List<IGameObject>();

            //removing all objs
            foreach (KeyValuePair<String, List<IGameObject>> gameTypeAndObjectPair0 in sceneManager.getGameObjectListMapByTag())
            {
                foreach (IGameObject gameObject00 in gameTypeAndObjectPair0.Value)
                {
                    switch (gameTypeAndObjectPair0.Key)
                    {
                        case Tags.DEBUG_TAG:   //debug objs are removed implicitly
                            break;
                        default:
                            objsToRemove.Add(gameObject00); //can't remove object in this foreach (collection modified exception)
                            break;
                    }
                }                
            }

            foreach (IGameObject gameObject0 in objsToRemove)
            {
                sceneManager.removeGameObject(gameObject0);
            }

            //back to main menu
            GameLoop.getGameLoopSingleton().setSpawnerManager(new MainMenuSpawnerManager());
        }


        public int getRageLevel()
        {
            return 0;
        }
    }
}
