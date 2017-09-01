using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix
{
    class Maps
    {
        public string mapNum;
        public string climate;
        public string battleBGPath;

        public void Initialize()
        {

        }

        public string[] getMap(string environment, bool fishing)
        {
            string[] mapPaths = new string[2];
            switch (environment)
            {
                case "grass":
                    if (fishing)
                    {
                        mapPaths[0] = "sand_1";
                        mapPaths[1] = "sand_1_place_fish";
                    }
                    else
                    {
                        mapPaths[0] = "grass_0";
                        mapPaths[1] = "grass_0_place";
                    }
                    return mapPaths;
                default:
                    //Shouldn't happen
                    return mapPaths;
            }
        }
    }
}
