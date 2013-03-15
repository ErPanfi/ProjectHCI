using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace ProjectHCI.KinectEngine
{
    public class UIGameObj_NewGameButton : UIGameObjectBase
    {

        private int timeToLive;
        private bool fired;

        #region ctors and dtors

        public UIGameObj_NewGameButton(Geometry geometry, ImageSource imgSource, ActivationDelegate _actDelegate)
            : base(geometry, imgSource, _actDelegate)
        {
            this.timeToLive = 5000;
            this.fired = false;

            /*
            this._imageSource = new BitmapImage(new Uri(@"pack://application:,,,/Resources/NewGameButton.png"));
            this._imageSource.Freeze();
             */
        }

        #endregion

        #region UIGameObjectBase members

        /*
        /// <summary>
        /// When this button activates a new game starts
        /// </summary>
        protected override void activate()
        {
            //menu cleaning accomplished with spawner switching
            //fetch User Interface Game Objects enumerator
            List<IGameObject>.Enumerator listEnumerator = GameLoop.getSceneManager().getGameObjectListMapByTypeEnum()[GameObjectTypeEnum.UIObject].ToList<IGameObject>().GetEnumerator(); //TODO improve object fetching method: fetch all and only UIGameObjects of current menu
            ISceneManager sceneManager = GameLoop.getSceneManager();

            //mark all objects as not-renderized, so that they'll be removed
            while (listEnumerator.MoveNext())
            {
                if (listEnumerator.Current.GetType().IsSubclassOf(typeof(UIGameObjectBase)))
                {
                    UIGameObjectBase currObj = (UIGameObjectBase)listEnumerator.Current;
                    currObj.setRendered(false);
                }

                / *
                Debug.Assert(listEnumerator.Current != null, "Expected game object to remove to be not null");
                sceneManager.removeGameObject(listEnumerator.Current);
                * /
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

                / *
                Debug.Assert(listEnumerator.Current != null, "Expected game object to remove to be not null");
                sceneManager.removeGameObject(listEnumerator.Current);
                * /
            }

            //set the game spawner on the game loop object
            GameLoop.getGameLoopSingleton().setSpawnerManager(new GameSpawnerManager());

            //let the madness begin!!!
        }
        */
        
        public override void updateTimeToLive(int deltaTimeMillis)
        {
            timeToLive -= deltaTimeMillis;

            if (!fired && timeToLive <= 0)
            {
                fired = true;
                this._activationDelegate();
            }
        }

        #endregion
    }
}
