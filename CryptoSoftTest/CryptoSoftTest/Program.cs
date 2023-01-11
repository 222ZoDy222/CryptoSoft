using System;
using System.Collections.Generic;
using CryptoSoftTest.ClassContainer;

namespace CryptoSoftTest
{
    class Program
    {
        static CoordinatePlane plane;

        static FireCalculator fireCalculator = new FireCalculator();

        static void Main(string[] args)
        {
            // Test

            //int test = Convert.ToInt32(Math.Round(5.0 / 2));
            //Console.WriteLine(test);
            //test = Convert.ToInt32(Math.Floor(5.0 / 2));
            //Console.WriteLine(test);
            //


            List<string> fileText = WriterReader.ReadFile();

            bool parseResult = ParseFileText(fileText);
            
            if (!parseResult)
            {
                Console.WriteLine("Wrong information!");
                return;
            } else
            {
                Console.WriteLine("Information is success!");
            }

            int[] result = fireCalculator.Calculate(plane);
            if (result == null) Console.WriteLine("Fatal Error");
            if (result[2] == -1) Console.WriteLine("Some match can't fire");

            WriterReader.WriteResult(result);
            Console.WriteLine($"Result is\n{result[0]} {result[1]} \n{result[2]}");
            Console.ReadKey();
        }






        private static bool ParseFileText(List<string> fileText)
        {
            plane = new CoordinatePlane();
            int matchesLength = 0;
            try
            {
                matchesLength = Convert.ToInt32(fileText[0]);
            } catch
            {
                Console.WriteLine("Wrond match length");
                return false;
            }
            
            plane.matches = new List<Match>();

            for (int i = 0; i < matchesLength; i++)
            {
                string[] splited = fileText[i+1].Split(' ');
                
                try
                {
                    int _x1 = Convert.ToInt32(splited[0]);
                    int _x2 = Convert.ToInt32(splited[1]);
                    int _y1 = Convert.ToInt32(splited[2]);
                    int _y2 = Convert.ToInt32(splited[3]);
                    uint _time = Convert.ToUInt32(splited[4]);
                    plane.matches.Add(new Match(_x1, _x2, _y1, _y2, _time));


                }
                catch
                {
                    return false;
                }

            }


            return true;

        }


    }
}
