using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board_Game_Manager.Models {
    public class GameSession {
        public int SessionID { get; set; }
        public int GameID { get; set; }
        public int GMID { get; set; }
        public int? TournamentID { get; set; }
        public DateTime GameStartedAt { get; set; }
        public int PlayTimeMinutes { get; set; }

        public Game? Game { get; set; }
        public GameMaster? GameMaster { get; set; }
        public Tournament? Tournament { get; set; }
        public ICollection<SessionParticipant> SessionParticipants { get; set; } = new List<SessionParticipant>();
    }
}
