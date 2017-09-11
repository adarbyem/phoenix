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
        int shinyChance = 3000;//Higher = more rare default 3000
        int femaleChance = 40;//Default 40
        //Condition moves (Not to be assigned)
        public moves poisonDamage;
        public moves paralyzeDamage;
        public moves blank;
        public moves sleepDamage;
        public moves confusionDamage;
        public moves frozenDamage;
        public moves burnDamage;
        

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
        public moves sleepPowder;
        public moves poisonPowder;
        public moves stunPowder;
        public moves vineWhip;
        public moves bubble;
        public moves bite;
        public moves ember;
        public moves smokeScreen;
        public moves thunderWave;
        public moves peck;
        public moves thunderShock;

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
            public bool isFemale;
            public bool hasFemaleSprite;
            public int totalAffinity;
            public int currentMoveIndex;
            public string type1;
            public string type2;
            public int startAffinity;

            public void setStats(string _name, int _MaxHP, moves _move1, moves _move2, moves _move3, moves _move4, bool _isShiny, int _dex, string _dexSuffix, int _pp1, int _pp2, int _pp3, int _pp4, bool _isFemale, bool _hasSprite)
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
                isFemale = _isFemale;
                hasFemaleSprite = _hasSprite;
                totalAffinity = 0;
                currentMoveIndex = 0;
            }

            public void setAttrib(int _atk, int _def, int _satk, int _sdef, int _evade, int _spd, string _status, string _type1, string _type2)
            {
                atk = _atk;
                def = _def;
                satk = _satk;
                sdef = _sdef;
                evade = _evade;
                spd = _spd;
                status = _status;
                type1 = _type1;
                type2 = _type2;
            }

            public void train(int stat)
            {
                switch (stat)
                {
                    case 1:
                        atk++;
                        break;
                    case 2:
                        satk++;
                        break;
                    case 3:
                        def++;
                        break;
                    case 4:
                        sdef++;
                        break;
                    case 5:
                        evade++;
                        break;
                    case 6:
                        spd++;
                        break;
                    case 7:
                        MaxHP++;
                        HP = MaxHP;
                        break;
                    case 8:
                        PP_move1++;
                        PP_move1_current = PP_move1;
                        break;
                    case 9:
                        PP_move2++;
                        PP_move2_current = PP_move2;
                        break;
                    case 10:
                        PP_move3++;
                        PP_move3_current = PP_move3;
                        break;
                    case 11:
                        PP_move4++;
                        PP_move4_current = PP_move4;
                        break;
                    case 12:
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

            public void fullHeal()
            {
                HP = MaxHP;
                status = "normal";
                PP_move1_current = PP_move1;
                PP_move2_current = PP_move2;
                PP_move3_current = PP_move3;
                PP_move4_current = PP_move4;
            }

            public void boostAffinity()
            {
                for(int affinityIndex = 0; affinityIndex < attackAffinity.Length; affinityIndex++)
                {
                    attackAffinity[affinityIndex]++;
                    defenseAffinity[affinityIndex]++;
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
        //15 = Poison
        //16 = Toxic
        //17 = Charm
        //18 = +Acc
        //19 = -Acc
        //20 = -Evade
        //21 = +Evade
        //22 = Burn
        //23 = Frozen


        public void InitializeMoves()
        {
            //placehoder and effect moves not to be assigned
            NONE.setMoves("NONE", "", "", 0, 0, 0);
            poisonDamage.setMoves("poison", "poison", "Poison", 0, 0, 0);
            paralyzeDamage.setMoves("paralyze", "paralyze", "Normal", 0, 0, 0);
            blank.setMoves("blank", "blank", "Normal", 0, 0, 0);
            sleepDamage.setMoves("sleep", "sleep", "Normal", 0, 0, 0);
            confusionDamage.setMoves("confuse", "confuse", "Normal", 0, 0, 0);
            frozenDamage.setMoves("frozen", "frozen", "Normal", 0, 0, 0);
            burnDamage.setMoves("burn", "burn", "Normal", 0, 0, 0);

            //Assignable Moves
            tackle.setMoves("Tackle", "", "Normal", 2, 1, 35);
            tailWhip.setMoves("Tail Whip","tail_whip", "Normal", 5, 3, 30);
            gust.setMoves("Gust", "gust", "Flying", 2, 1, 35);
            sandAttack.setMoves("Sand Attack", "sand_attack", "Normal", 30, 19, 30);
            growl.setMoves("Growl", "growl", "Normal", 5, 8, 30);
            scratch.setMoves("Scratch", "scratch", "Normal", 2, 1, 35);
            highJumpKick.setMoves("High Jump Kick", "high_jump_kick", "Fighting", 1, 1, 15);
            leer.setMoves("Leer", "leer", "Normal", 5, 3, 30);
            stunPowder.setMoves("Stun Powder", "stun_powder", "Grass", 0, 13, 20);
            sleepPowder.setMoves("Sleep Powder", "sleep_powder", "Grass", 0, 11, 20);
            poisonPowder.setMoves("Poison Powder", "poison_powder", "Poison", 0, 15, 20);
            thunderShock.setMoves("Thunder Shock", "thunder_shock", "Electric", 4, 2, 30);
            vineWhip.setMoves("Vine Whip", "vine_whip", "Grass", 4, 1, 30);
            bubble.setMoves("Bubble", "bubble", "Water", 4, 2, 30);
            ember.setMoves("Ember", "ember", "Fire", 4, 2, 30);
            smokeScreen.setMoves("Smokescreen", "smoke_screen", "Fire", 30, 19, 30);
            bite.setMoves("Bite", "bite", "Dark", 4, 1, 30);
            thunderWave.setMoves("Thunder Wave", "thunder_wave", "Electric", 0, 13, 30);
            peck.setMoves("Peck", "peck", "Flying", 4, 1, 30);
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
        public pokemon generatePokemon(string id)
        {
            bool isShiny = false;
            bool isFemale = false;
            if (rng.Next(0, shinyChance) + 1 == 1) isShiny = true;
            if (rng.Next(0, 110) + 1 < femaleChance) isFemale = true;
            else isFemale = false;
            
            //Default base stats = 50, 50, 50, 50, 50, 50
            newPokemon = new pokemon();
            switch (id)
            {
                case "1":
                    newPokemon.setStats("Bulbasaur", 10, tackle, leer, NONE, NONE, isShiny, int.Parse(id), "", tackle.PP, growl.PP, NONE.PP, NONE.PP, isFemale, false);
                    newPokemon.setAttrib(50, 50, 50, 50, 50, 50, "normal", "grass", "blank");
                    break;
                case "4":
                    newPokemon.setStats("Charmander", 10, scratch, growl, NONE, NONE, isShiny, int.Parse(id), "", scratch.PP, growl.PP, NONE.PP, NONE.PP, isFemale, false);
                    newPokemon.setAttrib(50, 50, 50, 50, 50, 50, "normal", "fire", "blank");
                    break;
                case "7":
                    newPokemon.setStats("Squirtle", 10, tackle, tailWhip, poisonPowder, stunPowder, isShiny, int.Parse(id), "", tackle.PP, tailWhip.PP, NONE.PP, NONE.PP, isFemale, false);
                    newPokemon.setAttrib(50, 50, 50, 50, 50, 50, "normal", "water", "blank");
                    break;
                case "16":
                    newPokemon.setStats("Pidgey", 10, gust, sandAttack, NONE, NONE, isShiny, int.Parse(id), "", gust.PP, sandAttack.PP, NONE.PP, NONE.PP, isFemale, false);
                    newPokemon.setAttrib(50, 50, 50, 50, 50, 50, "normal", "flying", "blank");
                    break;
                case "19":
                    newPokemon.setStats("Rattata", 10, tackle, tailWhip, NONE, NONE, isShiny, int.Parse(id), "", tackle.PP, tailWhip.PP, NONE.PP, NONE.PP, isFemale, true);
                    newPokemon.setAttrib(50, 50, 50, 50, 50, 50, "normal", "normal", "blank");
                    break;
                case "25":
                    newPokemon.setStats("Pikachu", 10, tackle, tailWhip, NONE, NONE, isShiny, int.Parse(id), "", tackle.PP, tailWhip.PP, NONE.PP, NONE.PP, isFemale, true);
                    newPokemon.setAttrib(50, 50, 50, 50, 50, 50, "normal", "electric", "blank");
                    break;
                case "69":
                    newPokemon.setStats("Bellsprout", 10, vineWhip, NONE, NONE, NONE, isShiny, int.Parse(id), "", tackle.PP, tailWhip.PP, NONE.PP, NONE.PP, isFemale, false);
                    newPokemon.setAttrib(50, 50, 50, 50, 50, 50, "normal", "grass", "blank");
                    break;
                default:
                    newPokemon.setStats("MissingNo", 10, NONE, NONE, NONE, NONE, false, 0, "", tackle.PP, growl.PP, NONE.PP, NONE.PP, isFemale, false);
                    newPokemon.setAttrib(50, 50, 50, 50, 50, 50, "normal", "blank", "blank");
                    break;
            }
            //Default affinities to 5000
            for(int x = 0; x < 18; x++)
            {
                newPokemon.attackAffinity[x] = 5000;
                newPokemon.defenseAffinity[x] = 5000;
            }
            //Function to adjust affinity based on location
            adjustAffinityByType();//Function to adjust the affinities based on pokemon type
            adjustDifficulty();//Function to adjust base states based on difficulty
            return newPokemon;
        }

        //Adjust difficulty based on map difficulty
        public void adjustDifficulty()
        {
            LevelUp levelUp = new LevelUp();
            int sum = 0;
            for (int x = 0; x < difficulty * 20; x++)
            {
                newPokemon.attackAffinity[rng.Next(0, 17) + 1]++;
                newPokemon.defenseAffinity[rng.Next(0, 17) + 1]++;
            }
            foreach(int x in newPokemon.attackAffinity)
            {
                sum += x;
            }
            foreach(int x in newPokemon.defenseAffinity)
            {
                sum += x;
            }
            for (int x = 0; x < difficulty; x++)
            {
                switch(rng.Next(0, 7) + 1)
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
                        newPokemon.MaxHP++;
                        newPokemon.HP++;
                        break;
                    case 6:
                        newPokemon.spd++;
                        break;
                    case 7:
                        newPokemon.evade++;
                        break;
                    default:
                        Console.WriteLine("WTF stat are you trying to improve?");
                        break;
                }
                moves moveToLearn = levelUp.moveCheck(newPokemon.startAffinity - sum, newPokemon.pokedex, newPokemon.currentMoveIndex);
                if (moveToLearn.name != "blank")
                {
                    if (newPokemon.move3.name != "NONE")
                    {
                        if (newPokemon.move4.name != "NONE")
                        {
                            newPokemon.move1 = newPokemon.move2;
                            newPokemon.move2 = newPokemon.move3;
                            newPokemon.move3 = newPokemon.move4;
                            newPokemon.move4 = moveToLearn;
                        }
                        else newPokemon.move4 = moveToLearn;
                    }
                    else newPokemon.move3 = moveToLearn;
                    newPokemon.currentMoveIndex++;
                }
            }

        }

        //Adjust affinity based on type
        public void adjustAffinityByType()
        {
            string type;
            for(int x = 0; x < 2; x++)
            {
                if (x == 0) type = newPokemon.type1;
                else type = newPokemon.type2;
                switch (type)
                {
                    case ("normal"):
                        newPokemon.defenseAffinity[2] -= 2500;
                        newPokemon.defenseAffinity[11] += 2500;
                        break;
                    case ("flying"):
                        newPokemon.defenseAffinity[8] += 2500;
                        newPokemon.defenseAffinity[2] += 2500;
                        newPokemon.defenseAffinity[3] += 2500;
                        newPokemon.defenseAffinity[6] -= 2500;
                        newPokemon.defenseAffinity[7] -= 2500;
                        newPokemon.defenseAffinity[12] -= 2500;
                        break;
                    case ("fighting"):
                        newPokemon.defenseAffinity[8] += 2500;
                        newPokemon.defenseAffinity[13] += 2500;
                        newPokemon.defenseAffinity[12] += 2500;
                        newPokemon.defenseAffinity[1] -= 2500;
                        newPokemon.defenseAffinity[14] -= 2500;
                        newPokemon.defenseAffinity[15] -= 2500;
                        break;
                    case ("grass"):
                        newPokemon.defenseAffinity[3] += 2500;
                        newPokemon.defenseAffinity[6] += 2500;
                        newPokemon.defenseAffinity[9] += 2500;
                        newPokemon.defenseAffinity[10] += 2500;
                        newPokemon.defenseAffinity[5] -= 2500;
                        newPokemon.defenseAffinity[7] -= 2500;
                        newPokemon.defenseAffinity[1] -= 2500;
                        newPokemon.defenseAffinity[8] -= 2500;
                        newPokemon.defenseAffinity[4] -= 2500;
                        break;
                    case ("poison"):
                        newPokemon.defenseAffinity[2] += 2500;
                        newPokemon.defenseAffinity[4] += 2500;
                        newPokemon.defenseAffinity[3] += 2500;
                        newPokemon.defenseAffinity[10] -= 2500;
                        newPokemon.defenseAffinity[8] -= 2500;
                        newPokemon.defenseAffinity[14] -= 2500;
                        break;
                    case ("fire"):
                        newPokemon.defenseAffinity[5] += 2500;
                        newPokemon.defenseAffinity[8] += 2500;
                        newPokemon.defenseAffinity[3] += 2500;
                        newPokemon.defenseAffinity[7] += 2500;
                        newPokemon.defenseAffinity[16] += 2500;
                        newPokemon.defenseAffinity[9] -= 2500;
                        newPokemon.defenseAffinity[10] -= 2500;
                        newPokemon.defenseAffinity[12] -= 2500;
                        break;
                    case ("electric"):
                        newPokemon.defenseAffinity[6] += 2500;
                        newPokemon.defenseAffinity[1] += 2500;
                        newPokemon.defenseAffinity[16] += 2500;
                        newPokemon.defenseAffinity[10] -= 2500;
                        break;
                    case ("ice"):
                        newPokemon.defenseAffinity[7] += 2500;
                        newPokemon.defenseAffinity[2] -= 2500;
                        newPokemon.defenseAffinity[5] -= 2500;
                        newPokemon.defenseAffinity[12] -= 2500;
                        newPokemon.defenseAffinity[16] -= 2500;
                        break;
                    case ("bug"):
                        newPokemon.defenseAffinity[2] += 2500;
                        newPokemon.defenseAffinity[3] += 2500;
                        newPokemon.defenseAffinity[10] += 2500;
                        newPokemon.defenseAffinity[5] -= 2500;
                        newPokemon.defenseAffinity[1] -= 2500;
                        newPokemon.defenseAffinity[12] -= 2500;
                        break;
                    case ("water"):
                        newPokemon.defenseAffinity[5] += 2500;
                        newPokemon.defenseAffinity[7] += 2500;
                        newPokemon.defenseAffinity[16] += 2500;
                        newPokemon.defenseAffinity[9] += 2500;
                        newPokemon.defenseAffinity[6] -= 2500;
                        newPokemon.defenseAffinity[3] -= 2500;
                        break;
                    case ("ground"):
                        newPokemon.defenseAffinity[4] += 2500;
                        newPokemon.defenseAffinity[12] += 2500;
                        newPokemon.defenseAffinity[10] += 2500;
                        newPokemon.defenseAffinity[3] -= 2500;
                        newPokemon.defenseAffinity[7] -= 2500;
                        newPokemon.defenseAffinity[9] -= 2500;
                        break;
                    case ("ghost"):
                        newPokemon.defenseAffinity[0] += 2500;
                        newPokemon.defenseAffinity[2] += 2500;
                        newPokemon.defenseAffinity[8] += 2500;
                        newPokemon.defenseAffinity[4] += 2500;
                        newPokemon.defenseAffinity[13] -= 2500;
                        newPokemon.defenseAffinity[11] -= 2500;
                        break;
                    case ("rock"):
                        newPokemon.defenseAffinity[5] += 2500;
                        newPokemon.defenseAffinity[1] += 2500;
                        newPokemon.defenseAffinity[0] += 2500;
                        newPokemon.defenseAffinity[4] += 2500;
                        newPokemon.defenseAffinity[2] -= 2500;
                        newPokemon.defenseAffinity[3] -= 2500;
                        newPokemon.defenseAffinity[10] -= 2500;
                        newPokemon.defenseAffinity[16] -= 2500;
                        newPokemon.defenseAffinity[9] -= 2500;
                        break;
                    case ("dark"):
                        newPokemon.defenseAffinity[14] += 2500;
                        newPokemon.defenseAffinity[13] += 2500;
                        newPokemon.defenseAffinity[11] += 2500;
                        newPokemon.defenseAffinity[8] -= 2500;
                        newPokemon.defenseAffinity[15] -= 2500;
                        newPokemon.defenseAffinity[2] -= 2500;
                        break;
                    case ("psychic"):
                        newPokemon.defenseAffinity[14] += 2500;
                        newPokemon.defenseAffinity[13] += 2500;
                        newPokemon.defenseAffinity[8] -= 2500;
                        newPokemon.defenseAffinity[15] -= 2500;
                        newPokemon.defenseAffinity[2] -= 2500;
                        break;
                    case ("fairy"):
                        newPokemon.defenseAffinity[8] += 2500;
                        newPokemon.defenseAffinity[2] += 2500;
                        newPokemon.defenseAffinity[14] += 2500;
                        newPokemon.defenseAffinity[17] += 2500;
                        newPokemon.defenseAffinity[4] -= 2500;
                        newPokemon.defenseAffinity[16] -= 2500;
                        break;
                    case ("steel"):
                        newPokemon.defenseAffinity[8] += 2500;
                        newPokemon.defenseAffinity[17] += 2500;
                        newPokemon.defenseAffinity[15] += 2500;
                        newPokemon.defenseAffinity[1] += 2500;
                        newPokemon.defenseAffinity[3] += 2500;
                        newPokemon.defenseAffinity[7] += 2500;
                        newPokemon.defenseAffinity[0] += 2500;
                        newPokemon.defenseAffinity[14] += 2500;
                        newPokemon.defenseAffinity[12] += 2500;
                        newPokemon.defenseAffinity[16] += 2500;
                        newPokemon.defenseAffinity[4] += 2500;
                        newPokemon.defenseAffinity[2] -= 2500;
                        newPokemon.defenseAffinity[5] -= 2500;
                        newPokemon.defenseAffinity[10] -= 2500;
                        break;
                    case ("dragon"):
                        newPokemon.defenseAffinity[6] += 2500;
                        newPokemon.defenseAffinity[5] += 2500;
                        newPokemon.defenseAffinity[3] += 2500;
                        newPokemon.defenseAffinity[9] += 2500;
                        newPokemon.defenseAffinity[17] -= 2500;
                        newPokemon.defenseAffinity[15] -= 2500;
                        newPokemon.defenseAffinity[7] -= 2500;
                        break;
                }
            }
            foreach(int x in newPokemon.attackAffinity)
            {
                newPokemon.startAffinity += x;
            }
            foreach(int x in newPokemon.defenseAffinity)
            {
                newPokemon.startAffinity += x;
            }
        }
        /*
                    "Normal": 0
                    "Flying": 1
                    "Fighting": 2
                    "Grass": 3
                    "Poison": 4
                    "Fire": 5
                    "Electric": 6
                    "Ice": 7
                    "Bug": 8
                    "Water": 9
                    "Ground": 10
                    "Ghost": 11
                    "Rock": 12
                    "Dark": 13
                    "Psychic": 14
                    "Fairy: 15
                    "Steel": 16
                    "Dragon: 17
                   */
    }
}
