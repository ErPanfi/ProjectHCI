using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectHCI.KinectEngine
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

        private const int MAX_NUM_OF_CHOPS_DEFAULT = 5;

        private const int MAX_NUM_OF_FRIENDLY_OBJS_DEFAULT = 5;

        private const int MIN_CHOPS_COOLDOWN_DEFAULT = 2000;

        private const int MAX_CHOPS_COOLDOWN_DEFAULT = 5000;

        private const int MIN_FRIENDLY_OBJS_COOLDOWN_DEFAULT = 2000;

        private const int MAX_FRIENDLY_OBJS_COOLDOWN_DEFAULT = 5000;

        #endregion

        /**
         * it is mandatory to use attributes in order to automate XML serialization: 
         * can't specify a read nor a write delegate method
         * needs public reading access
         * it's good to have a private writing access
         */ 
        #region configuration attributes

        /// <summary>
        /// Maximum number of chops allowed for the spawner
        /// </summary>
        public int maxNumOfChopsAllowed { get; set; }

        /// <summary>
        /// Maximum number of friendly objects allowed
        /// </summary>
        public int maxNumOfFriendlyObjectsAllowed { get; set; }

        /// <summary>
        /// Minimum cooldown between one chop spawn and another
        /// </summary>
        public int minChopSpawnCooldownTimeMillis { get; set; }

        /// <summary>
        /// Maximum cooldown between one chop spawn and another
        /// </summary>
        public int maxChopSpawnCooldownTimeMillis { get; set; }

        /// <summary>
        /// Minimum cooldown between one friendly object spawn and another
        /// </summary>
        public int minFriendlyObjectSpawnCooldownTimeMillis { get; set; }

        /// <summary>
        /// Maximum cooldown between one friendly object spawn and another
        /// </summary>
        public int maxFriendlyObjectSpawnCoooldownTimeMillis { get; set; }

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

        #endregion

        #region ctors and dtors

        /// <summary>
        /// Private constructor: object must be accessed from singleton access methods
        /// </summary>
        private Configuration()
        {
            this.maxNumOfChopsAllowed = MAX_NUM_OF_CHOPS_DEFAULT;
            this.maxNumOfFriendlyObjectsAllowed = MAX_NUM_OF_FRIENDLY_OBJS_DEFAULT;
            this.minChopSpawnCooldownTimeMillis = MIN_CHOPS_COOLDOWN_DEFAULT;
            this.maxChopSpawnCooldownTimeMillis = MAX_CHOPS_COOLDOWN_DEFAULT;
            this.minFriendlyObjectSpawnCooldownTimeMillis = MIN_FRIENDLY_OBJS_COOLDOWN_DEFAULT;
            this.maxFriendlyObjectSpawnCoooldownTimeMillis = MAX_FRIENDLY_OBJS_COOLDOWN_DEFAULT;
        }

        /*
        ~Configuration()
        {
            this.serializeToXML();
        }
         */ 

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
                    _configuration.serializeToXML();
                }

                //now it's surely assigned 
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
            catch (Exception e)
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
