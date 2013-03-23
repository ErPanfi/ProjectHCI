using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectHCI.KinectEngine
{
    /// <summary>
    /// This interface implements the spawner behaviour
    /// </summary>
    public interface ISpawnerManager
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObjectSpawnRequest"></param>
        void specialRequestToSpawn(GameObjectSpawnRequest gameObjectSpawnRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        void specialRequestToKillGameObject(IGameObject gameObject);


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //List<KeyValuePair<IGameObject, IGameObject>> onStartSpawnGameObjects();


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //List<KeyValuePair<IGameObject, IGameObject>> spawnGameObjects();


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="gameObjectTypeEnum"></param>
        //void removeSpawnedGameObjectByType(GameObjectTypeEnum gameObjectTypeEnum);



        /// <summary>
        /// 
        /// </summary>
        void awaken();
    }
}
