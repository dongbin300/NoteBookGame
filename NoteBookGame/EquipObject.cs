using System;

namespace NoteBookGame
{
    class EquipObject
    {
        public enum EquipObjectTypes { Gun, Armor, Necklace, Avatar, Pendant, Others, AbilityStone, Potion };
        public EquipObjectTypes type;
        public string name;
        public int level;
        public int price;
        public int weight;
        public Ability effect;

        public EquipObject()
        {

        }

        public EquipObject(EquipObjectTypes type, string name, int level, int price, int weight, Ability effect)
        {
            this.type = type;
            this.name = name;
            this.level = level;
            this.price = price;
            this.weight = weight;
            this.effect = effect;
        }

        public void ShowDescription()
        {
            Console.WriteLine($"==={name}===");
            Console.WriteLine($"레벨 {level}");
            Console.WriteLine($"{weight}kg");

            if (effect.attack != 0)
                Console.WriteLine($"공격력 +{effect.attack}");
            if (effect.attackSpeed != 0)
                Console.WriteLine($"공격속도 +{effect.attackSpeed}");
            if (effect.defense != 0)
                Console.WriteLine($"방어력 +{effect.defense}");
            if (effect.specialDefense != 0)
                Console.WriteLine($"특수방어력 +{effect.specialDefense}");
            if (effect.hp.max != 0)
                Console.WriteLine($"HP +{effect.hp.max}");
            if (effect.mp.max != 0)
                Console.WriteLine($"MP +{effect.mp.max}");
            if (effect.hpRecovery != 0)
                Console.WriteLine($"HP회복 +{effect.hpRecovery}");
            if (effect.mpRecovery != 0)
                Console.WriteLine($"MP회복 +{effect.mpRecovery}");
            if (effect.inventoryWeight.max != 0)
                Console.WriteLine($"인무 +{effect.inventoryWeight.max}kg");
            if (effect.expBonus != 0)
                Console.WriteLine($"Exp +{effect.expBonus}%");
            if (effect.goldBonus != 0)
                Console.WriteLine($"골드 +{effect.goldBonus}%");
            if (effect.spBonus != 0)
                Console.WriteLine($"Sp획득 +{effect.spBonus}%");

            Console.WriteLine($"{price}골드 (현재골드 {Character.GetInstance().gold})");
        }
    }

    class EODB
    {
        public EquipObject[] equipObjects = new EquipObject[1000];
        public int equipObjectCount = 0;

        private static EODB instance = new EODB();

