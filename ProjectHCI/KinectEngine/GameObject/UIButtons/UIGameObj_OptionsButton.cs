using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjectHCI.KinectEngine
{
    public class UIGameObj_OptionsButton : UIGameObjectBase
    {
        #region ctors and dtors

        public UIGameObj_OptionsButton(Geometry geometry)
            : base(geometry, null)
        {
            this._imageSource = new BitmapImage(new Uri(@"pack://application:,,,/Resources/OptionsButton.png"));
            this._imageSource.Freeze();
        }

        public UIGameObj_OptionsButton()
            : this(null)
        {
        }

        #endregion

        #region UIGameObjectBase members

        protected override void activate()
        {
            //do nothing now
        }

        #endregion
    }
}
