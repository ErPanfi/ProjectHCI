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

        void cleanDeadGameObject();

        void think(int deltaTimeMillis, List<KeyValuePair<IGameObject, IGameObject>> collidedGameObjectPairList);



        //Dictionary<GameObjectTypeEnum, List<IGameObject>> getAllGameObjectListMapByTypeEnum();

        //Dictionary<GameObjectTypeEnum, List<IGameObject>> getCollidableGameObjectListMapByTypeEnum();
        
        int getMaxNumberOfChopAllowed();

        int getMaxNumberOfUserFriendlyGameObjectAllowed();

        float getBonusPercentiege();
    }
}
