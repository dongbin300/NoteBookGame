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
        public int questProgress;
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
                switch(eo.type)
                {
                    case EquipObject.EquipObjectTypes.Gun:
                        gun = eo;
                        break;
                    case EquipObject.EquipObjectTypes.Armor:
                        armor = eo;
                        break;
                    case EquipObject.EquipObjectTypes.Necklace:
                        necklace = eo;
                        break;
                    case EquipObject.EquipObjectTypes.Avatar:
                        avatar = eo;
                        break;
                    case EquipObject.EquipObjectTypes.Pendant:
                        pendant = eo;
                        break;
                    case EquipObject.EquipObjectTypes.Others:
                        others = eo;
                        break;
                    case EquipObject.EquipObjectTypes.AbilityStone:
                        abilityStone = eo;
                        break;
                }
                Console.WriteLine(">" + eo.name + "을 장착했습니다.");
            }
            else
            {
                Console.WriteLine(">레벨이 낮아서 장착할 수 없습니다.");
            }
        }

        public void Buy(EquipObject eo)
        {
            if (gold >= eo.price)
            {
                if (level >= eo.level)
                {
                    gold -= eo.price;
                    Equip(eo);
                }
                else
                {
                    Console.WriteLine(">레벨이 낮아서 구매할 수 없습니다.");
                }
            }
            else
            {
                Console.WriteLine(">돈이 부족해서 구매할 수 없습니다.");
            }
        }

        public bool Attack(Monster monster)
        {
            ability.hp.current += ability.hpRecovery;
            monster.hpCur -= ability.damage;
            Console.WriteLine($">{monster.name} 피해: {ability.damage}");

            /* 몬스터가 죽음 */
            if (monster.hpCur <= 0)
            {
                monster.Dispose();
                int getGold = (int)(monster.gold * (1 + (float)ability.goldBonus / 100));
                int getExp = (int)(monster.exp * (1 + (float)ability.expBonus / 100));
                int getSp = (int)(monster.sp * (1 + (float)ability.spBonus / 100));
                gold += getGold;
                exp += getExp;
                sp += getSp;
                LevelUp();
                Console.WriteLine($">{monster.name}를 잡았습니다. 골드+ {getGold}, Exp+ {getExp}, Sp+ {getSp}");

                return false;
            }
            else
            {
                Random random = new Random();
                int specialDefenseOn = random.Next(2);
                int monsterDamage = monster.attack - ability.defense / 10 - ability.specialDefense / 10 * specialDefenseOn;
                /* 몬스터의 공격력이 캐릭터의 방어력보다 낮음 */
                if (monsterDamage < 0)
                {
                    monsterDamage = 0;
                }

                ability.hp.current -= monsterDamage;
                Console.WriteLine($">{nickname} 피해: {monsterDamage}");

                /* 캐릭터가 죽음 */
                if (ability.hp.current <= 0)
                {
                    return false;
                }

                return true;
            }
        }

        private void LevelUp()
        {
            if (exp >= requireEXP)
            {
                exp -= requireEXP;
                level++;
                requireEXP = ability.requireEXP[level];
                Console.WriteLine(">레벨 업!");
            }
        }
    }
}
