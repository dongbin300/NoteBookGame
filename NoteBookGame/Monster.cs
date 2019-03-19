using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteBookGame
{
    class Monster
    {
        public string name;
        public int level;
        public long hpMax;
        public long hpCur;
        public long gold;
        public long exp;
        public int sp;
        public int attack;

        public Monster()
        {

        }

        public Monster(string name, int level, long hpMax, long gold, long exp, int sp) 
        {
            this.name = name;
            this.level = level;
            this.hpMax = hpMax;
            this.gold = gold;
            this.exp = exp;
            this.sp = sp;
            attack = (int)Math.Sqrt(hpMax);
        }

        public static Monster Create(Deongeon deongeon, string name)
        {
            return deongeon.CreateMonster(name);
        }
    }
}
