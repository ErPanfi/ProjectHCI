using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectHCI.KinectEngine
{


    public class Time
    {

        static private Time timeSingleton;
        

        private Time()
        {
        }


        public void tick()
        {
            throw new NotSupportedException();
        }

        public void pause()
        {
            throw new NotSupportedException();
        }

        public void resume()
        {
            throw new NotSupportedException();
        }


        static Time getTimeSingleton()
        {
            throw new NotSupportedException();
        }


        static int getDeltaTime()
        {
            throw new NotSupportedException();
        }


        static int getTotalTime()
        {
            throw new NotSupportedException();
        }


    }
}
