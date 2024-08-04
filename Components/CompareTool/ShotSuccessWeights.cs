namespace NBADemo.Components.CompareTool
{
    public class ShotSuccessWeights
    {
        public int FGASuccess_0_3 { get; set; } = 15;
        public int FGASuccess_3_10 { get; set; } = 15;
        public int FGASuccess_10_16 { get; set; } = 15;
        public int FGASuccess_16_3PT { get; set; } = 15;
        public int FGASuccess_3PT { get; set; } = 25;
        public int FGA3PTSuccessCorner { get; set; } = 15;

        public bool Equals100
        {
            get
            {
                return FGASuccess_0_3 + FGASuccess_3_10 + FGASuccess_10_16 + FGASuccess_16_3PT + FGA3PTSuccessCorner + FGASuccess_3PT == 100;
            }
        }

        public int Total
        {
            get
            {
                return FGASuccess_0_3 + FGASuccess_3_10 + FGASuccess_10_16 + FGASuccess_16_3PT + FGA3PTSuccessCorner + FGASuccess_3PT;
            }
        }
    }
}
