using System;
using System.Collections.Generic;

namespace GreenHouse
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, string> dictErrorCode;
            dictErrorCode = new Dictionary<int, string>();
            dictErrorCode[100] = "A parancs végrehajtásra került!";
            dictErrorCode[101] = "Hibás kalkuláció!";
            dictErrorCode[102] = "Parancs került kiküldésre egy éppen parancsot végrehajtó eszköznek!";
            dictErrorCode[103] = "Hibás parancs került kiküldésre a kazánnak!";
            dictErrorCode[104] = "Hibás parancs került kiküldésre a locsolónak!";
            dictErrorCode[105] = "Az üzenetben lévő token nem érvényes!";
            dictErrorCode[106] = "Az üzenetben szereplő üvegház nem található!";
            dictErrorCode[107] = "Általános üzenet feldolgozási hiba!";
            Console.WriteLine("Csatlakozás a szerverhez, adatok lekérése.");
            try
            {
                Controller houseController = new Controller();
                foreach (Greenhouse gHouse in houseController.green1490.greenhouseList)
                {
                    Console.WriteLine(gHouse.ToString());
                    int response = houseController.MonitorAndControlHouse(gHouse);
                    Console.WriteLine("Válasz a szervertől: {0}", response);
                    try
                    {
                        Console.WriteLine(dictErrorCode[response]);
                    }
                    catch
                    {

                    }
                    Console.WriteLine();
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
