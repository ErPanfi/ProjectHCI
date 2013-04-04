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
            HashSet<KeyValuePair<String, String>> collidableTypeEnumHashSet = new HashSet<KeyValuePair<String, String>>();
            collidableTypeEnumHashSet.Add(new KeyValuePair<String, String>(Tags.USER_TAG, Tags.FRUIT_TAG));
            collidableTypeEnumHashSet.Add(new KeyValuePair<String, String>(Tags.USER_TAG, Tags.BUTTON_TAG));
            collidableTypeEnumHashSet.Add(new KeyValuePair<String, String>(Tags.USER_TAG, Tags.CUT_TAG));
            collidableTypeEnumHashSet.Add(new KeyValuePair<String, String>(Tags.CUT_TAG, Tags.FRUIT_TAG));
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

    }
}
