using DungeonLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGenTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Dungeon d = new Dungeon("test", 25, 25, 0);
            Console.WriteLine(d.ToString());
        }
    }
}
