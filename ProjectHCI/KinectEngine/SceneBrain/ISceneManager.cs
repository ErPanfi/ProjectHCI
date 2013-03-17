using System;
using System.Collections.Generic;

using System.Windows;
using System.Windows.Controls;

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

        void canvasUpdateImage(IGameObject gameObject, int zIndex);

        void canvasRemoveImage(IGameObject gameObject);




        //void boundUiElementToGameObject(UIElement uiElement, IGameObject targetGameObject);

        //void unboundAllUiElementFromGameObject(IGameObject targetGameObject);

        //void unboundUiElementFromGameObject(IGameObject targetGameObject, UIElement targetUiElement);



        //List<UIElement> getUiElementListBoundToGameObject(IGameObject gameObject);



        //Canvas getTargetCanvas();

    }
}
