using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectHCI.KinectEngine;

namespace ProjectHCI.ReverseFruitNinja
{
    /// <summary>
    /// This class handles the game debriefing
    /// </summary>
    /// <remarks>To be used once the game is finished.</remarks>
    class GameDebriefingSceneBrain : SceneBrain, IGameStateTracker
    {
        private int gameScore;
        private int gameLength;

        #region ctors and dtors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gameTracker">The tracker of the just finished game, which contains most recent tracked values</param>
        public GameDebriefingSceneBrain(IGameStateTracker gameTracker)
        {
            this.gameLength = gameTracker.getGameLengthMillis();
            this.gameScore = gameTracker.getGameScore();
        }

        #endregion

        #region IGameStateTracker members
        public int getGameScore()
        {
            return this.gameScore;
        }

        public int getGameLengthMillis()
        {
            return this.gameLength;
        }
        #endregion
    }
}
