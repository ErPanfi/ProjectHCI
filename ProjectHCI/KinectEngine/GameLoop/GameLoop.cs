using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

namespace ProjectHCI.KinectEngine
{
    public class GameLoop
    {
        
        private bool gameStillRunning;
        private int lastTimeMillis;

        private ISceneBrain sceneBrain;
        private ISpawnerManager spawnerManager;
        private IUpdateRenderer updateRenderer;
        private ITimerManager timerManager;
        private ICollisionManager collisionManager;


        private Thread gameLoopThread;

        /// <summary>
        /// Game loop constructor, needs all component to start.
        /// </summary>
        /// <param name="sceneBrain"></param>
        /// <param name="spawnerManager"></param>
        /// <param name="updateRenderer"></param>
        /// <param name="timerManager"></param>
        /// <param name="collisionManager"></param>
        public GameLoop(ISceneBrain sceneBrain,
                        ISpawnerManager spawnerManager,
                        IUpdateRenderer updateRenderer,
                        ITimerManager timerManager,
                        ICollisionManager collisionManager)
        {
            this.gameStillRunning = true;
            this.lastTimeMillis = 0;


            this.sceneBrain = sceneBrain;
            this.spawnerManager = spawnerManager;
            this.updateRenderer = updateRenderer;
            this.timerManager = timerManager;
            this.collisionManager = collisionManager;


            gameLoopThread = new Thread(new ThreadStart(this.runLoop));
            gameLoopThread.Name = "GameLoopThread";
            gameLoopThread.IsBackground = true; //ensures that will be terminated on application close
         
        }

        /// <summary>
        /// Starts the game loop
        /// </summary>
        public void start()
        {
            this.gameLoopThread.Start();
        }



        /// <summary>
        /// The run method of the gameLoopThread
        /// </summary>
        private void runLoop()
        {

            this.gameStillRunning = true;
            this.lastTimeMillis = System.Environment.TickCount;
           

            while (gameStillRunning)
            //for(int i = 0; i < 100; i++)
            {

                int currentTimeMillis = System.Environment.TickCount;
                int deltaTimeMillis = currentTimeMillis - this.lastTimeMillis; 

                this.lastTimeMillis = currentTimeMillis;



                this.spawnerManager.awaken();
                this.timerManager.tick(deltaTimeMillis);
                //List<KeyValuePair<IGameObject, IGameObject>> collidedGameObjectPairList = this.collisionManager.createCollisionList();
                List<KeyValuePair<IGameObject, IGameObject>> collidedGameObjectPairList = null;
                this.sceneBrain.think(deltaTimeMillis, collidedGameObjectPairList);
                this.updateRenderer.drawObject();

            }
        }


        /// <summary>
        /// Stops the thread as soon as possible, usually this method must not be invoked, 
        /// the game-loop thread terminates automatically when the application closes.
        /// </summary>
        public void stop()
        {
            this.gameStillRunning = false;
        }

    }
}
