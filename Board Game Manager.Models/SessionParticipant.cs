using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board_Game_Manager.Models {
    public class SessionParticipant {
        public int SessionID { get; set; }
        public int PlayerID { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
        public bool IsWinner { get; set; }
        public DateTime JoinedAt { get; set; }

        public GameSession? GameSession { get; set; }
        public Player? Player { get; set; }
    }
}
