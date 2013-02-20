using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectHCI.KinectEngine
{
    public class FakeCollisionManager : ICollisionManager
    {

        private ISceneBrain sceneBrain;


        public FakeCollisionManager(ISceneBrain sceneBrain)
        {
            this.sceneBrain = sceneBrain;
        }




        public List<KeyValuePair<IGameObject, IGameObject>> createCollisionList()
        {

            //Fake implementation

            List<IGameObject> userFriendlyGameObjectList = this.sceneBrain.getCollaidableGameObjectList(typeof(UserFriendlyGameObject));


            return null;
        }

    }
}
