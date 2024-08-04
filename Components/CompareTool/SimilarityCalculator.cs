using NBADemo.Components.CompareTool.Weights;

namespace NBADemo.Components.CompareTool
{
    public class SimilarityCalculator
    {
        private List<CompareToolRow> _allRows;

        private PrimaryWeights _primaryWeights;
        private PositionWeights _positionWeights;
        private Per100StatWeights _per100StatWeights;
        private ShotLocationWeights _shotLocationWeights;
        private ShotSuccessWeights _shotSuccessWeights;

        public SimilarityCalculator(PrimaryWeights primaryWeights, PositionWeights positionWeights, Per100StatWeights per100StatWeights, ShotLocationWeights shotLocationWeights, ShotSuccessWeights shotSuccessWeights, List<CompareToolRow> rows)
        {
            _allRows = rows;

            _primaryWeights = primaryWeights;
            _positionWeights = positionWeights;
            _per100StatWeights = per100StatWeights;
            _shotLocationWeights = shotLocationWeights;
            _shotSuccessWeights = shotSuccessWeights;
        }

        public double CalculateDistance(CompareToolRow row1, CompareToolRow row2)
        {
            double distance = 0;

            // Find max values for normalization
            double maxHeight = _allRows.Max(r => r.Height);
            double maxWeight = _allRows.Max(r => r.Weight);            
            double maxPointsPer100 = _allRows.Max(r => r.PointsPer100);
            double maxAssistsPer100 = _allRows.Max(r => r.AssistsPer100);
            double maxOffensiveReboundsPer100 = _allRows.Max(r => r.OffensiveReboundsPer100);
            double maxDefensiveReboundsPer100 = _allRows.Max(r => r.DefensiveReboundsPer100);
            double maxBlocksPer100 = _allRows.Max(r => r.BlocksPer100);
            double maxStealsPer100 = _allRows.Max(r => r.StealsPer100);
            double maxTurnoversPer100 = _allRows.Max(r => r.TurnoversPer100);
            double maxFreeThrowsPer100 = _allRows.Max(r => r.FreeThrowMakesPer100);
            double maxFreeThrowAttemptsPer100 = _allRows.Max(r => r.FreeThrowAttemptsPer100);
            double maxPersonalFoulsPer100 = _allRows.Max(r => r.PersonalFoulsPer100);
            double maxUsage = _allRows.Max(r => r.Usage);

            // Height and Weight
            distance += _primaryWeights.Height * Math.Pow((row1.Height - row2.Height) / maxHeight, 2);
            distance += _primaryWeights.Weight * Math.Pow((row1.Weight - row2.Weight) / maxWeight, 2);

            // ShotLocation
            distance += _primaryWeights.ShotLocation * (
                _shotLocationWeights.FGARate_0_3 * Math.Pow(row1.Shot_0To3_AttemptPercent - row2.Shot_0To3_AttemptPercent, 2) +
                _shotLocationWeights.FGARate_3_10 * Math.Pow(row1.Shot_3To10_AttemptPercent - row2.Shot_3To10_AttemptPercent, 2) +
                _shotLocationWeights.FGARate_10_16 * Math.Pow(row1.Shot_10To16_AttemptPercent - row2.Shot_10To16_AttemptPercent, 2) +
                _shotLocationWeights.FGARate_16_3PT * Math.Pow(row1.Shot_16To3Pt_AttemptPercent - row2.Shot_16To3Pt_AttemptPercent, 2) +
                _shotLocationWeights.FGARate_3PT * Math.Pow(row1.Shot_3Pt_AttemptPercent - row2.Shot_3Pt_AttemptPercent, 2) +
                _shotLocationWeights.FGA3PTRateCorner * Math.Pow(row1.Shot_Corner_3PtOf3s_AttemptPercent - row2.Shot_Corner_3PtOf3s_AttemptPercent, 2) +
                _shotLocationWeights.FGARate_Dunks * Math.Pow(row1.Shot_Dunk_AttemptPercent - row2.Shot_Dunk_AttemptPercent, 2)
            );

            // ShotSuccess
            distance += _primaryWeights.ShotSuccess * (
                _shotSuccessWeights.FGASuccess_0_3 * Math.Pow(row1.Shot_0To3_MakePercent - row2.Shot_0To3_MakePercent, 2) +
                _shotSuccessWeights.FGASuccess_3_10 * Math.Pow(row1.Shot_3To10_MakePercent - row2.Shot_3To10_MakePercent, 2) +
                _shotSuccessWeights.FGASuccess_10_16 * Math.Pow(row1.Shot_10To16_MakePercent - row2.Shot_10To16_MakePercent, 2) +
                _shotSuccessWeights.FGASuccess_16_3PT * Math.Pow(row1.Shot_16To3Pt_MakePercent - row2.Shot_16To3Pt_MakePercent, 2) +
                _shotSuccessWeights.FGASuccess_3PT * Math.Pow(row1.Shot_3Pt_MakePercent - row2.Shot_3Pt_MakePercent, 2) +
                _shotSuccessWeights.FGA3PTSuccessCorner * Math.Pow(row1.Shot_Corner_3Pt_MakePercent - row2.Shot_Corner_3Pt_MakePercent, 2)
            );

            // Per100Stats
            distance += _primaryWeights.Per100Stats * (
                _per100StatWeights.PTSPer100 * Math.Pow((row1.PointsPer100 - row2.PointsPer100) / maxPointsPer100, 2) +
                _per100StatWeights.ASTPer100 * Math.Pow((row1.AssistsPer100 - row2.AssistsPer100) / maxAssistsPer100, 2) +
                _per100StatWeights.ORBPer100 * Math.Pow((row1.OffensiveReboundsPer100 - row2.OffensiveReboundsPer100) / maxOffensiveReboundsPer100, 2) +
                _per100StatWeights.DRBPer100 * Math.Pow((row1.DefensiveReboundsPer100 - row2.DefensiveReboundsPer100) / maxDefensiveReboundsPer100, 2) +
                _per100StatWeights.BLKPer100 * Math.Pow((row1.BlocksPer100 - row2.BlocksPer100) / maxBlocksPer100, 2) +
                _per100StatWeights.STLPer100 * Math.Pow((row1.StealsPer100 - row2.StealsPer100) / maxStealsPer100, 2) +
                _per100StatWeights.TOVPer100 * Math.Pow((row1.TurnoversPer100 - row2.TurnoversPer100) / maxTurnoversPer100, 2) +
                _per100StatWeights.FTPer100 * Math.Pow((row1.FreeThrowMakesPer100 - row2.FreeThrowMakesPer100) / maxFreeThrowsPer100, 2) +
                _per100StatWeights.FTAPer100 * Math.Pow((row1.FreeThrowAttemptsPer100 - row2.FreeThrowAttemptsPer100) / maxFreeThrowAttemptsPer100, 2) +
                _per100StatWeights.PFPer100 * Math.Pow((row1.PersonalFoulsPer100 - row2.PersonalFoulsPer100) / maxPersonalFoulsPer100, 2)
            );

            // Position
            distance += _primaryWeights.Position * (
                _positionWeights.PGPercent * Math.Pow((row1.PGPercent - row2.PGPercent) / 100, 2) +
                _positionWeights.SGPercent * Math.Pow((row1.SGPercent - row2.SGPercent) / 100, 2) +
                _positionWeights.SFPercent * Math.Pow((row1.SFPercent - row2.SFPercent) / 100, 2) +
                _positionWeights.PFPercent * Math.Pow((row1.PFPercent - row2.PFPercent) / 100, 2) +
                _positionWeights.CPercent * Math.Pow((row1.CPercent - row2.CPercent) / 100, 2)
            );

            // Usage
            distance += _primaryWeights.Usage * Math.Pow((row1.Usage - row2.Usage) / 100, 2);

            return Math.Sqrt(distance);
        }
    }
}
