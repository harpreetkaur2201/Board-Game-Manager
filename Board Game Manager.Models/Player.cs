using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Board_Game_Manager.Models
{
    public class Player
    {
        public int PlayerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public DateTime RegisteredAt { get; set; }
        public int TotalHours { get; set; }
        public int TotalScore { get; set; }

        public GameMaster? GameMaster { get; set; }

        public ICollection<SessionParticipant> SessionParticipants { get; set; } = new List<SessionParticipant>();
    }
}

