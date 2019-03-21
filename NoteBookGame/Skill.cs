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
        public int attackCount;
        public bool preSkill;

        public Skill()
        {

        }

        public Skill(string name, int level, int sp, int mp, int masterLevel)
        {
            this.name = name;
            this.level = level;
            this.sp = sp;
            this.mp = mp;
            this.masterLevel = masterLevel;
        }
    }

    class SkillDB
    {
        public Skill[] skills = new Skill[200];
        public int skillCount = 0;

        private static SkillDB instance = new SkillDB();

        private SkillDB()
        {
            AddSkill("라이징샷", 3, 20, 16, 0);
            AddSkill("잭스파이크", 5, 20, 18, 0);
            AddSkill("개틀링건M-20", 5, 20, 23, 0);
            AddSkill("퍼니셔", 7, 25, 21, 0);
            AddSkill("윈드밀", 18, 20, 28, 0);
            AddSkill("화염방사기F-70", 20, 25, 35, 0);
            AddSkill("바베큐", 25, 30, 35, 0);
            AddSkill("슈타이어", 30, 30, 36, 0);
            AddSkill("마하킥", 35, 30, 32, 0);
            AddSkill("슈타이어마스터", 40, 0, 0, 1);
        }

        public static SkillDB GetInstance()
        {
            return instance;
        }


        public void AddSkill(string name, int level, int sp, int mp, int masterLevel)
        {
            skills[skillCount++] = new Skill(name, level, sp, mp, masterLevel);
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
