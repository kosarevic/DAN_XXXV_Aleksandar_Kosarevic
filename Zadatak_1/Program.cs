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

        static void Main(string[] args)
        {
            Thread t = new Thread(Menu);
            t.Start();
            t.Join();

            Thread Thread_Generator = new Thread(Generator);

        }

        static void Menu()
        {
            Console.WriteLine("Input how many users will participate in a game.");
            users = int.Parse(Console.ReadLine());
            Console.WriteLine("Input number to be guessed. (1-100)");
            number = int.Parse(Console.ReadLine());
        }

        static void Generator()
        {

        }
    }
}