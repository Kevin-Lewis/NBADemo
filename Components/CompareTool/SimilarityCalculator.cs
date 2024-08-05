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

        private double _maxHeight;
        private double _minHeight;
        private double _maxWeight;
        private double _minWeight;
        private double _maxPointsPer100;
        private double _maxAssistsPer100;
        private double _maxOffensiveReboundsPer100;
        private double _maxDefensiveReboundsPer100;
        private double _maxBlocksPer100;
        private double _maxStealsPer100;
        private double _maxTurnoversPer100;
        private double _maxFreeThrowsPer100;
        private double _maxFreeThrowAttemptsPer100;
        private double _maxPersonalFoulsPer100;

        public SimilarityCalculator(PrimaryWeights primaryWeights, PositionWeights positionWeights, Per100StatWeights per100StatWeights, ShotLocationWeights shotLocationWeights, ShotSuccessWeights shotSuccessWeights, List<CompareToolRow> rows)
        {
            _allRows = rows;

            _primaryWeights = primaryWeights;
            _positionWeights = positionWeights;
            _per100StatWeights = per100StatWeights;
            _shotLocationWeights = shotLocationWeights;
            _shotSuccessWeights = shotSuccessWeights;

            _maxHeight = _allRows.Max(r => r.Height);
            _minHeight = _allRows.Min(r => r.Height);
            _maxWeight = _allRows.Max(r => r.Weight);
            _minWeight = _allRows.Min(r => r.Weight);
            _maxPointsPer100 = _allRows.Max(r => r.PointsPer100);
            _maxAssistsPer100 = _allRows.Max(r => r.AssistsPer100);
            _maxOffensiveReboundsPer100 = _allRows.Max(r => r.OffensiveReboundsPer100);
            _maxDefensiveReboundsPer100 = _allRows.Max(r => r.DefensiveReboundsPer100);
            _maxBlocksPer100 = _allRows.Max(r => r.BlocksPer100);
            _maxStealsPer100 = _allRows.Max(r => r.StealsPer100);
            _maxTurnoversPer100 = _allRows.Max(r => r.TurnoversPer100);
            _maxFreeThrowsPer100 = _allRows.Max(r => r.FreeThrowMakesPer100);
            _maxFreeThrowAttemptsPer100 = _allRows.Max(r => r.FreeThrowAttemptsPer100);
            _maxPersonalFoulsPer100 = _allRows.Max(r => r.PersonalFoulsPer100);
        }

        public double CalculateDistance(CompareToolRow row1, CompareToolRow row2)
        {
            double distance = 0;

            // Height and Weight
            distance += (_primaryWeights.Height / 100.0) * Math.Pow((row1.Height - row2.Height) / (_maxHeight - _minHeight), 2);
            distance += (_primaryWeights.Weight / 100.0) * Math.Pow((row1.Weight - row2.Weight) / (_maxWeight - _minWeight), 2);

            // ShotLocation
            distance += (_primaryWeights.ShotLocation / 100.0) * (
                (_shotLocationWeights.FGARate_0_3 / 100.0) * Math.Pow(row1.Shot_0To3_AttemptPercent - row2.Shot_0To3_AttemptPercent, 2) +
                (_shotLocationWeights.FGARate_3_10 / 100.0) * Math.Pow(row1.Shot_3To10_AttemptPercent - row2.Shot_3To10_AttemptPercent, 2) +
                (_shotLocationWeights.FGARate_10_16 / 100.0) * Math.Pow(row1.Shot_10To16_AttemptPercent - row2.Shot_10To16_AttemptPercent, 2) +
                (_shotLocationWeights.FGARate_16_3PT / 100.0) * Math.Pow(row1.Shot_16To3Pt_AttemptPercent - row2.Shot_16To3Pt_AttemptPercent, 2) +
                (_shotLocationWeights.FGARate_3PT / 100.0) * Math.Pow(row1.Shot_3Pt_AttemptPercent - row2.Shot_3Pt_AttemptPercent, 2) +
                (_shotLocationWeights.FGA3PTRateCorner / 100.0) * Math.Pow(row1.Shot_Corner_3PtOf3s_AttemptPercent - row2.Shot_Corner_3PtOf3s_AttemptPercent, 2) +
                (_shotLocationWeights.FGARate_Dunks / 100.0) * Math.Pow(row1.Shot_Dunk_AttemptPercent - row2.Shot_Dunk_AttemptPercent, 2)
            );

            // ShotSuccess
            distance += (_primaryWeights.ShotSuccess / 100.0) * (
                (_shotSuccessWeights.FGASuccess_0_3 / 100.0) * Math.Pow(row1.Shot_0To3_MakePercent - row2.Shot_0To3_MakePercent, 2) +
                (_shotSuccessWeights.FGASuccess_3_10 / 100.0) * Math.Pow(row1.Shot_3To10_MakePercent - row2.Shot_3To10_MakePercent, 2) +
                (_shotSuccessWeights.FGASuccess_10_16 / 100.0) * Math.Pow(row1.Shot_10To16_MakePercent - row2.Shot_10To16_MakePercent, 2) +
                (_shotSuccessWeights.FGASuccess_16_3PT / 100.0) * Math.Pow(row1.Shot_16To3Pt_MakePercent - row2.Shot_16To3Pt_MakePercent, 2) +
                (_shotSuccessWeights.FGASuccess_3PT / 100.0) * Math.Pow(row1.Shot_3Pt_MakePercent - row2.Shot_3Pt_MakePercent, 2) +
                (_shotSuccessWeights.FGA3PTSuccessCorner / 100.0) * Math.Pow(row1.Shot_Corner_3Pt_MakePercent - row2.Shot_Corner_3Pt_MakePercent, 2)
            );

            // Per100Stats
            distance += (_primaryWeights.Per100Stats / 100.0) * (
                (_per100StatWeights.PTSPer100 / 100.0) * Math.Pow((row1.PointsPer100 - row2.PointsPer100) / _maxPointsPer100, 2) +
                (_per100StatWeights.ASTPer100 / 100.0) * Math.Pow((row1.AssistsPer100 - row2.AssistsPer100) / _maxAssistsPer100, 2) +
                (_per100StatWeights.ORBPer100 / 100.0) * Math.Pow((row1.OffensiveReboundsPer100 - row2.OffensiveReboundsPer100) / _maxOffensiveReboundsPer100, 2) +
                (_per100StatWeights.DRBPer100 / 100.0) * Math.Pow((row1.DefensiveReboundsPer100 - row2.DefensiveReboundsPer100) / _maxDefensiveReboundsPer100, 2) +
                (_per100StatWeights.BLKPer100 / 100.0) * Math.Pow((row1.BlocksPer100 - row2.BlocksPer100) / _maxBlocksPer100, 2) +
                (_per100StatWeights.STLPer100 / 100.0) * Math.Pow((row1.StealsPer100 - row2.StealsPer100) / _maxStealsPer100, 2) +
                (_per100StatWeights.TOVPer100 / 100.0) * Math.Pow((row1.TurnoversPer100 - row2.TurnoversPer100) / _maxTurnoversPer100, 2) +
                (_per100StatWeights.FTPer100 / 100.0) * Math.Pow((row1.FreeThrowMakesPer100 - row2.FreeThrowMakesPer100) / _maxFreeThrowsPer100, 2) +
                (_per100StatWeights.FTAPer100 / 100.0) * Math.Pow((row1.FreeThrowAttemptsPer100 - row2.FreeThrowAttemptsPer100) / _maxFreeThrowAttemptsPer100, 2) +
                (_per100StatWeights.PFPer100 / 100.0) * Math.Pow((row1.PersonalFoulsPer100 - row2.PersonalFoulsPer100) / _maxPersonalFoulsPer100, 2)
            );

            // Position
            distance += (_primaryWeights.Position / 100.0) * (
                (_positionWeights.PGPercent / 100.0) * Math.Pow((row1.PGPercent - row2.PGPercent) / 100, 2) +
                (_positionWeights.SGPercent / 100.0) * Math.Pow((row1.SGPercent - row2.SGPercent) / 100, 2) +
                (_positionWeights.SFPercent / 100.0) * Math.Pow((row1.SFPercent - row2.SFPercent) / 100, 2) +
                (_positionWeights.PFPercent / 100.0) * Math.Pow((row1.PFPercent - row2.PFPercent) / 100, 2) +
                (_positionWeights.CPercent / 100.0) * Math.Pow((row1.CPercent - row2.CPercent) / 100, 2)
            );

            // Usage
            distance += (_primaryWeights.Usage / 100.0) * Math.Pow((row1.Usage - row2.Usage) / 100, 2);

            double maxDistance = 1;

            var similarity = 1 - (Math.Sqrt(distance) / maxDistance);

            return similarity;
        }
    }
}
