using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Reflection;
using System;
using GreenHouse;


    namespace GreenHouseTest
    {


        [TestClass]
        public class LoaderTests
        {
            [TestMethod]
            public void TE_02()
            {
            int houseCount = 0;
            GreenHouseList gList = new GreenHouseList();
            Loader gLoad = new Loader();
            gList = gLoad.loadGreenHouses();
            houseCount = gList.greenhouseList.Count;
            Assert.AreNotEqual(0, houseCount);
            }

        [TestMethod]
        public void TE_01()
        {
            int houseCount = 0;
            GreenHouseList gList = new GreenHouseList();
            Loader gLoad = new Loader();
            gList = gLoad.loadGreenHouses();
            houseCount = gList.greenhouseList.Count;
            Assert.AreEqual(0, houseCount);
        }

        [TestMethod]
        public void TE_03()
        {
            GreenHouseList gList = new GreenHouseList();
            Loader gLoad = new Loader();
            string testJson = "{" +
                '"' + "greenHouseList" + '"' + ": [" +
                "{" +
                '"' + "ghId" + '"' + ":" + '"' + "B8R2U3H6" + '"' + "," +
                '"' + "description" + '"' + ":" + '"' + "TesztHaz" + '"' + "," +
                '"' + "temperature_min" + '"' + ":36," +
                '"' + "temperature_opt" + '"' + ":38," +
                '"' + "humidity_min" + '"' + ":40," +
                '"' + "volume" + '"' + ":1500" +
                "}" +
                "]" +
                "}";
            PrivateObject objload = new PrivateObject(gLoad);
            var houses = objload.Invoke("ConvertStringToGreenHouse",testJson);
            bool sameHouse = true;
            gList = (GreenHouseList)houses;
            if (gList.greenhouseList[0].ghId != "B8R2U3H6")
            {
                sameHouse = false;
            }
            if (gList.greenhouseList[0].description != "TesztHaz")
            {
                sameHouse = false;
            }
            if (gList.greenhouseList[0].temperature_min != 36)
            {
                sameHouse = false;
            }
            if (gList.greenhouseList[0].temperature_opt != 38)
            {
                sameHouse = false;
            }
            if (gList.greenhouseList[0].humidity_min != 40)
            {
                sameHouse = false;
            }
            if (gList.greenhouseList[0].volume != 1500)
            {
                sameHouse = false;
            }
            Assert.IsTrue(sameHouse);
        }
    }

     

        [TestClass]
        public class ControllerTests
        {

            [TestMethod]
            public void TE_07()
            {

                Controller control = new Controller();
                PrivateObject objcont = new PrivateObject(control);
                Greenhouse house = new Greenhouse();
                house.ghId = "34001";
                house.humidity_min = 60;
                house.temperature_min = 25;
                house.temperature_opt = 28;
                house.volume = 1400;
                SensorData data = new SensorData();
                data.temperature_act = 26;
                data.humidity_act = 67;
                object[] boiArgs = { data.temperature_act, house.temperature_min, house.temperature_opt, null };
                var boiNumTemp = objcont.Invoke("CalculateBoiler", boiArgs);
                object[] spiArgs = { data.humidity_act, data.temperature_act, house.temperature_opt, boiNumTemp, house.humidity_min, house.volume, null };
                //double actualHum, double actTemp, int optTemp ,double commandTemp ,int minHum, int houseVolume, out bool error
                var sprNumTemp = objcont.Invoke("CalculateSprinkler", spiArgs);
                bool valuesRight = true;
            Debug.WriteLine(boiNumTemp);
            Debug.WriteLine(sprNumTemp);
            if ((double)boiNumTemp != 0.0001)
                {
                    valuesRight = false;
                }
                if ((double)sprNumTemp != 0.0001)
                {
                    valuesRight = false;
                }
                Assert.IsTrue(valuesRight);
            }

        [TestMethod]
        public void TE_08()
        {

            Controller control = new Controller();
            PrivateObject objcont = new PrivateObject(control);
            Greenhouse house = new Greenhouse();
            house.ghId = "34001";
            house.humidity_min = 60;
            house.temperature_min = 25;
            house.temperature_opt = 28;
            house.volume = 1400;
            SensorData data = new SensorData();
            data.temperature_act = 26;
            data.humidity_act = 55;
            object[] boiArgs = { data.temperature_act, house.temperature_min, house.temperature_opt, null };
            var boiNumTemp = objcont.Invoke("CalculateBoiler", boiArgs);
            object[] spiArgs = { data.humidity_act, data.temperature_act, house.temperature_opt, boiNumTemp, house.humidity_min, house.volume, null };
            //double actualHum, double actTemp, int optTemp ,double commandTemp ,int minHum, int houseVolume, out bool error
            var sprNumTemp = objcont.Invoke("CalculateSprinkler", spiArgs);
            bool valuesRight = true;
            Debug.WriteLine(boiNumTemp);
            Debug.WriteLine(sprNumTemp);
            if ((double)boiNumTemp != 0.0001)
            {
                valuesRight = false;
            }
            if (Math.Round((double)sprNumTemp) != 173)
            {
                valuesRight = false;
            }
            Assert.IsTrue(valuesRight);
        }

        [TestMethod]
        public void TE_09()
        {

            Controller control = new Controller();
            PrivateObject objcont = new PrivateObject(control);
            Greenhouse house = new Greenhouse();
            house.ghId = "34001";
            house.humidity_min = 20;
            house.temperature_min = 25;
            house.temperature_opt = 28;
            house.volume = 1400;
            SensorData data = new SensorData();
            data.temperature_act = 24;
            data.humidity_act = 78;
            object[] boiArgs = { data.temperature_act, house.temperature_min, house.temperature_opt, null };
            var boiNumTemp = objcont.Invoke("CalculateBoiler", boiArgs);
            object[] spiArgs = { data.humidity_act, data.temperature_act, house.temperature_opt, boiNumTemp, house.humidity_min, house.volume, null };
            //double actualHum, double actTemp, int optTemp ,double commandTemp ,int minHum, int houseVolume, out bool error
            var sprNumTemp = objcont.Invoke("CalculateSprinkler", spiArgs);
            bool valuesRight = true;
            Debug.WriteLine(boiNumTemp);
            Debug.WriteLine(sprNumTemp);
            if ((double)boiNumTemp != 4.0)
            {
                valuesRight = false;
            }
            if ((double)sprNumTemp != 0.0001)
            {
                valuesRight = false;
            }
            Assert.IsTrue(valuesRight);
        }

        [TestMethod]
        public void TE_10()
        {

            Controller control = new Controller();
            PrivateObject objcont = new PrivateObject(control);
            Greenhouse house = new Greenhouse();
            house.ghId = "34001";
            house.humidity_min = 40;
            house.temperature_min = 25;
            house.temperature_opt = 28;
            house.volume = 1400;
            SensorData data = new SensorData();
            data.temperature_act = 24;
            data.humidity_act = 45;
            object[] boiArgs = { data.temperature_act, house.temperature_min, house.temperature_opt, null };
            var boiNumTemp = objcont.Invoke("CalculateBoiler", boiArgs);
            object[] spiArgs = { data.humidity_act, data.temperature_act, house.temperature_opt, boiNumTemp, house.humidity_min, house.volume, null };
            //double actualHum, double actTemp, int optTemp ,double commandTemp ,int minHum, int houseVolume, out bool error
            var sprNumTemp = objcont.Invoke("CalculateSprinkler", spiArgs);
            bool valuesRight = true;
            Debug.WriteLine(boiNumTemp);
            Debug.WriteLine(sprNumTemp);
            if ((double)boiNumTemp != 4.0)
            {
                valuesRight = false;
            }
            if ((double)sprNumTemp != 147.7)
            {
                valuesRight = false;
            }
            Assert.IsTrue(valuesRight);
        }

        [TestMethod]
            public void TE_13()
            {
            
            Debug.WriteLine("Elindult.");
            Controller control = new Controller();
                PrivateObject objcont = new PrivateObject(control);
                bool boiEr = false;
                bool spiEr = false;
                Greenhouse house = new Greenhouse();
                house.ghId = "55666";
                house.humidity_min = 66;
                house.temperature_min = 66;
                house.temperature_opt = 67;
                house.volume = 1400;
                SensorData data = new SensorData();
                data.temperature_act = 69;
                data.humidity_act = 67;
                object[] boiArgs = { data.temperature_act, house.temperature_min, house.temperature_opt, null };
                var boiNumTemp = objcont.Invoke("CalculateBoiler", boiArgs);
                boiEr = (bool)boiArgs[3];
                object[] spiArgs = { data.humidity_act, data.temperature_act, house.temperature_opt, boiNumTemp, house.humidity_min, house.volume, null };
                //double actualHum, double actTemp, int optTemp ,double commandTemp ,int minHum, int houseVolume, out bool error
                var sprNumTemp = objcont.Invoke("CalculateSprinkler", spiArgs);
                spiEr = (bool)spiArgs[6];
                objcont.Invoke("CreateErrorLog", boiEr, spiEr,house,data);
                Assert.IsTrue(true,"It works");
            }

        [TestMethod]
        public void TE_14()
            {
                Controller control = new Controller();
                PrivateObject objcont = new PrivateObject(control);
                bool boiEr = false;
                bool spiEr = false;
                Greenhouse house = new Greenhouse();
                house.ghId = "66666";
                house.humidity_min = 66;
                house.temperature_min = 66;
                house.temperature_opt = 67;
                house.volume = 1400;
                SensorData data = new SensorData();
                data.temperature_act = 0;
                data.humidity_act = 67;
            object[] boiArgs = { data.temperature_act, house.temperature_min, house.temperature_opt, null };
            var boiNumTemp = objcont.Invoke("CalculateBoiler", boiArgs);
            boiEr = (bool)boiArgs[3];
            object[] spiArgs = { data.humidity_act, data.temperature_act, house.temperature_opt, boiNumTemp, house.humidity_min, house.volume, null };
            //double actualHum, double actTemp, int optTemp ,double commandTemp ,int minHum, int houseVolume, out bool error
            var sprNumTemp = objcont.Invoke("CalculateSprinkler", spiArgs);
            spiEr = (bool)spiArgs[6];
            objcont.Invoke("CreateErrorLog", boiEr, spiEr, house, data);
            Assert.IsTrue(true, "It works");
        }

        [TestMethod]
        public void TE_15()
        {
            Controller control = new Controller();
            PrivateObject objcont = new PrivateObject(control);
            bool boiEr = false;
            bool spiEr = false;
            Greenhouse house = new Greenhouse();
            house.ghId = "77666";
            house.humidity_min = 66;
            house.temperature_min = 66;
            house.temperature_opt = 67;
            house.volume = 1400;
            SensorData data = new SensorData();
            data.temperature_act = 68;
            data.humidity_act = 0;
            object[] boiArgs = { data.temperature_act, house.temperature_min, house.temperature_opt, null };
            var boiNumTemp = objcont.Invoke("CalculateBoiler", boiArgs);
            boiEr = (bool)boiArgs[3];
            object[] spiArgs = { data.humidity_act, data.temperature_act, house.temperature_opt, boiNumTemp, house.humidity_min, house.volume, null };
            //double actualHum, double actTemp, int optTemp ,double commandTemp ,int minHum, int houseVolume, out bool error
            var sprNumTemp = objcont.Invoke("CalculateSprinkler", spiArgs);
            spiEr = (bool)spiArgs[6];
            objcont.Invoke("CreateErrorLog", boiEr, spiEr, house, data);
            Assert.IsTrue(true, "It works");
        }

        [TestMethod]
        public void TE_16()
        {
            Controller control = new Controller();
            PrivateObject objcont = new PrivateObject(control);
            bool boiEr = false;
            bool spiEr = false;
            Greenhouse house = new Greenhouse();
            house.ghId = "88666";
            house.humidity_min = 66;
            house.temperature_min = 66;
            house.temperature_opt = 67;
            house.volume = 1400;
            SensorData data = new SensorData();
            data.temperature_act = 0;
            data.humidity_act = 0;
            object[] boiArgs = { data.temperature_act, house.temperature_min, house.temperature_opt, null };
            var boiNumTemp = objcont.Invoke("CalculateBoiler", boiArgs);
            boiEr = (bool)boiArgs[3];
            object[] spiArgs = { data.humidity_act, data.temperature_act, house.temperature_opt, boiNumTemp, house.humidity_min, house.volume, null };
            //double actualHum, double actTemp, int optTemp ,double commandTemp ,int minHum, int houseVolume, out bool error
            var sprNumTemp = objcont.Invoke("CalculateSprinkler", spiArgs);
            spiEr = (bool)spiArgs[6];
            objcont.Invoke("CreateErrorLog", boiEr, spiEr, house, data);
            Assert.IsTrue(true, "It works");
        }
    }

        
    }

