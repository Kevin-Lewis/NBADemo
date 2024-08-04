namespace NBADemo.Components.CompareTool
{
    public class PositionWeights
    {
        public int PGPercent { get; set; } = 20;
        public int SGPercent { get; set; } = 20;
        public int SFPercent { get; set; } = 20;
        public int PFPercent { get; set; } = 20;
        public int CPercent { get; set; } = 20;

        public bool Equals100
        {
            get
            {
                return PGPercent + SGPercent + SFPercent + PFPercent + CPercent == 100;
            }
        }

        public int Total
        {
            get
            {
                return PGPercent + SGPercent + SFPercent + PFPercent + CPercent;
            }
        }
    }
}
