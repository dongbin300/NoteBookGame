using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteBookGame
{
    /* Character
             * -닉네임
             * -직업
             * -레벨
             * -전직
             * -퀘스트진행
             * -인무
             * -경험치
             * -골드
             * -HP
             * -MP
             * -데미지
             * -무기(총/갑옷/목걸이/아바타/팬던트/기타)
             * -능력의돌
             * -공격력/방어력/특방/HPMAX/MPMAX/공속/HP회복/MP회복/돈획/경획/sp획
             * -추가HP+/MP+/공+/방+/공속+
             */

    class State
    {
        public int current;
        public int max;

        public State(int current, int max)
        {
            this.current = current;
            this.max = max;
        }
    }

    class Character
    {
        public string nickname;
        public string profession;
        public int professionLevel;
        public int level;
        public Ability ability;
        public Ability additionalAbility;
        public State questProgress;
        public long requireEXP;
        public long exp;
        public int gold;
        public int sp;
        public Skill skill;
        public EquipObject gun;
        public EquipObject armor;
        public EquipObject necklace;
        public EquipObject avatar;
        public EquipObject pendant;
        public EquipObject others;
        public EquipObject abilityStone;

        private static Character instance = new Character();

        private Character()
        {
        }

        public static Character GetInstance()
        {
            return instance;
        }

        public void Equip(EquipObject eo)
        {
            if (level >= eo.level)
            {
                gun = eo;
                Console.WriteLine(">" + eo.name + "을 장착했습니다.");
            }
            else
            {
                Console.WriteLine(">레벨이 낮아 장착할 수 없습니다.");
            }
        }

        public void Attack(Monster monster)
        {
            monster.hpCur -= ability.damage;
            Console.WriteLine($">{monster.name} 피해: {ability.damage}");

            if (monster.hpCur <= 0)
            {
                int getGold = (int)(monster.gold * (1 + (float)ability.goldBonus / 100));
                int getExp = (int)(monster.exp * (1 + (float)ability.expBonus / 100));
                int getSp = (int)(monster.sp * (1 + (float)ability.spBonus / 100));
                gold += getGold;
                exp += getExp;
                sp += getSp;
                Console.WriteLine($">{monster.name}를 잡았습니다. 골드+ {getGold}, Exp+ {getExp}, Sp+ {getSp}");
            }
            else
            {
                Random random = new Random();
                int specialDefenseOn = random.Next(2);
                int monsterDamage = monster.attack - ability.defense - ability.specialDefense * specialDefenseOn;
                if (monsterDamage < 0)
                {
                    monsterDamage = 0;
                }

                ability.hp.current -= monsterDamage;
                Console.WriteLine($">{nickname} 피해: {monsterDamage}");
            }
        }
    }
}
