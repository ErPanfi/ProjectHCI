﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectHCI.KinectEngine
{
    public interface ICollisionManager
    {

        List<KeyValuePair<IGameObject, IGameObject>> createCollisionList();

    }
}
