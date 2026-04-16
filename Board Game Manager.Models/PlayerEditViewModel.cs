using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board_Game_Manager.Models {
    public class PlayerEditViewModel {
        public int PlayerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public DateTime RegisteredAt { get; set; }

        public bool IsGameMaster { get; set; }
    }
}
