using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data;
using System.Drawing;


namespace ConsoleApplication1
{
    class Program
    {

        static void Main(string[] args)
        {
            int global_word_break = 0;
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

            Console.Out.WriteLine("Enter max number of letter for a word to find graph.(0 for finding full dictionary graph maytake time depending upon computer performance Approx 12 min for i3- 1.7 GHz)\nEnter Please: ");

            try
            {
                global_word_break = Convert.ToInt32(Console.ReadLine());

                if(global_word_break <0){
                    Console.Out.WriteLine("Wrong entry. Graphing full dictionary");                
                }
            }
            catch {
                Console.Out.WriteLine("Wrong entry. Graphing full dictionary");
            }
            Console.Out.WriteLine("Finding links Please wait");

            
            //list of list maintaing links inside
            List<List<string>> link = new List<List<string>>();
            List<List<string>> all_paths = new List<List<string>>();


            //used to create links of word with every other word
            List<string> temp = new List<string>();

            int start_point = 0;
            int start_word = 0;
            start_time = DateTime.Now;
            
            //match all words to find links
            for (int i = 0; i < SortedList.Count; i++)
            {
                Console.Out.WriteLine(i + "/" + SortedList.Count() + " Words done.");

                if((start_word-1) == global_word_break && global_word_break!= 0){
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
                Console.Out.WriteLine("3->All chains. Data Analysis");
                Console.Out.WriteLine("0->Exit");


                problem_solver_level = Console.In.ReadLine();

                if (problem_solver_level == "1")
                {

                    
                    //two words matching
                    Console.Out.WriteLine("Enter input word: ");

                    string input = Console.ReadLine();


                    Console.Out.WriteLine("Enter output word: ");

                    string output = Console.ReadLine();

                    if (input.Length == output.Length && Utility.isValidWord(SortedList, input) && Utility.isValidWord(SortedList, output))
                    {
                        List<string> path = new List<string>();
                        path.Add(input);

                        path = Utility.bfs(link, path, output);

                        Utility.printPath(path);

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
                else if (problem_solver_level == "3")
                {

                    Console.Out.WriteLine("Finding Please wait.....");


                    start_point = 0;
                    start_word = 0;

                    for (int i = 0; i < link.Count; i++)
                    {
                        Console.Out.WriteLine(i+"/"+ link.Count()+" Words done.");

                        
                        if (start_word != link[i][0].Length)
                        {
                            start_word = link[i][0].Length;
                            start_point = i;
                        }

                        if ((start_word - 1) == global_word_break && global_word_break != 0)
                        {
                            break;
                        }

                        for (int j = start_point; j < link.Count; j++)
                        {
                            if (link[i].Count <= 1 || link[j].Count < 1)
                            {
                                continue;
                            }else
                            if (link[i][0] != link[j][0] && link[i][0].Length == link[j][0].Length)
                            {
                                List<string> path = new List<string>();
                                
                                path.Add(link[i][0]);
                                path = Utility.bfs(link, path, link[j][0]);

                                all_paths.Add(new List<string>(path));


                            }//if we reached the point where word is greater then the checking word donot check next words
                            else if (link[j][0].Length > link[i][0].Length)
                            {
                                break;
                            }
                        }
                    }//end for

                    Console.Clear();

                    Console.WriteLine("Statistics: ");
                    
                    var shortest = from src in all_paths
                                   where src.Count == all_paths.Min(t => t.Count)
                                   select src;

                    var largest = from src in all_paths
                                   where src.Count == all_paths.Max(t => t.Count)
                                   select src;

                    var frequency = all_paths.GroupBy(size=> size.Count).OrderBy(size=>size.Key);

                    //CHART

                    //populate dataset with some demo data..
                    DataSet dataSet = new DataSet();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Name", typeof(string));
                    dt.Columns.Add("Counter", typeof(int));

                    int counter = 0;
                    foreach(var f in frequency){
                        DataRow r = dt.NewRow();
                        r[0] = f.Key;
                        r[1] = f.Count();
                        dt.Rows.Add(r);
                    }
                    dataSet.Tables.Add(dt);

                    //prepare chart control...
                    Chart chart = new Chart();
                    chart.DataSource = dataSet.Tables[0];
                    chart.Width = 600;
                    chart.Height = 350;
                    //create serie...
                    
                    Series serie1 = new Series();
                    serie1.Name = "Serie1";
                    serie1.Color = Color.FromArgb(112, 255, 200);
                    serie1.BorderColor = Color.FromArgb(164, 164, 164);
                    serie1.ChartType = SeriesChartType.Column;
                    serie1.BorderDashStyle = ChartDashStyle.Solid;
                    serie1.BorderWidth = 1;
                    serie1.ShadowColor = Color.FromArgb(128, 128, 128);
                    serie1.ShadowOffset = 1;
                    serie1.IsValueShownAsLabel = true;
                    serie1.XValueMember = "Name";
                    serie1.YValueMembers = "Counter";
                    serie1.Font = new Font("Tahoma", 8.0f);
                    serie1.BackSecondaryColor = Color.FromArgb(0, 102, 153);
                    serie1.LabelForeColor = Color.FromArgb(100, 100, 100);
                    chart.Series.Add(serie1);
                    //create chartareas...
                    ChartArea ca = new ChartArea();
                    ca.Name = "ChartArea1";
                    ca.BackColor = Color.White;
                    ca.BorderColor = Color.FromArgb(26, 59, 105);
                    ca.BorderWidth = 0;
                    ca.BorderDashStyle = ChartDashStyle.Solid;
                    ca.AxisX = new Axis();
                    ca.AxisY = new Axis();
                    chart.ChartAreas.Add(ca);
                    //databind...
                    chart.DataBind();
                    //save result...
                    chart.SaveImage(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Chart.png" , ChartImageFormat.Png);
                    //CHART
                    Console.WriteLine("Smallest chain");
                    Utility.printPath(shortest.FirstOrDefault());

                    Console.WriteLine("Largest chain");
                    Utility.printPath(largest.FirstOrDefault());

                    Console.WriteLine(@"Please Check " + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Chart.png" + " for frequency distribution");

                }
                else
                {
                    break;//exit
                }

                
            }//while loop exit
            
        }//main exit
    }//class exit
}//namespace exit