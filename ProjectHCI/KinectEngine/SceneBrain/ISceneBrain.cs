using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectHCI.KinectEngine
{

    public interface ISceneBrain
    {

        //void addGameObject(IGameObject gameObject);

        //void addCollidableGameObject(IGameObject collidableGameObject);



        //List<IGameObject> getCollaidableGameObjectList(GameObjectTypeEnum gameObjectTypeEnum);

        /// <summary>
        /// This ask the sceneManager to remove all the dead objects
        /// </summary>
        void cleanDeadGameObject();

        /// <summary>
        /// This method is called once per frame, it's used to evaluate and commit changes in game state
        /// </summary>
        /// <param name="collidedGameObjectPairList">A list of pair of objects that has collided in the current frame</param>
        void think(List<KeyValuePair<IGameObject, IGameObject>> collidedGameObjectPairList);



        //Dictionary<GameObjectTypeEnum, List<IGameObject>> getAllGameObjectListMapByTypeEnum();

        //Dictionary<GameObjectTypeEnum, List<IGameObject>> getCollidableGameObjectListMapByTypeEnum();

        #region methods cleaned with refactoring
        /*
        int getMaxNumberOfChopAllowed();

        int getMaxNumberOfUserFriendlyGameObjectAllowed();

        float getBonusPercentiege();
         */
        #endregion
    }
}
