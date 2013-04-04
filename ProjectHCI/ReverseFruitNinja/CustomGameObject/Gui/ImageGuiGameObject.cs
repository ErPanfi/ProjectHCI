using ProjectHCI.KinectEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ProjectHCI.ReverseFruitNinja
{
    public class ImageGuiGameObject : GameObject
    {

        public ImageGuiGameObject(double xPosition,
                                  double yPosition,
                                  Image image)
        {
            
            base._xPosition = xPosition;
            base._yPosition = yPosition;
            base._gameObjectTag = Tags.UI_TAG;
            base._image = image;

        }


        public override void update(int deltaTimeMillis)
        {
            // do nothing
        }

        public override bool isCollidable()
        {
            return false;
        }

        public override bool isDead()
        {
            return false;
        }



        public override void onRendererDisplayDelegate()
        {
            ISceneManager sceneManager = GameLoop.getSceneManager();
            sceneManager.canvasDisplayImage(this, 100);
        }

        public override void onRendererUpdateDelegate()
        {
            GameLoop.getSceneManager().canvasUpdateImage(this);
        }

        public override void onRendererRemoveDelegate()
        {
            ISceneManager sceneManager = GameLoop.getSceneManager();
            sceneManager.canvasRemoveImage(this);
        }





        public override void onCollisionEnterDelegate(IGameObject otherGameObject)
        {
            throw new NotImplementedException();
        }

        public override void onCollisionExitDelegate(IGameObject otherGameObject)
        {
            throw new NotImplementedException();
        }
    }
}
