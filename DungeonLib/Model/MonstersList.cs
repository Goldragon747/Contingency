using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContingencyDal;

namespace DungeonLib.Model
{
    class MonstersList
    { 
            public List<ContingencyDal.Monster> Monsters{ get; set; }

            public MonstersList()
            {
                Monsters = new List<ContingencyDal.Monster>();
            }

    }
}
