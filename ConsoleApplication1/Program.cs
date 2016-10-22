using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace ConsoleApplication1
{
    class Program
    {

        public static bool onedifference(string s1, string s2)
        {
            int count = 0;
            for (int i = 0; i < s1.Length;i++ )
            {
                if (count <= 1)
                {
                    if (s1[i] != s2[i])
                    {
                        count++;
                    }
                }
                else {
                    break;
                }
            }

            if(count  == 1){
                return true;
            }
            return false;
        }
        static void Main(string[] args)
        {
            DateTime start_time;
            DateTime end_time;
            Console.Out.WriteLine("Reading and Parsing File Please Wait.....");

            //opening file
            start_time = DateTime.Now;
            var reader = new StreamReader(File.OpenRead("dictionary.json"));
            //reading file line
            var s = reader.ReadLine();
            //parsing json to object

            dynamic h = JObject.Parse(s);
            //putting converting object to dictionary
            Dictionary<string, string> dictObj = h.ToObject<Dictionary<string, string>>();
            //getting only keys of dictionary in list
            List<string> words = dictObj.Keys.ToList();
            //sorting list
            words.Sort();
            words.Remove("-");
            words.Remove("------");
            end_time = DateTime.Now;
            Console.Out.WriteLine("Content Read and Parsed Successfully in Time: " + Convert.ToString( end_time-start_time));
            Console.Out.WriteLine("Total Words: " + words.Count);

            //creating adjecncy matrix

            List<List<string>> link = new List<List<string>>();



            foreach(var x in words){
                foreach (var y in words)
                {
                    if(x != y && x.Length == y.Length){
                        if (onedifference(x,y))
                        {
                            //Console.Out.WriteLine(x + ":" + y + ":");
                        }
                    }else if(y.Length > x.Length){
                       // break;
                    }
                }
            }

            Console.Out.WriteLine("Done");


                //foreach(var x in words){
                //    Console.Out.WriteLine(x);
                //}
                Console.In.ReadLine();

        }
    }
}