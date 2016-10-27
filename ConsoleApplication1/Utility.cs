using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace ConsoleApplication1
{
    class Utility
    {
        public static void printPath(List<string> path){
            Console.WriteLine("\n--------------------------------------------------------\n");

            Console.WriteLine("Path Nodes : " + path.Count);

                        if (path.Count != 0)
                        {
                            foreach (var chain in path)
                            {
                                Console.Write(chain);

                                if (!chain.Equals(path.Last(), StringComparison.InvariantCultureIgnoreCase))
                                {
                                    Console.Write("->");
                                }
                            }
                        }

                        Console.WriteLine("\n--------------------------------------------------------\n");
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


        public static List<string> bfs(List<List<string>> links, List<string> start,string end)
        {
            List<string> discovered = new List<string>();
            List<string> path = new List<string>();
            Queue<List<string>> temp = new Queue<List<string>>();
            
            temp.Enqueue(start);


            string currentNode;
            while(temp.Count != 0){

                path = new List<string>(temp.Dequeue());

                currentNode = path.Last();

                if (currentNode.Equals(end, StringComparison.InvariantCultureIgnoreCase))
                {
                    return path;
                }
                else if (!discovered.Any(cus => cus.Equals( currentNode, StringComparison.InvariantCultureIgnoreCase)))
                {
                    discovered.Add(currentNode);

                    var result = from adj in links
                                   where adj[0].Equals(currentNode, StringComparison.InvariantCultureIgnoreCase)
                                   select adj;


                    List<string> adjecent = new List<string>(result.First());
                    adjecent.Remove(currentNode.ToUpper());

                    foreach(var n in adjecent){
                        List<string> newPath = new List<string>(path);
                        newPath.Add(n);
                        temp.Enqueue(newPath);
                    }


                }
            }

            return new List<string>();
        }

    }
}
