using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjectHCI.KinectEngine
{
    public class UIGameObj_NewGameButton : UIGameObjectBase
    {

        #region ctors and dtors

        public UIGameObj_NewGameButton()
            : this(null)
        {
        }

        public UIGameObj_NewGameButton(Geometry geometry)
            : base(geometry, null)
        {
            this._imageSource = new BitmapImage(new Uri(@"pack://application:,,,/Resources/NewGameButton.png"));
            this._imageSource.Freeze();
        }

        #endregion

        #region UIGameObjectBase members

        /// <summary>
        /// When this button activates a new game starts
        /// </summary>
        protected override void activate()
        {
            //fetch User Interface Game Objects enumerator
            List<IGameObject>.Enumerator listEnumerator = GameLoop.getSceneManager().getGameObjectListMapByTypeEnum()[GameObjectTypeEnum.UIObject].GetEnumerator();
            
            //mark all objects as not-renderized, so that they'll be removed
            while (listEnumerator.MoveNext())
            {
                if (listEnumerator.Current.GetType().IsSubclassOf(typeof(UIGameObjectBase)))
                {
                    UIGameObjectBase currObj = (UIGameObjectBase)listEnumerator.Current;
                    currObj.setRendered(false);
                }
            }

            //set the game spawner on the game loop object
            GameLoop.getGameLoopSingleton().setSpawnerManager(new GameSpawnerManager());

            //let the madness begin!!!
        }

        #endregion
    }
}
