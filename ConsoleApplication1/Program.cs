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

            //sorting list by their charachter number used for effecient linking
            List<string> SortedList = words.OrderBy(o => o.Length).ToList();
            end_time = DateTime.Now;
            
            Console.Out.WriteLine("Content Read and Parsed Successfully in Time: " + Convert.ToString( end_time-start_time));
            Console.Out.WriteLine("Total Words: " + words.Count);

            Console.Out.WriteLine("Finding links Please wait");

            
            //list of list maintaing links inside
            List<List<string>> link = new List<List<string>>();

            //used to create links of word with every other word
            List<string> temp = new List<string>();

            int start_point = 0;
            int start_word = 0;
            start_time = DateTime.Now;
            
            //match all words to find links
            for (int i = 0; i < SortedList.Count; i++)
            {
                //used for breaking only for test purpose to limit linking to only n words
                if(start_word == 5){
                    break;
                }
                temp = new List<string>();
                //first word will be the checking word
                temp.Add(SortedList[i]);
                
                //set start word to first word having same charchters
                if(start_word != SortedList[i].Length){
                    start_word = SortedList[i].Length;
                    start_point = i;
                }   

                //compare with all words starting from starting point calculated above
                for (int j = start_point; j < SortedList.Count; j++)
                {
                    //if size of both words match and they are not same word
                    if (SortedList[i] != SortedList[j] && SortedList[i].Length == SortedList[j].Length)
                    {
                        //add the word to link if it has only 1 letter difference
                        if (Utility.onedifference(SortedList[i], SortedList[j]))
                        {
                            temp.Add(SortedList[j]);
                            //Console.Out.WriteLine(SortedList[i] + ":" + SortedList[j] + ":");
                        }
                    }//if we reached the point where word is greater then the checking word donot check next words
                    else if (SortedList[j].Length > SortedList[i].Length)
                    {
                        break;
                    }
                }
                //add the finded list to link list
                link.Add(temp);

            }

            end_time = DateTime.Now;
            Console.Out.WriteLine("Done in time: " + (end_time-start_time));

            


            while (true)
            {

                Console.Out.WriteLine("Press any key to continue...");

                Console.ReadKey();
                Console.Clear();

                string problem_solver_level;
                Console.Out.WriteLine("\nEnter the correponding number to perform functionality: ");
                Console.Out.WriteLine("1->Two word problem(Shortest Distance)");
                Console.Out.WriteLine("2->All words that dont have chain");
                Console.Out.WriteLine("3->All words that dont have chain");
                Console.Out.WriteLine("0->Exit");


                problem_solver_level = Console.In.ReadLine();

                if (problem_solver_level == "1")
                {

                    
                    //two words matching
                    Console.Out.WriteLine("Enter input word: ");

                    string input = Console.ReadLine();


                    Console.Out.WriteLine("Enter output word: ");

                    string output = Console.ReadLine();

                    Utility.initialize();
                    if (input.Length == output.Length && Utility.isValidWord(SortedList, input) && Utility.isValidWord(SortedList, output))
                    {
                        var result = from w in link
                                     where w[0].Equals(input, StringComparison.InvariantCultureIgnoreCase)
                                     select w;

                        foreach (var x in result)
                        {
                            Utility.check(link, x.ToList(), output, new List<string>(), new List<string>());
                            Task.WaitAll();

                            if (!Utility.solutionFound())
                            {
                                Console.WriteLine("No Solution Possible");
                            }
                        }
                    }
                    else
                    {
                        if (!Utility.isValidWord(SortedList, input))
                        {
                            Console.Out.WriteLine(input + " is not a valid word in dictionary.");
                        }
                        else if (!Utility.isValidWord(SortedList, output))
                        {
                            Console.Out.WriteLine(output + " is not a valid word in dictionary.");
                        }
                        else
                        {
                            Console.Out.WriteLine("Solution not Possible valid word in dictionary.");
                        }
                    }
                    Console.WriteLine("Compiled");

                }//end case 1
                else if (problem_solver_level == "2")
                {
                    Console.WriteLine("Finding out all the words that do not have a chain....");
                    //count total nodes having some links
                    var count = from w in link
                                 where w.Count == 1
                                 select w;

                    Console.Out.WriteLine("Total Nodes: " + (count.Count()));
                    Console.Out.WriteLine("Writing All Words to File Please Wait.... ");

                    StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\No_links.txt");
                    sw.WriteLine("Total Words: " + (link.Count() - count.Count()));
                    foreach(var x in count){
                        sw.WriteLine(x[0]);
                    }
                    sw.Close();

                    Console.Out.WriteLine("File saved in My documents as No_links.txt");
                    


                }
                else
                {
                    break;//exit
                }

                
            }//while loop exit
            
        }//main exit
    }//class exit
}//namespace exit