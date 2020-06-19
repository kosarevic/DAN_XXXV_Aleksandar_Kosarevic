using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadatak_1
{
    class Program
    {
        public static int users = 0;
        public static int number = 0;
        public static List<Thread> threads = new List<Thread>();
        public static Random r = new Random();
        private static readonly object theLock = new object();

        static void Main(string[] args)
        {
            Thread t = new Thread(Menu);
            t.Start();
            t.Join();
            Console.ReadLine();
        }

        static void Menu()
        {
            Console.WriteLine("Input how many users will participate in a game.");
            users = int.Parse(Console.ReadLine());
            Console.WriteLine("Input number to be guessed. (1-100)");
            number = int.Parse(Console.ReadLine());
            Console.WriteLine();

            Thread Thread_Generator = new Thread(Generator);
            Thread_Generator.Start();

            Console.WriteLine("Number of participants is: {0}", users);
            Console.WriteLine("Chosen number to be guessed: {0}\n", number);
        }

        static void Generator()
        {
            for (int i = 1; i <= users; i++)
            {
                Thread t = new Thread(StartThreads);
                t.Name = string.Format("Participant_{0}", i);
                threads.Add(t);
            }
            Console.WriteLine("Thread_Generator has completed its task.\n");

            foreach (Thread t in threads)
            {
                t.Start();
            }
        }

        static void StartThreads()
        {
            Thread.Sleep(100);
            int random = r.Next(1, 100);
            Console.WriteLine("{0} tried to guess {1}", Thread.CurrentThread.Name, random);

        }
    }
}