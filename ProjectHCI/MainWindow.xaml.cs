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


            ISceneBrain sceneBrain = new SceneBrain();
            ISpawnerManager spawnerManager = new FakeSpawnerManager(sceneBrain);
            IUpdateRenderer updateRenderer = new FakeUpdateRenderer(sceneBrain);
            ITimerManager timerManager = new TimerManager(sceneBrain);
            ICollisionManager collisionManager = new FakeCollisionManager(sceneBrain);


            updateRenderer.setDisplayGameObjectEventHandler(new EventHandler<GameObjectEventArgs>(this.displayGameObject));
            updateRenderer.setRemoveGameObjectEventHandler(new EventHandler<GameObjectEventArgs>(this.removeGameObject));
            updateRenderer.setUpdateGameObjectEventHandler(new EventHandler<GameObjectEventArgs>(this.updateGameObject));


            GameLoop gameLoop = new GameLoop(sceneBrain, spawnerManager, updateRenderer, timerManager, collisionManager);
            gameLoop.start();

       

        }


        public enum RGB
        {
            Blue = 0,
            Green,
            Red
        };



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
                    String uid = gameObject.getUid();

                    
                    foreach (UIElement childUiElement0 in this.targetCanvas.Children)
                    {
                        if (childUiElement0.Uid.Equals(uid))
                        {
                            this.targetCanvas.Children.Remove(childUiElement0);
                            break;
                        }
                    }


                }
            ));

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        public void updateGameObject(object sender, GameObjectEventArgs gameObjectEventArgs)
        {
            //TODO
            //System.Diagnostics.Debug.WriteLine("*******************update");
        }



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

                    //******fake change color
                    RgbData rgbData = BitmapUtility.getRgbData((BitmapSource)gameObject.getImageSource());

                    for (int i = 0; i < rgbData.dataLength; i += 4)
                    {
                        rgbData.rawRgbByteArray[i + (int)RGB.Blue] = 0;	 //blue
                        rgbData.rawRgbByteArray[i + (int)RGB.Green] = 0; //green;
                    }
                    BitmapSource bitmapSource = BitmapUtility.createBitmapSource(rgbData);
                    //********


                    Image image = new Image();
                    image.Source = bitmapSource;
                    image.Uid = gameObject.getUid();

                    this.targetCanvas.Children.Add(image);

                    Canvas.SetTop(image, gameObject.getGeometry().Bounds.X);
                    Canvas.SetLeft(image, gameObject.getGeometry().Bounds.Y);
                }
            ));


        }
    }
}
