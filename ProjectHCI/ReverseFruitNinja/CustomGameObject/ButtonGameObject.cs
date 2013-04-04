using ProjectHCI.KinectEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectHCI.ReverseFruitNinja
{
    public class ButtonGameObject : GameObject
    {

        public delegate void ActivationDelegate();


        private const int USER_INTERACTION_DELAY = 3000;
        private bool pointedByObject;
        private HourglassGUIGameObject runningHourglass;
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
            base._boundingBoxGeometry = boundingBoxGeometry;
            base._image = image;
            base._gameObjectTag = Tags.BUTTON_TAG;

            this.activationDelegate = activationDelegate;
            this.pointedByObject = false;
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
            if (this.pointedByObject)
            {
                if (this.runningHourglass != null)
                {
                    int desiredState = (int)(
                                                (1.0 - ((double)this.internalCountDown / (double)USER_INTERACTION_DELAY))
                                               * runningHourglass.getHourglassStates()
                                            );
                    if (desiredState > runningHourglass.getHourglassCurrentState())
                    {
                        runningHourglass.incHourglassState();
                    }

                    if (this.internalCountDown <= 0)
                    {
                        this.activate();
                    }
                }

                this.internalCountDown -= deltaTimeMillis;
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
            sceneManager.canvasDisplayImage(this, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void onRendererUpdateDelegate()
        {
            GameLoop.getSceneManager().canvasUpdateImage(this);
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
            if (otherGameObject.getGameObjectTag() == Tags.USER_TAG)
            {
                GameLoop.getSceneManager().applyScale(this, 1.2, 1.2, this.getImage().Width * 0.5, this.getImage().Height * 0.5, true);
                this.pointedByObject = true;
                this.runningHourglass = new HourglassGUIGameObject(otherGameObject, new string[] { @"Hourglass_1.png", @"Hourglass_2.png", @"Hourglass_3.png", @"Hourglass_4.png" });
                GameLoop.getSceneManager().addGameObject(runningHourglass, otherGameObject);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherGameObject"></param>
        public override void onCollisionExitDelegate(IGameObject otherGameObject)
        {
            if (otherGameObject.getGameObjectTag() == Tags.USER_TAG)
            {
                ISceneManager sceneManager = GameLoop.getSceneManager();
                sceneManager.applyScale(this, 1 / 1.2, 1 / 1.2, this.getImage().Width * 0.5, this.getImage().Height * 0.5, true);
                this.pointedByObject = false;
                this.internalCountDown = USER_INTERACTION_DELAY;
                if (this.runningHourglass != null)
                {
                    sceneManager.removeGameObject(runningHourglass);
                }
            }
        }
    }
}
