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
        /// 
        /// </summary>
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
            gameLoopThread.IsBackground = true; //ensures that will be terminated on application close
         
        }

        /// <summary>
        /// 
        /// </summary>
        public void start()
        {
            this.gameLoopThread.Start();
        }



        /// <summary>
        /// 
        /// </summary>
        private void runLoop()
        {

            this.gameStillRunning = true;
            this.lastTimeMillis = System.Environment.TickCount;

            //Component initialization
            //ISceneBrain sceneBrain = new SceneBrain();
            //ISpawnerManager spawnerManager = new FakeSpawnerManager(sceneBrain);
            //IUpdateRenderer updateRenderer = new FakeUpdateRenderer(sceneBrain);
            //ITimerManager timerManager = new TimerManager(sceneBrain);
            //ICollisionManager collisionManager = new FakeCollisionManager(sceneBrain);
            

            while (gameStillRunning)
            {

                int currentTimeMillis = System.Environment.TickCount;
                int deltaTimeMillis = currentTimeMillis - this.lastTimeMillis; 

                this.lastTimeMillis = currentTimeMillis;



                this.spawnerManager.awaken();
                this.timerManager.tick(deltaTimeMillis);
                List<KeyValuePair<IGameObject, IGameObject>> collidedGameObjectPairList = this.collisionManager.createCollisionList();
                this.sceneBrain.think(deltaTimeMillis, collidedGameObjectPairList);
                this.updateRenderer.drawObject();


               
                //gameStillRunning = false;
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
