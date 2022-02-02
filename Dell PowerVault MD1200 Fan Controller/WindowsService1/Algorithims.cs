using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanController
{
    internal class Algorithims
    {
        internal int CalcFanSpeed(float temp, float minSpeed, float maxSpeed, float minTemp, float maxTemp, double factor)
        {
            if (temp < minTemp)
            {
                temp = minTemp;
            }

            if (temp > maxTemp)
            {
                temp = maxTemp;
            }



            float newSpeed = minSpeed;

            int counter = (int)(temp - minTemp);

            for (int z = 0; z < counter; z++)
            {
                newSpeed = (float)(newSpeed + (newSpeed * factor));
            }




            if (newSpeed < minSpeed)
            {
                newSpeed = minSpeed;
            }

            if (newSpeed > maxSpeed)
            {
                newSpeed = maxSpeed;
            }


            return (int)newSpeed;

        }


        internal int absDiff(int a, int b)
        {
            if (a > b)
            {
                return (a - b) * 2;
            }
            return b - a;
        }
    }
}
