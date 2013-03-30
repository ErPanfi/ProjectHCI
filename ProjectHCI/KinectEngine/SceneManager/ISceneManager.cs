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

        void removeGameObjectsByTag(String gameObjectTag);

        void removeGameObject(IGameObject gameObject);





        Dictionary<String, List<IGameObject>> getCollidableGameObjectListMapByTag();

        Dictionary<String, List<IGameObject>> getGameObjectListMapByTag();

        List<IGameObject> getCollaidableGameObjectList(String gameObjectTag);

        bool gameObjectExist(IGameObject gameObject);





        void canvasDisplayImage(IGameObject gameObject, int zIndex);

        void canvasUpdateImage(IGameObject gameObject);

        void canvasRemoveImage(IGameObject gameObject);

        void applyTranslation(IGameObject gameObject, double xOffset, double yOffset, bool propagateToChild);

        void applyRotation(IGameObject gameObject, double clockwiseDegreeAngle, double xRelativeRotationCenter, double yRelativeRotationCenter, bool propagateToChild);

        void applyScale(IGameObject gameObject, double xScale, double yScale, double xCenter, double yCenter, bool propagateToChild);




        bool isGameObjectDisplayed(IGameObject gameObject);




        double getCanvasWidth();

        double getCanvasHeight();




        //void boundUiElementToGameObject(UIElement uiElement, IGameObject targetGameObject);

        //void unboundAllUiElementFromGameObject(IGameObject targetGameObject);

        //void unboundUiElementFromGameObject(IGameObject targetGameObject, UIElement targetUiElement);



        //List<UIElement> getUiElementListBoundToGameObject(IGameObject gameObject);



        //Canvas getTargetCanvas();

    }
}
