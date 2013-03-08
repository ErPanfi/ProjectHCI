using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.Windows.Controls;
using System.Windows;


namespace ProjectHCI.KinectEngine
{
    public class SceneManager : ISceneManager
    {

        private Canvas targetCanvas;

        private Dictionary<GameObjectTypeEnum, List<IGameObject>> gameObjectListMapByTypeEnum;
        private Dictionary<GameObjectTypeEnum, List<IGameObject>> collidableGameObjectListMapByTypeEnum;

        private Dictionary<String, UIElement> uiElementMapByUid;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetCanvas"></param>
        public SceneManager(Canvas targetCanvas)
        {
            this.targetCanvas = targetCanvas;

            this.gameObjectListMapByTypeEnum = new Dictionary<GameObjectTypeEnum, List<IGameObject>>(10);
            this.collidableGameObjectListMapByTypeEnum = new Dictionary<GameObjectTypeEnum, List<IGameObject>>(10);
            this.uiElementMapByUid = new Dictionary<String, UIElement>(100);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="targetGameObjectListMapByTypeEnum"></param>
        private void registerGameObject(IGameObject gameObject, Dictionary<GameObjectTypeEnum, List<IGameObject>> targetGameObjectListMapByTypeEnum)
        {
            Debug.Assert(gameObject != null, "expected gameObject not null.");
            Debug.Assert(targetGameObjectListMapByTypeEnum != null, "expected targetGameObjectListMapByTypeEnum not null");

            //Type gameObjectType = gameObject.GetType();
            //switched to enum
            GameObjectTypeEnum gameObjectTypeEnum = gameObject.getObjectTypeEnum();


            if (!targetGameObjectListMapByTypeEnum.ContainsKey(gameObjectTypeEnum))
            {
                targetGameObjectListMapByTypeEnum.Add(gameObjectTypeEnum, new List<IGameObject>(20));
            }
            targetGameObjectListMapByTypeEnum[gameObjectTypeEnum].Add(gameObject);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="targetGameObjectListMapByTypeEnum"></param>
        private void deleteGameObject(IGameObject gameObject, Dictionary<GameObjectTypeEnum, List<IGameObject>> targetGameObjectListMapByTypeEnum)
        {
            Debug.Assert(gameObject != null, "expected gameObject not null.");
            Debug.Assert(targetGameObjectListMapByTypeEnum != null, "expected targetGameObjectListMapByTypeEnum not null");



            //Type gameObjectType = gameObject.GetType();
            GameObjectTypeEnum gameObjectTypeEnum = gameObject.getObjectTypeEnum();  //switched to enum value

            if (targetGameObjectListMapByTypeEnum.ContainsKey(gameObjectTypeEnum)
                    && targetGameObjectListMapByTypeEnum[gameObjectTypeEnum].Contains(gameObject))
            {
                targetGameObjectListMapByTypeEnum[gameObjectTypeEnum].Remove(gameObject);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObjectTypeEnum"></param>
        /// <returns></returns>
        public List<IGameObject> getCollaidableGameObjectList(GameObjectTypeEnum gameObjectTypeEnum)
        {
            Dictionary<GameObjectTypeEnum, List<IGameObject>> collidableGameObjectListMapByTypeEnum = this.getCollidableGameObjectListMapByTypeEnum();
            if (collidableGameObjectListMapByTypeEnum.ContainsKey(gameObjectTypeEnum))
            {
                return this.getCollidableGameObjectListMapByTypeEnum()[gameObjectTypeEnum];
            }
            else
            {
                return new List<IGameObject>();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        public void addGameObject(IGameObject gameObject)
        {
            this.registerGameObject(gameObject, this.gameObjectListMapByTypeEnum);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="collidableGameObject"></param>
        public void registerAsCollidableGameObject(IGameObject collidableGameObject)
        {
            Debug.Assert(this.gameObjectListMapByTypeEnum.ContainsKey(collidableGameObject.getObjectTypeEnum()), "add the specified object before try to register it as collidable.");
            Debug.Assert(this.gameObjectListMapByTypeEnum[collidableGameObject.getObjectTypeEnum()].Contains(collidableGameObject), "add the specified object before try to register it as collidable.");

            this.registerGameObject(collidableGameObject, this.collidableGameObjectListMapByTypeEnum);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiElement"></param>
        public void registerUiElement(UIElement uiElement)
        {
            Debug.Assert(!this.uiElementMapByUid.ContainsKey(uiElement.Uid), "uiElement already present in uiElementListMapByUid");

            this.uiElementMapByUid.Add(uiElement.Uid, uiElement);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public UIElement getUiElementByUid(String uid)
        {
            Debug.Assert(this.uiElementMapByUid.ContainsKey(uid), "unkown uiElement");

            return this.uiElementMapByUid[uid];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiElement"></param>
        public void unregisterUiElement(UIElement uiElement)
        {
            Debug.Assert(this.uiElementMapByUid.ContainsKey(uiElement.Uid), "unkown uiElement");

            this.uiElementMapByUid.Remove(uiElement.Uid);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        public void removeGameObject(IGameObject gameObject)
        {
            this.deleteGameObject(gameObject, this.gameObjectListMapByTypeEnum);
            this.deleteGameObject(gameObject, this.collidableGameObjectListMapByTypeEnum);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<GameObjectTypeEnum, List<IGameObject>> getGameObjectListMapByTypeEnum()
        {
            return this.gameObjectListMapByTypeEnum;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<GameObjectTypeEnum, List<IGameObject>> getCollidableGameObjectListMapByTypeEnum()
        {
            return this.collidableGameObjectListMapByTypeEnum;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Canvas getTargetCanvas()
        {
            return this.targetCanvas;
        }

    }
}
