using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace ConsoleApplication1
{
    class Program
    {

        public static bool onedifference(string s1, string s2)
        {
            if (s1.Length == s2.Length)
            {
                int count = 0;
                for (int i = 0; i < s1.Length; i++)
                {
                    if (count <= 1)
                    {
                        if (s1[i] != s2[i])
                        {
                            count++;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (count == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public static void findlink(List<string> words, string x, ref List<List<string>> link) {
            List<string> temp = new List<string>();

            temp.Add(x);
            foreach (var y in words)
            {

                if (x != y && x.Length == y.Length)
                {
                    if (onedifference(x, y))
                    {
                        temp.Add(y);
                        //Console.Out.WriteLine(x + ":" + y + ":");
                    }
                }
                else if (y.Length > x.Length)
                {
                    break;
                }
            }

            link.Add(temp);
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


            Console.Out.WriteLine("Finding links Please wait");

            List<List<string>> link = new List<List<string>>();
            List<string> temp = new List<string>();
            start_time = DateTime.Now;
            int cou = 0;


            //traverse and find links
            foreach(var x in words){
                //findlink(words, x, ref link);
                bool flag = false;
                //temp = new List<string>();
                //temp.Add(x);

                var result = from w in words
                                 where onedifference(w, x)
                                 select w;
                  
                //foreach (var y in words)
                //{
                    
                //    if (x != y && x.Length == y.Length)
                //    {
                //        if (onedifference(x, y))
                //        {
                //            temp.Add(y);
                //            Console.Out.WriteLine(x + ":" + y + ":");
                //        }
                //    }
                //    else if (y.Length > x.Length)
                //    {
                //        flag = true;
                //        break;
                //    }

                //}


                //foreach (var y in result)
                //{

                //    Console.Out.WriteLine(x+":"+y);
                //}
                
                link.Add(result.ToList());
                if(cou%1000 == 0){
                    Console.WriteLine(cou++);
                };
                cou++;
                //temp.Clear();  
            }

            end_time = DateTime.Now;
            Console.Out.WriteLine("Done in time: " + (end_time-start_time));

            var count = (from w in link
                         where w.Count >= 1
                         select w).Count();

            Console.Out.WriteLine("Total Nodes: " + count);
 

                //foreach(var x in words){
                //    Console.Out.WriteLine(x);
                //}
                Console.In.ReadLine();

        }
    }
}