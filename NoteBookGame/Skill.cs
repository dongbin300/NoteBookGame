using System;
using System.Collections.Generic;

namespace NoteBookGame
{
    class Skill
    {
        public enum SkillTypes { Active, Passive, SkillBonus };
        public SkillTypes type;
        public string code;
        public string name;
        public int level; // 스킬 습득 필요 레벨
        public int sp;
        public int mp;
        public int masterLevel; // 스킬 마스터 레벨
        public int damage; // 데미지
        public int damageP; // 데미지%
        public int damageUp; // 데미지+
        public int attackCount; // 공격 수
        public string preSkill; // 선행스킬목록

        public int skillLevel; // 스킬 레벨

        public Dictionary<string, int> effectDict = new Dictionary<string, int>();
        public Dictionary<string, int> preSkillDict = new Dictionary<string, int>();
        public Dictionary<string, int> skillBonusDict = new Dictionary<string, int>();
        
        public Skill()
        {

        }

        // 액티브 스킬
        public Skill(string code, string name, int level, int sp, int mp, string effect, string preSkill)
        {
            type = SkillTypes.Active;
            this.code = code;
            this.name = name;
            this.level = level;
            this.sp = sp;
            this.mp = mp;
            masterLevel = 0;

            effectDict = FormatString.ParseInt(effect);

            foreach (KeyValuePair<string, int> temp in effectDict)
            {
                switch (temp.Key)
                {
                    case "s": damage = temp.Value; break;
                    case "p": damageP = temp.Value; break;
                    case "c": attackCount = temp.Value; break;
                    case "+": damageUp = temp.Value; break;
                }
            }
        }

        // 스킬 레벨+ 스킬
        public Skill(string code, string name, int level, string effect, string preSkill)
        {
            type = SkillTypes.SkillBonus;
            this.code = code;
            this.name = name;
            this.level = level;
            masterLevel = 1;

            Dictionary<string, int> effectDict = FormatString.ParseInt(effect);

            foreach (KeyValuePair<string, int> temp in effectDict)
            {
                skillBonusDict.Add(temp.Key, temp.Value);
            }
        }

        // 패시브 스킬
        public Skill(string code, string name, int level, int sp, string effect, string preSkill)
        {
            type = SkillTypes.Passive;
            this.code = code;
            this.name = name;
            this.level = level;
            this.sp = sp;
            masterLevel = 0;

            Dictionary<string, int> effectDict = FormatString.ParseInt(effect);
            Ability passiveEffect = new Ability();

            foreach (KeyValuePair<string, int> temp in effectDict)
            {
                switch (temp.Key)
                {
                    case "a": passiveEffect.attack = temp.Value; break;
                    case "d": passiveEffect.defense = temp.Value; break;
                    case "c": passiveEffect.specialDefense = temp.Value; break;
                    case "t": passiveEffect.attackSpeed = temp.Value; break;
                    case "h": passiveEffect.hp.max = temp.Value; break;
                    case "m": passiveEffect.mp.max = temp.Value; break;
                    case "r": passiveEffect.hpRecovery = temp.Value; break;
                    case "v": passiveEffect.mpRecovery = temp.Value; break;
                    case "w": passiveEffect.inventoryWeight.max = temp.Value; break;
                    case "e": passiveEffect.expBonus = temp.Value; break;
                    case "g": passiveEffect.goldBonus = temp.Value; break;
                    case "s": passiveEffect.spBonus = temp.Value; break;
                }
            }
        }

        public bool preSkillCheck(SkillDB skilldb, Skill skill)
        {
            preSkillDict = FormatString.ParseInt(preSkill);

            foreach (KeyValuePair<string, int> temp in preSkillDict)
            {
                if (skilldb.GetSkill(temp.Key).skillLevel < temp.Value)
                    return false;
            }
            return true;
        }

        public void ShowDescription()
        {
            Console.WriteLine($"==={name}===");
            Console.WriteLine($"필요레벨 {level}");
            Console.WriteLine($"스킬레벨 {skillLevel} / {masterLevel}");
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
            //s:고정뎀 p:퍼뎀 c:공격회수 +:스킬레벨+ 증가치
            AddSkill("aa", "라이징샷", 3, 20, 16, "p100+25c1");
            AddSkill("ab", "잭스파이크", 5, 20, 18, "p135+5c1");
            AddSkill("ac", "개틀링건M-20", 5, 20, 23, "s85+8c10");
            AddSkill("ad", "퍼니셔", 7, 25, 21, "s92+17c5");
            AddSkill("ae", "윈드밀", 18, 20, 28, "s185+35c1");
            AddSkill("af", "화염방사기F-70", 20, 25, 35, "s132+18c8");
            AddSkill("ag", "바베큐", 25, 30, 35, "s177+31c10", "ab5ac3");
            AddSkill("ah", "슈타이어", 30, 30, 36, "s1330+86c1");
            AddSkill("ai", "마하킥", 35, 30, 32, "s520+104c1");
            AddSkill("aj", "슈타이어마스터", 40, "ac1af1ag1ah1ai1");
        }

        public static SkillDB GetInstance()
        {
            return instance;
        }

        public void AddSkill(string code, string name, int level, int sp, int mp, string effect, string preSkill = "")
        {
            skills[skillCount++] = new Skill(code, name, level, sp, mp, effect, preSkill);
        }

        public void AddSkill(string code, string name, int level, string effect, string preSkill = "")
        {
            skills[skillCount++] = new Skill(code, name, level, effect, preSkill);
        }

        public void AddSkill(string code, string name, int level, int sp, string effect, string preSkill = "")
        {
            skills[skillCount++] = new Skill(code, name, level, sp, effect, preSkill);
        }

        public void Learn(string code)
        {
            GetSkill(code).level++;
        }

        public Skill GetSkill(string code)
        {
            for (int i = 0; i < skillCount; i++)
                if (code == skills[i].code)
                    return skills[i];
            return null;
        }
    }
}
