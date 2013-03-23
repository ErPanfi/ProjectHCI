using System;
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
using ProjectHCI.ReverseFruitNinja;


namespace ProjectHCI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Canvas targetCanvas;


        public MainWindow()
        {
            InitializeComponent();

            targetCanvas = windowsCanvas; //windowsCanvas is defined in the xaml.


            ISceneManager sceneManager = new SceneManager(targetCanvas);
            ISceneBrain sceneBrain = new SceneBrain();            
            IUpdateRenderer updateRenderer = new UpdateRenderer();

            ICollisionManager collisionManager = new CollisionManager();
            HashSet<KeyValuePair<GameObjectTypeEnum, GameObjectTypeEnum>> collidableTypeEnumHashSet = new HashSet<KeyValuePair<GameObjectTypeEnum, GameObjectTypeEnum>>();
            collidableTypeEnumHashSet.Add(new KeyValuePair<GameObjectTypeEnum, GameObjectTypeEnum>(GameObjectTypeEnum.UserObject, GameObjectTypeEnum.FriendlyObject));
            collidableTypeEnumHashSet.Add(new KeyValuePair<GameObjectTypeEnum, GameObjectTypeEnum>(GameObjectTypeEnum.UserObject, GameObjectTypeEnum.UIObject));
            collidableTypeEnumHashSet.Add(new KeyValuePair<GameObjectTypeEnum, GameObjectTypeEnum>(GameObjectTypeEnum.UserObject, GameObjectTypeEnum.UnfriendlyObject));
            collidableTypeEnumHashSet.Add(new KeyValuePair<GameObjectTypeEnum, GameObjectTypeEnum>(GameObjectTypeEnum.UnfriendlyObject, GameObjectTypeEnum.FriendlyObject));
            collisionManager.setCollisionToHandle(collidableTypeEnumHashSet);

            ISpawnerManager spawnerManager = new MainMenuSpawnerManager();
            ITimerManager timerManager = new TimerManager();



            GameLoop gameLoop = GameLoop.getGameLoopSingleton();
            gameLoop.setCollisionManager(collisionManager);
            gameLoop.setSceneBrain(sceneBrain);
            gameLoop.setSceneManager(sceneManager);
            gameLoop.setSpawnerManager(spawnerManager);
            gameLoop.setTimerManager(timerManager);
            gameLoop.setUpdateRenderer(updateRenderer);
            

            gameLoop.start();

       

        }

        


        //public enum RGB
        //{
        //    Blue = 0,
        //    Green,
        //    Red
        //};


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="gameObject"></param>
        //public void displayGameObject(object sender, GameObjectEventArgs gameObjectEventArgs)
        //{
            
        //    Dispatcher.Invoke(new Action(
        //        delegate()
        //        {

        //            IGameObject gameObject = gameObjectEventArgs.getGameObject();
        //            gameObject.onRendererDisplayDelegate();


        //            //IGameObject gameObject = gameObjectEventArgs.getGameObject();

        //            ////******fake change color
        //            //RgbData rgbData = BitmapUtility.getRgbData((BitmapSource)gameObject.getImageSource());

        //            //for (int i = 0; i < rgbData.dataLength; i += 4)
        //            //{
        //            //    rgbData.rawRgbByteArray[i + (int)RGB.Blue] = 0;	 //blue
        //            //    rgbData.rawRgbByteArray[i + (int)RGB.Green] = 0; //green;
        //            //}
        //            //BitmapSource bitmapSource = BitmapUtility.createBitmapSource(rgbData);
        //            ////********


        //            //Image image = new Image();
        //            //image.Source = bitmapSource;
        //            //image.Uid = gameObject.getUid();

        //            //this.targetCanvas.Children.Add(image);

        //            //Canvas.SetTop(image, gameObject.getGeometry().Bounds.X);
        //            //Canvas.SetLeft(image, gameObject.getGeometry().Bounds.Y);
        //        }
        //    ));


        //}



    }
}
