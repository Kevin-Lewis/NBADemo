using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using NBADemo.Components.CompareTool.Dialogs;
using NBADemo.Components.CompareTool.Weights;
using NBADemo.Data;
using NBADemo.Services;
using NBAShared;
using System.Diagnostics.Metrics;
using static MudBlazor.CategoryTypes;
using static MudBlazor.Colors;

namespace NBADemo.Components.CompareTool
{
    public partial class CompareTool
    {
        private int _minMinutes = 500;
        private CompareToolRow _selectedPlayer;
        private int _selectedExperience = 0;

        private bool _isCalculate;

        [Inject]
        ApplicationDbContext DbContext { get; set; }       
        public List<PlayerSeasonInfo> PlayerSeasonInfo { get; set; }
        public Dictionary<short, PlayerCareerInfo> PlayerCareerInfo { get; set; }
        public Dictionary<string, PlayerPer100Stats> PlayerPer100Stats { get; set; }
        public Dictionary<string, PlayerPositionBreakdown> PlayerPositionBreakdown { get; set; }
        public Dictionary<string, PlayerShotLocation> PlayerShotLocation { get; set; }
        public Dictionary<string, PlayerShotSuccess> PlayerShotSuccess { get; set; }

        public List<CompareToolRow> CompareToolRows { get; set; }
        public List<CompareToolRow> BaseCompareToolRows { get; set; }

        public PrimaryWeights PrimaryWeights { get; set; }
        public ShotLocationWeights ShotLocationWeights { get; set; }
        public ShotSuccessWeights ShotSuccessWeights { get; set; }
        public PositionWeights PositionWeights { get; set; }
        public Per100StatWeights Per100StatWeights { get; set; }

        public SimilarityCalculator Calculator { get; set; }

        public Color ShotSuccessIconColor
        {
            get
            {
                return ShotSuccessWeights.Equals100 ? Color.Success : Color.Error;
            }
        }

        public Color ShotLocationIconColor
        {
            get
            {
                return ShotLocationWeights.Equals100 ? Color.Success : Color.Error;
            }
        }

        public Color Per100IconColor
        {
            get
            {
                return Per100StatWeights.Equals100 ? Color.Success : Color.Error;
            }
        }

        public Color PositionIconColor
        {
            get
            {
                return PositionWeights.Equals100 ? Color.Success : Color.Error;
            }
        }

        public Color WeightProgressColor
        {
            get
            {
                return PrimaryWeights.Equals100 ? Color.Success : Color.Error;
            }
        }

