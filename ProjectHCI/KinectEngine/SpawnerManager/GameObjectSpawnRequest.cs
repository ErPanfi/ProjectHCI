using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectHCI.KinectEngine
{
    public class GameObjectSpawnRequest
    {

        private IGameObject gameObjectToSpawn;
        private IGameObject parentGameObject;


        public GameObjectSpawnRequest(IGameObject gameObjectToSpawn, IGameObject parentGameObject)
        {

            this.gameObjectToSpawn = gameObjectToSpawn;
            this.parentGameObject = parentGameObject;
            
        }


        public IGameObject getGameObjectToSpawn()
        {
            return this.gameObjectToSpawn;
        }

        public IGameObject getParentGameObject()
        {
            return this.parentGameObject;
        }

    }
}
