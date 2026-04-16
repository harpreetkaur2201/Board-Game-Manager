using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board_Game_Manager.Models {
    public class Game {
        public int GameID { get; set; }
        public string Name { get; set; }
        public string? URL { get; set; }
        public string Description { get; set; }
        public int AvgDuration { get; set; }
        public int MinPlayerCount { get; set; }
        public int MaxPlayerCount { get; set; }
        public int AgeReq { get; set; }

        public ICollection<GameSession> GameSessions { get; set; } = new List<GameSession>();
        public ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
    }
}

