using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("thingy");

        Thread.Sleep(int.Parse(args[0]));

        Console.WriteLine("another thing");
    }
}
