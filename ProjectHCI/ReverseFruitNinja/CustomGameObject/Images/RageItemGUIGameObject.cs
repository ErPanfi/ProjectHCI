using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ProjectHCI.Utility;

namespace ProjectHCI.ReverseFruitNinja
{
    public class RageItemGUIGameObject : ImageGuiGameObject
    {
        private int thresholdLevel;
        private IGameStateTracker gameStateTracker;
        private bool opaque;

        public const double IMAGE_WIDTH = 150;
        public const double IMAGE_HEIGHT = 150;


        #region ctors

        public RageItemGUIGameObject(IGameStateTracker gameStateTracker, int thresholdLevel, double xPosition, double yPosition)
            : base(xPosition, yPosition, null)
        {
            this.thresholdLevel = thresholdLevel;
            this.gameStateTracker = gameStateTracker;
            this._image = this.createKunaiImage();
        }

        #endregion

        #region image handling methods

        private Image createKunaiImage()
        {            
            Image kunaiImage = new Image();

            if (this.gameStateTracker.getRageLevel() < thresholdLevel)
            {
                opaque = false;
                kunaiImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"rage_off.png")));
            }
            else
            {
                opaque = true;
                kunaiImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(BitmapUtility.getImgResourcePath(@"rage_on.png")));
            }

            kunaiImage.Height = IMAGE_HEIGHT;
            kunaiImage.Width = IMAGE_WIDTH;
            kunaiImage.Stretch = System.Windows.Media.Stretch.Fill;
            kunaiImage.StretchDirection = StretchDirection.Both;

            return kunaiImage;
        }

        #endregion



        public override void  update(int deltaTimeMillis)
        {
            base.update(deltaTimeMillis);

            if (this.opaque && this.gameStateTracker.getRageLevel() < thresholdLevel
             || !this.opaque && this.gameStateTracker.getRageLevel() >= thresholdLevel)
            {
                this._image = this.createKunaiImage();
            }
        }
    }
}
    