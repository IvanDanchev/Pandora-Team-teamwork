using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu
{
    class Menu
    {
        static void PlayPC() { Console.WriteLine("This is play vs pc"); }
        static void PlayPVP() { Console.WriteLine("This is play vs player"); }
        static void History() { Console.WriteLine("This is history"); }
        static void Main()
        {
            Console.Title = "Pandora Lines";
            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(80, 30);
            Console.SetWindowSize(80, 30);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(@"
█████████   ██████    ████     ██ ████████   ███████ █████████    ██████
██     ██  ██    ██   ██  ██   ██ ██      ██ ██   ██ ██     ██   ██    ██
█████████  ██    ██   ██   ██  ██ ██      ██ ██   ██ █████████   ██    ██
██        ██████████  ██    ██ ██ ██      ██ ██   ██ ██ ████    ██████████ 
██       ██        ██ ██      ███ ████████   ███████ ██  █████ ██        ██

                  ███      ██████  █████    ██  ████  ███████
                  ███        ██    ███  █   ██  ██    ██ 
                  ███        ██    ███   █  ██  ████  ███████
                  ███████  ██████  ███    ████  ██         ██
      TELERIKACADEMY                            ████  ███████
C#2 TEAMWORK - TEAM PANDORA
███████████████████████████████████████████████████████████████████████████████
                                         
           ▒▒▒▒▓▒▒▓▒▒▒▒       ▒▒▒███▒▒██▒▒███▒▒▒      ░▓░▓░▓░▓░▓░▓ 
           ▒▒▒▒▓▒▒▓▒▒▒▒       ▒▒██▒▒▒▒██▒▒▒▒██▒▒      ▓░▓░▓░▓░▓░▓░
           ▒▒▒▒▒▒▒▒▒▒▒▒       ▒██▒▒▒▒▒██▒▒▒▒▒██▒      ░▓░▓░▓░▓░▓░▓
           ▒▓▒▒▒▒▒▒▒▒▓▒       ▒██▒▒▒▒████▒▒▒▒██▒      ▓░▓░▓░▓░▓░▓░
           ▒▒▓▓▓▓▓▓▓▓▒▒       ▒██▒▒▒██████▒▒▒██▒      ░▓░▓░▓░▓░▓░▓
           ▒▒▒▒▒▒▒▒▒▒▒▒       ▒▒██▒██▒██▒██▒██▒▒      ░▓░▓░▓░▓░▓░▓
           ▒▒▒▒▒▒▒▒▒▒▒▒       ▒▒▒███▒▒██▒▒███▒▒▒      ░▓░▓░▓░▓░▓░▓
           ▒▒▒▒▒▒▒▒▒▒▒▒       ▒▒▒▒▒████████▒▒▒▒▒      ░▓░▓░▓░▓░▓░▓
         Player vs Player     Player vs Computer     History of games
            (Press P)             (Press C)             (Press H)
");                                                
            Console.ForegroundColor = ConsoleColor.Black;
            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();
            switch(key.KeyChar)
            {
                case 'p': PlayPVP(); break;
                case 'c': PlayPC(); break;
                case 'h': History(); break;
            }
        }
    }
}
