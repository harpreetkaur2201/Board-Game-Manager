using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Board_Game_Manager.Models
{
    public class Tournament
    {
        public int TournamentID { get; set; }

        public string Name { get; set; }

        
        public int GameID { get; set; }            
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Location { get; set; }

        
        public string Description { get; set; }

        public Game? Game { get; set; }                          // Tournament -> Game
        public ICollection<GameSession> GameSessions { get; set; } = new List<GameSession>();
    }
}