        public bool AreAllWeightValuesCorrect
        {
            get
            {
                return PrimaryWeights.Equals100 && ShotLocationWeights.Equals100 && ShotSuccessWeights.Equals100 && PositionWeights.Equals100 && Per100StatWeights.Equals100;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            InitializeWeights();

            PlayerSeasonInfo = await GetProcessedPlayerSeasonInfo();
            PlayerCareerInfo = await DbContext.PlayerCareerInfo.ToDictionaryAsync(x => x.PlayerId, x => x);
            PlayerPer100Stats = await GetProcessedPlayerPer100Stats();
            PlayerPositionBreakdown = await GetProcessedPlayerPositionBreakdown();
            PlayerShotLocation = await GetProcessedPlayerShotLocation();
            PlayerShotSuccess = await GetProcessedPlayerShotSuccess();

            BuildCompareToolRows();
        }

        private void InitializeWeights()
        {
            PrimaryWeights = new PrimaryWeights();
            ShotLocationWeights = new ShotLocationWeights();
            ShotSuccessWeights = new ShotSuccessWeights();
            Per100StatWeights = new Per100StatWeights();
            PositionWeights = new PositionWeights();
        }

        private void BuildCompareToolRows()
        {            
            CompareToolRows = new List<CompareToolRow>();            
            foreach (var item in PlayerSeasonInfo)
            {
                PlayerCareerInfo ci;
                PlayerCareerInfo.TryGetValue((short)item.PlayerId, out ci);

                PlayerPositionBreakdown pb;
                PlayerPositionBreakdown.TryGetValue($"{item.PlayerId}:{item.SeasonId}", out pb);

                PlayerPer100Stats per100;
                PlayerPer100Stats.TryGetValue($"{item.PlayerId}:{item.SeasonId}", out per100);

                PlayerShotLocation sl;
                PlayerShotLocation.TryGetValue($"{item.PlayerId}:{item.SeasonId}", out sl);

                PlayerShotSuccess ss;
                PlayerShotSuccess.TryGetValue($"{item.PlayerId}:{item.SeasonId}", out ss);

                if (ci is not null && pb is not null && ss is not null && sl is not null && per100 is not null && ci.Height is not null && ci.Weight is not null && item.Usage is not null)
                {
                    var row = new CompareToolRow(item, ci, per100, pb, ss, sl);
                    if (row.Minutes >= _minMinutes 
                        && (row.Experience == _selectedExperience || _selectedExperience == 0 || (row.PlayerId == _selectedPlayer.PlayerId && row.Season == _selectedPlayer.Season)))
                    {
                        CompareToolRows.Add(row);                       
                    }
                }
            }
            Calculator = new SimilarityCalculator(PrimaryWeights, PositionWeights, Per100StatWeights, ShotLocationWeights, ShotSuccessWeights, CompareToolRows);
            foreach (var row in CompareToolRows)
            {
                if (_selectedPlayer is not null && _isCalculate)
                {
                    var selectedRow = BaseCompareToolRows.FirstOrDefault(x => x.PlayerId == _selectedPlayer.PlayerId && x.Season == _selectedPlayer.Season);
                    if (selectedRow is not null)
                    {
                        row.Similarity = Calculator.CalculateDistance(row, selectedRow) * 100;
                    }
                }
            }
            CompareToolRows = CompareToolRows.OrderByDescending(x=> x.Similarity).ToList();
            if (BaseCompareToolRows is null)
            {
                BaseCompareToolRows = new List<CompareToolRow>(CompareToolRows);
            }
            _isCalculate = false;
        }

        private async Task<List<PlayerSeasonInfo>> GetProcessedPlayerSeasonInfo()
        {
            var playerSeasonInfos = await DbContext.PlayerSeasonInfo.ToListAsync();

            return playerSeasonInfos
                .GroupBy(p => new { p.PlayerId, p.Season })
                .Select(g =>
                {
                    var totRow = g.FirstOrDefault(p => p.Team == "TOT");
                    if (totRow != null)
                    {
                        totRow.Team = string.Join("/", g.Where(p => p.Team != "TOT").Select(p => p.Team));
                        return totRow;
                    }
                    return g.First();
                })
                .ToList();
        }

        private async Task<Dictionary<string, PlayerPer100Stats>> GetProcessedPlayerPer100Stats()
        {
            var playerPer100Stats = await DbContext.PlayerPer100Stats.ToListAsync();
            return playerPer100Stats
                .GroupBy(p => new { p.PlayerId, p.SeasonId })
                .Select(g =>
                {
                    var totRow = g.FirstOrDefault(p => p.Team == "TOT");
                    if (totRow != null)
                    {
                        totRow.Team = string.Join("/", g.Where(p => p.Team != "TOT").Select(p => p.Team));
                        return totRow;
                    }
                    return g.First();
                })
                .ToDictionary(x => $"{x.PlayerId}:{x.SeasonId}", x => x);
        }

        private async Task<Dictionary<string, PlayerPositionBreakdown>> GetProcessedPlayerPositionBreakdown()
        {
            var playerPositionBreakdown = await DbContext.PlayerPositionBreakdown.ToListAsync();
            return playerPositionBreakdown
                .GroupBy(p => new { p.PlayerId, p.SeasonId })
                .Select(g =>
                {
                    var totRow = g.FirstOrDefault(p => p.Team == "TOT");
                    if (totRow != null)
                    {
                        totRow.Team = string.Join("/", g.Where(p => p.Team != "TOT").Select(p => p.Team));
                        return totRow;
                    }
                    return g.First();
                })
                .ToDictionary(x => $"{x.PlayerId}:{x.SeasonId}", x => x);
        }

        private async Task<Dictionary<string, PlayerShotLocation>> GetProcessedPlayerShotLocation()
        {
            var data = await DbContext.PlayerShotLocation.ToListAsync();
            return data
                .GroupBy(p => new { p.PlayerId, p.SeasonId })
                .Select(g =>
                {
                    var totRow = g.FirstOrDefault(p => p.Team == "TOT");
                    if (totRow != null)
                    {
                        totRow.Team = string.Join("/", g.Where(p => p.Team != "TOT").Select(p => p.Team));
                        return totRow;
                    }
                    return g.First();
                })
                .ToDictionary(x => $"{x.PlayerId}:{x.SeasonId}", x => x);
        }

        private async Task<Dictionary<string, PlayerShotSuccess>> GetProcessedPlayerShotSuccess()
        {
            var data = await DbContext.PlayerShotSuccess.ToListAsync();
            return data
                .GroupBy(p => new { p.PlayerId, p.SeasonId })
                .Select(g =>
                {
                    var totRow = g.FirstOrDefault(p => p.Team == "TOT");
                    if (totRow != null)
                    {
                        totRow.Team = string.Join("/", g.Where(p => p.Team != "TOT").Select(p => p.Team));
                        return totRow;
                    }
                    return g.First();
                })
                .ToDictionary(x => $"{x.PlayerId}:{x.SeasonId}", x => x);
        }

        private void Calculate()
        {
            _isCalculate = true;
            BuildCompareToolRows();
        }

        private Task OpenShotSuccessWeights()
        {
            var parameters = new DialogParameters<ShotSuccessWeights>
            {
                { x => x.FGASuccess_0_3, ShotSuccessWeights.FGASuccess_0_3 },
                { x => x.FGASuccess_3_10, ShotSuccessWeights.FGASuccess_3_10 },
                { x => x.FGASuccess_10_16, ShotSuccessWeights.FGASuccess_10_16 },
                { x => x.FGASuccess_16_3PT, ShotSuccessWeights.FGASuccess_16_3PT },
                { x => x.FGASuccess_3PT, ShotSuccessWeights.FGASuccess_3PT },
                { x => x.FGA3PTSuccessCorner, ShotSuccessWeights.FGA3PTSuccessCorner },
                { "OnValuesUpdated", EventCallback.Factory.Create<(int, int, int, int, int, int)>(this, UpdateShotSuccessValues) }
            };
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth=MaxWidth.Large, FullWidth=true };
            return DialogService.ShowAsync<ShotSuccessDialog>("Shot Success Weights", parameters ,options);
        }

