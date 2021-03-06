﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectHCI.KinectEngine
{
    /// <summary>
    /// Used to rise event from the UpdateRender to the GUI canvas
    /// </summary>
    public class GameObjectEventArgs : EventArgs
    {
        private IGameObject gameObject;

        public GameObjectEventArgs(IGameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public IGameObject getGameObject()
        {
            return this.gameObject;
        }
    }
}
