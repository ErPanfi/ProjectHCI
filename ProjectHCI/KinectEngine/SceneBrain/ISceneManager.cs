using System;
using System.Collections.Generic;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectHCI.KinectEngine
{
    public interface ISceneManager
    {


        void promoteToCollidableGameObject(IGameObject collidableGameObject);

        void addGameObject(IGameObject gameObject, IGameObject parentGameObject);

        void removeGameObject(IGameObject gameObject);




        Dictionary<GameObjectTypeEnum, List<IGameObject>> getCollidableGameObjectListMapByTypeEnum();

        Dictionary<GameObjectTypeEnum, List<IGameObject>> getGameObjectListMapByTypeEnum();

        List<IGameObject> getCollaidableGameObjectList(GameObjectTypeEnum gameObjectTypeEnum);




        void canvasDisplayImage(IGameObject gameObject, int zIndex);

        void canvasUpdateImage(IGameObject gameObject);

        void canvasRemoveImage(IGameObject gameObject);

        void applyTranslation(IGameObject gameObject, double xCanvasPosition, double yCanvasPosition);

        void applyRotation(IGameObject gameObject, double clockwiseDegreeAngle, double xRelativeRotationCenter, double yRelativeRotationCenter);




        double getCanvasWidth();

        double getCanvasHeight();




        //void boundUiElementToGameObject(UIElement uiElement, IGameObject targetGameObject);

        //void unboundAllUiElementFromGameObject(IGameObject targetGameObject);

        //void unboundUiElementFromGameObject(IGameObject targetGameObject, UIElement targetUiElement);



        //List<UIElement> getUiElementListBoundToGameObject(IGameObject gameObject);



        //Canvas getTargetCanvas();

    }
}
