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
        Random shiny = new Random();
        int shinyChance = 1000;//Higher = more rare
        //Create moves
        public moves NONE;
        public moves tackle;
        public moves tailWhip;
        public moves gust;
        public moves sandAttack;
        public moves growl;
        public moves scratch;
        public moves highJumpKick;

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
            public int SPD;
            public int PP_move1;
            public int PP_move2;
            public int PP_move3;
            public int PP_move4;
            public int[] attackAffinity;
            public int[] defenseAffinity;
            public moves move1;
            public moves move2;
            public moves move3;
            public moves move4;
            public bool isShiny;
            public int pokedex;
            public string pokedexSuffix;

            public void setStats(string _name, moves _move1, moves _move2, moves _move3, moves _move4, bool _isShiny, int _dex, string _dexSuffix, int _spd)
            {
                name = _name;
                move1 = _move1;
                move2 = _move2;
                move3 = _move3;
                move4 = _move4;
                isShiny = _isShiny;
                pokedex = _dex;
                pokedexSuffix = _dexSuffix;
                SPD = _spd;
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
        //17 = +Acc
        //18 = -Acc
        public void InitializeMoves()
        {
            NONE.setMoves("", "", 0, 0, 0);
            tackle.setMoves("Tackle", "Normal", 1, 1, 35);
            tailWhip.setMoves("Tail Whip", "Normal", 1, 4, 30);
            gust.setMoves("Gust", "Flying", 1, 2, 30);
            sandAttack.setMoves("Sand Attack", "Normal", 1, 18, 30);
            growl.setMoves("Growl", "Normal", 1, 8, 30);
            scratch.setMoves("Scratch", "Normal", 1, 1, 35);
            highJumpKick.setMoves("High Jump Kick", "Fighting", 1, 1, 15);
        }
        
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
                    encounters = new int[mapNodes.Count][];
                    for(int y = 0; y < mapNodes.Count; y++)
                    {
                        encounters[y] = new int[2];
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
            if (shiny.Next(0, shinyChance) + 1 == 1) isShiny = true;
            pokemon newPokemon = new pokemon();
            switch (id)
            {
                case "1":
                    newPokemon.setStats("Bulbasaur", highJumpKick, highJumpKick, highJumpKick, highJumpKick, isShiny, int.Parse(id), "", 1);
                    break;
                case "4":
                    newPokemon.setStats("Charmander", scratch, growl, NONE, NONE, isShiny, int.Parse(id), "", 1);
                    break;
                case "7":
                    newPokemon.setStats("Squritle", tackle, tailWhip, NONE, NONE, isShiny, int.Parse(id), "", 1);
                    break;
                case "16":
                    newPokemon.setStats("Pidgey", gust, sandAttack, NONE, NONE, isShiny, int.Parse(id), "", 1);
                    break;
                case "19":
                    newPokemon.setStats("Rattata", tackle, tailWhip, NONE, NONE, isShiny, int.Parse(id), "", 1);
                    break;
                case "25":
                    newPokemon.setStats("Pikachu", tackle, tailWhip, NONE, NONE, isShiny, int.Parse(id), "", 1);
                    break;
                case "130":
                    newPokemon.setStats("Gyarados", tackle, tailWhip, NONE, NONE, isShiny, int.Parse(id), "", 1);
                    break;
                default:
                    newPokemon.setStats("MissingNo", NONE, NONE, NONE, NONE, false, 0, "", 0);
                    break;
            }
            return newPokemon;
        }
    }
}
