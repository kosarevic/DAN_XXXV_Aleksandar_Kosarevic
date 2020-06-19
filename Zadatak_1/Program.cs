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

        static void Main(string[] args)
        {
            Thread t = new Thread(Menu);
            t.Start();
            t.Join();

        }

        static void Menu()
        {
            Console.WriteLine("Input how many users will participate in a game.");
            users = int.Parse(Console.ReadLine());
            Console.WriteLine("Input number to be guessed. (1-100)");
            number = int.Parse(Console.ReadLine());

            Thread Thread_Generator = new Thread(Generator);
            Thread_Generator.Start();

            Console.WriteLine("User successfully inserted participants.");
            Console.WriteLine("Number of participants is: {0}", users);
            Console.WriteLine("Chosen number to be guessed: {0}", number);
        }

        static void Generator()
        {
            for (int i = 1; i <= users; i++)
            {
                Thread t = new Thread();
                t.Name = string.Format("Participant_{0}", i);
                threads.Add(t);
            }
        }

        static void AddToList()
        {

        }
    }
}