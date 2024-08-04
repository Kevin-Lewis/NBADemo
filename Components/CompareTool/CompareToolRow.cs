using Microsoft.EntityFrameworkCore;
using NBAShared;

namespace NBADemo.Components.CompareTool
{   
    public class CompareToolRow
    {
        public CompareToolRow(PlayerSeasonInfo si, PlayerCareerInfo ci, PlayerPer100Stats per100, PlayerPositionBreakdown pb, PlayerShotSuccess ss, PlayerShotLocation sl)
        {
            PlayerId = si.PlayerId;
            PlayerName = si.PlayerName;
            Season = si.Season;
            Team = si.Team;
            Age = si.Age;
            Experience = si.Experience;
            Height = ci.Height;
            Weight = ci.Weight;
            FirstSeason = ci.FirstSeason;

            Minutes = pb.Minutes;
            PGPercent = pb.PGPercent ?? 0;
            SGPercent = pb.SGPercent ?? 0;
            SFPercent = pb.SFPercent ?? 0;
            PFPercent = pb.PFPercent ?? 0;
            CPercent = pb.CPercent ?? 0;

            PointsPer100 = per100.Points ?? 0;
            AssistsPer100 = per100.Assists ?? 0;
            OffensiveReboundsPer100 = per100.OffensiveRebounds ?? 0;
            DefensiveReboundsPer100 = per100.DefensiveRebounds ?? 0;
            BlocksPer100 = per100.Blocks ?? 0;
            StealsPer100 = per100.Steals ?? 0;
            TurnoversPer100 = per100.Turnovers ?? 0;
            FreeThrowMakesPer100 = per100.FreeThrowMakes ?? 0;
            FreeThrowAttemptsPer100 = per100.FreeThrowAttempts ?? 0;
            PersonalFoulsPer100 = per100.PersonalFouls ?? 0;

            Shot_0To3_AttemptPercent = sl.Shot_0To3 ?? 0;
            Shot_3To10_AttemptPercent = sl.Shot_3To10 ?? 0;
            Shot_10To16_AttemptPercent = sl.Shot_10To16 ?? 0;
            Shot_16To3Pt_AttemptPercent = sl.Shot_16To3Pt ?? 0;
            Shot_3Pt_AttemptPercent = sl.Shot_3Pt ?? 0;
            Shot_Corner_3PtOf3s_AttemptPercent = sl.Shot_Corner_3PtOf3s ?? 0;
            Shot_Dunk_AttemptPercent = sl.Shot_Dunk ?? 0;

            Shot_0To3_MakePercent = ss.Shot_0To3 ?? 0;
            Shot_3To10_MakePercent = ss.Shot_3To10 ?? 0;
            Shot_10To16_MakePercent = ss.Shot_10To16 ?? 0;
            Shot_16To3Pt_MakePercent = ss.Shot_16To3Pt ?? 0;
            Shot_3Pt_MakePercent = ss.Shot_3Pt ?? 0;
            Shot_Corner_3Pt_MakePercent = ss.Shot_Corner_3Pt ?? 0;
        }

        public double? Similarity { get; set; }
        public int PlayerId { get; set; }
        public int Season { get; set; }
        public string PlayerName { get; set; }
        public string Team { get; set; }
        public double? Age { get; set; }
        public byte? Experience { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public short? FirstSeason { get; set; }

        public short? Minutes { get; set; }
        public double? PGPercent { get; set; }
        public double? SGPercent { get; set; }
        public double? SFPercent { get; set; }
        public double? PFPercent { get; set; }
        public double? CPercent { get; set; }

        public double? PointsPer100 { get; set; }
        public double? AssistsPer100 { get; set; }
        public double? OffensiveReboundsPer100 { get; set; }
        public double? DefensiveReboundsPer100 { get; set; }
        public double? BlocksPer100 { get; set; }
        public double? StealsPer100 { get; set; }
        public double? TurnoversPer100 { get; set; }
        public double? FreeThrowAttemptsPer100 { get; set; }
        public double? FreeThrowMakesPer100 { get; set; }
        public double? PersonalFoulsPer100 { get; set; }

        public double? Shot_0To3_AttemptPercent { get; set; }
        public double? Shot_3To10_AttemptPercent { get; set; }
        public double? Shot_10To16_AttemptPercent { get; set; }
        public double? Shot_16To3Pt_AttemptPercent { get; set; }
        public double? Shot_3Pt_AttemptPercent { get; set; }
        public double? Shot_Corner_3PtOf3s_AttemptPercent { get; set; }
        public double? Shot_Dunk_AttemptPercent { get; set; }

        public double? Shot_0To3_MakePercent { get; set; }
        public double? Shot_3To10_MakePercent { get; set; }
        public double? Shot_10To16_MakePercent { get; set; }
        public double? Shot_16To3Pt_MakePercent { get; set; }
        public double? Shot_3Pt_MakePercent { get; set; }
        public double? Shot_Corner_3Pt_MakePercent { get; set; }

        public string HeightText
        {
            get
            {
                return Height is null ? string.Empty : $"{(int)Height / 12}'{(int)Height % 12}\"";
            }
        }
    }
}
