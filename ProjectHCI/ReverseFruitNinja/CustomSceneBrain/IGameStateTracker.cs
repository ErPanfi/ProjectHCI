using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectHCI.ReverseFruitNinja
{
    public interface IGameStateTracker
    {
        int getGameScore();
        
        int getGameLengthMillis();

        int getRageLevel();
    }
}
