using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HerbstmatchRanking
{
    public class Participant
    {
        public int Rank { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Points { get; set; }

        public string RankString
        {
            get
            {
                return Rank + ". Platz";
            }
        }

        public string PointString
        {
            get
            {
                return Points + " Punkte";
            }
        }
    }
}