        private EODB()
        {
            // a:공 d:방 c:특방 w:인무 t:공속 h:HP m:MP r:HP회복 v:MP회복 e:EXP획 g:골드획 s:SP획
            // 기본
            AddEquipObject(EquipObject.EquipObjectTypes.Gun, "기본총", 0, 0, 0, new Ability());
            AddEquipObject(EquipObject.EquipObjectTypes.Armor, "기본갑옷", 0, 0, 0, new Ability());
            AddEquipObject(EquipObject.EquipObjectTypes.Necklace, "기본목걸이", 0, 0, 0, new Ability());
            AddEquipObject(EquipObject.EquipObjectTypes.Avatar, "기본아바타", 0, 0, 0, new Ability());
            AddEquipObject(EquipObject.EquipObjectTypes.Pendant, "기본펜던트", 0, 0, 0, new Ability());
            AddEquipObject(EquipObject.EquipObjectTypes.Others, "기본기타", 0, 0, 0, new Ability());
            AddEquipObject(EquipObject.EquipObjectTypes.AbilityStone, "기본능력의돌", 0, 0, 0, new Ability());

            // 0~1차 (Lv0~47)
            AddEquipObject(EquipObject.EquipObjectTypes.Gun, "낡은구식총", 3, 100, 1, new Ability("a16"));
            AddEquipObject(EquipObject.EquipObjectTypes.Gun, "낡은총", 8, 300, 1, new Ability("a26"));
            AddEquipObject(EquipObject.EquipObjectTypes.Gun, "구식총", 12, 750, 1, new Ability("a38w1"));
            AddEquipObject(EquipObject.EquipObjectTypes.Gun, "정교한총", 18, 1600, 1, new Ability("a56w1"));
            AddEquipObject(EquipObject.EquipObjectTypes.Gun, "구식포터블", 20, 2200, 1, new Ability("a72"));
            AddEquipObject(EquipObject.EquipObjectTypes.Gun, "포터블", 26, 3000, 2, new Ability("a105t-5"));
            AddEquipObject(EquipObject.EquipObjectTypes.Gun, "구식라이징건", 30, 10000, 2, new Ability("a138t2w3h10"));
            AddEquipObject(EquipObject.EquipObjectTypes.Gun, "견고한메이식건", 40, 22000, 4, new Ability("a286t1h28"));
            AddEquipObject(EquipObject.EquipObjectTypes.Armor, "레지스트링", 3, 50, 1, new Ability("d22"));
            AddEquipObject(EquipObject.EquipObjectTypes.Armor, "레지스트링2", 9, 400, 1, new Ability("d48"));
            AddEquipObject(EquipObject.EquipObjectTypes.Armor, "레지스트링3", 15, 800, 2, new Ability("d76"));
            AddEquipObject(EquipObject.EquipObjectTypes.Armor, "레지스트링4", 22, 2500, 2, new Ability("d106c85"));
            AddEquipObject(EquipObject.EquipObjectTypes.Armor, "레지스트링5", 28, 3000, 3, new Ability("d128h6"));
            AddEquipObject(EquipObject.EquipObjectTypes.Armor, "구식링메일", 30, 8000, 4, new Ability("d175h15"));
            AddEquipObject(EquipObject.EquipObjectTypes.Armor, "견고한링", 40, 20000, 6, new Ability("d400h38"));
            AddEquipObject(EquipObject.EquipObjectTypes.Necklace, "목걸이", 3, 60, 1, new Ability("d34"));
            AddEquipObject(EquipObject.EquipObjectTypes.Necklace, "목걸이2", 6, 150, 1, new Ability("d62"));
            AddEquipObject(EquipObject.EquipObjectTypes.Necklace, "목걸이3", 16, 1800, 1, new Ability("d175c172h8"));
            AddEquipObject(EquipObject.EquipObjectTypes.Necklace, "목걸이4", 18, 700, 1, new Ability("d162"));
            AddEquipObject(EquipObject.EquipObjectTypes.Necklace, "목걸이5", 21, 3000, 1, new Ability("d258h12m8w2"));
            AddEquipObject(EquipObject.EquipObjectTypes.Necklace, "목걸이6", 26, 4000, 1, new Ability("d324h13m9w3"));
            AddEquipObject(EquipObject.EquipObjectTypes.Necklace, "목걸이7", 28, 5200, 2, new Ability("d362h15m9w4a6"));
            AddEquipObject(EquipObject.EquipObjectTypes.Necklace, "구식목걸이", 30, 9800, 2, new Ability("d405c328w2a15h17m12"));
            AddEquipObject(EquipObject.EquipObjectTypes.Necklace, "견고한목걸이", 40, 25000, 4, new Ability("d808c655w4a25h24m35"));


        }

        public static EODB GetInstance()
        {
            return instance;
        }

        public void AddEquipObject(EquipObject.EquipObjectTypes type, string name, int level, int price, int weight, Ability effect)
        {
            equipObjects[equipObjectCount++] = new EquipObject(type, name, level, price, weight, effect);
        }

        public EquipObject Equip(string name)
        {
            return GetEquipObject(name);
        }

        public EquipObject GetEquipObject(string name)
        {
            for (int i = 0; i < equipObjectCount; i++)
                if (name == equipObjects[i].name)
                    return equipObjects[i];
            return null;
        }
    }
}
