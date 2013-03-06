﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ProjectHCI.KinectEngine;
using System.Runtime.InteropServices;
using ProjectHCI.Utility;
using System.Diagnostics;


namespace ProjectHCI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Canvas targetCanvas;
        private Dictionary<String, UIElement> uiElementMapByUid;


        public MainWindow()
        {
            InitializeComponent();

            targetCanvas = windowsCanvas; //windowsCanvas is defined in the xaml.
            uiElementMapByUid = new Dictionary<String, UIElement>();


            ISceneBrain sceneBrain = new SceneBrain();
            

            IUpdateRenderer updateRenderer = new UpdateRenderer(sceneBrain);

            updateRenderer.setDisplayGameObjectEventHandler(new EventHandler<GameObjectEventArgs>(this.displayGameObject));
            updateRenderer.setRemoveGameObjectEventHandler(new EventHandler<GameObjectEventArgs>(this.removeGameObject));
            updateRenderer.setUpdateGameObjectEventHandler(new EventHandler<GameObjectEventArgs>(this.updateGameObject));


            ICollisionManager collisionManager = new CollisionManager(sceneBrain);

            HashSet<KeyValuePair<Type, Type>> collidableTypeHashSet = new HashSet<KeyValuePair<Type, Type>>();
            collidableTypeHashSet.Add(new KeyValuePair<Type, Type>(typeof(UserGameObject), typeof(UserFriendlyGameObject)));
            collidableTypeHashSet.Add(new KeyValuePair<Type, Type>(typeof(UserGameObject), typeof(NotUserFriendlyGameObject)));
            collidableTypeHashSet.Add(new KeyValuePair<Type, Type>(typeof(NotUserFriendlyGameObject), typeof(UserFriendlyGameObject)));

            collisionManager.setCollisionToHandle(collidableTypeHashSet);



            //ISpawnerManager spawnerManager = new FakeSpawnerManager(sceneBrain);
            ISpawnerManager spawnerManager = new SpawnerManager(sceneBrain);
            ITimerManager timerManager = new TimerManager(sceneBrain);

            GameLoop gameLoop = new GameLoop(sceneBrain, spawnerManager, updateRenderer, timerManager, collisionManager);
            gameLoop.start();

       

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
        public void deregisterUiElement(UIElement uiElement)
        {
            Debug.Assert(this.uiElementMapByUid.ContainsKey(uiElement.Uid), "unkown uiElement");

            this.uiElementMapByUid.Remove(uiElement.Uid);

        }


        //public enum RGB
        //{
        //    Blue = 0,
        //    Green,
        //    Red
        //};


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        public void displayGameObject(object sender, GameObjectEventArgs gameObjectEventArgs)
        {
            
            Dispatcher.Invoke(new Action(
                delegate()
                {

                    IGameObject gameObject = gameObjectEventArgs.getGameObject();
                    gameObject.onRendererDisplayDelegate(targetCanvas, this);


                    //IGameObject gameObject = gameObjectEventArgs.getGameObject();

                    ////******fake change color
                    //RgbData rgbData = BitmapUtility.getRgbData((BitmapSource)gameObject.getImageSource());

                    //for (int i = 0; i < rgbData.dataLength; i += 4)
                    //{
                    //    rgbData.rawRgbByteArray[i + (int)RGB.Blue] = 0;	 //blue
                    //    rgbData.rawRgbByteArray[i + (int)RGB.Green] = 0; //green;
                    //}
                    //BitmapSource bitmapSource = BitmapUtility.createBitmapSource(rgbData);
                    ////********


                    //Image image = new Image();
                    //image.Source = bitmapSource;
                    //image.Uid = gameObject.getUid();

                    //this.targetCanvas.Children.Add(image);

                    //Canvas.SetTop(image, gameObject.getGeometry().Bounds.X);
                    //Canvas.SetLeft(image, gameObject.getGeometry().Bounds.Y);
                }
            ));


        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        public void updateGameObject(object sender, GameObjectEventArgs gameObjectEventArgs)
        {
            Dispatcher.Invoke(new Action(
                delegate()
                {

                    IGameObject gameObject = gameObjectEventArgs.getGameObject();
                    gameObject.onRendererUpdateDelegate(targetCanvas, this);

                }
            ));
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        public void removeGameObject(object sender, GameObjectEventArgs gameObjectEventArgs)
        {
            
            Dispatcher.Invoke(new Action(
                delegate()
                {

                    IGameObject gameObject = gameObjectEventArgs.getGameObject();
                    gameObject.onRendererRemoveDelegate(targetCanvas, this);
                    
                }
            ));

        }

    }
}
