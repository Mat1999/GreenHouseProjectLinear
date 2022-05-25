using System;
using System.Collections.Generic;
using System.Text;

namespace GreenHouse
{
    class SensorData
    {
        public string ghId { get; set; }
        public string token { get; set; }
        public double temperature_act { get; set; }
        public double humidity_act { get; set; }
        public bool boiler_on { get; set; }
        public bool sprinkler_on { get; set; }

        public override string ToString()
        {
            string data;
            data = "Token: " + token + "\n";
            data += "Aktuális hőmérséklet: " + temperature_act.ToString() + " C, aktuális páratartalom: " + humidity_act.ToString() + "\n";
            data += "Bojler bekapcsolva: " + boiler_on.ToString() + " - locsoló bekapcsolva: " + sprinkler_on.ToString();
            return data;
        }
    }

}
