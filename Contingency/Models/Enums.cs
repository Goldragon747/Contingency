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
            None,
            Small,
            Medium,
            Large,
            Massive
        }
        public enum EnemyGroups
        {
            Unspecified,
            Undead,
            Humanoid,
            NonHumanoid
        }
        public enum LootGroups
        {
            Unspecified,
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
        public enum EnemyGroupAmounts
        {
            Scarce,
            Plentiful,
            Overflowing
        }
    }
}