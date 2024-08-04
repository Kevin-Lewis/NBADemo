namespace NBADemo.Components.CompareTool
{
    public class PrimaryWeights
    {
        public int Height { get; set; } = 10;
        public int Weight { get; set; } = 10;
        public int ShotLocation { get; set; } = 16;
        public int ShotSuccess { get; set; } = 16;
        public int Per100Stats { get; set; } = 16;
        public int Position { get; set; } = 16;
        public int Usage { get; set; } = 16;

        public bool Equals100 
        {
            get 
            {
                return Height + Weight + ShotLocation + ShotSuccess + Per100Stats + Position + Usage == 100;
            }
        }

        public int Total
        {
            get
            {
                return Height + Weight + ShotLocation + ShotSuccess + Per100Stats + Position + Usage;
            }
        }
    }
}
