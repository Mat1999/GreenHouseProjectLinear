using System;
using System.Collections.Generic;
using System.Text;

namespace GreenHouse
{
    interface IDriver
    {
        public int sendCommand(Greenhouse gh, string token, double boilerValue, double sprinklerValue);
    }
}
