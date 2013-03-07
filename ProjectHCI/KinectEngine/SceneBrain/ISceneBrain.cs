using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectHCI.KinectEngine
{
    public interface ISceneBrain
    {

        void addGameObject(IGameObject gameObject);

        void addCollidableGameObject(IGameObject collidableGameObject);



        List<IGameObject> getCollaidableGameObjectList(GameObjectTypeEnum gameObjectType);

        void cleanDeadGameObject();

        void think(int deltaTimeMillis, List<KeyValuePair<IGameObject, IGameObject>> collidedGameObjectPairList);



        Dictionary<GameObjectTypeEnum, List<IGameObject>> getAllGameObjectListMapByType();

        Dictionary<GameObjectTypeEnum, List<IGameObject>> getCollidableGameObjectListMapByType();
        
        int getMaxNumberOfChopAllowed();

        int getMaxNumberOfUserFriendlyGameObjectAllowed();

        float getBonusPercentiege();
    }
}
