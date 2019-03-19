using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NoteBookGame
{
    class Program
    {
        static Character user;
        static EODB eodb;
        static DeongeonDB deongeondb;

        static void Main(string[] args)
        {
            /*
             * Require EXP
             * Character
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
             * 몬스터
             * 장비아이템
             * 소모아이템
             * 스킬
             * 퀘스트
             * 아바타
             * 팬던트
             * 던전(맵)
             * 
             * 구현하지 않는 것
             * -채널
             * -보스몬스터
             * -캐쉬시스템
             * -팬던트 시간제한
             * 
             */

            FileStream fs = new FileStream("account.cha", FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            string nickname = sr.ReadLine();
            string profession = sr.ReadLine();
            int level = int.Parse(sr.ReadLine());
            Console.WriteLine(">계정 데이터를 불러왔습니다.");

            user = Character.GetInstance();
            eodb = EODB.GetInstance();
            deongeondb = DeongeonDB.GetInstance();
            user.nickname = nickname;
            user.profession = profession;
            user.level = level;
            user.gun = null;
            user.armor = null;
            user.necklace = null;
            user.ability = new Ability("");
            CalculateCharacterAbility();
            user.ability.hp.current = user.ability.hp.max;

            //ShowUserCurrentStat();

            Monster goblin = Monster.Create(deongeondb.human, "고블린");

            while(true)
            {
                ShowBattle(goblin);
                ConsoleKeyInfo keys = Console.ReadKey(true);
                switch(keys.Key)
                {
                    case ConsoleKey.X:
                        Console.Clear();
                        user.Attack(goblin);
                        break;
                }
            }

            /*user.Equip(eodb.necklace006);

            ShowUserCurrentStat();

            ShowDeongeonMonster();*/

            sr.Close();
            fs.Close();
        }

        static void CalculateCharacterAbility()
        {
            user.ability.CalculateProfession();
            user.ability.CalculateEXP();
            user.ability.CalculateAttack();
            user.ability.CalculateDefense();
            user.ability.CalculateSpecialDefense();
            user.ability.CalculateHP();
            user.ability.CalculateMP();
            user.ability.CalculateHPRecovery();
            user.ability.CalculateMPRecovery();
            user.ability.CalculateAttackSpeed();
            user.ability.CalculateInventoryWeight();
            user.ability.CalculateDamage();
        }

        static void ShowUserCurrentStat()
        {
            CalculateCharacterAbility();
            Console.WriteLine(">닉네임: " + user.nickname);
            Console.WriteLine($">전직: {user.profession}, {user.professionLevel}차");
            Console.WriteLine($">레벨: {user.level} ({user.exp}/{user.requireEXP})");
            Console.WriteLine(">공격력: " + user.ability.attack);
            Console.WriteLine(">방어력: " + user.ability.defense);
            Console.WriteLine(">특수방어력: " + user.ability.specialDefense);
            Console.WriteLine(">HP MAX: " + user.ability.hp.max);
            Console.WriteLine(">MP MAX: " + user.ability.mp.max);
            Console.WriteLine(">HP 회복: " + user.ability.hpRecovery);
            Console.WriteLine(">MP 회복: " + user.ability.mpRecovery);
            Console.WriteLine(">공격속도: " + user.ability.attackSpeed);
            Console.WriteLine($">인무: {user.ability.inventoryWeight.max}kg");
        }

        static void ShowBattle(Monster monster)
        {
            CalculateCharacterAbility();
            Console.WriteLine();
            Console.WriteLine(">닉네임: " + user.nickname);
            Console.WriteLine($">레벨: {user.level} ({user.exp}/{user.requireEXP})");
            Console.WriteLine(">HP: " + user.ability.hp.current);
            Console.WriteLine(">MP: " + user.ability.mp.current);
            Console.WriteLine(">공격력: " + user.ability.attack);
            Console.WriteLine(">방어력: " + user.ability.defense);
            Console.WriteLine(">특수방어력: " + user.ability.specialDefense);
            Console.WriteLine();
            Console.WriteLine(">공격 [x]");
            Console.WriteLine();
            Console.WriteLine(">몬스터 이름: " + monster.name);
            Console.WriteLine(">레벨: " + monster.level);
            Console.WriteLine(">HP: " + monster.hpCur);
            Console.WriteLine(">공격력: " + monster.attack);
        }

        static void ShowDeongeonMonster()
        {
            Console.WriteLine(">몬스터 이름: " + deongeondb.human.monsters[0].name);
            Console.WriteLine(">레벨: " + deongeondb.human.monsters[0].level);
            Console.WriteLine(">HP: " + deongeondb.human.monsters[0].hpMax);
            Console.WriteLine(">공격력: " + deongeondb.human.monsters[0].attack);
            Console.WriteLine(">골드: " + deongeondb.human.monsters[0].gold);
            Console.WriteLine(">경험치: " + deongeondb.human.monsters[0].exp);
            Console.WriteLine(">SP: " + deongeondb.human.monsters[0].sp);
        }
    }
}
