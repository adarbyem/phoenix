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
    }
}
/*
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
*/