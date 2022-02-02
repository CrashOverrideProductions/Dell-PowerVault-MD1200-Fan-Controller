using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanController
{
    internal struct settingsStruct
    {
        public string serialPort;

        public float minSpeed;
        public float maxSpeed;

        public float minTemp;
        public float maxTemp;

        public double factor;
    }

    internal class Settings
    {
        internal settingsStruct getSettings()
        {
            try
            {
                string FileLocation = AppDomain.CurrentDomain.BaseDirectory + @"\Settings.ini";
                string line;

                settingsStruct configFile = new settingsStruct();

                using (StreamReader file = new StreamReader(FileLocation))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line.StartsWith("serialPort"))
                        {
                            string[] type = line.Split('=');
                            configFile.serialPort = type[1];
                        }

                        if (line.StartsWith("minSpeed"))
                        {
                            string[] type = line.Split('=');
                            configFile.minSpeed = float.Parse(type[1]);
                        }

                        if (line.StartsWith("maxSpeed"))
                        {
                            string[] type = line.Split('=');
                            configFile.maxSpeed = float.Parse(type[1]);
                        }

                        if (line.StartsWith("minTemp"))
                        {
                            string[] type = line.Split('=');
                            configFile.minTemp = float.Parse(type[1]);
                        }

                        if (line.StartsWith("maxTemp"))
                        {
                            string[] type = line.Split('=');
                            configFile.maxTemp = float.Parse(type[1]);
                        }

                        if (line.StartsWith("factor"))
                        {
                            string[] type = line.Split('=');
                            double factor = Convert.ToDouble(type[1]);
                            configFile.factor = factor;
                        }
                    }
                }
                return configFile;
            }
            catch (Exception ex)
            {
                // Add Log to Application Event Log
                string errorMsg = "Failed to Get Settings" + Environment.NewLine + ex.Message;
                EventLog.WriteEntry("Dell PowerVaul MD1200 Fan Controller", errorMsg, EventLogEntryType.Error); // "App Name", "Message"

                // Stop Service Goes Here
                Console.WriteLine(ex.Message);
                // Return Empty Config
                settingsStruct configFile = new settingsStruct();
                return configFile;
            }

        }
    }
}
