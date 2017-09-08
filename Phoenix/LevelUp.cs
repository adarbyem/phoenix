using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix
{
    class LevelUp
    {
        Random rng = new Random();
        Pokemon pokemon = new Pokemon();
        

        //Determine what stat to increase
        public string levelUp()
        {
            switch(rng.Next(0, 7) + 1)
            {
                case 1:
                    return "ATK";
                case 2:
                    return "S.ATK";
                case 3:
                    return "DEF";
                case 4:
                    return "S.DEF";
                case 5:
                    return "HP";
                case 6:
                    return "SPD";
                case 7:
                    return "EVD";
                default:
                    return "???";
            }
        }

        public Pokemon.moves moveCheck(int affinityLevel, int dex, int index)
        {
            pokemon.InitializeMoves();
            switch (dex)
            {
                case 1:
                    if (affinityLevel > 60 && index < 1) return pokemon.vineWhip;
                    else if (affinityLevel > 100 && index < 2) return pokemon.poisonPowder;
                    break;
                case 4:
                    if (affinityLevel > 60 && index < 1) return pokemon.ember;
                    else if (affinityLevel > 100 && index < 2) return pokemon.smokeScreen;
                    break;
                case 7:
                    if (affinityLevel > 60 && index < 1) return pokemon.bubble;
                    else if (affinityLevel > 100 && index < 2) return pokemon.bite;
                    break;
                case 16:
                    if (affinityLevel > 60 && index < 1) return pokemon.leer;
                    else if (affinityLevel > 100 && index < 2) return pokemon.peck;
                    break;
                case 19:
                    if (affinityLevel > 60 && index < 1) return pokemon.bite;
                    else if (affinityLevel > 100 && index < 2) return pokemon.growl;
                    break;
                case 25:
                    if(affinityLevel > 60 && index < 1) return pokemon.thunderShock;
                    else if (affinityLevel > 100 && index < 2) return pokemon.thunderWave;
                    break;
                case 69:
                    if (affinityLevel > 60 && index < 1) return pokemon.stunPowder;
                    else if (affinityLevel > 100 && index < 2) return pokemon.poisonPowder;
                    break;
            }
            return pokemon.blank;
        }
    }
}