        private void UpdateShotSuccessValues((int, int, int, int, int, int) values)
        {
            (ShotSuccessWeights.FGASuccess_0_3, ShotSuccessWeights.FGASuccess_3_10, ShotSuccessWeights.FGASuccess_10_16, ShotSuccessWeights.FGASuccess_16_3PT, ShotSuccessWeights.FGASuccess_3PT, ShotSuccessWeights.FGA3PTSuccessCorner) = values;
        }

        private Task OpenShotLocationWeights()
        {
            var parameters = new DialogParameters<ShotLocationWeights>
            {
                { x => x.FGARate_0_3, ShotLocationWeights.FGARate_0_3 },
                { x => x.FGARate_3_10, ShotLocationWeights.FGARate_3_10 },
                { x => x.FGARate_10_16, ShotLocationWeights.FGARate_10_16 },
                { x => x.FGARate_16_3PT, ShotLocationWeights.FGARate_16_3PT },
                { x => x.FGARate_3PT, ShotLocationWeights.FGARate_3PT },
                { x => x.FGA3PTRateCorner, ShotLocationWeights.FGA3PTRateCorner },
                { x => x.FGARate_Dunks, ShotLocationWeights.FGARate_Dunks },
                { "OnValuesUpdated", EventCallback.Factory.Create<(int, int, int, int, int, int, int)>(this, UpdateShotLocationValues) }
            };
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };
            return DialogService.ShowAsync<ShotLocationDialog>("Shot Location Weights", parameters, options);
        }

        private void UpdateShotLocationValues((int, int, int, int, int, int, int) values)
        {
            (ShotLocationWeights.FGARate_0_3, ShotLocationWeights.FGARate_3_10, ShotLocationWeights.FGARate_10_16, ShotLocationWeights.FGARate_16_3PT, ShotLocationWeights.FGARate_3PT, ShotLocationWeights.FGA3PTRateCorner, ShotLocationWeights.FGARate_Dunks) = values;
        }

