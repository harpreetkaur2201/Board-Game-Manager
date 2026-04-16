using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board_Game_Manager.Models {
    public class GameMaster {
        public int GMID { get; set; }
        public int PlayerID { get; set; } 

        public Player Player { get; set; }
        public ICollection<GameSession> GameSessions { get; set; } = new List<GameSession>();
    }
}
