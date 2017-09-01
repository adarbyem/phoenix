using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix
{
    class Pokemon
    {
        //Create moves
        public moves NONE;
        public moves tackle;
        public moves tailWhip;
        public moves gust;
        

        //Create pokemon
        public pokemon rattata;
        public pokemon pidgey;

        public struct moves
        {
            public string name;
            public string type;
            public int magnitude;
            public int effect;
            public int PP;

            public void setMoves(string _name, string _type, int _magnitude, int _effect, int _PP)
            {
                name = _name;
                type = _type;
                magnitude = _magnitude;
                effect = _effect;
                PP = _PP;
            }
        }

        public struct pokemon
        {

            public string name;
            public int HP;
            public int PP_move2;
            public int PP_move3;
            public int PP_move4;
            public int[] attackAffinity;
            public int[] defenseAffinity;
            public moves move1;
            public moves move2;
            public moves move3;
            public moves move4;

            public void setStats(string _name, moves _move1, moves _move2, moves _move3, moves _move4)
            {
                name = _name;
                move1 = _move1;
                move2 = _move2;
                move3 = _move3;
                move4 = _move4;
            }
        }


        //Effects
        //1 = Physcial Damage
        //2 = Magic Damage
        //3 = PDef-
        //4 = PDef+
        //5 = MDef-
        //6 = MDef+
        //7 = Atk+
        //8 = Atk-
        //9 = Spd+
        //10 = Spd-
        //11 = Sleep
        //12 = Paralyse
        //13 = Stun
        //14 = Confuse
        //15 = Toxic
        //16 = Charm
        public void InitializeMoves()
        {
            NONE.setMoves("", "", 0, 0, 0);
            tackle.setMoves("Tackle", "Normal", 1, 1, 35);
            tailWhip.setMoves("Tail Whip", "Normal", 1, 4, 30);
            gust.setMoves("Gust", "Flying", 1, 2, 30);
        }

        public void InitializePokemon()
        {
            rattata.setStats("Rattata", tackle, tailWhip, NONE, NONE);
            pidgey.setStats("Pidgey", gust, NONE, NONE, NONE);
        }
    }
}
