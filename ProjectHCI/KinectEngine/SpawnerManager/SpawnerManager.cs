using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ProjectHCI.KinectEngine
{
    public class SpawnerManager : ISpawnerManager
    {

        protected bool _firstTime;
        private ISceneManager _sceneManager;
        private ConcurrentQueue<GameObjectSpawnRequest> _spawnRequestConcurrentQueue;
        private ConcurrentQueue<IGameObject> _removeGameObjectRequestConcurrentQueue;


        public SpawnerManager()
        {
            this._sceneManager = null; // post init
            this._spawnRequestConcurrentQueue = new ConcurrentQueue<GameObjectSpawnRequest>();
            this._removeGameObjectRequestConcurrentQueue = new ConcurrentQueue<IGameObject>();
            this._firstTime = true;
        }



        public void awaken()
        {
            if (this._firstTime)
            {
                this._sceneManager = GameLoop.getSceneManager();

                this.addGameObjectToScene(this.spawnGameObjectsOnStart());   
                this._firstTime = false;
            }


            this.processSpecialSpawnRequest();
            this.addGameObjectToScene(this.spawnGameObjectsPerFrame());
            this.processSpecialRemoveRequest();

        }




        protected void addGameObjectToScene(List<KeyValuePair<IGameObject, IGameObject>> gameObjectParentGameObjectPairList)
        {
            Debug.Assert(gameObjectParentGameObjectPairList != null, "Object to insert list must be != null");

            foreach (KeyValuePair<IGameObject, IGameObject> gameObjectParentGameObjectPair0 in gameObjectParentGameObjectPairList)
            {
                IGameObject childGameObject0 = gameObjectParentGameObjectPair0.Key;
                IGameObject parentGameObject0 = gameObjectParentGameObjectPair0.Value;

                Debug.Assert(this._sceneManager.gameObjectExist(parentGameObject0), "Parent game object must exist. hint: put parent creation first.");
                Debug.Assert(!this._sceneManager.gameObjectExist(childGameObject0), "childGameObject already exist");
                Debug.Assert(childGameObject0 != null, "expected childGameObject0 != null");

                this._sceneManager.addGameObject(gameObjectParentGameObjectPair0.Key, gameObjectParentGameObjectPair0.Value);
            }
        }




        public void specialRequestToSpawn(GameObjectSpawnRequest gameObjectSpawnRequest)
        {
            this._spawnRequestConcurrentQueue.Enqueue(gameObjectSpawnRequest);
        }



        protected void processSpecialSpawnRequest()
        {

            //single shot consuming

            GameObjectSpawnRequest gameObjectSpawnRequest;
            if (this._spawnRequestConcurrentQueue.TryDequeue(out gameObjectSpawnRequest))
            {

                List<KeyValuePair<IGameObject, IGameObject>> gameObjectParentGameObjectPairList = new List<KeyValuePair<IGameObject, IGameObject>>();
                gameObjectParentGameObjectPairList.Add(new KeyValuePair<IGameObject, IGameObject>(gameObjectSpawnRequest.getGameObjectToSpawn(), gameObjectSpawnRequest.getParentGameObject()));

                this.addGameObjectToScene(gameObjectParentGameObjectPairList);

            }

        }

        /// <summary>
        /// This is a request 
        /// </summary>
        /// <param name="gameObject">an object</param>
        public void specialRequestToKillGameObject(IGameObject gameObject)
        {
            this._removeGameObjectRequestConcurrentQueue.Enqueue(gameObject);
        }


        protected void processSpecialRemoveRequest()
        {

            //single shot consuming

            IGameObject gameObjectToRemove;
            if (this._removeGameObjectRequestConcurrentQueue.TryDequeue(out gameObjectToRemove))
            {
                this._sceneManager.removeGameObject(gameObjectToRemove);
            }

        }





        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual List<KeyValuePair<IGameObject, IGameObject>> spawnGameObjectsOnStart()
        {
            return new List<KeyValuePair<IGameObject, IGameObject>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual List<KeyValuePair<IGameObject, IGameObject>> spawnGameObjectsPerFrame()
        {
            return new List<KeyValuePair<IGameObject, IGameObject>>();
        }


        






    }
}