        private Task OpenPositionWeights()
        {
            var parameters = new DialogParameters<PositionWeights>
            {
                { x => x.PGPercent, PositionWeights.PGPercent },
                { x => x.SGPercent, PositionWeights.SGPercent },
                { x => x.SFPercent, PositionWeights.SFPercent },
                { x => x.PFPercent, PositionWeights.PFPercent },
                { x => x.CPercent, PositionWeights.CPercent },
                { "OnValuesUpdated", EventCallback.Factory.Create<(int, int, int, int, int)>(this, UpdatePositionValues) }
            };
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };
            return DialogService.ShowAsync<PositionDialog>("Position Weights", parameters, options);
        }

        private void UpdatePositionValues((int, int, int, int, int) values)
        {
            (PositionWeights.PGPercent, PositionWeights.SGPercent, PositionWeights.SFPercent, PositionWeights.PFPercent, PositionWeights.CPercent) = values;
        }

        private Task OpenPer100Weights()
        {
            var parameters = new DialogParameters<Per100StatWeights>
            {
                { x => x.PTSPer100, Per100StatWeights.PTSPer100 },
                { x => x.ASTPer100, Per100StatWeights.ASTPer100 },
                { x => x.ORBPer100, Per100StatWeights.ORBPer100 },
                { x => x.DRBPer100, Per100StatWeights.DRBPer100 },
                { x => x.TOVPer100, Per100StatWeights.TOVPer100 },
                { x => x.BLKPer100, Per100StatWeights.BLKPer100 },
                { x => x.STLPer100, Per100StatWeights.STLPer100 },
                { x => x.FTPer100, Per100StatWeights.FTPer100 },
                { x => x.FTAPer100, Per100StatWeights.FTAPer100 },
                { x => x.PFPer100, Per100StatWeights.PFPer100 },
                { "OnValuesUpdated", EventCallback.Factory.Create<(int, int, int, int, int, int, int, int, int, int)>(this, UpdatePer100Values) }
            };
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Large, FullWidth = true };
            return DialogService.ShowAsync<Per100Dialog>("Per 100 Weights", parameters, options);
        }

        private void UpdatePer100Values((int, int, int, int, int, int, int, int, int, int) values)
        {
            (Per100StatWeights.PTSPer100, Per100StatWeights.ASTPer100, Per100StatWeights.ORBPer100, Per100StatWeights.DRBPer100, Per100StatWeights.TOVPer100, Per100StatWeights.FTAPer100, Per100StatWeights.FTPer100, Per100StatWeights.BLKPer100, Per100StatWeights.STLPer100, Per100StatWeights.PFPer100) = values;
        }

        private async Task<IEnumerable<CompareToolRow>> PlayerSearch(string value, CancellationToken token)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }                
            return BaseCompareToolRows.OrderBy(x => x.PlayerName).ThenByDescending(x => x.Season).Select(x => x).Where(x => x.PlayerName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private void OnPlayerChange(CompareToolRow value)
        {
            _selectedPlayer = value;
            if (_selectedPlayer is not null)
            {
                _selectedPlayer.Similarity = 100;
            }
            _selectedExperience = value?.Experience ?? 0;

            BuildCompareToolRows();
        }

        private Func<CompareToolRow, string?> DisplayPlayerInSearch = item => item is null ? null : $"{item.PlayerName} - {item.Season} (Year {item.Experience})";
        private Func<int, string?> DisplayExperience = item => item == 0 ? "All" : item.ToString();

        private Func<CompareToolRow, int, string> RowStyleFunc => (x, i) =>
        {
            if (i % 2 == 1)
                return "background-color:lightgrey;";

            return "";
        };

        private Func<CompareToolRow, string> SimilarityStyleFunc => x =>
        {
            string style = x.Similarity switch
            {
                _ when x.Similarity >= 95 => "color:#00FF00;",
                _ when x.Similarity >= 93 => "color:#80FF00;",
                _ when x.Similarity >= 90 => "color:#CCFF00;",
                _ when x.Similarity >= 88 => "color:#FFFF00;",
                _ when x.Similarity >= 85 => "color:#FF8000;",
                _ when x.Similarity >= 0 => "color:#FF0000;",
                _ => ""
            };
            style += " font-weight:bold; background: #2E2E2E";
            return style;
        };

    }
}
