using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;


namespace ProjectHCI.KinectEngine
{
    public class SceneManager : ISceneManager
    {

        private Canvas targetCanvas;

        private SceneNode sceneNodeTree;

        private Dictionary<GameObjectTypeEnum, List<IGameObject>> gameObjectListMapByTypeEnum;
        private Dictionary<GameObjectTypeEnum, List<IGameObject>> collidableGameObjectListMapByTypeEnum;
        private Dictionary<IGameObject, String> uiThreadImageUidMapByGameObject;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetCanvas"></param>
        public SceneManager(Canvas targetCanvas)
        {
            this.targetCanvas = targetCanvas;

            this.gameObjectListMapByTypeEnum = new Dictionary<GameObjectTypeEnum, List<IGameObject>>(10);
            this.collidableGameObjectListMapByTypeEnum = new Dictionary<GameObjectTypeEnum, List<IGameObject>>(10);
            this.uiThreadImageUidMapByGameObject = new Dictionary<IGameObject, String>(100);

            //creation of the rootNode
            this.sceneNodeTree = new SceneNode(null); 
        }







        /// <summary>
        /// 
        /// </summary>
        /// <param name="collidableGameObject"></param>
        public void promoteToCollidableGameObject(IGameObject collidableGameObject)
        {
            Debug.Assert(this.gameObjectListMapByTypeEnum.ContainsKey(collidableGameObject.getGameObjectTypeEnum()), "add the specified object before try to register it as collidable.");
            Debug.Assert(this.gameObjectListMapByTypeEnum[collidableGameObject.getGameObjectTypeEnum()].Contains(collidableGameObject), "add the specified object before try to register it as collidable.");

            this.registerGameObject(collidableGameObject, this.collidableGameObjectListMapByTypeEnum);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="parentGameObject"></param>
        public void addGameObject(IGameObject gameObject, IGameObject parentGameObject)
        {
            Debug.Assert(gameObject != parentGameObject, "a gameObject cannot be the parent of himself.");

            SceneNode targetSceneNode = SceneNode.getSceneNodeByGameObject(this.sceneNodeTree, parentGameObject);
            Debug.Assert(targetSceneNode != null, "no specified parentGameObject found in SceneNode tree");

            if (parentGameObject != null)
            {
                double xPositionRelativeToParent = parentGameObject.getXPosition() + gameObject.getXPosition();
                double yPositionRelativeToParent = parentGameObject.getYPosition() + gameObject.getYPosition();
                gameObject.setPosition(xPositionRelativeToParent, yPositionRelativeToParent);
            }
            

            targetSceneNode.addChild(new SceneNode(gameObject));

            this.registerGameObject(gameObject, this.gameObjectListMapByTypeEnum);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        public void removeGameObject(IGameObject gameObject)
        {
            SceneNode targetSceneNode = SceneNode.getSceneNodeByGameObject(this.sceneNodeTree, gameObject);

            //if targetSceneNode equals to null then it is the special case when its parent node and this gameObject node has
            //expired at the same frame, so sceneBrain calls removeGameObject on both, but it calls removeGameObject on parent node first,
            //forcing the sceneManager to delete all the childs node too. Consequentially, the later call to removeGameObject on the child node
            //traduces to a tree-search miss.
            if (targetSceneNode == null)
            {
                //assert that all data of this game object was already removed.
                Debug.Assert(!this.gameObjectListMapByTypeEnum[gameObject.getGameObjectTypeEnum()].Contains(gameObject));
                Debug.Assert(!this.collidableGameObjectListMapByTypeEnum[gameObject.getGameObjectTypeEnum()].Contains(gameObject));
            }
            else
            {
                this.recursiveRemoveGameObject(targetSceneNode);
            }
            
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
        public void canvasDisplayImage(IGameObject gameObject, int zIndex)
        {
            Debug.Assert(gameObject != null, "expected gameObject != null");
            Debug.Assert(gameObject.getImage() != null, "cannot display this gameObject because its image is null.");
            Debug.Assert(!this.uiThreadImageUidMapByGameObject.ContainsKey(gameObject), "this gameObject was already present in the canvas, try to remove it before ask to display again");
            

            Image nonUiThreadImage = gameObject.getImage();
            ImageSource imageSourceFrozen = (ImageSource) nonUiThreadImage.Source.GetAsFrozen();
            double imageWidth = nonUiThreadImage.Width;
            double imageHeight = nonUiThreadImage.Height;

            String uid = Guid.NewGuid().ToString();

            this.targetCanvas.Dispatcher.Invoke(new Action(
                delegate()
                {
                    Image uiThreadImage = new Image();
                    uiThreadImage.Uid = uid;
                    uiThreadImage.Source = imageSourceFrozen;
                    uiThreadImage.Width = imageWidth;
                    uiThreadImage.Height = imageHeight;

                    this.targetCanvas.Children.Add(uiThreadImage);

                    Canvas.SetTop(uiThreadImage, gameObject.getYPosition());
                    Canvas.SetLeft(uiThreadImage, gameObject.getXPosition());
                    Canvas.SetZIndex(uiThreadImage, zIndex);

                }
             ));

            this.uiThreadImageUidMapByGameObject.Add(gameObject, uid);

            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        public void canvasUpdateImage(IGameObject gameObject, int zIndex)
        {

            Debug.Assert(gameObject != null, "expected gameObject != null");
            Debug.Assert(this.uiThreadImageUidMapByGameObject.ContainsKey(gameObject), "you must display this gameObject before try to update it.");

            this.canvasRemoveImage(gameObject);
            this.canvasDisplayImage(gameObject, zIndex);


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        public void canvasRemoveImage(IGameObject gameObject)
        {

            Debug.Assert(gameObject != null, "expected gameObject != null");
            Debug.Assert(this.uiThreadImageUidMapByGameObject.ContainsKey(gameObject), "you must display this gameObject before try to remove from the canvas.");


            String uiThreadImageUid = this.uiThreadImageUidMapByGameObject[gameObject];


            this.targetCanvas.Dispatcher.Invoke(new Action(
               delegate()
               {

                   UIElement uiElementToRemove = null;
                   foreach (UIElement uiElement0 in this.targetCanvas.Children)
                   {
                       if (uiElement0.Uid.Equals(uiThreadImageUid))
                       {
                           uiElementToRemove = uiElement0;
                           break;
                       }
                   }

                   Debug.Assert(uiElementToRemove != null, "the specified gameObject is no longer present into the canvas! unexpected remove! use ISceneManager.removeFromCanvas for a safety remove.");
                   this.targetCanvas.Children.Remove(uiElementToRemove);
               }
            ));


            this.uiThreadImageUidMapByGameObject.Remove(gameObject);

        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="sceneNode"></param>
        private void recursiveRemoveGameObject(SceneNode sceneNode)
        {
            if (sceneNode.getSceneNodeGameObject() != null)
            {
                this.deleteGameObject(sceneNode.getSceneNodeGameObject(), this.gameObjectListMapByTypeEnum);
                this.deleteGameObject(sceneNode.getSceneNodeGameObject(), this.collidableGameObjectListMapByTypeEnum);
            }

            foreach (SceneNode childSceneNode0 in sceneNode.getChildList())
            {
                this.recursiveRemoveGameObject(childSceneNode0);
            }

            sceneNode.getParent().removeChild(sceneNode);
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

            GameObjectTypeEnum gameObjectTypeEnum = gameObject.getGameObjectTypeEnum();


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


            GameObjectTypeEnum gameObjectTypeEnum = gameObject.getGameObjectTypeEnum(); 

            if (targetGameObjectListMapByTypeEnum.ContainsKey(gameObjectTypeEnum)
                    && targetGameObjectListMapByTypeEnum[gameObjectTypeEnum].Contains(gameObject))
            {
                targetGameObjectListMapByTypeEnum[gameObjectTypeEnum].Remove(gameObject);
            }

        }

        


        



        


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="uiElement"></param>
        //public void registerUiElement(UIElement uiElement)
        //{
        //    Debug.Assert(!this.uiElementMapByUid.ContainsKey(uiElement.Uid), "uiElement already present in uiElementListMapByUid");

        //    this.uiElementMapByUid.Add(uiElement.Uid, uiElement);

        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="uid"></param>
        ///// <returns></returns>
        //public UIElement getUiElementByUid(String uid)
        //{
        //    Debug.Assert(this.uiElementMapByGameObjectUid.ContainsKey(uid), "unkown uiElement");

        //    return this.uiElementMapByGameObjectUid[uid];
        //}



        

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="uiElement"></param>
        ///// <param name="targetGameObject"></param>
        //public void boundUiElementToGameObject(UIElement uiElement, IGameObject targetGameObject)
        //{

        //    if (!this.uiElementListMapByGameObjectUid.ContainsKey(targetGameObject.getUid()))
        //    {
        //        this.uiElementListMapByGameObjectUid.Add(targetGameObject.getUid(), new List<UIElement>(5));
        //    }

        //    this.uiElementListMapByGameObjectUid[targetGameObject.getUid()].Add(uiElement);

        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="targetGameObject"></param>
        //public void unboundAllUiElementFromGameObject(IGameObject targetGameObject)
        //{
        //    Debug.Assert(this.uiElementListMapByGameObjectUid.ContainsKey(targetGameObject.getUid()), "this gameObject is not bound to any uiElement");

        //    this.uiElementListMapByGameObjectUid.Remove(targetGameObject.getUid());

        //}



        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="targetGameObject"></param>
        ///// <param name="targetUiElement"></param>
        //public void unboundUiElementFromGameObject(IGameObject targetGameObject, UIElement targetUiElement)
        //{
        //    Debug.Assert(this.uiElementListMapByGameObjectUid.ContainsKey(targetGameObject.getUid()), "this gameObject is not bound to any uiElement");

        //    List<UIElement> boundedUiElementList = this.uiElementListMapByGameObjectUid[targetGameObject.getUid()];
        //    Debug.Assert(boundedUiElementList.Contains(targetUiElement), "the specified uiElement is not bound to the specified gameObject");

        //    boundedUiElementList.Remove(targetUiElement);
        //}



        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="gameObject"></param>
        ///// <returns></returns>
        //public List<UIElement> getUiElementListBoundToGameObject(IGameObject gameObject)
        //{
        //    Debug.Assert(this.uiElementListMapByGameObjectUid.ContainsKey(gameObject.getUid()), "this gameObject is not bound to any uiElement");

        //    return new List<UIElement>(this.uiElementListMapByGameObjectUid[gameObject.getUid()]);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="uiElement"></param>
        //public void unregisterUiElement(UIElement uiElement)
        //{
        //    Debug.Assert(this.uiElementMapByUid.ContainsKey(uiElement.Uid), "unkown uiElement");

        //    this.uiElementMapByUid.Remove(uiElement.Uid);

        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="gameObject"></param>
        //public void removeGameObject(IGameObject gameObject)
        //{
        //    this.deleteGameObject(gameObject, this.gameObjectListMapByTypeEnum);
        //    this.deleteGameObject(gameObject, this.collidableGameObjectListMapByTypeEnum);

        //    //if (this.uiElementListMapByGameObjectUid.ContainsKey(gameObject.getUid()))
        //    //{
        //    //    this.unboundAllUiElementFromGameObject(gameObject);
        //    //}

        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public Dictionary<GameObjectTypeEnum, List<IGameObject>> getGameObjectListMapByTypeEnum()
        //{
        //    return this.gameObjectListMapByTypeEnum;
        //}



        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public Dictionary<GameObjectTypeEnum, List<IGameObject>> getCollidableGameObjectListMapByTypeEnum()
        //{
        //    return this.collidableGameObjectListMapByTypeEnum;
        //}



        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public Canvas getTargetCanvas()
        //{
        //    return this.targetCanvas;
        //}



    }
}
