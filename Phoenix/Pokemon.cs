using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix
{
    class Pokemon
    {
        //Create RNG for Shiny
        Random rng = new Random();
        pokemon newPokemon;
        int shinyChance = 1000;//Higher = more rare
        //Create moves
        int difficulty;
        public moves NONE;
        public moves tackle;
        public moves tailWhip;
        public moves gust;
        public moves sandAttack;
        public moves growl;
        public moves scratch;
        public moves highJumpKick;
        public moves leer;

        public struct moves
        {
            public string name;
            public string type;
            public int magnitude;
            public int effect;
            public int PP;
            public string path;

            public void setMoves(string _name, string _path, string _type, int _magnitude, int _effect, int _PP)
            {
                path = _path;
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
            public int MaxHP;
            public int PP_move1;
            public int PP_move2;
            public int PP_move3;
            public int PP_move4;
            public int PP_move1_current;
            public int PP_move2_current;
            public int PP_move3_current;
            public int PP_move4_current;
            public int[] attackAffinity;
            public int[] defenseAffinity;
            public moves move1;
            public moves move2;
            public moves move3;
            public moves move4;
            public bool isShiny;
            public int pokedex;
            public string pokedexSuffix;
            public int atk;
            public int def;
            public int satk;
            public int sdef;
            public int evade;
            public int spd;
            public string status;
            public int condMaginitude;

            public void setStats(string _name, int _MaxHP, moves _move1, moves _move2, moves _move3, moves _move4, bool _isShiny, int _dex, string _dexSuffix, int _pp1, int _pp2, int _pp3, int _pp4)
            {
                attackAffinity = new int[18];
                defenseAffinity = new int[18];
                MaxHP = _MaxHP;
                HP = MaxHP;
                name = _name;
                move1 = _move1;
                move2 = _move2;
                move3 = _move3;
                move4 = _move4;
                PP_move1 = _pp1;
                PP_move2 = _pp2;
                PP_move3 = _pp3;
                PP_move4 = _pp4;
                PP_move1_current = PP_move1;
                PP_move2_current = PP_move2;
                PP_move3_current = PP_move3;
                PP_move4_current = PP_move4;
                isShiny = _isShiny;
                pokedex = _dex;
                pokedexSuffix = _dexSuffix;
            }

            public void setAttrib(int _atk, int _def, int _satk, int _sdef, int _evade, int _spd, string _status, int _condMagnitude)
            {
                atk = _atk;
                def = _def;
                satk = _satk;
                sdef = _sdef;
                evade = _evade;
                spd = _spd;
                status = _status;
                condMaginitude = _condMagnitude;
            }

            public void train(string stat)
            {
                switch (stat)
                {
                    case "atk":
                        atk++;
                        break;
                    case "satk":
                        satk++;
                        break;
                    case "def":
                        def++;
                        break;
                    case "sdef":
                        sdef++;
                        break;
                    case "evade":
                        evade++;
                        break;
                    case "spd":
                        spd++;
                        break;
                    case "HP":
                        MaxHP++;
                        HP = MaxHP;
                        break;
                    case "move1_pp":
                        PP_move1++;
                        PP_move1_current = PP_move1;
                        break;
                    case "move2_pp":
                        PP_move2++;
                        PP_move2_current = PP_move2;
                        break;
                    case "move3_pp":
                        PP_move3++;
                        PP_move3_current = PP_move3;
                        break;
                    case "move4_pp":
                        PP_move4++;
                        PP_move4_current = PP_move4;
                        break;
                    case "all":
                        atk++;
                        def++;
                        satk++;
                        sdef++;
                        evade++;
                        spd++;
                        MaxHP++;
                        HP = MaxHP;
                        PP_move1++;
                        PP_move1_current = PP_move1;
                        PP_move2++;
                        PP_move2_current = PP_move2;
                        PP_move3++;
                        PP_move3_current = PP_move3;
                        PP_move4++;
                        PP_move4_current = PP_move4;
                        break;
                }
            }

        }


        //Effects
        //1 = Atk Dmg
        //2 = SAtk Dmg
        //3 = Def-
        //4 = Def+
        //5 = SDef-
        //6 = SDef+
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
        //17 = +Acc
        //18 = -Acc
        //19 = -Evade
        //20 = +Evade

        public void InitializeMoves()
        {
            NONE.setMoves("NONE", "", "", 0, 0, 0);
            tackle.setMoves("Tackle", "", "Normal", 1, 1, 35);
            tailWhip.setMoves("Tail Whip","tail_whip", "Normal", 1, 4, 30);
            gust.setMoves("Gust", "gust", "Flying", 1, 1, 30);
            sandAttack.setMoves("Sand Attack", "sand_attack", "Normal", 1, 18, 30);
            growl.setMoves("Growl", "growl", "Normal", 1, 8, 30);
            scratch.setMoves("Scratch", "scratch", "Normal", 1, 1, 35);
            highJumpKick.setMoves("High Jump Kick", "high_jump_kick", "Fighting", 1, 1, 15);
            leer.setMoves("Leer", "leer", "Normal", 1, 3, 30);
        }
        //Affinity
        //0 = Normal
        //1 = Flying
        //2 = Fighting
            

        //Get pokemon for the current Map
        public int[][] getPokemonID(string mapID)
        {
            XmlDocument mapFile = new XmlDocument();
            mapFile.Load("Content/maps/MapMeta.xml");
            XmlNodeList nodes = mapFile.DocumentElement.SelectNodes("/metadata/map");
            XmlNodeList mapNodes;
            int[][] encounters = new int[0][];
            

            for(int x = 0; x < nodes.Count; x++)
            {
                if(nodes[x].Attributes["encounterID"].Value == mapID)
                {
                    mapNodes = nodes[x].SelectNodes("pokemon");
                    difficulty = int.Parse(nodes[x].Attributes["difficulty"].Value);
                    encounters = new int[mapNodes.Count][];
                    for(int y = 0; y < mapNodes.Count; y++)
                    {
                        encounters[y] = new int[3];
                        encounters[y][0] = int.Parse(mapNodes[y].Attributes["id"].Value);
                        encounters[y][1] = int.Parse(mapNodes[y].Attributes["weight"].Value);
                    }
                }
            }
            return encounters;
        }

        //Generate the Pokemon based on ID (pokedex) number and suffix
        public pokemon generatePokemon(string id, int difficulty)
        {
            bool isShiny = false;
            if (rng.Next(0, shinyChance) + 1 == 1) isShiny = true;
            newPokemon = new pokemon();
            switch (id)
            {
                case "1":
                    newPokemon.setStats("Bulbasaur", 10, tackle, leer, NONE, NONE, isShiny, int.Parse(id), "", tackle.PP, growl.PP, NONE.PP, NONE.PP);
                    newPokemon.setAttrib(5, 5, 5, 5, 5, 5, "Normal", 1);
                    break;
                case "4":
                    newPokemon.setStats("Charmander", 10, scratch, growl, NONE, NONE, isShiny, int.Parse(id), "", scratch.PP, growl.PP, NONE.PP, NONE.PP);
                    newPokemon.setAttrib(5, 5, 5, 5, 5, 5, "Normal", 1);
                    break;
                case "7":
                    newPokemon.setStats("Squritle", 10, tackle, tailWhip, NONE, NONE, isShiny, int.Parse(id), "", tackle.PP, tailWhip.PP, NONE.PP, NONE.PP);
                    newPokemon.setAttrib(5, 5, 5, 5, 5, 5, "Normal", 1);
                    break;
                case "16":
                    newPokemon.setStats("Pidgey", 10, gust, sandAttack, NONE, NONE, isShiny, int.Parse(id), "", gust.PP, sandAttack.PP, NONE.PP, NONE.PP);
                    newPokemon.setAttrib(5, 5, 5, 5, 5, 5, "Normal", 1);
                    break;
                case "19":
                    newPokemon.setStats("Rattata", 10, tackle, tailWhip, NONE, NONE, isShiny, int.Parse(id), "", tackle.PP, tailWhip.PP, NONE.PP, NONE.PP);
                    newPokemon.setAttrib(5, 5, 5, 5, 5, 5, "Normal", 1);
                    break;
                case "25":
                    newPokemon.setStats("Pikachu", 10, tackle, tailWhip, NONE, NONE, isShiny, int.Parse(id), "", tackle.PP, tailWhip.PP, NONE.PP, NONE.PP);
                    newPokemon.setAttrib(5, 5, 5, 5, 5, 5, "Normal", 1);
                    break;
                default:
                    newPokemon.setStats("MissingNo", 10, NONE, NONE, NONE, NONE, false, 0, "", tackle.PP, growl.PP, NONE.PP, NONE.PP);
                    newPokemon.setAttrib(5, 5, 5, 5, 5, 5, "Normal", 1);
                    break;
            }
            //Default affinities to 5000
            for(int x = 0; x < 18; x++)
            {
                newPokemon.attackAffinity[x] = 5000;
                newPokemon.defenseAffinity[x] = 5000;
            }
            //Function to adjust affinity based on type
            //Function to adjust affinity based on location
            adjustDifficulty();//Function to adjust base states based on difficulty
            return newPokemon;
        }

        public void adjustDifficulty()
        {
            for(int x = 0; x < difficulty; x++)
            {
                switch(rng.Next(0, 6) + 1)
                {
                    case 1:
                        newPokemon.atk++;
                        break;
                    case 2:
                        newPokemon.satk++;
                        break;
                    case 3:
                        newPokemon.def++;
                        break;
                    case 4:
                        newPokemon.sdef++;
                        break;
                    case 5:
                        newPokemon.evade++;
                        break;
                    case 6:
                        newPokemon.spd++;
                        break;
                    default:
                        Console.WriteLine("WTF stat are you trying to improve?");
                        break;
                }
            }
        }
    }
}
