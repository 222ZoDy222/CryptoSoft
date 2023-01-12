using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CryptoSoftTest
{
    public static class WriterReader
    {

        public static List<string> ReadFile()
        {
            string line;
            List<string> result = new List<string>();
            try
            {
                
                StreamReader sr = new StreamReader("..\\f.in");

                line = sr.ReadLine();
                //Continue to read until you reach end of file
                while (line != null)
                {
                    result.Add(line);
                    
                    line = sr.ReadLine();
                }

                sr.Close();
                Console.WriteLine("File is read");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            return result;

        }


        public static void WriteResult(int[] result, double time)
        {


            if (result == null || result.Length < 2) 
            {
                Console.WriteLine("Wrong result!");
            }

            string path = "..\\f.out";

            File.WriteAllText(path,$"{result[0].ToString()} {result[1].ToString()} \n\n{string.Format($"{{0:f{3}}}", time)}");


        }


    }
}
