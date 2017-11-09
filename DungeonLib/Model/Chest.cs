using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLib.Model
{
    public class Chest
    {
        public List<Item> Items { get; set; }
        public int ValueOfItems { get; set; }
    }
}
