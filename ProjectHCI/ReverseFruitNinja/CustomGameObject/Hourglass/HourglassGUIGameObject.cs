using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using ProjectHCI.Utility;
using System.Diagnostics;
using ProjectHCI.KinectEngine;

namespace ProjectHCI.ReverseFruitNinja
{
    public class HourglassGUIGameObject : ImageGuiGameObject
    {
        //public const string[] IMG_FILE_NAMES = { @"Hourglass_1.png", @"Hourglass_2.png", @"Hourglass_3.png", @"Hourglass_4.png" };

        public const int HEIGHT_MARGIN = 30;
        public const int WIDTH_MARGIN = 30;

        #region protected List<string> imgFileNames;

        protected string[] imgFileNames;

        public int getHourglassStates()
        {
            return this.imgFileNames.Length;
        }

        #endregion


        #region protected int hourglassState {public get; set;}

        protected int hourglassState;
        protected int previousState;

        public int getHourglassCurrentState()
        {
            return this.hourglassState;
        }

        public void incHourglassState()
        {
            if (this.hourglassState < this.imgFileNames.Length - 1)
            {
                this.hourglassState++;
                this._image = this.buildImageFromOwner();
                this.previousState = this.hourglassState;
                GameLoop.getSceneManager().canvasUpdateImage(this);
            }
            else
            {
                this.hourglassState = this.imgFileNames.Length;
            }
        }

        #endregion

        IGameObject owner;

        #region Owner-based autoconfiguration methods


        protected Image buildImageFromOwner()
        {
            Image ownerImage = this.owner.getImage();
            Image localImage = new Image();
            localImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(BitmapUtility.getImgResourcePath(this.imgFileNames[this.hourglassState])));
            localImage.Height = ownerImage.Height + HEIGHT_MARGIN;
            localImage.Width = ownerImage.Width + WIDTH_MARGIN;
            localImage.Stretch = System.Windows.Media.Stretch.Fill;
            localImage.StretchDirection = StretchDirection.Both;

            return localImage;
        }

        protected KeyValuePair<double, double> obtainXYOffsetFromOwner()
        {
            Image ownerImage = this.owner.getImage();
            double x = (ownerImage.Width - this._image.Width) / 2.0;
            double y = (ownerImage.Height - this._image.Height) / 2.0;

            return new KeyValuePair<double, double>(x, y);
        }

        #endregion

        #region Ctors and dtors

        public HourglassGUIGameObject(IGameObject owner, string[] fileNames)
            : this(owner, 0, fileNames)
        {
        }

        public HourglassGUIGameObject(IGameObject owner, int initialState, string[] fileNames)
            : base(0, 0, null)  //fake base call, the appropriate values will be set here
        {
            Debug.Assert(fileNames != null && fileNames.Length > 0, "Needs at least one image to build hourglass!");
            this.imgFileNames = fileNames;
            this.owner = owner;
            this.hourglassState = (initialState < imgFileNames.Length && initialState >= 0) ? initialState : 0;
            this.previousState = this.hourglassState;
            this._image = this.buildImageFromOwner();
            KeyValuePair<double, double> xy = this.obtainXYOffsetFromOwner();
            this._xPosition = xy.Key;
            this._yPosition = xy.Value;
        }
        #endregion

        public override bool isDead()
        {
            return this.hourglassState == this.imgFileNames.Length;
        }
    }
}
