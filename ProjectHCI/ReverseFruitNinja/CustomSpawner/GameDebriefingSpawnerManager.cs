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

        protected override List<KeyValuePair<IGameObject, IGameObject>> spawnGameObjectsOnStart()
        {
            List<KeyValuePair<IGameObject, IGameObject>> ret = base.spawnGameObjectsOnStart();
            ISceneManager sceneManager = GameLoop.getSceneManager();

            //add user object
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"crosshair.gif")));
            image.Height = 200;
            image.Width = 200;

            Geometry boundingBoxGeometry = new EllipseGeometry(new Point(100, 100), 80, 80);

            double halfCanvasWidth = sceneManager.getCanvasWidth() * 0.5;
            double halfCanvasHeight = sceneManager.getCanvasHeight() * 0.5;

            HandUserGameObject userGameObject = new HandUserGameObject(0, 0, boundingBoxGeometry, image, SkeletonSmoothingFilter.MEDIUM_SMOOTHING_LEVEL);
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(userGameObject, null));
#if DEBUG
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewGameObject(userGameObject), userGameObject));
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
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewGameObject(gameObject), userGameObject));
#endif


            //game length label
            gameObject = new GameLengthLabelObject(0, VERTICAL_LABEL_SPACE + gameObject.getBoundingBoxGeometry().Bounds.Height, "Game length : ", this);
            gameObject.update(0);
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, centeredScreenAreaGameObject));
#if DEBUG
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewGameObject(gameObject), userGameObject));
#endif


            //back to main button
            Image buttonImage = new Image();
            buttonImage.Source = new BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"BackButton.png")));
            buttonImage.Height = 300;
            buttonImage.Width = 400;
            buttonImage.Stretch = Stretch.Fill;
            buttonImage.StretchDirection = StretchDirection.Both;

            gameObject = new ButtonGameObject(15,
                                                5 * VERTICAL_LABEL_SPACE + 2 * gameObject.getBoundingBoxGeometry().Bounds.Height,
                                                new RectangleGeometry(new System.Windows.Rect(0, 0, buttonImage.Width, buttonImage.Height)),
                                                buttonImage,
                                                true,
                                                new ButtonGameObject.ActivationDelegate(this.backButtonActivationDelegate)
                                             );

            ret.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, centeredScreenAreaGameObject));
#if DEBUG
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewGameObject(userGameObject), userGameObject));
#endif

            return ret;
        }

        //when the button is pressed remove all objects and go back to main menu
        public void backButtonActivationDelegate()
        {
            ISceneManager sceneManager = GameLoop.getSceneManager();
            List<IGameObject> objsToRemove = new List<IGameObject>();

            //removing all objs
            foreach (KeyValuePair<GameObjectTypeEnum, List<IGameObject>> gameTypeAndObjectPair0 in sceneManager.getGameObjectListMapByTypeEnum())
            {
                foreach (IGameObject gameObject00 in gameTypeAndObjectPair0.Value)
                {
                    switch (gameTypeAndObjectPair0.Key)
                    {
                        case GameObjectTypeEnum.DebugObject:   //debug objs are removed implicitly
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
    }
}
