using System;
using System.Collections.Generic;
using System.Text;

namespace GreenHouse
{
    public class Greenhouse
    {
        public string ghId { get; set; }
        public string description { get; set; }
        public int temperature_min { get; set; }
        public int temperature_opt { get; set; }
        public int humidity_min { get; set; }
        public int volume { get; set; }

        public override string ToString()
        {
            string data;
            data = ghId + " - " + description + "\n";
            data += "Minimális hőmérséklet: " + temperature_min.ToString() + " C, optimális hőmérséklet: " + temperature_opt.ToString() + " C \n";
            data += "Minimális páratartalom: " + humidity_min.ToString() + ", térfogat: " + volume.ToString() + " m3";
            return data;
        }
    }

}
