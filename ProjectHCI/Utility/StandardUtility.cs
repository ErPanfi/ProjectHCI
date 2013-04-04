using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectHCI.Utility
{
    public static class StandardUtility
    {
        public static double mapValueToNewRange(double value,
                                                double oldLowerLimit,
                                                double oldHigherLimit,
                                                double newLowerLimit,
                                                double newHigherLimit)
        {

            double oldRange = oldHigherLimit - oldLowerLimit;
            double newRange = newHigherLimit - newLowerLimit;

            return (((value - oldLowerLimit) * newRange) / oldRange) + newLowerLimit;
        }
    }
}
