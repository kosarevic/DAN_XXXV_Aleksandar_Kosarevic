using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public static readonly object theLock = new object();
        public static bool mainPair = false;
        public static bool guessed = false;

        static void Main(string[] args)
        {
            Thread t = new Thread(Menu);
            t.Start();
            t.Join();
        }

        static void Menu()
        {
            while (true)
            {
                Console.WriteLine("Input how many users will participate in a game.");
                bool success = int.TryParse(Console.ReadLine(), out users);
                Console.WriteLine("Input number to be guessed. (1-100)");
                success = int.TryParse(Console.ReadLine(), out number);
                Console.WriteLine();

                if (success && users > 0 && number > 0 && number < 100)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Incorrect input, please try again\n");
                    continue;
                }
            }

            if (number % 2 == 0) mainPair = true;

            Thread Thread_Generator = new Thread(Generator);
            Thread_Generator.Start();

            Console.WriteLine("Number of participants is: {0}", users);
            Console.WriteLine("Chosen number to be guessed: {0}\n", number);
        }

        static void Generator()
        {
            for (int i = 1; i <= users; i++)
            {
                Thread t = new Thread(StartThreads)
                {
                    Name = string.Format("Participant_{0}", i)
                };
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
            while (!guessed)
            {
                Thread.Sleep(100);
                int random = r.Next(1, 100);
                bool guessPair = false;
                if (guessed)
                {
                    Thread.CurrentThread.Abort();
                }
                else if (random % 2 == 0)
                {
                    guessPair = true;
                }

                lock (theLock)
                {
                    if (random == number && !guessed)
                    {
                        guessed = true;
                        Console.WriteLine("{0} has won with the number {1}\n", Thread.CurrentThread.Name, random);
                        Console.WriteLine("Input any key to repeat, or input ~ to exit application.");
                        string input = Console.ReadLine();
                        if (input == "~")
                        {
                            Environment.Exit(0);
                        }
                        else
                        {
                            var fileName = Assembly.GetExecutingAssembly().Location;
                            System.Diagnostics.Process.Start(fileName);
                        }
                    }
                    else if (guessPair == mainPair && !guessed)
                    {
                        Console.WriteLine("{0} tried to guess {1}, and has guessed number parity.", Thread.CurrentThread.Name, random);
                    }
                    else if (!guessed)
                    {
                        Console.WriteLine("{0} tried to guess {1}", Thread.CurrentThread.Name, random);
                    }
                }
            }
        }
    }
}