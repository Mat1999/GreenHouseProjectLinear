using System;
using System.Collections.Generic;
using System.Text;

namespace GreenHouse
{
    interface IMonitor
    {
        public SensorData getSensorData(string ghId);
    }
}
