using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Contingency.Models.Enums;

namespace Contingency.Models
{
    public class DungeonCreator
    {
        public DungeonSize DS { get; set; }
        public EnemyGroups EG { get; set; }
        public LootGroups LG { get; set; }
        public CRPresets CR { get; set; }
        public bool Traps { get; set; }
        public int LootByAmount { get; set; }
        public int MinValue { get; set; }
    }
}