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



        List<IGameObject> getCollaidableGameObjectList(Type gameObjectClassType);

        void cleanDeadGameObject();

        void think(int deltaTimeMillis, List<KeyValuePair<IGameObject, IGameObject>> collidedGameObjectPairList);



        Dictionary<Type, List<IGameObject>> getAllGameObjectListMapByType();

        Dictionary<Type, List<IGameObject>> getCollidableGameObjectListMapByType();
        
        int getMaxNumberOfChopAllowed();

        int getMaxNumberOfUserFriendlyGameObjectAllowed();

        float getBonusPercentiege();
    }
}
