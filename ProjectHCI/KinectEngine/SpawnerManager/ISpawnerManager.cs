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
        /// This method is called when the spawner should spawn new objects
        /// </summary>
        void awaken();

        /// <summary>
        /// This will return a list of the game objects just spawned.
        /// </summary>
        /// <returns>The list of the object spawned during the last <c>awaken</c> invocation.</returns>
        /// <remarks>Multiple call of this method should return always the same result, ignoring any modification made to the list by callers.</remarks>       
        List<IGameObject> getSpawnedObjects();
    }
}
