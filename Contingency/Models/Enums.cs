using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contingency.Models
{
    public class Enums
    {
        public enum DungeonSize
        {
            small,
            medium,
            large,
            massive
        }
        public enum EnemyGroups
        {
            undead,
            humanoid,
            nonHumanoid
        }
        public enum LootGroups
        {
            Barren,
            Minimum,
            Mediocre,
            Medium,
            Full,
            Overflowing,
            Insanity
        }
        public enum CRPresets
        {
            Lvl1,
            Lvl2,
            Lvl3,
            Lvl4,
            Lvl5,
            Lvl6,
            Lvl7,
            Lvl8,
            Lvl9,
            Lvl10,
            Lvl11,
            Lvl12,
            Lvl13,
            Lvl14,
            Lvl15,
            Lvl16,
            Lvl17,
            Lvl18,
            Lvl19,
            Lvl20
        }
    }
}