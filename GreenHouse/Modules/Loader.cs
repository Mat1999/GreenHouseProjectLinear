using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace GreenHouse
{
    public class Loader : ILoader
    {
        HttpClient loaderClient;
        

        public GreenHouseList loadGreenHouses()
        {
            string responseJson;
            GreenHouseList result = new GreenHouseList();
            try
            {
                var taskString = loaderClient.GetStringAsync("http://193.6.19.58:8181/greenhouse");
                responseJson = taskString.Result;
                result = ConvertStringToGreenHouse(responseJson);
            }
            catch
            {
                Console.WriteLine("Hiba történt a kapcsolódás közben! ");
            }
            return result;
        }

        private GreenHouseList ConvertStringToGreenHouse(string jsonstring)
        {
            GreenHouseList houses = new GreenHouseList();
            houses = JsonConvert.DeserializeObject<GreenHouseList>(jsonstring);
            return houses;
        }

        public Loader()
        {
            loaderClient = new HttpClient();
        }
    }
}
