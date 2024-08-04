using Microsoft.EntityFrameworkCore;
using NBAShared;

namespace NBADemo.Components.CompareTool
{   
    public class CompareToolRow
    {
        public CompareToolRow(PlayerSeasonInfo si, PlayerCareerInfo ci)
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
        public string HeightText
        {
            get
            {
                return Height is null ? string.Empty : $"{(int)Height / 12}'{(int)Height % 12}\"";
            }
        }
    }
}
