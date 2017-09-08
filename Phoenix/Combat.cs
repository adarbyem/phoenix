using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix
{
    class Combat
    {
        public double getAtkMult(int offense, int defense, int atkAffinity, int defAffinity, int magnitude)
        {
            if (offense < 0) offense = 0;
            if (defense <= 0) defense = 1;
            double result = (((double)offense / (double)defense) * ((double)atkAffinity / (double)defAffinity) * (magnitude) * ((double)offense / 50));
            result = Math.Ceiling(result);
            return result;
        }
    }
}
