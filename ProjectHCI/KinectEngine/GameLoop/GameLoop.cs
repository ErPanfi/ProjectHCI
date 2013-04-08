using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Diagnostics;
using System.Windows;

namespace ProjectHCI.KinectEngine
{
    public class GameLoop
    {

        public const int LOOP_ITERATION_WAIT_MILLIS = 20;

        private static GameLoop gameLoopSingleton;

        private static object staticLock = new object();




        private bool gameStillRunning;

        private ISceneManager sceneManager;
        private ISceneBrain sceneBrain;
        private ISpawnerManager spawnerManager;
        private IUpdateRenderer updateRenderer;
        private ITimerManager timerManager;
        private ICollisionManager collisionManager;


        private Thread gameLoopThread;

        /// <summary>
        /// 
        /// </summary>
        private GameLoop()
        {
            this.gameStillRunning = true;

            this.sceneManager = null;
            this.sceneBrain = null;
            this.spawnerManager = null;
            this.updateRenderer = null;
            this.timerManager = null;
            this.collisionManager = null;


            gameLoopThread = new Thread(new ThreadStart(this.runLoop));
            gameLoopThread.SetApartmentState(ApartmentState.STA);
            gameLoopThread.Name = "GameLoopThread";
            gameLoopThread.IsBackground = true; //ensures that will be terminated on application close

        }

        /// <summary>
        /// Starts the game loop
        /// </summary>
        public void start()
        {
            Debug.Assert(GameLoop.gameLoopSingleton.sceneManager != null, "expected sceneManager != null");
            Debug.Assert(GameLoop.gameLoopSingleton.sceneBrain != null, "expected sceneBrain != null");
            Debug.Assert(GameLoop.gameLoopSingleton.spawnerManager != null, "expected spawnerManager != null");
            Debug.Assert(GameLoop.gameLoopSingleton.updateRenderer != null, "expected updateRenderer != null");
            Debug.Assert(GameLoop.gameLoopSingleton.timerManager != null, "expected timerManager != null");
            Debug.Assert(GameLoop.gameLoopSingleton.collisionManager != null, "expected collisionManager != null");


            this.gameLoopThread.Start();
        }



        /// <summary>
        /// The run method of the gameLoopThread
        /// </summary>
        private void runLoop()
        {

            Time time = Time.getTimeSingleton();
            time.start();

            while (gameStillRunning)
            {

                Thread.Sleep(LOOP_ITERATION_WAIT_MILLIS);

                time.tick();


                this.spawnerManager.awaken();
                this.timerManager.tick();
                List<KeyValuePair<IGameObject, IGameObject>> collidedGameObjectPairList = this.collisionManager.createCollisionList();
                this.sceneBrain.think(collidedGameObjectPairList);
                this.updateRenderer.drawObject();

            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void stop()
        {
            this.gameStillRunning = false;
            Application.Current.Dispatcher.Invoke(new Action(
            delegate()
            {
                Application.Current.MainWindow.Close();
            }
            ));
        }




        public static GameLoop getGameLoopSingleton()
        {
            lock (staticLock)
            {
                if (GameLoop.gameLoopSingleton == null)
                {
                    GameLoop.gameLoopSingleton = new GameLoop();
                }

                return GameLoop.gameLoopSingleton;

            }
        }





        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ISceneManager getSceneManager()
        {
            Debug.Assert(GameLoop.gameLoopSingleton.sceneManager != null, "expected sceneManager != null");

            return GameLoop.gameLoopSingleton.sceneManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sceneManager"></param>
        public void setSceneManager(ISceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ISceneBrain getSceneBrain()
        {
            Debug.Assert(GameLoop.gameLoopSingleton.sceneBrain != null, "expected sceneBrain != null");

            return GameLoop.gameLoopSingleton.sceneBrain;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sceneBrain"></param>
        public void setSceneBrain(ISceneBrain sceneBrain)
        {
            this.sceneBrain = sceneBrain;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ISpawnerManager getSpawnerManager()
        {
            Debug.Assert(GameLoop.gameLoopSingleton.spawnerManager != null, "expected spawnerManager != null");

            return GameLoop.gameLoopSingleton.spawnerManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spawnerManager"></param>
        public void setSpawnerManager(ISpawnerManager spawnerManager)
        {
            this.spawnerManager = spawnerManager;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IUpdateRenderer getUpdateRenderer()
        {
            Debug.Assert(GameLoop.gameLoopSingleton.updateRenderer != null, "expected updateRenderer != null");

            return GameLoop.gameLoopSingleton.updateRenderer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateRenderer"></param>
        public void setUpdateRenderer(IUpdateRenderer updateRenderer)
        {
            this.updateRenderer = updateRenderer;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ITimerManager getTimerManager()
        {
            Debug.Assert(GameLoop.gameLoopSingleton.timerManager != null, "expected timerManager != null");

            return GameLoop.gameLoopSingleton.timerManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timerManager"></param>
        public void setTimerManager(ITimerManager timerManager)
        {
            this.timerManager = timerManager;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ICollisionManager getCollisionManager()
        {
            Debug.Assert(GameLoop.gameLoopSingleton.collisionManager != null, "expected collisionManager != null");

            return GameLoop.gameLoopSingleton.collisionManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collisionManager"></param>
        public void setCollisionManager(ICollisionManager collisionManager)
        {
            this.collisionManager = collisionManager;
        }


    }
}
