﻿using System;
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

            //set position relative to the parent.
            if (parentGameObject != null)
            {
                double xPositionRelativeToParent = parentGameObject.getXPosition() + gameObject.getXPosition();
                double yPositionRelativeToParent = parentGameObject.getYPosition() + gameObject.getYPosition();
                gameObject.setPosition(xPositionRelativeToParent, yPositionRelativeToParent);
            }

            //update the geometry position relative to the gameObject position.
            if (gameObject.getBoundingBoxGeometry() != null)
            {
                gameObject.getBoundingBoxGeometry().Transform = new TranslateTransform(gameObject.getXPosition(), gameObject.getYPosition());
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
            Stretch streachImage = nonUiThreadImage.Stretch;
            StretchDirection streachDirection = nonUiThreadImage.StretchDirection;
            Transform renderTransformFrozen = (Transform)nonUiThreadImage.RenderTransform.GetAsFrozen();

            String uid = Guid.NewGuid().ToString();

            this.targetCanvas.Dispatcher.Invoke(new Action(
                delegate()
                {
                    Image uiThreadImage = new Image();
                    uiThreadImage.Uid = uid;
                    uiThreadImage.Source = imageSourceFrozen;
                    uiThreadImage.Width = imageWidth;
                    uiThreadImage.Height = imageHeight;
                    uiThreadImage.Stretch = streachImage;
                    uiThreadImage.StretchDirection = streachDirection;
                    uiThreadImage.RenderTransform = renderTransformFrozen;

                    this.targetCanvas.Children.Add(uiThreadImage);
                    this.uiThreadSetCanvasPosition(uiThreadImage, gameObject.getXPosition(), gameObject.getYPosition(), zIndex);


                }
             ));

            this.uiThreadImageUidMapByGameObject.Add(gameObject, uid);

            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        public void canvasUpdateImage(IGameObject gameObject)
        {

            Debug.Assert(gameObject != null, "expected gameObject != null");
            Debug.Assert(this.uiThreadImageUidMapByGameObject.ContainsKey(gameObject), "you must display this gameObject before try to update it.");

            String uiThreadImageUid = this.uiThreadImageUidMapByGameObject[gameObject];

            ImageSource imageSourceFrozen = (ImageSource)gameObject.getImage().Source.GetAsFrozen();
            double imageWidth = gameObject.getImage().Width;
            double imageHeight = gameObject.getImage().Height;
            Stretch streachImage = gameObject.getImage().Stretch;
            StretchDirection streachDirection = gameObject.getImage().StretchDirection;
            Transform renderTransformFrozen = (Transform)gameObject.getImage().RenderTransform.GetAsFrozen();

            

            this.targetCanvas.Dispatcher.Invoke(new Action(
                delegate()
                {
                    UIElement uiElement = this.uiThreadGetUiElement(uiThreadImageUid);
                    int zIndex = Canvas.GetZIndex(uiElement);
                    
                    this.targetCanvas.Children.Remove(uiElement);

                    Image uiThreadImage = new Image();
                    uiThreadImage.Uid = uiThreadImageUid;
                    uiThreadImage.Source = imageSourceFrozen;
                    uiThreadImage.Width = imageWidth;
                    uiThreadImage.Height = imageHeight;
                    uiThreadImage.Stretch = streachImage;
                    uiThreadImage.StretchDirection = streachDirection;
                    uiThreadImage.RenderTransform = renderTransformFrozen;

                    this.targetCanvas.Children.Add(uiThreadImage);
                    this.uiThreadSetCanvasPosition(uiThreadImage, gameObject.getXPosition(), gameObject.getYPosition(), zIndex);

                }
             ));
 
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

                   this.targetCanvas.Children.Remove(this.uiThreadGetUiElement(uiThreadImageUid));
               }
            ));


            this.uiThreadImageUidMapByGameObject.Remove(gameObject);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="xCanvasPosition"></param>
        /// <param name="yCanvasPosition"></param>
        public void applyTranslation(IGameObject gameObject, double xOffset, double yOffset)
        {

            Debug.Assert(gameObject != null, "expected gameObject != null");
            SceneNode targetSceneNode = SceneNode.getSceneNodeByGameObject(this.sceneNodeTree, gameObject);
            Debug.Assert(targetSceneNode != null, "gameObject not found in the scene node tree");


            this.recursiveApplyTranslation(targetSceneNode, xOffset, yOffset);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="clockwiseDegreeAngle"></param>
        /// <param name="xRelativeRotationCenter"></param>
        /// <param name="yRelativeRotationCenter"></param>
        public void applyRotation(IGameObject gameObject, double clockwiseDegreeAngle, double xRelativeRotationCenter, double yRelativeRotationCenter)
        {
            Debug.Assert(gameObject != null, "expected gameObject != null");
            SceneNode targetSceneNode = SceneNode.getSceneNodeByGameObject(this.sceneNodeTree, gameObject);
            Debug.Assert(targetSceneNode != null, "gameObject not found in the scene node tree");

            this.recursiveApplyRotation(targetSceneNode, clockwiseDegreeAngle, xRelativeRotationCenter, yRelativeRotationCenter);

        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double getCanvasWidth()
        {
            return this.targetCanvas.RenderSize.Width;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double getCanvasHeight()
        {
            return this.targetCanvas.RenderSize.Height;
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
        /// <param name="sceneNode"></param>
        /// <param name="transformGroup"></param>
        /// <param name="zIndex"></param>
        private void recursiveApplyTranslation(SceneNode sceneNode, double xOffset, double yOffset)
        {
            IGameObject gameObject = sceneNode.getSceneNodeGameObject();
            gameObject.setPosition(gameObject.getXPosition() + xOffset, gameObject.getYPosition() + yOffset);

            if (gameObject.getBoundingBoxGeometry() != null)
            {
                gameObject.getBoundingBoxGeometry().Transform = new TranslateTransform(gameObject.getXPosition(), 
                                                                                       gameObject.getYPosition());
            }


            if (this.uiThreadImageUidMapByGameObject.ContainsKey(gameObject))
            {
                this.canvasUpdateImage(gameObject);
            }


            foreach (SceneNode childSceneNode0 in sceneNode.getChildList())
            {
                this.recursiveApplyTranslation(childSceneNode0, xOffset, yOffset);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetSceneNode"></param>
        /// <param name="clockwiseDegreeAngle"></param>
        /// <param name="xRelativeRotationCenter"></param>
        /// <param name="yRelativeRotationCenter"></param>
        public void recursiveApplyRotation(SceneNode targetSceneNode, double clockwiseDegreeAngle, double xRelativeRotationCenter, double yRelativeRotationCenter)
        {

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiElementUid"></param>
        /// <returns></returns>
        private UIElement uiThreadGetUiElement(String uiElementUid)
        {
            UIElement uiElement = null;
            foreach (UIElement uiElement0 in this.targetCanvas.Children)
            {
                if (uiElement0.Uid.Equals(uiElementUid))
                {
                    uiElement = uiElement0;
                    break;
                }
            }
            Debug.Assert(uiElement != null, "no uiElement found with the specified uid");

            return uiElement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiElement"></param>
        /// <param name="xPosition"></param>
        /// <param name="yPosition"></param>
        /// <param name="zIndex"></param>
        private void uiThreadSetCanvasPosition(UIElement uiElement, double xPosition, double yPosition, int zIndex)
        {
            Canvas.SetTop(uiElement, yPosition);
            Canvas.SetLeft(uiElement, xPosition);
            Canvas.SetZIndex(uiElement, zIndex);
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


    }
}
