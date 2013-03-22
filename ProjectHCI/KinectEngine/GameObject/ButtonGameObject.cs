using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectHCI.KinectEngine
{
    public class ButtonGameObject : GameObject
    {

        public delegate void ActivationDelegate();


        private const int USER_INTERACTION_DELAY = 3000;
        private bool pointedByUser;
        private int internalCountDown;
        private bool enabled;

        protected ActivationDelegate activationDelegate;


        #region public bool enabled {get;  set;}

        public bool isEnabled()
        {
            return this.enabled;
        }

        public void setEnabled(bool _enabled)
        {
            this.enabled = _enabled;
        }

        #endregion

        


        public ButtonGameObject(double xPosition,
                                double yPosition,
                                Geometry boundingBoxGeometry,
                                Image image,
                                bool enabled,
                                ActivationDelegate activationDelegate)
        {
            base._xPosition = xPosition;
            base._yPosition = yPosition;
            base._extraData = null;
            base._boundingBoxGeometry = boundingBoxGeometry;
            base._image = image;
            base._gameObjectTypeEnum = GameObjectTypeEnum.UIObject;
            base._uid = Guid.NewGuid().ToString();

            this.activationDelegate = activationDelegate;
            this.pointedByUser = false;
            this.enabled = enabled;
            this.internalCountDown = USER_INTERACTION_DELAY;

        }



        /// <summary>
        /// This is the method invoked for each timer tick.
        /// Instead of a time to live counter it track how much the user pointer stays into the object boundaries
        /// </summary>
        /// <param name="deltaTimeMillis">The number of millisecond elapsed since the last timer tick</param>
        public override void update(int deltaTimeMillis)
        {
            if (this.pointedByUser)
            {
                this.internalCountDown -= deltaTimeMillis;

                if (this.internalCountDown <= 0)
                {
                    this.activate();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void activate()
        {
            this.activationDelegate();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool isCollidable()
        {
            return this.enabled;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool isDead()
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void onRendererDisplayDelegate()
        {
            ISceneManager sceneManager = GameLoop.getSceneManager();
            sceneManager.canvasDisplayImage(this, 1);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void onRendererUpdateDelegate()
        {
            //TODO: change state color
        }

        /// <summary>
        /// 
        /// </summary>
        public override void onRendererRemoveDelegate()
        {
            ISceneManager sceneManager = GameLoop.getSceneManager();
            sceneManager.canvasRemoveImage(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherGameObject"></param>
        public override void onCollisionEnterDelegate(IGameObject otherGameObject)
        {
            if (otherGameObject.getGameObjectTypeEnum() == GameObjectTypeEnum.UserObject)
            {
                pointedByUser = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherGameObject"></param>
        public override void onCollisionExitDelegate(IGameObject otherGameObject)
        {
            if (otherGameObject.getGameObjectTypeEnum() == GameObjectTypeEnum.UserObject)
            {
                this.pointedByUser = false;
                this.internalCountDown = USER_INTERACTION_DELAY;
            }
        }
    }
}
