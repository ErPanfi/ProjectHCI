using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace ProjectHCI.KinectEngine
{


    public class Time
    {

        static private Time timeSingleton;

        static private object staticLock = new object();



        private int startingTimeMillis;
        private int lastTickCount;
        private bool isRunning;

        private int deltaTimeMillis;





        private Time()
        {
            startingTimeMillis = 0;
            lastTickCount = 0;

            deltaTimeMillis = 0;

            isRunning = false;
        }


        public void start()
        {
            this.isRunning = true;

            this.startingTimeMillis = System.Environment.TickCount;
            this.lastTickCount = System.Environment.TickCount;

        }


        public void tick()
        {
            Debug.Assert(isRunning, "the timer was not started!");

            int currentTickCount = System.Environment.TickCount;

            this.deltaTimeMillis = currentTickCount - this.lastTickCount;
            this.lastTickCount = currentTickCount;

        }


        public static Time getTimeSingleton()
        {
            lock (staticLock)
            {
                if (Time.timeSingleton == null)
                {
                    Time.timeSingleton = new Time();
                }

                return Time.timeSingleton;
            }
        }



        public void pause()
        {
            throw new NotSupportedException();
        }

        public void resume()
        {
            throw new NotSupportedException();
        }

        public void stop()
        {
            this.isRunning = false;
            this.deltaTimeMillis = 0;
            this.lastTickCount = -1;
            this.startingTimeMillis = -1;
        }




        public static int getDeltaTimeMillis()
        {
            Debug.Assert(Time.timeSingleton.isRunning, "the timer must be running!");
            return Time.timeSingleton.deltaTimeMillis;
        }


        public static int getTotalTimeMillis()
        {
            Debug.Assert(Time.timeSingleton.isRunning, "the timer must be running!");
            return System.Environment.TickCount - Time.timeSingleton.startingTimeMillis;
        }


    }
}
