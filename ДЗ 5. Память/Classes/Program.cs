﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dungeon dungeon = new Dungeon();
            dungeon.ShowRooms();

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadLine();
        }

    }
}
