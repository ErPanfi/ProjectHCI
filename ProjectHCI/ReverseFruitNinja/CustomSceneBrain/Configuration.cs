using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectHCI.ReverseFruitNinja
{
    /// <summary>
    /// This class stores all the configuration information, in order to offer an easy configuration access procedure to all objects
    /// </summary>
    public class Configuration
    {
        #region Default const values

        /// <summary>
        /// Default path for configuration file
        /// </summary>
        public static readonly string DEFAULT_FILE_PATH = System.IO.Path.Combine(Environment.CurrentDirectory, "Configuration.xml");

        
        #endregion

        /**
         * it is mandatory to use attributes in order to automate XML serialization: 
         * can't specify a read nor a write delegate method
         * and XMLSerializer needs public read/write access
         * in order to correctly manage serialization (i.e.: if a value is not specified use default) a custom set method must be used
         * kinda sucks, but saves lot of work
         */ 
        #region configuration accessible attributes

        #region int maxNumOfChopsAllowed
		 
        public const int MAX_NUM_OF_CHOPS_ALLOWED_DEFAULT = 5;

        private int _maxNumOfChopsAllowed; 

        /// <summary>
        /// Maximum number of chops allowed for the spawner
        /// </summary>        
        public int maxNumOfChopsAllowed 
        { 
            get
            {
                return _maxNumOfChopsAllowed;
            }

            set
            {
                if(value != 0)
                {
                    _maxNumOfChopsAllowed = value;
                }
                else
                {
                    _maxNumOfChopsAllowed = MAX_NUM_OF_CHOPS_ALLOWED_DEFAULT;
                }
            }
        }
	    #endregion

        #region int minChopLifetimeMillis

        public const int MIN_CHOP_LIFETIME_MILLIS_DEFAULT = 2000;

        private int _minChopLifetimeMillis;

        public int minChopLifetimeMillis
        {
            get
            {
                return _minChopLifetimeMillis;
            }

            set
            {
                if (value != 0)
                {
                    _minChopLifetimeMillis = MIN_CHOP_LIFETIME_MILLIS_DEFAULT;
                }
                else
                {
                    _minChopLifetimeMillis = value;
                }
            }
        }
        #endregion

        #region int maxChopLifetimeMillis

        public const int MAX_CHOP_LIFETIME_MILLIS_DEFAULT = 6000;

        private int _maxChopLifetimeMillis;

        public int maxChopLifetimeMillis
        {
            get
            {
                return _maxChopLifetimeMillis;
            }

            set
            {
                if (value != 0)
                {
                    _maxChopLifetimeMillis = value;
                }
                else
                {
                    _maxChopLifetimeMillis = MAX_CHOP_LIFETIME_MILLIS_DEFAULT;
                }
            }
        }

        #endregion

        #region difficulty settings
        public enum GameDifficultyEnum
        {
            Easy,
            Medium,
            Hard
        }

        public const GameDifficultyEnum GAME_DIFFICULTY_DEFAULT = GameDifficultyEnum.Easy;

        private GameDifficultyEnum _gameDifficulty;

        public GameDifficultyEnum gameDifficulty
        {
            get
            {
                return _gameDifficulty;
            }

            set
            {
                if (value != 0)
                {
                    _gameDifficulty = value;
                }
                else
                {
                    _gameDifficulty = GAME_DIFFICULTY_DEFAULT;
                }
            }
        }


        #endregion

        #region User control method settings

        public enum UserControlMethodEnum
        {
            Head,
            Hand
        }

        public const UserControlMethodEnum USER_CONTROL_METHOD_DEFAULT = UserControlMethodEnum.Head;

        private UserControlMethodEnum _userControlMethod;

        public UserControlMethodEnum userControlMethod
        {
            get
            {
                return _userControlMethod;
            }

            set
            {
                if (value != 0)
                {
                    _userControlMethod = value;
                }
                else
                {
                    _userControlMethod = USER_CONTROL_METHOD_DEFAULT;
                }
            }
        }

        #endregion

        #region string userFruitImage

        public const string USER_FRUIT_1_FILENAME = @"fruit_user1.png";
        public const string USER_FRUIT_2_FILENAME = @"fruit_user2.png";
        public const string USER_FRUIT_3_FILENAME = @"fruit_user3.png";

        private string _userFruitImage;

        public string userFruitImage
        {
            get
            {
                return _userFruitImage;
            }

            set
            {
                if (value == "")
                {
                    _userFruitImage = USER_FRUIT_1_FILENAME;
                }
                else
                {
                    _userFruitImage = value;
                }
            }
        }

        #endregion

        #region int maxNumOfFriendlyObjectsAllowed

        public const int MAX_NUM_OF_FRIENDLY_OBJS_ALLOWED_DEFAULT = 5;
        private int _maxNumOfFriendlyObjectsAllowed;

        /// <summary>
        /// Maximum number of friendly objects allowed
        /// </summary>
        public int maxNumOfFriendlyObjectsAllowed
        { 
            get
            {
                return _maxNumOfFriendlyObjectsAllowed;
            }

            set
            {
                if(value != 0)
                {
                    _maxNumOfFriendlyObjectsAllowed = value;
                }
                else
                {
                    _maxNumOfFriendlyObjectsAllowed = MAX_NUM_OF_FRIENDLY_OBJS_ALLOWED_DEFAULT;
                }
            }
        }
        #endregion

        #region int minChopSpawnCooldownTimeMillis
		 
        public const int MIN_CHOPS_COOLDOWN_DEFAULT = 2000;
        private int _minChopSpawnCooldownTimeMillis;

        /// <summary>
        /// Minimum cooldown between one chop spawn and another
        /// </summary>
        public int minChopSpawnCooldownTimeMillis
        { 
            get
            {
                return _minChopSpawnCooldownTimeMillis;
            }

            set
            {
                if(value != 0)
                {
                    _minChopSpawnCooldownTimeMillis = value;
                }
                else
                {
                    _minChopSpawnCooldownTimeMillis = MIN_CHOPS_COOLDOWN_DEFAULT;
                }
            }
        }
	    #endregion

        #region int maxChopSpawnCooldownTimeMillis
        public const int MAX_CHOPS_COOLDOWN_DEFAULT = 5000;
        private int _maxChopSpawnCooldownTimeMillis;

        /// <summary>
        /// Maximum cooldown between one chop spawn and another
        /// </summary>
        public int maxChopSpawnCooldownTimeMillis
        { 
            get
            {
                return _maxChopSpawnCooldownTimeMillis;
            }

            set
            {
                if(value != 0)
                {
                    _maxChopSpawnCooldownTimeMillis = value;
                }
                else
                {
                    _maxChopSpawnCooldownTimeMillis = MAX_CHOPS_COOLDOWN_DEFAULT;
                }
            }
        }
        #endregion

        #region int minFriendlyObjectSpawnCooldownTimeMillis

        public const int MIN_FRIENDLY_OBJS_COOLDOWN_DEFAULT = 2000;
        private int _minFriendlyObjectSpawnCooldownTimeMillis;

        /// <summary>
        /// Minimum cooldown between one friendly object spawn and another
        /// </summary>
        public int minFriendlyObjectSpawnCooldownTimeMillis
        { 
            get
            {
                return _minFriendlyObjectSpawnCooldownTimeMillis;
            }

            set
            {
                if(value != 0)
                {
                    _minFriendlyObjectSpawnCooldownTimeMillis = value;
                }
                else
                {
                    _minFriendlyObjectSpawnCooldownTimeMillis = MIN_FRIENDLY_OBJS_COOLDOWN_DEFAULT;
                }
            }
        }
        #endregion

        #region int maxFriendlyObjectSpawnCoooldownTimeMillis

        public const int MAX_FRIENDLY_OBJS_COOLDOWN_DEFAULT = 5000;
        private int _maxFriendlyObjectSpawnCoooldownTimeMillis;

        /// <summary>
        /// Maximum cooldown between one friendly object spawn and another
        /// </summary>
        public int maxFriendlyObjectSpawnCoooldownTimeMillis
        { 
            get
            {
                return _maxFriendlyObjectSpawnCoooldownTimeMillis;
            }

            set
            {
                if(value != 0)
                {
                    _maxFriendlyObjectSpawnCoooldownTimeMillis = value;
                }
                else
                {
                    _maxFriendlyObjectSpawnCoooldownTimeMillis = MAX_FRIENDLY_OBJS_COOLDOWN_DEFAULT;
                }
            }
        }

        #endregion

        #region int fruitCollectionPoints

        public const int FRUIT_COLLECTION_POINTS_DEFAULT = 10;
        private int _fruitCollectionPoints;

        /// <summary>
        /// Number of points awarded for fruit collection
        /// </summary>
        public int fruitCollectionPoints
        { 
            get
            {
                return _fruitCollectionPoints;
            }

            set
            {
                if(value != 0)
                {
                    _fruitCollectionPoints = value;
                }
                else
                {
                    _fruitCollectionPoints = FRUIT_COLLECTION_POINTS_DEFAULT;
                }
            }
        }

        #endregion

        #region int fruitDeathPoints

        public const int FRUIT_DEATH_POINTS_DEFAULT = -3;
        private int _fruitDeathPoints;

        /// <summary>
        /// Number of points awarded for fruit death (should be negative)
        /// </summary>
        public int fruitDeathPoints
        { 
            get
            {
                return _fruitDeathPoints;
            }

            set
            {
                if(value != 0)
                {
                    _fruitDeathPoints = value;
                }
                else
                {
                    _fruitDeathPoints = FRUIT_DEATH_POINTS_DEFAULT;
                }
            }
        }

	    #endregion

        #region double tryToCutPlayerProbability

        public const double TRY_TO_CUT_PLAYER_PROBABILITY_DEFAULT = 0.6;
        private double _tryToCutPlayerProbability;

        /// <summary>
        /// This is the probability the spawner will try to cut the player instead the fruits
        /// </summary>
        public double tryToCutPlayerProbability
        {
            get
            {
                return _tryToCutPlayerProbability;
            }

            set
            {
                if (value != 0)
                {
                    _tryToCutPlayerProbability = value;
                }
                else
                {
                    _tryToCutPlayerProbability = TRY_TO_CUT_PLAYER_PROBABILITY_DEFAULT;
                }
            }
        }
        #endregion

        #endregion

        #region setters & getters commented code
        /*
            public int getMaxNumOfChopsAllowed()
            {
                return maxNumOfChopsAllowed;
            }

            public void setMaxNumOfChopsAllowed(int newValue)
            {
                maxNumOfChopsAllowed = newValue;
            }

            public int getMaxNumOfFriendlyObjectsAllowed()
            {
                return maxNumOfFriendlyObjectsAllowed;
            }

            public void setMaxNumOfFriendlyObjectsAllowed(int newValue)
            {
                maxNumOfFriendlyObjectsAllowed = newValue;
            }

            public int getChopSpawnCooldownTimeMillis()
            {
                return chopSpawnCooldownTimeMillis;
            }

            public void setChopSpawnCooldownTimeMillis(int newValue)
            {
                chopSpawnCooldownTimeMillis = newValue;
            }

            public int getFriendlyObjectSpawnCooldownTimeMillis()
            {
                return friendlyObjectSpawnCooldownTimeMillis;
            }

            public void setFriendlyObjectSpawnCooldownTimeMillis(int newValue)
            {
                friendlyObjectSpawnCooldownTimeMillis = newValue;
            }
            */
            #endregion

        #region ctors and dtors

        /// <summary>
        /// Private constructor: object must be accessed from singleton access methods
        /// </summary>
        private Configuration()
        {
            //if assigning 0 default value is loaded instead
            this.maxNumOfChopsAllowed                       = MAX_NUM_OF_CHOPS_ALLOWED_DEFAULT;
            this.maxNumOfFriendlyObjectsAllowed             = MAX_NUM_OF_FRIENDLY_OBJS_ALLOWED_DEFAULT;
            this.minChopSpawnCooldownTimeMillis             = MIN_CHOPS_COOLDOWN_DEFAULT;
            this.maxChopSpawnCooldownTimeMillis             = MAX_CHOPS_COOLDOWN_DEFAULT;
            this.minFriendlyObjectSpawnCooldownTimeMillis   = MIN_FRIENDLY_OBJS_COOLDOWN_DEFAULT;
            this.maxFriendlyObjectSpawnCoooldownTimeMillis  = MAX_FRIENDLY_OBJS_COOLDOWN_DEFAULT;
            this.fruitCollectionPoints                      = FRUIT_COLLECTION_POINTS_DEFAULT;
            this.fruitDeathPoints                           = FRUIT_DEATH_POINTS_DEFAULT;
            this.gameDifficulty                             = GAME_DIFFICULTY_DEFAULT;
            this.userControlMethod                          = USER_CONTROL_METHOD_DEFAULT;
            this.userFruitImage                             = USER_FRUIT_1_FILENAME;
        }

        ~Configuration()
        {
            //save to disk option modifications
            this.serializeToXML();
        }

        #endregion

        #region singleton access methods

        private static Configuration _configuration = null;

        public static Configuration getCurrentConfiguration()
        {
            // if not defined deserialize from XML
            if (_configuration == null)
            {

                _configuration = createFromXML();

                //if it's still undefined create with default values AND serialize to XML
                if (_configuration == null)
                {
                    _configuration = new Configuration();
                }

                //now all attributes are surely assigned: refresh file on disk
                _configuration.serializeToXML();
            }

            return _configuration;
        }

        #endregion

        #region Serialization methods

        /// <summary>
        /// Deserialize the configuration class from a given XML file
        /// </summary>
        /// <param name="filePath">The path of the file to deserialize</param>
        /// <returns>The instance of configuration class</returns>
        public static Configuration createFromXML(string filePath)
        {
            Configuration ret = null;

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    ret = (Configuration)new System.Xml.Serialization.XmlSerializer(typeof(Configuration)).Deserialize(new System.IO.StreamReader(filePath));
                }
                catch
                {
                    ret = null;
                }
            }

            return ret;
        }

        /// <summary>
        /// Deserialize the configuration class from a given XML file
        /// </summary>
        /// <returns>The instance of configuration class</returns>
        public static Configuration createFromXML()
        {
            return createFromXML(DEFAULT_FILE_PATH);
        }

        /// <summary>
        /// Serialize the configuration object to the given XML file
        /// </summary>
        /// <remarks>If the file doesn't exists it will be created, otherwise it will be overwritten</remarks>
        /// <param name="filePath">The path of the XML file to create</param>
        public void serializeToXML(string filePath)
        {
            try
            {
                new System.Xml.Serialization.XmlSerializer(typeof(Configuration)).Serialize(new System.IO.StreamWriter(filePath), this);
            }
            catch (Exception)
            {
                //TODO notify error
            }
        }

        /// <summary>
        /// Serialize the configuration object to the given XML file
        /// </summary>
        /// <remarks>If the file doesn't exists it will be created, otherwise it will be overwritten</remarks>
        public void serializeToXML()
        {
            serializeToXML(DEFAULT_FILE_PATH);
        }
        
        #endregion 

    }
}
