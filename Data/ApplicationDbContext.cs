using Microsoft.EntityFrameworkCore;
using NBAShared;

namespace NBADemo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerSeasonInfo>(entity =>
            {
                entity.ToTable("PlayerSeasonInfo");
                entity.Property(e => e.PlayerId).HasColumnName("player_id");
                entity.Property(e => e.SeasonId).HasColumnName("seas_id");
                entity.Property(e => e.PlayerName).HasColumnName("player");
                entity.Property(e => e.Team).HasColumnName("tm");
                entity.Property(e => e.League).HasColumnName("lg");
                entity.Property(e => e.Position).HasColumnName("pos");
            });

            modelBuilder.Entity<PlayerCareerInfo>(entity =>
            {
                entity.ToTable("PlayerCareerInfo");
                entity.Property(e => e.PlayerId).HasColumnName("player_id");
                entity.Property(e => e.PlayerName).HasColumnName("player");
                entity.Property(e => e.FirstSeason).HasColumnName("first_seas");
                entity.Property(e => e.Height).HasColumnName("height");
                entity.Property(e => e.Weight).HasColumnName("weight");
            });
        }

        public DbSet<PlayerSeasonInfo> PlayerSeasonInfo { get; set; }
        public DbSet<PlayerCareerInfo> PlayerCareerInfo { get; set; }
    }
}
