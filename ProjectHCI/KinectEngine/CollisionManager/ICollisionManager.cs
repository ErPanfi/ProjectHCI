using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectHCI.KinectEngine
{
    public interface ICollisionManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<KeyValuePair<IGameObject, IGameObject>> createCollisionList();

        void setCollisionToHandle(ISet<KeyValuePair<GameObjectTypeEnum, GameObjectTypeEnum>> typeEnumCollidablePairSet);

    }
}
