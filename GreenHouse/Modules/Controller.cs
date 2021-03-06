using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Diagnostics;

namespace GreenHouse
{
    public class Controller
    {
        public GreenHouseList green1490 { get; set; }


        public Controller()
        {
            Loader greenLoader = new Loader();
            green1490 = greenLoader.loadGreenHouses();
        }

        public int MonitorAndControlHouse(Greenhouse greenHouse)
        {
            Monitor greenMonitor = new Monitor();
            SensorData actualData = greenMonitor.getSensorData(greenHouse.ghId);
            Console.WriteLine(actualData.ToString());
            Driver greenDriver = new Driver(); 
            double boilerNumber = 0.0;
            double sprinklerNumber = 0.0;
            bool boilerError = false;
            bool sprinklerError = false;
            if (!actualData.boiler_on && !actualData.sprinkler_on)
            {
                boilerNumber = CalculateBoiler(actualData.temperature_act, greenHouse.temperature_min, greenHouse.temperature_opt, out boilerError);
                sprinklerNumber = CalculateSprinkler(actualData.humidity_act, actualData.temperature_act, greenHouse.temperature_opt ,boilerNumber,greenHouse.humidity_min, greenHouse.volume, out sprinklerError);
                if (boilerNumber == 0.0001)
                {
                    Console.WriteLine("A hőmérsékleten nem kell változtatni, 0 lesz az utasításban küldve.");
                }
                else
                {
                    Console.WriteLine("Változtatandó hőmérséklet: " + boilerNumber.ToString() + " C");
                }
                if (sprinklerNumber == 0.0001)
                {
                    Console.WriteLine("A páratartalmon nem kell változtatni, 0 lesz az utasításban küldve.");
                }
                else
                {
                    Console.WriteLine("Felhasználandó vízmennyiség: " + sprinklerNumber.ToString() + " liter");
                }
            }
            else
            {
                Console.WriteLine("Az eszközök éppen működnek, üres utasítás lesz küldve.");
            }
            CreateErrorLog(boilerError, sprinklerError, greenHouse, actualData);
            int responseFromServer = greenDriver.sendCommand(greenHouse, actualData.token, boilerNumber, sprinklerNumber);
            return responseFromServer;
        }

        private double CalculateBoiler(double actualTemp, int minTemp, int optTemp, out bool error)
        {
            error = false;
            double rightTemp = 0.0;
            if (actualTemp < minTemp)
            {
                if (actualTemp < minTemp - 5)
                {
                    error = true;
                }
                rightTemp = optTemp - actualTemp;
            }
            else
            {
                rightTemp = 0.0001;
            }
            return rightTemp;
        }

        //1,2   1,4    1,6
        private double CalculateSprinkler(double actualHum, double actTemp, int optTemp ,double commandTemp ,int minHum, int houseVolume, out bool error)
        {
            error = false;
            int flooredTemp = (int)Math.Floor(actTemp);
            double waterToSprink = 0.0;
            double maxHum = 40.0;
            if (commandTemp == 0.0 || commandTemp == 0.0001)
            {
                if (actualHum < minHum) {
                    double targetHum;
                    double currentAirHum;
                    if (actTemp >= 20.00 && actTemp < 26.00)
                    {
                        maxHum = ((flooredTemp - 20) * 1.2) + 17.3;
                        
                    }
                    else if (actTemp >= 26.00 && actTemp < 31.00)
                    {
                        maxHum = ((flooredTemp-26) * 1.4) + 24.7;
                    }
                    else if (actTemp >= 31.00 && actTemp < 36.00)
                    {
                        maxHum = ((flooredTemp-31) * 1.6) + 31.9;
                    }
                    targetHum = maxHum * (minHum * 0.01);
                    currentAirHum = maxHum * (actualHum * 0.01);
                    waterToSprink = (targetHum - currentAirHum) / 10 * houseVolume;
                }
                else
                {
                    waterToSprink = 0.0001;
                }
            }
            else
            {
                int newTemp = optTemp;
                double maxHumAct = 0.0;
                double currentWaterMass = 0.0;
                if (actTemp >= 20.00 && actTemp < 26.00)
                {
                    maxHumAct = ((flooredTemp - 20) * 1.2) + 17.3;
                }
                else if (actTemp >= 26.00 && actTemp < 31.00)
                {
                    maxHumAct = ((flooredTemp - 26) * 1.4) + 24.7;
                }
                else if (actTemp >= 31.00 && actTemp < 36.00)
                {
                    maxHumAct = ((flooredTemp - 31) * 1.6) + 31.9;
                }
                currentWaterMass = maxHumAct * (actualHum * 0.01) * houseVolume;
                //új hőfoknál számítás
                if (newTemp >= 20.00 && newTemp < 26.00)
                {
                    maxHum = ((newTemp - 20) * 1.2) + 17.3;
                }
                else if (newTemp >= 26.00 && newTemp < 31.00)
                {
                    maxHum = ((newTemp - 26) * 1.4) + 24.7;
                }
                else if (newTemp >= 31.00 && newTemp < 36.00)
                {
                    maxHum = ((newTemp - 31) * 1.6) + 31.9;
                }
                double targetMinMass = maxHum * (minHum * 0.01) * houseVolume;
                if (currentWaterMass < targetMinMass)
                {
                    waterToSprink = (targetMinMass - currentWaterMass) / 10 ;
                }
                else
                {
                    waterToSprink = 0.0001;
                }

            }
            if (minHum - actualHum > 20)
            {
                error = true;
            }
            return waterToSprink;
        }

        private void CreateErrorLog(bool bError, bool sError, Greenhouse house, SensorData sData)
        {
            
            if (bError || sError)
            {
               
                string path = System.AppContext.BaseDirectory + "\\log_" + house.ghId + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + "_" + DateTime.Now.Millisecond.ToString() + ".txt";
                FileStream stream = new FileStream(path, FileMode.Create);
                StreamWriter writer = new StreamWriter(stream);
                if (bError)
                {
                    Console.WriteLine("A bojler feltehetőleg meghibásodott! Aktuális hőmérséklet: " + sData.temperature_act.ToString() + " C, a minimálisan megkövetelt hőmérséklet: " + house.temperature_min.ToString() + " C");
                    writer.WriteLine("A bojler feltehetőleg meghibásodott! Aktuális hőmérséklet: " + sData.temperature_act.ToString() + " C, a minimálisan megkövetelt hőmérséklet: " + house.temperature_min.ToString() + " C");
                }
                if (sError)
                {
                    Console.WriteLine("A locsoló feltehetőleg meghibásodott! Aktuális páratartalom: " + sData.humidity_act.ToString() + " százalék, a minimálisan elvárt: " + house.humidity_min.ToString() + " százalék");
                    writer.WriteLine("A locsoló feltehetőleg meghibásodott! Aktuális páratartalom: " + sData.humidity_act.ToString() + " százalék, a minimálisan elvárt: " + house.humidity_min.ToString() + " százalék");
                }
                writer.Close();
                stream.Close();
            }
        }
    }
}
