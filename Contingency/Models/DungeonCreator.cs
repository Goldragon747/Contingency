using DungeonLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Contingency.Models.Enums;

namespace Contingency.Models
{
    public class DungeonCreator
    {
        public DungeonSize Preset { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        /*
         * Small sizes: 3x3, 5x5, 5x3, 7x5, 7x3
         * Medium: 7x7, 9x7, 9x9, 11x7, 11x9, 11x11, 11x5,11x3
         * Large: 13x13, 13x9, 13x5, 13x3, 15x15, 15x13, 15x11, 15x9, 15x7, 15x5, 15x3
         * Massive: 17x17, 17x13, 17x9, 17x5, 19x19, 19x15, 19x7, 21x21
         */
        public int RoomWeightSmall { get; set; }
        public int RoomWeightMedium { get; set; }
        public int RoomWeightLarge { get; set; }
        public int RoomWeightMassive { get; set; }
        public EnemyGroups EnemyGroup { get; set; }
        public EnemyGroupAmounts EnemyGroupAmount { get; set; }
        public LootGroups LootGroups { get; set; }
        public CRPresets CR { get; set; }
        public string MinCR { get; set; }
        public string MaxCR { get; set; }
        public bool Traps { get; set; }
        public int LootByAmount { get; set; }
        public int MinValue { get; set; }
        public int[,] Dungeon { get; set; }

        public void MakeDungeon()
        {
            Dungeon d = new Dungeon("DungeonGenerator",Width,Height,1);
            Dungeon = d.Map;
        }
    }
}