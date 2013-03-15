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

        public UIGameObj_OptionsButton(Geometry geometry, ActivationDelegate _actDelegate)
            : base(geometry, null, _actDelegate)
        {
            this._imageSource = new BitmapImage(new Uri(@"pack://application:,,,/Resources/OptionsButton.png"));
            this._imageSource.Freeze();
        }

        #endregion

        #region UIGameObjectBase members

        /*
        protected override void activate()
        {
            //do nothing now
        }
         */ 

        #endregion
    }
}
