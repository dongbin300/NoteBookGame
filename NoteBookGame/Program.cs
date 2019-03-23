﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NoteBookGame
{
    class Program
    {
        public enum Screens
        {
            Main,
            Deongeon, DeongeonMonster,
            Shop, ShopChild, ShopChildStat,
            Skill, SkillChild, SkillChidStat,
            Save,
            MyStat
        };
        static Screens screen;
        static Deongeon selectedDeongeon;
        static EquipObject.EquipObjectTypes equipObjectType;
        static EquipObject selectedEquipObject;
        static int selectedSkillLevelMin, selectedSkillLevelMax;
        static Skill selectedSkill;

        static Character user;
        static EODB eodb;
        static DeongeonDB deongeondb;
        static SkillDB skilldb;
        static string menuOrder = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
        static ConsoleKey[] keyOrder = { ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D4, ConsoleKey.D5, ConsoleKey.D6, ConsoleKey.D7, ConsoleKey.D8, ConsoleKey.D9, ConsoleKey.D0, ConsoleKey.Q, ConsoleKey.W, ConsoleKey.E, ConsoleKey.R, ConsoleKey.T, ConsoleKey.Y, ConsoleKey.U, ConsoleKey.I, ConsoleKey.O, ConsoleKey.P, ConsoleKey.A, ConsoleKey.S, ConsoleKey.D, ConsoleKey.F, ConsoleKey.G, ConsoleKey.H, ConsoleKey.J, ConsoleKey.K, ConsoleKey.L, ConsoleKey.Z, ConsoleKey.X, ConsoleKey.C, ConsoleKey.V, ConsoleKey.B, ConsoleKey.N, ConsoleKey.M };
        static bool play;

        static void Main(string[] args)
        {
            /* 
             * *저장해야할 데이터
             * Require EXP
             * Character
             * -닉네임*
             * -직업* 
             * -레벨*
             * -전직(자동)
             * -퀘스트진행*
             * -인무
             * -경험치*
             * -골드*
             * -HP
             * -MP
             * -데미지
             * -무기(총/갑옷/목걸이/아바타/팬던트/기타)*
             * -능력의돌*
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

            user = Character.GetInstance();
            eodb = EODB.GetInstance();
            deongeondb = DeongeonDB.GetInstance();
            skilldb = SkillDB.GetInstance();

            /* 캐릭터 로드 */
            FileStream fs = new FileStream("account.cha", FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            user.nickname = sr.ReadLine();
            user.profession = sr.ReadLine();
            user.level = int.Parse(sr.ReadLine());
            user.questProgress = int.Parse(sr.ReadLine());
            user.exp = int.Parse(sr.ReadLine());
            user.gold = int.Parse(sr.ReadLine());
            user.gun = eodb.Equip(sr.ReadLine());
            user.armor = eodb.Equip(sr.ReadLine());
            user.necklace = eodb.Equip(sr.ReadLine());
            user.avatar = eodb.Equip(sr.ReadLine());
            user.pendant = eodb.Equip(sr.ReadLine());
            user.others = eodb.Equip(sr.ReadLine());
            user.abilityStone = eodb.Equip(sr.ReadLine());
            sr.Close();
            fs.Close();
            Console.WriteLine(">계정 데이터를 불러왔습니다.");

            /* 캐릭터 능력치 계산 */
            user.ability = new Ability("");
            CalculateCharacterAbility();
            user.ability.hp.current = user.ability.hp.max;
            user.ability.mp.current = user.ability.mp.max;

            play = true;
            screen = Screens.Main;

            while (play)
            {
                switch (screen)
                {
                    case Screens.Main:
                        MainMenu();
                        break;
                    case Screens.Deongeon:
                        DeongeonMenu();
                        break;
                    case Screens.DeongeonMonster:
                        DeongeonMonsterMenu(selectedDeongeon);
                        break;
                    case Screens.Shop:
                        ShopMenu();
                        break;
                    case Screens.ShopChild:
                        ShopChildMenu(equipObjectType);
                        break;
                    case Screens.ShopChildStat:
                        ShopChildStatMenu(selectedEquipObject);
                        break;
                    case Screens.Skill:
                        SkillMenu();
                        break;
                    case Screens.SkillChild:

                    case Screens.Save:
                        SaveMenu();
                        break;
                    case Screens.MyStat:
                        MyStatMenu();
                        break;
                }
            }
        }

        static void MainMenu()
        {
            Console.WriteLine("===던파 RPG===");
            Console.WriteLine(">[1] 던전");
            Console.WriteLine(">[2] 상점");
            //Console.WriteLine(">[3] 던전");
            //Console.WriteLine(">[4] 던전");
            //Console.WriteLine(">[5] 던전");
            //Console.WriteLine(">[6] 던전");
            //Console.WriteLine(">[7] 던전");
            //Console.WriteLine(">[8] 던전");
            //Console.WriteLine(">[9] 던전");
            //Console.WriteLine(">[0] 던전");
            //Console.WriteLine(">[Q] 퀘스트");
            Console.WriteLine(">[S] 스킬");
            Console.WriteLine(">[A] 저장");
            Console.WriteLine(">[W] 내 스탯");
            Console.WriteLine(">[P] 게임 종료");

            ConsoleKeyInfo keys = Console.ReadKey(true);
            switch (keys.Key)
            {
                case ConsoleKey.D1:
                    screen = Screens.Deongeon;
                    break;
                case ConsoleKey.D2:
                    screen = Screens.Shop;
                    break;
                case ConsoleKey.S:
                    screen = Screens.Skill;
                    break;
                case ConsoleKey.A:
                    screen = Screens.Save;
                    break;
                case ConsoleKey.W:
                    screen = Screens.MyStat;
                    break;
                case ConsoleKey.P:
                    play = false;
                    break;
            }
        }

        static void DeongeonMonsterBattleMenu(Deongeon deongeon, Monster monster)
        {
            bool aliveMonster = true;
            while (aliveMonster)
            {
                ShowBattle(monster);
                ConsoleKeyInfo keys = Console.ReadKey(true);
                switch (keys.Key)
                {
                    case ConsoleKey.X:
                        Console.Clear();
                        aliveMonster = user.Attack(monster);
                        Console.WriteLine();
                        break;
                    case ConsoleKey.C:
                        Console.Clear();
                        monster.Dispose();
                        aliveMonster = false;
                        break;
                }
            }
        }

        static void DeongeonMonsterMenu(Deongeon deongeon)
        {
            Console.WriteLine($"==={deongeon.name}===");
            Console.WriteLine(">[ESC] 나가기");
            for (int i = 0; i < deongeon.monsterCount; i++)
            {
                Console.WriteLine($">[{menuOrder[i]}] {deongeon.monsters[i].name}");
            }
            ConsoleKeyInfo keys = Console.ReadKey(true);
            for (int i = 0; i < deongeon.monsterCount; i++)
            {
                if (keys.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    screen = Screens.Deongeon;
                }
                if (keys.Key == keyOrder[i])
                {
                    Console.Clear();
                    Monster monster = Monster.Create(deongeon, deongeon.monsters[i].name);
                    DeongeonMonsterBattleMenu(deongeon, monster);
                    break;
                }
            }
        }

        static void DeongeonMenu()
        {
            Console.Clear();
            Console.WriteLine("===던전===");
            Console.WriteLine(">[ESC] 나가기");
            Console.WriteLine(">[1] 휴먼");
            Console.WriteLine(">[2] 고블린나라");
            Console.WriteLine(">[3] 신전외곽");
            Console.WriteLine(">[4] 신전내부");
            Console.WriteLine(">[5] 도둑아지트");
            Console.WriteLine(">[6] 동화나라");
            Console.WriteLine(">[7] 히든맵1");
            Console.WriteLine(">[8] 히든맵2");
            Console.WriteLine(">[9] 썬더젠틀맨의숙소");
            Console.WriteLine(">[0] 헤드스핀의숙소");
            Console.WriteLine(">[Q] 던전길목");

            ConsoleKeyInfo keys = Console.ReadKey(true);
            switch (keys.Key)
            {
                case ConsoleKey.D1:
                    Console.Clear();
                    screen = Screens.DeongeonMonster;
                    selectedDeongeon = deongeondb.human;
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    screen = Screens.Main;
                    break;
            }
        }

        static void ShopChildStatMenu(EquipObject eo)
        {
            eo.ShowDescription();
            Console.WriteLine();
            Console.WriteLine(">[X] 구입");
            Console.WriteLine(">[C] 취소");

            ConsoleKeyInfo keys = Console.ReadKey(true);
            switch (keys.Key)
            {
                case ConsoleKey.X:
                    Console.Clear();
                    user.Buy(eo);
                    break;
                case ConsoleKey.C:
                    Console.Clear();
                    screen = Screens.ShopChild;
                    break;
            }
        }

        static void ShopChildMenu(EquipObject.EquipObjectTypes type)
        {
            Console.Clear();
            Console.WriteLine($"==={type}===");
            Console.WriteLine(">[ESC] 나가기");
            EquipObject[] tempEquipObjects = new EquipObject[40];
            int idx = 0;
            for (int i = 0; i < eodb.equipObjectCount; i++)
            {
                if (eodb.equipObjects[i].type == type)
                {
                    tempEquipObjects[idx] = eodb.equipObjects[i];
                    Console.WriteLine($">[{menuOrder[idx]}] {tempEquipObjects[idx].name} Lv{tempEquipObjects[idx].level}");
                    idx++;
                }
            }
            ConsoleKeyInfo keys = Console.ReadKey(true);
            for (int i = 0; i < idx; i++)
            {
                if (keys.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    screen = Screens.Shop;
                }
                if (keys.Key == keyOrder[i])
                {
                    Console.Clear();
                    screen = Screens.ShopChildStat;
                    selectedEquipObject = tempEquipObjects[i];
                }
            }
        }

        static void ShopMenu()
        {
            Console.Clear();
            Console.WriteLine("===상점===");
            Console.WriteLine(">[ESC] 나가기");
            Console.WriteLine(">[1] 총");
            Console.WriteLine(">[2] 갑옷");
            Console.WriteLine(">[3] 목걸이");
            Console.WriteLine(">[4] 아바타");
            Console.WriteLine(">[5] 펜던트");
            Console.WriteLine(">[6] 포션");

            ConsoleKeyInfo keys = Console.ReadKey(true);
            switch (keys.Key)
            {
                case ConsoleKey.D1:
                    screen = Screens.ShopChild;
                    equipObjectType = EquipObject.EquipObjectTypes.Gun;
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    screen = Screens.Main;
                    break;
            }
        }

        static void SkillChildStatMenu(Skill skill)
        {
            skill.ShowDescription();
            Console.WriteLine();
            Console.WriteLine(">[X] 습득");
            Console.WriteLine(">[C] 취소");

            ConsoleKeyInfo keys = Console.ReadKey(true);
            switch (keys.Key)
            {
                case ConsoleKey.X:
                    Console.Clear();
                    user.Get(skill);
                    break;
                case ConsoleKey.C:
                    Console.Clear();
                    screen = Screens.SkillChild;
                    break;
            }
        }

        static void SkillChildMenu(int min, int max)
        {
            Console.Clear();
            Console.WriteLine($"===Lv{min}~{max} 스킬===");
            Console.WriteLine(">[ESC] 나가기");
            Skill[] tempSkills = new Skill[40];
            int idx = 0;
            for (int i = 0; i < skilldb.skillCount; i++)
            {
                if (skilldb.skills[i].level >= min && skilldb.skills[i].level <= max)
                {
                    tempSkills[idx] = skilldb.skills[i];
                    Console.WriteLine($">[{menuOrder[idx]}] {tempSkills[idx].name}({tempSkills[idx].level}) Lv{tempSkills[idx].skillLevel}");
                    idx++;
                }
            }
            ConsoleKeyInfo keys = Console.ReadKey(true);
            for (int i = 0; i < idx; i++)
            {
                if (keys.Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    screen = Screens.Skill;
                }
                if (keys.Key == keyOrder[i])
                {
                    Console.Clear();
                    screen = Screens.SkillChidStat;
                    selectedSkill = tempSkills[i];
                }
            }
        }

        static void SkillMenu()
        {
            Console.Clear();
            Console.WriteLine("===상점===");
            Console.WriteLine(">[ESC] 나가기");
            Console.WriteLine(">[1] 0~2차 스킬");
            Console.WriteLine(">[2] 3~4차 스킬");
            Console.WriteLine(">[3] 5~6차 스킬");
            Console.WriteLine(">[4] 7~8차 스킬");
            Console.WriteLine(">[5] 9~10차 스킬");

            ConsoleKeyInfo keys = Console.ReadKey(true);
            switch (keys.Key)
            {
                case ConsoleKey.D1:
                    screen = Screens.SkillChild;
                    selectedSkillLevelMin = 0;
                    selectedSkillLevelMax = 67;
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    screen = Screens.Main;
                    break;
            }
        }

        static void SaveMenu()
        {
            Console.Clear();
            FileStream fsw = new FileStream("account.cha", FileMode.Open);
            StreamWriter sw = new StreamWriter(fsw);
            sw.WriteLine(user.nickname);
            sw.WriteLine(user.profession);
            sw.WriteLine(user.level);
            sw.WriteLine(user.questProgress);
            sw.WriteLine(user.exp);
            sw.WriteLine(user.gold);
            sw.WriteLine(user.gun.name);
            sw.WriteLine(user.armor.name);
            sw.WriteLine(user.necklace.name);
            sw.WriteLine(user.avatar.name);
            sw.WriteLine(user.pendant.name);
            sw.WriteLine(user.others.name);
            sw.WriteLine(user.abilityStone.name);
            sw.Flush();
            sw.Close();
            fsw.Close();
            Console.WriteLine(">계정 데이터를 저장했습니다.");
            screen = Screens.Main;
        }

        static void MyStatMenu()
        {
            Console.Clear();
            ShowUserCurrentStat();
            screen = Screens.Main;
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
            user.ability.CalculateInventoryWeightBonus();
            user.ability.CalculateDamage();
        }

        static void ShowUserCurrentStat()
        {
            CalculateCharacterAbility();
            Console.WriteLine(">닉네임: " + user.nickname);
            Console.WriteLine($">전직: {user.profession}, {user.professionLevel}차");
            Console.WriteLine($">레벨: {user.level} ({user.exp}/{user.requireEXP})");
            Console.WriteLine(">골드: " + user.gold);
            Console.WriteLine(">SP: " + user.sp);
            Console.WriteLine(">공격력: " + user.ability.attack);
            Console.WriteLine(">방어력: " + user.ability.defense);
            Console.WriteLine(">특수방어력: " + user.ability.specialDefense);
            Console.WriteLine($">HP: {user.ability.hp.current} / {user.ability.hp.max}");
            Console.WriteLine($">MP: {user.ability.mp.current} / {user.ability.mp.max}");
            Console.WriteLine(">HP 회복: " + user.ability.hpRecovery);
            Console.WriteLine(">MP 회복: " + user.ability.mpRecovery);
            Console.WriteLine(">공격속도: " + user.ability.attackSpeed);
            Console.WriteLine($">인무: {user.ability.inventoryWeight.current} / {user.ability.inventoryWeight.max}kg");
            Console.WriteLine(">총: " + user.gun.name);
            Console.WriteLine(">갑옷: " + user.armor.name);
            Console.WriteLine(">목걸이: " + user.necklace.name);
            Console.WriteLine(">아바타: " + user.avatar.name);
            Console.WriteLine(">펜던트: " + user.pendant.name);
            Console.WriteLine(">기타: " + user.others.name);
            Console.WriteLine(">능력의돌: " + user.abilityStone.name);
            Console.WriteLine();
        }

        static void ShowBattle(Monster monster)
        {
            CalculateCharacterAbility();
            Console.WriteLine(">닉네임: " + user.nickname);
            Console.WriteLine($">레벨: {user.level} ({user.exp}/{user.requireEXP})");
            Console.WriteLine($">HP: {user.ability.hp.current} / {user.ability.hp.max}");
            Console.WriteLine($">MP: {user.ability.mp.current} / {user.ability.mp.max}");
            Console.WriteLine(">공격력: " + user.ability.attack);
            Console.WriteLine(">방어력: " + user.ability.defense);
            Console.WriteLine(">특수방어력: " + user.ability.specialDefense);
            Console.WriteLine();
            Console.WriteLine(">[x] 공격");
            Console.WriteLine(">[c] 도망");
            Console.WriteLine();
            Console.WriteLine(">몬스터 이름: " + monster.name);
            Console.WriteLine(">레벨: " + monster.level);
            Console.WriteLine($">HP: {monster.hpCur} / {monster.hpMax}");
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
