using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjectHCI.KinectEngine
{
    public class UIGameObj_ExitButton : UIGameObjectBase
    {
        #region ctors and dtors

        public UIGameObj_ExitButton(Geometry geometry)
            : base(geometry, null)
        {
            this._imageSource = new BitmapImage(new Uri(@"pack://application:,,,/Resources/ExitButton.png"));
            this._imageSource.Freeze();
        }

        public UIGameObj_ExitButton()
            : this(null)
        {
        }

        #endregion

        #region UIGameObjectBase members

        protected override void activate()
        {
            //stop game loop
            GameLoop.getGameLoopSingleton().stop();
        }

        #endregion
    }
}
