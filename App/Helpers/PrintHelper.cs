﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCompression.App.Helpers
{
    public class PrintHelper
    {
        public static void TableLine(string header, string value, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{header.PadRight(60).PadLeft(15)} : ");
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }
    }
}
