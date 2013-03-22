using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ProjectHCI.KinectEngine
{
    class CenteredScreenAreaGameObject : GameObject
    {


        public CenteredScreenAreaGameObject(double canvasWidth,
                                            double canvasHeight,
                                            double gameObjectAreaWidth,
                                            double gameObjectAreaHeight)
        {

            base._xPosition = canvasWidth * 0.5 - gameObjectAreaWidth *0.5;
            base._yPosition = canvasHeight * 0.5 - gameObjectAreaHeight * 0.5;
            base._boundingBoxGeometry = new RectangleGeometry(new Rect(new Point(0, 0), new Point(gameObjectAreaWidth, gameObjectAreaHeight)));
            base._extraData = null;
            base._image = null;
            base._uid = Guid.NewGuid().ToString();
            base._gameObjectTypeEnum = GameObjectTypeEnum.UIObject;
            
        }


        public override void update(int deltaTimeMillis)
        {
            //do nothing
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
            //do nothing
        }

        public override void onRendererUpdateDelegate()
        {
            //do nothing
        }

        public override void onRendererRemoveDelegate()
        {
            //do nothing
        }

        public override void onCollisionEnterDelegate(IGameObject otherGameObject)
        {
            //do nothing
        }

        public override void onCollisionExitDelegate(IGameObject otherGameObject)
        {
            //do nothing
        }
    }
}
