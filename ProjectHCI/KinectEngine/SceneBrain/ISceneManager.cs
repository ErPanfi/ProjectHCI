using System;
using System.Collections.Generic;

using System.Windows;
using System.Windows.Controls;

namespace ProjectHCI.KinectEngine
{
    public interface ISceneManager
    {
        void registerAsCollidableGameObject(IGameObject collidableGameObject);

        void addGameObject(IGameObject gameObject);

        void removeGameObject(IGameObject gameObject);


        Dictionary<GameObjectTypeEnum, List<IGameObject>> getCollidableGameObjectListMapByTypeEnum();

        Dictionary<GameObjectTypeEnum, List<IGameObject>> getGameObjectListMapByTypeEnum();

        List<IGameObject> getCollaidableGameObjectList(GameObjectTypeEnum gameObjectTypeEnum);


        void registerUiElement(UIElement uiElement);

        void unregisterUiElement(UIElement uiElement);

        UIElement getUiElementByUid(String uid);


        Canvas getTargetCanvas();

    }
}
