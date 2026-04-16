using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board_Game_Manager.Models
{
    public class PlayerLeaderboardItem
    {
        public int PlayerID { get; set; }
        public string Name { get; set; } = "";
        public int TotalScore { get; set; }
        public double TotalHours { get; set; }
        public int GMContribution { get; set; }
    }
}
