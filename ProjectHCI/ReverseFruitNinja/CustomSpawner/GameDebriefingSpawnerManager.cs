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
    public class GameDebriefingSpawnerManager : SpawnerManager
    {
        public const int VERTICAL_LABEL_SPACE = 10;

        public const double POINTS_PER_MILLIS = (1 / 2000.0);
        public const double POINTS_PER_RAGE = (1 / 5.0);
        public const double POINTS_PER_BONUS = 1;
        public const double HEAD_BONUS = 3;
        public const double HAND_BONUS = 1;
        public const double EASY_BONUS = 1;
        public const double MEDIUM_BONUS = 1.5;
        public const double HARD_BONUS = 2;

        private int bonusPoints;
        private int gameLength;
        private int ragePoints;
        private Configuration.GameDifficultyEnum gameDifficulty;
        private Configuration.UserControlMethodEnum userControlMethod;

        #region ctors and dtors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gameSceneBrain">The tracker of the just finished game, which contains most recent tracked values</param>
        public GameDebriefingSpawnerManager(GameSceneBrain gameSceneBrain)
            : base()
        {
            this.gameLength = gameSceneBrain.getGameLengthMillis();
            this.bonusPoints = gameSceneBrain.getGameScore();
            this.ragePoints = gameSceneBrain.getRagePoints();
            this.gameDifficulty = Configuration.getCurrentConfiguration().gameDifficulty;
            this.userControlMethod = Configuration.getCurrentConfiguration().userControlMethod;
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

        private string millis2time(int millis)
        {
            //also update game length text
            int hh = millis / 3600000;
            millis %= 3600000;
            int mm = millis / 60000;
            millis %= 60000;
            int ss = millis / 1000;

            return (hh < 10 ? "0" : "") + hh + ":" + (mm < 10 ? "0" : "") + mm + ":" + (ss < 10 ? "0" : "") + ss;
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

            //init accumulators
            double currentHeight = 0;
            double finalScore = 0;

            double diffMultiplier = 1;
            switch (this.gameDifficulty)
            {
                case Configuration.GameDifficultyEnum.Medium:
                    diffMultiplier = 1.5;
                    break;
                case Configuration.GameDifficultyEnum.Hard:
                    diffMultiplier = 2;
                    break;
            }

            double controlMultiplier = 1;
            if (this.userControlMethod == Configuration.UserControlMethodEnum.Head)
            {
                controlMultiplier = 3;
            }



            //game score label
            double multiplier = POINTS_PER_BONUS * controlMultiplier;
            double currentScore = this.bonusPoints * multiplier;

            finalScore += currentScore;
            IGameObject gameObject = new GameLabelObject(0, currentHeight, "Saved fruits score : " + currentScore + " (" + this.bonusPoints + " x " + multiplier + ")", -1);
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, centeredScreenAreaGameObject));
#if DEBUG
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(gameObject), userGameObject));
#endif
            currentHeight += VERTICAL_LABEL_SPACE + gameObject.getBoundingBoxGeometry().Bounds.Height;

            //game length label
            multiplier = diffMultiplier * POINTS_PER_MILLIS;

            currentScore = this.gameLength * multiplier;
            finalScore += currentScore;
            gameObject = new GameLabelObject(0, currentHeight, "Survival bonus       : " + Math.Round(currentScore, 1) + " (" + this.millis2time(this.gameLength) + ")", -1);
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, centeredScreenAreaGameObject));
#if DEBUG
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(gameObject), userGameObject));
#endif
            currentHeight += VERTICAL_LABEL_SPACE + gameObject.getBoundingBoxGeometry().Bounds.Height;

            //rage score label
            multiplier = POINTS_PER_RAGE * diffMultiplier;
            currentScore = this.ragePoints * multiplier;
            finalScore += currentScore;
            gameObject = new GameLabelObject(0, currentHeight, "Enemy rage bonus  : " + currentScore + " (" + this.ragePoints + " x " + multiplier + ")", -1);
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, centeredScreenAreaGameObject));
#if DEBUG
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(gameObject), userGameObject));
#endif
            currentHeight += VERTICAL_LABEL_SPACE + gameObject.getBoundingBoxGeometry().Bounds.Height;

            gameObject = this.createBackButton();

            //difficulty bonus label
            finalScore *= diffMultiplier;
            gameObject = new GameLabelObject(0, currentHeight, "Difficulty bonus     " + (diffMultiplier > 1 ? "x " + diffMultiplier : "NO BONUS"), -1);
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, centeredScreenAreaGameObject));
#if DEBUG
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(gameObject), userGameObject));
#endif
            currentHeight += VERTICAL_LABEL_SPACE + gameObject.getBoundingBoxGeometry().Bounds.Height;

            //control bonus label
            finalScore *= controlMultiplier;
            gameObject = new GameLabelObject(0, currentHeight, "Control bonus        " + (controlMultiplier > 1 ? "x " + controlMultiplier : "NO BONUS"), -1);
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, centeredScreenAreaGameObject));
#if DEBUG
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(gameObject), userGameObject));
#endif
            currentHeight += VERTICAL_LABEL_SPACE + gameObject.getBoundingBoxGeometry().Bounds.Height;

            //final score label
            gameObject = new GameLabelObject(0, currentHeight, "FINAL SCORE             : " + Math.Round(finalScore), -1); 
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(gameObject, centeredScreenAreaGameObject));
#if DEBUG
            ret.Add(new KeyValuePair<IGameObject, IGameObject>(new BoundingBoxViewerGameObject(gameObject), userGameObject));
#endif
            currentHeight += VERTICAL_LABEL_SPACE + gameObject.getBoundingBoxGeometry().Bounds.Height;


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
