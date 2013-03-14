using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ProjectHCI.KinectEngine
{
    public abstract class UIGameObjectBase : GameObjectBase
    {

        public const int USER_INTERACTION_DELAY = 3000;

        #region public bool enabled {get;  set;}

        private bool enabled;

        public bool isEnabled()
        {
            return enabled;
        }

        public void setEnabled(bool _enabled)
        {
            enabled = _enabled;
        }

        #endregion

        #region public bool rendered {get; set;}

        private bool rendered;

        public bool isRendered()
        {
            return rendered;
        }

        public void setRendered(bool _rendered)
        {
            rendered = _rendered;
        }

        #endregion

        #region user interaction members

        private bool pointedByUser;
        private int internalCountDown;

        private void initUserInteractionCounters()
        {
            pointedByUser = false;
            internalCountDown = USER_INTERACTION_DELAY;
        }

        /// <summary>
        /// This will be invoked if the user pointers has stayed long enough into object boundaries
        /// </summary>
        protected abstract void activate();

        #endregion

        #region ctors and dtors

        public UIGameObjectBase(Geometry geometry, ImageSource imageSource)
        {
            this.initUserInteractionCounters();
            this._geometry = geometry;
            this._imageSource = imageSource;
            this._objectType = GameObjectTypeEnum.UIObject;
            this._uid = base.generateUid();
            this.rendered = true;
        }

        #endregion

        /// <summary>
        /// This method expose geometry to external manipulation, in order to reorder menu buttons after their instantiation
        /// </summary>
        /// <param name="geometry">The new geometry</param>
        /// <remarks>This is not in interface, so it require a cast</remarks>
        public void setGeometry(Geometry geometry)
        {
            this._geometry = geometry;
        }




        #region inherited GameObjectBase method

        /// <summary>
        /// A UIGameObject is collidable iff is enabled
        /// </summary>
        /// <returns>True if is enabled, false otherwise</returns>
        public override bool isCollidable()
        {
            return enabled;
        }

        /// <summary>
        /// A UIGameObject is dead iff the menu don't need to be rendered anymore
        /// </summary>
        /// <returns>true if the buttons must be shown, false otherwise</returns>
        public override bool isDead()
        {
            return !rendered;
        }

        /// <summary>
        /// This marks that a game object has entered the object boundaries
        /// </summary>
        /// <param name="otherGameObject">The other object that has collided with this object.</param>
        /// <remarks>Note that only User Game Objects should collide with UIGameObjects</remarks>
        public override void onCollisionEnterDelegate(IGameObject otherGameObject)
        {
            if (otherGameObject.getObjectTypeEnum() == GameObjectTypeEnum.UserObject)
            {
                pointedByUser = true;
            }
        }

        /// <summary>
        /// This marks that a game object has exited the object boundaries
        /// </summary>
        /// <param name="otherGameObject">The other object that has de-collided with this object.</param>
        /// <remarks>Note that only User Game Objects should collide with UIGameObjects</remarks>
        public override void onCollisionExitDelegate(IGameObject otherGameObject)
        {
            if (otherGameObject.getObjectTypeEnum() == GameObjectTypeEnum.UserObject)
            {
                this.initUserInteractionCounters();
            }
        }

        /// <summary>
        /// This is the method invoked for each timer tick.
        /// Instead of a time to live counter it track how much the user pointer stays into the object boundaries
        /// </summary>
        /// <param name="deltaTimeMillis">The number of millisecond elapsed since the last timer tick</param>
        public override void updateTimeToLive(int deltaTimeMillis)
        {
            //reduce the internal countDown
            if (pointedByUser)
            {
                internalCountDown -= deltaTimeMillis;
                    //if it reached zero it's time to activate the button
                    if (internalCountDown <= 0)
                    {
                        this.activate();
                    }
            }
        }

        #endregion
    }
}
