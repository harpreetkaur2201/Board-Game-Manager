using Board_Game_Manager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Board_Game_Manager.DAL
{
    public class BoardGameContext : IdentityDbContext<IdentityUser>
    {
        public BoardGameContext(DbContextOptions<BoardGameContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<GameMaster> GameMasters { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<SessionParticipant> SessionParticipants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Player>().HasKey(p => p.PlayerID);
            modelBuilder.Entity<GameMaster>().HasKey(gm => gm.GMID);
            modelBuilder.Entity<Game>().HasKey(g => g.GameID);
            modelBuilder.Entity<Tournament>().HasKey(t => t.TournamentID);
            modelBuilder.Entity<GameSession>().HasKey(gs => gs.SessionID);

            modelBuilder.Entity<SessionParticipant>()
                .HasKey(sp => new { sp.SessionID, sp.PlayerID });

            modelBuilder.Entity<Player>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Player>()
                .Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Email)
                .IsUnique();

            modelBuilder.Entity<GameMaster>()
                .HasIndex(gm => gm.PlayerID)
                .IsUnique();

            modelBuilder.Entity<Game>()
                .Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Game>()
                .Property(g => g.URL)
                .HasMaxLength(255);

            modelBuilder.Entity<Game>()
                .Property(g => g.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<Game>()
                .HasIndex(g => g.Name)
                .IsUnique();

            modelBuilder.Entity<Tournament>()
                .Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Tournament>()
                .Property(t => t.Location)
                .HasMaxLength(100);

            modelBuilder.Entity<Tournament>()
                .Property(t => t.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<Tournament>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<GameSession>()
                .Property(gs => gs.PlayTimeMinutes)
                .IsRequired();

            modelBuilder.Entity<SessionParticipant>()
                .Property(sp => sp.Score)
                .IsRequired();

            modelBuilder.Entity<SessionParticipant>()
                .Property(sp => sp.Rank)
                .IsRequired();

            modelBuilder.Entity<SessionParticipant>()
                .HasIndex(sp => new { sp.SessionID, sp.Rank })
                .IsUnique();


            // Player <-> GameMaster (One-to-One)
            modelBuilder.Entity<GameMaster>()
                .HasOne(gm => gm.Player)
                .WithOne(p => p.GameMaster)
                .HasForeignKey<GameMaster>(gm => gm.PlayerID)
                .OnDelete(DeleteBehavior.Cascade);

            // Game -> Tournaments
            modelBuilder.Entity<Tournament>()
                .HasOne(t => t.Game)
                .WithMany(g => g.Tournaments)
                .HasForeignKey(t => t.GameID)
                .OnDelete(DeleteBehavior.Restrict);

            // Game -> GameSessions
            modelBuilder.Entity<GameSession>()
                .HasOne(gs => gs.Game)
                .WithMany(g => g.GameSessions)
                .HasForeignKey(gs => gs.GameID)
                .OnDelete(DeleteBehavior.Restrict);

            // GameMaster -> GameSessions
            modelBuilder.Entity<GameSession>()
                .HasOne(gs => gs.GameMaster)
                .WithMany(gm => gm.GameSessions)
                .HasForeignKey(gs => gs.GMID)
                .OnDelete(DeleteBehavior.Restrict);

            // Tournament -> GameSessions
            modelBuilder.Entity<GameSession>()
                .HasOne(gs => gs.Tournament)
                .WithMany(t => t.GameSessions)
                .HasForeignKey(gs => gs.TournamentID)
                .OnDelete(DeleteBehavior.Restrict);

            // SessionParticipant
            modelBuilder.Entity<SessionParticipant>()
                .HasOne(sp => sp.GameSession)
                .WithMany(gs => gs.SessionParticipants)
                .HasForeignKey(sp => sp.SessionID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SessionParticipant>()
                .HasOne(sp => sp.Player)
                .WithMany(p => p.SessionParticipants)
                .HasForeignKey(sp => sp.PlayerID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}