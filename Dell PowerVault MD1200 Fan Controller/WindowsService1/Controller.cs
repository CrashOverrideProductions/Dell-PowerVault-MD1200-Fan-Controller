using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanController
{
    internal class Controller
    {
        // Settings Structure Instance
        settingsStruct details = new settingsStruct(); // Structure

        // Settings Instance
        Settings settingsClass = new Settings(); // Settings Class

        // Algorithims Instance
        Algorithims algorithimsClass = new Algorithims();

        // Serial Communication Instance
        SerialCommunication communication = new SerialCommunication();


        internal void controlMain()
        {
            // Get Settings
            details = settingsClass.getSettings(); // Get Settings Method

            float lastTemp = 0;
            float currentTemp;


            currentTemp = communication.GetTemperature(details.serialPort);

            int fanSpeed = algorithimsClass.CalcFanSpeed(currentTemp, details.minSpeed, details.maxSpeed, details.minTemp, details.maxTemp, details.factor);

            int tempDiff = algorithimsClass.absDiff((int)currentTemp, (int)lastTemp);
            if (tempDiff > 5)
            {
                lastTemp = currentTemp;

                // Send New Fan Speed
                communication.SetFanSpeed(details.serialPort, fanSpeed);
                //Console.WriteLine(fanSpeed);


            }
        }
    }
}
