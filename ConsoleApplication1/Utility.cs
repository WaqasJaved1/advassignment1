using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApplication1
{
    class Utility
    {
        static int matchat = 0;

        List<string> shortest;
        List<string> largest;

        List<List<string>> all;



        public static void initialize()
        {
            matchat = 0;
        }


        public static bool solutionFound() { 
            if(matchat == 0){
                return false;
            }

            return true;
        }
        public static bool onedifference(string s1, string s2)
        {
            s1 = s1.ToLower();
            s2 = s2.ToLower();
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

        public static void findlink(List<string> words, string x, ref List<List<string>> link)
        {
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
            
        public static bool isValidWord(List<string> words, string word)
        {
            word = word.ToUpper();
            var result = (from w in words
                          where w == word
                          select w).Count();

            if (result > 0)
            {
                return true;
            }
            return false;
        }

        public static void check(List<List<string>> links, List<string> list, string end, List<string> path, List<string> discovered)
        {
            Task[] t = new Task[list.Count];

            
            if (matchat != 0 && matchat <= path.Count)
            {
                return;
            }

            discovered.Add(list[0]);
            path.Add(list[0]);
            for (int i = 1; i < list.Count; i++)
            {
                if (matchat != 0 && matchat <= path.Count)
                {
                    return;
                }

                var test1 = (from disc in discovered
                             where disc == list[i]
                             select disc).Count();

                if (test1 == 0)
                {
                    if (list[i].Equals(end, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (matchat == 0 || matchat > (path.Count+1))
                        {
                            path.Add(end);
                            matchat = path.Count;

                            Console.Out.Write("\n-------------------------\n");

                            foreach (var path_i in path)
                            {
                                Console.Out.Write(path_i + "->");
                            }
                            Console.Out.Write("\n-------------------------\n");

                            Console.Out.Write("\nTotal Nodes in path: " + path.Count + "\n-------------------------\n");
                            return;
                        }
                    }
                    else
                    {
                        var result = from w in links
                                     where w[0].Equals(list[i], StringComparison.InvariantCultureIgnoreCase)
                                     select w;

                        foreach (var y in result)
                        {
                            if (matchat != 0 && matchat <= path.Count)
                            {
                                return;
                            }
                            if (y.ToList().Count > 1)
                            {

                                t[i] = new Task(() => check(links, y.ToList(), end, new List<string>(path), new List<string>(discovered)));
                                t[i].Start();

                                //check(links, y.ToList(), end, new List<string>(path), new List<string>(discovered));
                            }
                        }

                    }
                }


            }

            try
            {
                Task.WaitAll(t);
            }
            catch
            {
                //Console.WriteLine("\n++++++++++++++++++++++++++++++++++++++\n{0}");
            }
            return;
        }


    }
}
