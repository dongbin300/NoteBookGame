using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteBookGame
{
    class EquipObject
    {
        public string name;
        public int level;
        public int price;
        public int weight;
        public Ability effect;

        public EquipObject(string name, int level, int price, int weight, Ability effect)
        {
            this.name = name;
            this.level = level;
            this.price = price;
            this.weight = weight;
            this.effect = effect;
        }
    }

    class EODB
    {
        // a:공 d:방 c:특방 w:인무 t:공속 h:HP m:MP r:HP회복 v:MP회복 e:EXP획 g:골드획 s:SP획
        public EquipObject gun001 = new EquipObject("낡은구식총", 3, 100, 1, new Ability("a16"));
        public EquipObject gun002 = new EquipObject("낡은총", 8, 300, 1, new Ability("a26"));
        public EquipObject gun003 = new EquipObject("구식총", 12, 750, 1, new Ability("a38w1"));
        public EquipObject gun004 = new EquipObject("정교한총", 18, 1600, 1, new Ability("a56w1"));
        public EquipObject gun005 = new EquipObject("구식포터블", 20, 2200, 1, new Ability("a72"));
        public EquipObject gun006 = new EquipObject("포터블", 26, 3000, 2, new Ability("a105t-5"));

        public EquipObject armor001 = new EquipObject("레지스트링", 3, 50, 1, new Ability("d22"));
        public EquipObject armor002 = new EquipObject("레지스트링2", 9, 400, 1, new Ability("d48"));
        public EquipObject armor003 = new EquipObject("레지스트링3", 15, 800, 2, new Ability("d76"));
        public EquipObject armor004 = new EquipObject("레지스트링4", 22, 2500, 2, new Ability("d106c85"));
        public EquipObject armor005 = new EquipObject("레지스트링5", 28, 3000, 3, new Ability("d128h6"));

        public EquipObject necklace001 = new EquipObject("목걸이", 3, 60, 1, new Ability("d34"));
        public EquipObject necklace002 = new EquipObject("목걸이2", 6, 150, 1, new Ability("d62"));
        public EquipObject necklace003 = new EquipObject("목걸이3", 16, 1800, 1, new Ability("d175c172h8"));
        public EquipObject necklace004 = new EquipObject("목걸이4", 18, 700, 1, new Ability("d162"));
        public EquipObject necklace005 = new EquipObject("목걸이5", 21, 3000, 1, new Ability("d258h12m8w2"));
        public EquipObject necklace006 = new EquipObject("목걸이6", 26, 4000, 1, new Ability("d324h13m9w3"));
        public EquipObject necklace007 = new EquipObject("목걸이7", 28, 5200, 2, new Ability("d362h15m9w4a6"));

        private static EODB instance = new EODB();

        private EODB()
        {
        }

        public static EODB GetInstance()
        {
            return instance;
        }
    }
}
