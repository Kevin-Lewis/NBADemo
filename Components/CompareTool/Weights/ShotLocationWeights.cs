namespace NBADemo.Components.CompareTool.Weights
{
    public class ShotLocationWeights
    {
        public int FGARate_0_3 { get; set; } = 14;
        public int FGARate_3_10 { get; set; } = 14;
        public int FGARate_10_16 { get; set; } = 14;
        public int FGARate_16_3PT { get; set; } = 14;
        public int FGARate_3PT { get; set; } = 15;
        public int FGA3PTRateCorner { get; set; } = 14;
        public int FGARate_Dunks { get; set; } = 15;

        public bool Equals100
        {
            get
            {
                return FGARate_0_3 + FGARate_3_10 + FGARate_10_16 + FGARate_16_3PT + FGA3PTRateCorner + FGARate_Dunks + FGARate_3PT == 100;
            }
        }

        public int Total
        {
            get
            {
                return FGARate_0_3 + FGARate_3_10 + FGARate_10_16 + FGARate_16_3PT + FGA3PTRateCorner + FGARate_Dunks + FGARate_3PT;
            }
        }
    }
}
