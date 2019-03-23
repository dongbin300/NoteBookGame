using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteBookGame
{
    class Skill
    {
        public string name;
        public int level;
        public int sp;
        public int mp;
        public int masterLevel;
        public int damage;
        public int damageP;
        public int damageUp;
        public int attackCount;
        public bool preSkill;

        public int skillLevel;

        public Skill()
        {

        }

        public Skill(string name, int level, int sp, int mp, string effect)
        {
            this.name = name;
            this.level = level;
            this.sp = sp;
            this.mp = mp;
            masterLevel = 0;

            int se = 0;
            int val = 0;
            for (int i = 0; i < effect.Length; i++)
            {
                if (effect[i] >= '0' && effect[i] <= '9')
                {
                    if (i == effect.Length - 1)
                    {
                        AssignEffect(effect, se, val, i);
                    }
                }
                else
                {
                    if (i > 0)
                    {
                        AssignEffect(effect, se, val, i - 1);
                    }
                    se = i;
                }
            }
        }

        public void AssignEffect(string effect, int se, int val, int i)
        {
            val = int.Parse(effect.Substring(se + 1, i - se));
            switch (effect[se])
            {
                case 's':
                    damage = val;
                    break;
                case 'p':
                    damageP = val;
                    break;
                case 'c':
                    attackCount = val;
                    break;
                case '+':
                    damageUp = val;
                    break;

            }
        }

        public void ShowDescription()
        {
            Console.WriteLine($"==={name}===");
            Console.WriteLine($"필요레벨 {level}");
            Console.WriteLine($"스킬레벨 {skillLevel}");
            Console.WriteLine($"MP {mp}");

            Console.WriteLine($"{sp}SP");
        }
    }

    class SkillDB
    {
        public Skill[] skills = new Skill[200];
        public int skillCount = 0;

        private static SkillDB instance = new SkillDB();

        private SkillDB()
        {
            //s:고정뎀 p:퍼뎀 c:공격회수
            AddSkill("라이징샷", 3, 20, 16, "p100+25c1");
            AddSkill("잭스파이크", 5, 20, 18, "p135+5c1");
            AddSkill("개틀링건M-20", 5, 20, 23, "s85+8c10");
            AddSkill("퍼니셔", 7, 25, 21, "s92+17c5");
            AddSkill("윈드밀", 18, 20, 28, "s185+35c1");
            AddSkill("화염방사기F-70", 20, 25, 35, "s132+18c8");
            AddSkill("바베큐", 25, 30, 35, "s177+31c10");
            AddSkill("슈타이어", 30, 30, 36, "s1330+86c1");
            AddSkill("마하킥", 35, 30, 32, "s520+104c1");
            AddSkill("슈타이어마스터", 40, 0, 0, "");
        }

        public static SkillDB GetInstance()
        {
            return instance;
        }


        public void AddSkill(string name, int level, int sp, int mp, string effect)
        {
            skills[skillCount++] = new Skill(name, level, sp, mp, effect);
        }

        public Skill Equip(string name)
        {
            Skill skill = new Skill();
            for (int i = 0; i < skillCount; i++)
            {
                if (skills[i].name == name)
                {
                    skill.name = name;
                    skill.level = skills[i].level;
                    skill.sp = skills[i].sp;
                    skill.mp = skills[i].mp;
                    break;
                }
            }
            return skill;
        }
    }
}
