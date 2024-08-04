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

            modelBuilder.Entity<PlayerPositionBreakdown>(entity =>
            {
                entity.ToTable("PlayerPlayByPlay");
                entity.Property(e => e.PlayerId).HasColumnName("player_id");
                entity.Property(e => e.SeasonId).HasColumnName("seas_id");
                entity.Property(e => e.Team).HasColumnName("tm");
                entity.Property(e => e.Minutes).HasColumnName("mp");
                entity.Property(e => e.PGPercent).HasColumnName("pg_percent");
                entity.Property(e => e.SGPercent).HasColumnName("sg_percent");
                entity.Property(e => e.SFPercent).HasColumnName("sf_percent");
                entity.Property(e => e.PFPercent).HasColumnName("pf_percent");
                entity.Property(e => e.CPercent).HasColumnName("c_percent");
            });

            modelBuilder.Entity<PlayerPer100Stats>(entity =>
            {
                entity.ToTable("PlayerPer100");
                entity.Property(e => e.PlayerId).HasColumnName("player_id");
                entity.Property(e => e.SeasonId).HasColumnName("seas_id");
                entity.Property(e => e.Team).HasColumnName("tm");
                entity.Property(e => e.Points).HasColumnName("pts_per_100_poss");
                entity.Property(e => e.Assists).HasColumnName("ast_per_100_poss");
                entity.Property(e => e.OffensiveRebounds).HasColumnName("orb_per_100_poss");
                entity.Property(e => e.DefensiveRebounds).HasColumnName("drb_per_100_poss");
                entity.Property(e => e.Blocks).HasColumnName("blk_per_100_poss");
                entity.Property(e => e.Steals).HasColumnName("stl_per_100_poss");
                entity.Property(e => e.Turnovers).HasColumnName("tov_per_100_poss");
                entity.Property(e => e.FreeThrowAttempts).HasColumnName("fta_per_100_poss");
                entity.Property(e => e.FreeThrowMakes).HasColumnName("ft_per_100_poss");
                entity.Property(e => e.PersonalFouls).HasColumnName("pf_per_100_poss");
            });

            modelBuilder.Entity<PlayerShotSuccess>(entity =>
            {
                entity.ToTable("PlayerShotSuccess");
                entity.Property(e => e.PlayerId).HasColumnName("player_id");
                entity.Property(e => e.SeasonId).HasColumnName("seas_id");
                entity.Property(e => e.Team).HasColumnName("tm");
                entity.Property(e => e.Shot_0To3).HasColumnName("fg_percent_from_x0_3_range");
                entity.Property(e => e.Shot_3To10).HasColumnName("fg_percent_from_x3_10_range");
                entity.Property(e => e.Shot_10To16).HasColumnName("fg_percent_from_x10_16_range");
                entity.Property(e => e.Shot_16To3Pt).HasColumnName("fg_percent_from_x16_3p_range");
                entity.Property(e => e.Shot_3Pt).HasColumnName("fg_percent_from_x3p_range");
                entity.Property(e => e.Shot_Corner_3Pt).HasColumnName("corner_3_point_percent");
            });

            modelBuilder.Entity<PlayerShotLocation>(entity =>
            {
                entity.ToTable("PlayerShotLocation");
                entity.Property(e => e.PlayerId).HasColumnName("player_id");
                entity.Property(e => e.SeasonId).HasColumnName("seas_id");
                entity.Property(e => e.Team).HasColumnName("tm");
                entity.Property(e => e.Shot_0To3).HasColumnName("percent_fga_from_x0_3_range");
                entity.Property(e => e.Shot_3To10).HasColumnName("percent_fga_from_x3_10_range");
                entity.Property(e => e.Shot_10To16).HasColumnName("percent_fga_from_x10_16_range");
                entity.Property(e => e.Shot_16To3Pt).HasColumnName("percent_fga_from_x16_3p_range");
                entity.Property(e => e.Shot_3Pt).HasColumnName("percent_fga_from_x3p_range");
                entity.Property(e => e.Shot_Corner_3PtOf3s).HasColumnName("percent_corner_3s_of_3pa");
                entity.Property(e => e.Shot_Dunk).HasColumnName("percent_dunks_of_fga");
            });
        }

        public DbSet<PlayerSeasonInfo> PlayerSeasonInfo { get; set; }
        public DbSet<PlayerCareerInfo> PlayerCareerInfo { get; set; }
        public DbSet<PlayerPositionBreakdown> PlayerPositionBreakdown { get; set; }
        public DbSet<PlayerPer100Stats> PlayerPer100Stats { get; set; }
        public DbSet<PlayerShotSuccess> PlayerShotSuccess { get; set; }
        public DbSet<PlayerShotLocation> PlayerShotLocation { get; set; }
    }
}
