using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContingencyDal;
using DungeonLib.Model;

namespace DungeonLib.Service
{
    class ContingencyService
    {
        public MonstersList getAllMonsters()
        {
            MonstersList model = new MonstersList();
            using (var db = new ContingencyEntities())
            {
                var query = db.Monsters.Select(x => x);
                var monstersList = query.ToList();
                monstersList.ForEach(monster =>
                model.Monsters.Add(
                    new ContingencyDal.Monster()
                    {
                        Abilities = monster.Abilities,
                        AC = monster.AC,
                        Actions = monster.Actions,
                        CR = monster.CR,
                        Description = monster.Description,
                        HP = monster.HP,
                        Name = monster.Name,
                        Stats = monster.Stats
                    }
                    ));
            }
            return model;
        }
        public ContingencyDal.Monster GetMonsterByName(string Name)
        {
            return getAllMonsters().Monsters.Where(x => x.Name == Name).First();
        }
        
    }
}
