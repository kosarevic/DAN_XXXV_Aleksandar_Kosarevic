using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadatak_1
{
    /// <summary>
    /// Application simulates "guess the number" game.
    /// </summary>
    class Program
    {
        //Number of participants in the game.
        public static int users = 0;
        //Number to be guessed.
        public static int number = 0;
        //List for storing threads generated for each user.
        public static List<Thread> threads = new List<Thread>();
        public static Random r = new Random();
        public static readonly object theLock = new object();
        //Boolean value added for parity checking.
        public static bool mainPair = false;
        //Boolean value to end the game.
        public static bool guessed = false;

        static void Main(string[] args)
        {
            //Thread initiates menu for the user.
            Thread t = new Thread(Menu);
            t.Start();
            t.Join();
        }
        /// <summary>
        /// Method for simulating user interface.
        /// </summary>
        static void Menu()
        {
            while (true)
            {
                Console.WriteLine("Input how many will participate in a game.");
                bool success = int.TryParse(Console.ReadLine(), out users);
                Console.WriteLine("Input number to be guessed. (1-100)");
                success = int.TryParse(Console.ReadLine(), out number);
                Console.WriteLine();
                //Validation of previous inputs.
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
            //Parity checking for chosen number.
            if (number % 2 == 0) mainPair = true;
            //Thread that generate other "participiant threads" starts here.
            Thread Thread_Generator = new Thread(Generator);
            Thread_Generator.Start();

            Console.WriteLine("Number of participants is: {0}", users);
            Console.WriteLine("Chosen number to be guessed: {0}\n", number);
        }
        /// <summary>
        /// Method generates participiants threads in the game.
        /// </summary>
        static void Generator()
        {
            //Each participiant thread is being generated and assigned the name.
            for (int i = 1; i <= users; i++)
            {
                Thread t = new Thread(StartThreads)
                {
                    Name = string.Format("Participant_{0}", i)
                };
                threads.Add(t);
            }
            Console.WriteLine("Thread_Generator has completed its task.\n");
            //Participiant threads are being initiated here.
            foreach (Thread t in threads)
            {
                t.Start();
            }
        }
        /// <summary>
        /// Method for generating random number by each participiant continiously untill number have been guessed.
        /// </summary>
        static void StartThreads()
        {
            while (!guessed)
            {
                Thread.Sleep(100);
                int random = r.Next(1, 100);
                bool guessPair = false;
                //Parity checking between random number and chosen number.
                if (random % 2 == 0)
                {
                    guessPair = true;
                }
                //Lock placed for avoiding multiple threads writing in the console if game ends.

                //Condition checks if game should end.
                if (random == number && !guessed)
                {
                    //Lock placed here for purpose of displaying only ONE participant that managed to guess the number.
                    lock (theLock)
                    {
                        guessed = true;
                        Console.WriteLine("{0} has won with the number {1}\n", Thread.CurrentThread.Name, random);
                        Console.WriteLine("Input any key to repeat, or input ~ to exit application.");
                        string input = Console.ReadLine();
                        if (input == "~")
                        {
                            //Application exit, if user inserts ~.
                            Environment.Exit(0);
                        }
                        else
                        {
                            //Application restarts with any other input.
                            var fileName = Assembly.GetExecutingAssembly().Location;
                            System.Diagnostics.Process.Start(fileName);
                        }
                    }
                }
                //Parity checking condition.
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